using UnityEngine;
using Fusion;
using System;
using System.Linq;
using FacundoColomboMethods;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
  

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    NetworkString<_16> Nickname { get; set; }

    CharacterInputHandler _handler;

    [SerializeField]GameObject view;
     NicknameText _myNickname;

    public ParticleSystem teleportPS;

    float interactRadius;
    Color newColor;

    public event Action OnLeft = delegate { };

   public LifeHandler lifeHandler;

    public CharacterInputHandler GetInputHandler()
    {
        return GetComponent<CharacterInputHandler>();
    }

   public NetworkTransform ntwk_transform { get; private set; }
    private void Awake()
    {
        _handler = GetComponent<CharacterInputHandler>();
        lifeHandler = GetComponent<LifeHandler>();
    }

    public void SetElement(Element x)
    {
        var data  = LayerManager.instance.GetElementData(x);
        gameObject.layer = data.elementLayer.LayerMaskToLayerNumber();
     
    }
    public override void Spawned()
    {
        SetNickname(PlayerPrefs.GetString("PlayerNickname"));
        ntwk_transform = GetComponent<NetworkTransform>();


        if (!HasInputAuthority) return;
        

        update += () => GameManager.instance.SetCamera(this);
        Local = this;


    }
    Action update;
    private void Update()
    {
        update?.Invoke();
    }

   

    public override void FixedUpdateNetwork()
    {
        //Debug.Log("FixedUpdatePlayer");
        //NetworkInputData inputs = _handler.GetInputs();

        //Vector3 dir =Vector3.right * inputs.movementInput;


        //_movement.Move(dir);

        //if (inputs.isJumpPressed) _movement.Jump();

        //if (inputs.isInteractPressed) NearestInteractable();
    }
    #region NickName

    void SetNickname(string nickname)
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            newColor = Color.blue;

            //cambiar esto en vez de spawned q sea altocar boton
            RPC_SetNickname(nickname);
        }
        else if (Object.HasStateAuthority && !Object.HasInputAuthority)
            newColor = Color.yellow;
        else
            newColor = Color.red;


        //GetComponentInChildren<SkinnedMeshRenderer>().material.color = newColor;
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
        return;
        _myNickname.UpdateText(newName);
    }

    #endregion

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
