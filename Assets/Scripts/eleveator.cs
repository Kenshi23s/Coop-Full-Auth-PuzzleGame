using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eleveator : MonoBehaviour
{
    public event Action update = () => { };
    Vector3 desiredRotation;
    // Start is called before the first frame update
    void Start()
    {
        rotating = false;
    }
    bool rotating = false;
    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.R)&&!rotating)
        {
            rotating = true;
            update = null;
            StartRotation(true);
            Debug.Log("KeyPress");
        }
        update?.Invoke();
    }

    public void StartRotation(bool x)
    {
        int dir = x ? 1 : -1;
        desiredRotation = new Vector3(0,0,transform.rotation.eulerAngles.z + 90f * dir) ;
        Debug.Log("Empiezo");
        update += RotateTowards;

    }
    void RotateTowards()
    {
        Vector3 actualRotation = new Vector3(0, 0, transform.rotation.eulerAngles.z);
        Vector3 lerp = Vector3.Lerp(actualRotation, desiredRotation,Time.deltaTime*5);
        transform.rotation = Quaternion.Euler(lerp);
        Debug.Log("rotando");
        if (actualRotation.z >= desiredRotation.z-2)
        {
            transform.rotation = Quaternion.Euler(desiredRotation);
            Debug.Log("llegue");
            rotating = false;
            update -= RotateTowards;
        }
    }

  
}
