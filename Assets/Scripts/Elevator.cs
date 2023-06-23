using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
[RequireComponent(typeof(DebugableObject))]
public class Elevator : NetworkBehaviour
{
    public event Action NetUpdate = () => { };
    Vector3 desiredRotation;
    DebugableObject _debug;
    public float speed;

    NetworkBool rotating = false;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            rotating = false;
        }
       
    }
    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        ntwk_Transform = GetComponent<NetworkTransform>();

    }
  
    NetworkTransform ntwk_Transform;
    // Update is called once per frame
    void Update()
    {
       
      
      
    }

    public override void FixedUpdateNetwork()
    {
        NetUpdate?.Invoke();
    }
    public void StartRotation(bool x)
    {
        if (rotating) return;
        rotating = true;
        int dir = x ? 1 : -1;
        desiredRotation = new Vector3(0,0,transform.rotation.eulerAngles.z + 90f * dir) ;
        _debug.Log("Empiezo Rotacion");
        NetUpdate += RotateTowards;

    }


    void RotateTowards()
    {
        Vector3 actualRotation = new Vector3(0, 0, transform.rotation.eulerAngles.z);
        Vector3 lerp = Vector3.Lerp(actualRotation, desiredRotation,Runner.DeltaTime);
        ntwk_Transform.TeleportToRotation(Quaternion.Euler(lerp));

        _debug.Log("Rotando");
        if (actualRotation.z >= desiredRotation.z-2)
        {
            ntwk_Transform.TeleportToRotation(Quaternion.Euler(desiredRotation));
        
            rotating = false;
            NetUpdate -= RotateTowards;
        }
    }

  
}
