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
       
        GeneralMovement();
    }

    void GeneralMovement()
    {
        NetworkInputData networkInputData = handler.GetInputs();
        
            //Move
          
            Vector3 moveDir = Vector3.forward * networkInputData.movementInput;
            Debug.Log("me quiero mover hacia"+moveDir);
           _characterControllerCustom.Move(moveDir);


        //Jump

        if (networkInputData.isJumpPressed)
        {
             _characterControllerCustom.Jump();
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
