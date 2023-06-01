using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using Unity.VisualScripting;
using Fusion;
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
        Tuple<LayerMask, Material> x = LayerManager.instance.GetLayer(myElement);
        GetComponent<Renderer>().sharedMaterial = x.Item2;
    }
    // Update is called once per frame
    public void Destroy()
    {
        Runner.Despawn(this.Object);
    }

    private void OnValidate()
    {
        //ver porque no puedo pasar la layer mask y tengo q ponerlo a mano
        if (Application.isPlaying) return;
        return;
  
        if (gameObject.name.Contains("[Fire]"))
        {
            char y = "[".ToCharArray()[0];
            gameObject.name = gameObject.name.TrimStart("[Fire]");
        }
        else if(gameObject.name.Contains("[Water]"))
        {
            char y = "[".ToCharArray()[0];
            gameObject.name = gameObject.name.TrimStart("[Water]");
        }
        
        Tuple<Color,string,int> x = myElement == Element.Fire 
            ? Tuple.Create(Color.red,"[Fire]",6) 
            : Tuple.Create(Color.blue,"[Water]", 7);

        if (TryGetComponent(out Renderer z))
        {
            z.material.color = new Color(x.Item1.r, x.Item1.g, x.Item1.b, 0.5f);
        }

        gameObject.name = x.Item2 +  gameObject.name;
        gameObject.layer = x.Item3;
       


    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
