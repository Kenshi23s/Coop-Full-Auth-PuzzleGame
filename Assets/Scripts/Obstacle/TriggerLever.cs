using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(DebugableObject))]
[SelectionBase]
public class TriggerLever : MonoBehaviour,Iinteractable
{
    public UnityEvent OnTrigger;

    [SerializeField] float cooldown;
    [SerializeField] bool available;
    DebugableObject _debug;

   [SerializeField] bool button;
    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        _debug = GetComponent<DebugableObject>();
        available = true;
    }


    private void Update()
    {
        if (button)
        {
            button = false;
            RPC_Interact(default);
        }
    }
    public IEnumerator TriggerCD()
    {
        available = false;
        yield return new WaitForSeconds(cooldown);
        available = true;
    }

    public void RPC_Interact(NetworkPlayer whoInteracted)
    {
        if (!available) 
        {
            _debug.Log("No esta habilitado");

            return;
        }
        _debug.Log("Interadcuo con la palanca");

        OnTrigger?.Invoke();
        StartCoroutine(TriggerCD());
    }
}

