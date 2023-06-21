using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
[RequireComponent(typeof(LifeHandler))]
public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _characterControllerCustom;

    NetworkMecanimAnimator _animator;


    float _moveValue;

    private void Awake()
    {
       
      
    }
    public override void Spawned()
    {
        _characterControllerCustom = GetComponent<NetworkCharacterControllerCustom>();
        var lifeHandler = GetComponent<LifeHandler>();

        lifeHandler.OnDeadState += SetControllerEnabled;
        lifeHandler.OnRespawn += Respawn;
    }


    public override void FixedUpdateNetwork()
    {
        GeneralMovement();
    }

    void GeneralMovement()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            //Move
            Vector3 moveDir = Vector3.forward * networkInputData.movementInput;

            _characterControllerCustom.Move(moveDir);

            //Jump

            if (networkInputData.isJumpPressed)
            {
                _characterControllerCustom.Jump();
            }

            //Animator

            _moveValue = _characterControllerCustom.Velocity.x;

            //cambiar nombre por el parametro del animator
            _animator.Animator.SetFloat("", _moveValue);
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
