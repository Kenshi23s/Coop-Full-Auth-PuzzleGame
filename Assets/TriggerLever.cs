using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[RequireComponent(typeof(SphereCollider))]
public class TriggerLever : MonoBehaviour
{
    public UnityEvent OnTrigger;

    [SerializeField] float cooldown;
    [SerializeField] bool available;

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    public void Interact()
    {
        if (!available) return;
        
        OnTrigger?.Invoke();
        StartCoroutine(TriggerCD());

    }


    public IEnumerator TriggerCD()
    {
        available = false;
        yield return new WaitForSeconds(cooldown);
        available = true;
    }
}
public class Player:MonoBehaviour
{
    bool inputAutority;
    float interactRadius = 5f;
    //networked
    float life;

    private void Update()
    {
        if (!inputAutority) return;

        if (Input.GetKeyDown(KeyCode.F))       
            foreach (var item in Physics.OverlapSphere(transform.position,interactRadius))
                     if (item.TryGetComponent(out TriggerLever x)) x.Interact();
    }

    //rpc de input a state??
    public void TakeDamage(float dmg)
    {
        life-=dmg;
        if (life <= 0) Die();
    }
    
    void Die()
    {
        //gameover
    }
}
public class ElementPool : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player x))
        {
            x.TakeDamage(Time.deltaTime);
        }
    }


}
