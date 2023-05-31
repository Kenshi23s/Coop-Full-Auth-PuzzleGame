using UnityEngine;
using Fusion;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    NetworkString<_16> Nickname { get; set; }

    NicknameText _myNickname;

    Color newColor;

    public event Action OnLeft = delegate { };

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            newColor = Color.blue;            

            //cambiar esto en vez de spawned q sea altocar boton
            RPC_SetNickname("user");
        }
        else if(Object.HasStateAuthority && !Object.HasInputAuthority)
        {
            newColor = Color.yellow;
        }
        else
        {
             newColor = Color.red;
        }       

        GetComponentInChildren<SkinnedMeshRenderer>().material.color = newColor;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickname(string newName)
    {
        Nickname = newName;
    }

    static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        var networkPlayerBehaviour = changed.Behaviour;
        changed.Behaviour.UpdateNickname(networkPlayerBehaviour.Nickname.ToString());
    }

    void UpdateNickname(string newName)
    {
        _myNickname.UpdateText(newName);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
