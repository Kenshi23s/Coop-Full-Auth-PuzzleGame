using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(DebugableObject))]
[SelectionBase]
public class TriggerLever : MonoBehaviour,Iinteractable
{
    public UnityEvent OnTrigger;
    LineRenderer linerenderer;
    [SerializeField] float cooldown;
    [SerializeField] bool available;
    DebugableObject _debug;

   [SerializeField] bool button;
    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        _debug = GetComponent<DebugableObject>();
        available = true;
        linerenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        linerenderer.SetPosition(0,transform.position);
        linerenderer.SetPosition(1, OnTrigger.GetPersistentTarget(0).GameObject().transform.position);
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
        _debug.Log("Interactuo con la palanca");

        AudioManager.instance.Play("InteractButton");

        OnTrigger?.Invoke();
        Destroy(gameObject);
        StartCoroutine(TriggerCD());
    }

    private void OnDrawGizmos()
    {
        if (OnTrigger.GetPersistentTarget(0)!=null) 
        {
            for (int i = 0; i < OnTrigger.GetPersistentEventCount(); i++)
            {
                if (OnTrigger.GetPersistentTarget(i).GameObject()==null)                
                    continue;
                
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, OnTrigger.GetPersistentTarget(i).GameObject().transform.position);
            }
           
        }
        
    }
}

