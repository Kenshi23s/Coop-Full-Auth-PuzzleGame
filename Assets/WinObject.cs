using FacundoColomboMethods;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObject : NetworkBehaviour
{
    [SerializeField]Element myElement;

    private void Awake()
    {
      
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerManager.instance.GetElementData(myElement).Item1.LayerMaskToLayerNumber();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
            GameManager.instance.SetElement(myElement, player.gameObject.layer == gameObject.layer);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
            GameManager.instance.SetElement(myElement, false);
    }
}
