using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(LifeHandler))]
public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _characterControllerCustom;
    CharacterInputHandler handler;
    NetworkMecanimAnimator _animator;
    NetworkPlayer _player;

   [SerializeField] float interactRadius;
    float _moveValue;

    private void Awake()
    {
       
      
    }
    public override void Spawned()
    {
        _characterControllerCustom = GetComponent<NetworkCharacterControllerCustom>();
        var lifeHandler = GetComponent<LifeHandler>();
        handler = GetComponent<CharacterInputHandler>();
        _animator = GetComponent<NetworkMecanimAnimator>();
        _player = GetComponent<NetworkPlayer>();

        //lifeHandler.OnDeadState += SetControllerEnabled;
        //lifeHandler.OnRespawn += Respawn;
    }


    public override void FixedUpdateNetwork() 
    {
        RPC_GeneralMovement();
    }

   
    void RPC_GeneralMovement()
    {
        if (handler.GetInput(out NetworkInputData data))
        {
          
            Vector3 moveDir = Vector3.right * data.movementInput;
            _characterControllerCustom.Move(moveDir);
            //Move
            //Jump

            if (data.isJumpPressed)
            {
                _characterControllerCustom.Jump();
                _animator.Animator.SetBool("isJumping", true);
            }
            else
                _animator.Animator.SetBool("isJumping", false);

            if (data.isInteractPressed)
            {
                NearestInteractable();
            }
        }
        //Animator
        //cambiar nombre por el parametro del animator
        _animator.Animator.SetFloat("isMoving", _moveValue);
    }
    void NearestInteractable()
    {
        Collider col = Physics.OverlapSphere(transform.position, interactRadius)
            .Where(x => x.GetComponent<Iinteractable>() != null)
            .Minimum(x=>Vector3.Distance(x.transform.position,transform.position));
            

        if (col != null)
        {
            if (col.TryGetComponent(out Iinteractable y)) y.Interact(_player);
        }
        else
        {
            Debug.Log("no hay interactuable cerca");
        }
       
    }
    //falta asignar

    void Respawn()
    {
        //pasarle el spawn point en vez de transform.position
        
    }

    void SetControllerEnabled(bool enable)
    {
        _characterControllerCustom.Controller.enabled = enable;
    }



}
