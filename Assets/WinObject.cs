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
      
    }

    public override void Spawned()
    {
        gameObject.layer = LayerManager.instance.GetElementData(myElement).Item1.LayerMaskToLayerNumber();
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            Debug.Log($"Seteo el elemento{myElement} a {player.gameObject.layer == gameObject.layer} por layer");
            GameManager.instance.SetElement(myElement, player.gameObject.layer == gameObject.layer);
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out NetworkPlayer player))
        {
            Debug.Log($"Seteo el elemento{myElement} a {false} ya que el player q estaba se salio");
            GameManager.instance.SetElement(myElement, false);
        }
           
    }
}
