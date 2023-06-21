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
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
