using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using Unity.VisualScripting;
using Fusion;
using FacundoColomboMethods;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Collider))]

public class Obstacle : NetworkBehaviour
{
    [SerializeField] Element myElement;
    [SerializeField] GameObject view;

    NetworkPlayer aux;

    public Action<NetworkPlayer> PlayerEnter;
    public Action<NetworkPlayer> PlayerLeave;

    GameObject _splashParticle, enviromentParticle;

    public UnityEvent<Element> onElementChange;
    public UnityEvent<Vector3> FeedbackEnter;

    public void VFXEnter(Vector3 pos)
    {

        if (_splashParticle == null) { Debug.Log(" No Hago Particula"); return; } 

        Debug.Log("Hago Particula");
        var particle = Instantiate(_splashParticle, pos, Quaternion.identity);
        particle.transform.forward = -Vector3.up;
        Destroy(particle, particle.GetComponent<ParticleSystem>().time+4f);
    }
    private void Awake()
    {
        FeedbackEnter.AddListener(VFXEnter);
    }

    public override void Spawned()
    {
        RPC_SetElement();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_SetElement()
    {
        if (!HasStateAuthority) return;

        var x = LayerManager.instance.GetElementData(myElement);
        gameObject.layer = x.elementLayer.LayerMaskToLayerNumber();

        SetVisuals(myElement);

        RPC_SetVFX(myElement);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    void RPC_SetVFX(Element newElement) => SetVisuals(newElement);


    void SetVisuals(Element newElement)
    {
        var x = LayerManager.instance.GetElementData(newElement);

        Material mat = x.ElementMaterial;
        Renderer ren = view != null ? view.GetComponent<Renderer>() : GetComponent<Renderer>();
        ren.sharedMaterial = mat;

        _splashParticle = x.SplashParticle;

        if (enviromentParticle != null) Destroy(enviromentParticle);

        enviromentParticle = x.enviromentParticle;

        if (enviromentParticle == null) return;

        enviromentParticle = Instantiate(enviromentParticle, transform.position, Quaternion.identity * Quaternion.Euler(Vector3.up));
        enviromentParticle.transform.forward = Vector3.up;
        enviromentParticle.GetComponent<ParticleSystem>().Play();

    }


    public void DestroyObstacle() => Runner.Despawn(Object);

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
    public void RPC_FeedbackEnter(Vector3 playerPos)
    {
        FeedbackEnter?.Invoke(playerPos);
    }


    private void OnValidate()
    {
        //ver porque no puedo pasar la layer mask y tengo q ponerlo a mano
        //if (Application.isPlaying) return;      



        //Tuple<Color,string,int> x = myElement == Element.Fire 
        //    ? Tuple.Create(new Color (1,0,0,0.5f),"[Fire]",6) 
        //    : Tuple.Create(new Color(0,0,1,0.5f),"[Water]", 7);

        ////if (TryGetComponent(out Renderer z))
        ////{
        ////    if (z != null)
        ////        z.sharedMaterial.color = x.Item1;

        ////}


        //gameObject.layer = x.Item3;



    }


    IEnumerator DamageCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(4);
        while (aux != null)
        {
            aux.lifeHandler.RPC_TakeDamage(40);
            yield return wait;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out NetworkPlayer player))
        {
            PlayerEnter?.Invoke(player);
            if (player.gameObject.layer != gameObject.layer)
            {
                aux = player;
                StartCoroutine(DamageCoroutine());
                FeedbackEnter?.Invoke(player.transform.position);
                RPC_FeedbackEnter(player.transform.position);
            }
        }
    }

    public void ChangeElement()
    {
        // si soy de fuego me convierto en agua, si soy de agua paso a ser de fuego
        myElement = myElement == Element.Fire ? Element.Water : Element.Fire;
        RPC_SetElement(); onElementChange?.Invoke(myElement);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out NetworkPlayer player))
        {
            PlayerLeave?.Invoke(player);
            if (aux != null && aux == player)
            {

                StopAllCoroutines();
                aux = null;
            }
        }
    }
}
