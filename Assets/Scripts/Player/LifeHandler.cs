using UnityEngine;
using Fusion;
using System;
[RequireComponent(typeof(DebugableObject))]
public class LifeHandler : NetworkBehaviour
{
    DebugableObject _debug;
    const byte FULL_LIFE = 100;

    [Networked]
    byte _currentLife { get; set; }

    [SerializeField] GameObject _view;

    public event Action<bool> OnDeadState = delegate { };
    public event Action OnRespawn = delegate { };

    private void Awake() => _debug = GetComponent<DebugableObject>();
  

    private void Start() => _currentLife = FULL_LIFE;
  
    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(byte dmg)
    {
      
        RPC_FeedbackDMG(dmg);

        if (!HasStateAuthority) return;
    
       

        if (dmg > _currentLife)
            dmg = _currentLife;

        _debug.Log($"recibi {dmg}, mi vida actual es{_currentLife}");
        _currentLife -= dmg;
        AudioManager.instance.Play("OnDamage");

        if (_currentLife == 0)
        {
            GameManager.instance.RPC_GAMEOVER(false);
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_FeedbackDMG(byte dmg)
    {
        FloatingTextManager.instance.PopUpText(dmg.ToString(), transform.position);
    }



   
}
