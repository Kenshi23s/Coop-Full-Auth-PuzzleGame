using UnityEngine;
using Fusion;
using System;
using System.Linq;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    Vector3 moveDir = Vector3.zero;

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    NetworkString<_16> Nickname { get; set; }

    CharacterInputHandler _handler;
    NetworkCharacterControllerCustom _movement;
    NicknameText _myNickname;

    float interactRadius;
    Color newColor;

    public event Action OnLeft = delegate { };

    private void Awake()
    {
        _handler = GetComponent<CharacterInputHandler>();
        _movement = GetComponent<NetworkCharacterControllerCustom>();
    }
    public override void Spawned()
    {
        SetNickname();
    }

    private void Update()
    {
        NetworkInputData inputs = _handler.GetInputs();
        _movement.Move(new Vector3(inputs.movementInput, 0));

        if (inputs.isJumpPressed) _movement.Jump();

        if (inputs.isInteractPressed) NearestInteractable();
    }

    void NearestInteractable()
    {
        Collider col = Physics.OverlapSphere(transform.position, interactRadius)
            .Where(x => x.GetComponent<Iinteractable>() != null)
            .OrderByDescending(x => Vector3.Distance(x.transform.position, transform.position))
            .First();

        if (col.TryGetComponent(out Iinteractable y)) y.Interact(this);
    }

    public override void FixedUpdateNetwork()
    {
    }
    #region NickName

    void SetNickname()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            newColor = Color.blue;

            //cambiar esto en vez de spawned q sea altocar boton
            RPC_SetNickname("user");
        }
        else if (Object.HasStateAuthority && !Object.HasInputAuthority)
            newColor = Color.yellow;
        else
            newColor = Color.red;


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

    #endregion

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
