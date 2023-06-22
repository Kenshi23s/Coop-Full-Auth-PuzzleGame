using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(LifeHandler))]
public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _characterControllerCustom;
    CharacterInputHandler handler;
    NetworkMecanimAnimator _animator;


    float _moveValue;

    private void Awake()
    {
       
      
    }
    public override void Spawned()
    {
        _characterControllerCustom = GetComponent<NetworkCharacterControllerCustom>();
        var lifeHandler = GetComponent<LifeHandler>();
        handler = GetComponent<CharacterInputHandler>();

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
            }
        }
        //Animator
        //cambiar nombre por el parametro del animator
        //_animator.Animator.SetFloat("", _moveValue);
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
