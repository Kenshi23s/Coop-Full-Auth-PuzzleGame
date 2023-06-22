using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using Unity.VisualScripting;
using Fusion;
using FacundoColomboMethods;
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Collider))]
public class Obstacle : NetworkBehaviour
{
    [SerializeField]Element myElement;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SetElement();
    }

    void SetElement()
    {
        Tuple<LayerMask, Material> x = LayerManager.instance.GetElementData(myElement);
        gameObject.layer = x.Item1.LayerMaskToLayerNumber();
        GetComponent<Renderer>().material = x.Item2;
    
    }
    // Update is called once per frame
    public void DestroyObstacle()
    {
        Runner.Despawn(this.Object);
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

    NetworkPlayer aux;
    IEnumerator DamageCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        while (aux!=null)
        {
            aux.lifeHandler.RPC_TakeDamage(1);
            yield return wait;
        }
     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            if (player.gameObject.layer!=gameObject.layer)
            {
                aux = player;
                StartCoroutine(DamageCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            if (aux!=null&& aux ==player )
            {
                
                StopAllCoroutines();
                aux = null;
            }
        }
    }
}
