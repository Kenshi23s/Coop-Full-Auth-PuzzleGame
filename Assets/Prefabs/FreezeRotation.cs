using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{

    private void Awake()
    {
        StartCoroutine(Freeze());
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    IEnumerator Freeze()
    {
       
        yield return new WaitForEndOfFrame();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
   
}
