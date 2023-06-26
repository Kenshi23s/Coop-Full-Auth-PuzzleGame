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

    public override void Spawned()
    {
        _characterControllerCustom = GetComponent<NetworkCharacterControllerCustom>();
        var lifeHandler = GetComponent<LifeHandler>();
        handler = GetComponent<CharacterInputHandler>();
        _animator = GetComponent<NetworkMecanimAnimator>();
        _player = GetComponent<NetworkPlayer>();     
    }


    public override void FixedUpdateNetwork() => RPC_GeneralMovement();

    void RPC_GeneralMovement()
    {
        if (handler.GetInput(out NetworkInputData data))
        {          
            Vector3 moveDir = Vector3.right * data.movementInput;
            _characterControllerCustom.Move(moveDir);
           
            _animator.Animator.SetBool("isMoving", moveDir != Vector3.zero);

            if (data.isJumpPressed)
            {
                _characterControllerCustom.Jump();
                _animator.Animator.SetBool("isJumping", true);
            }
            else
                _animator.Animator.SetBool("isJumping", false);

            if (data.isInteractPressed)            
                NearestInteractable();            
        }
        else
        {
            _animator.Animator.SetBool("isMoving", false);
        }   
    }

    void NearestInteractable()
    {
        Collider col = Physics.OverlapSphere(transform.position, interactRadius)
            .Where(x => x.GetComponent<Iinteractable>() != null)
            .Minimum(x => Vector3.Distance(x.transform.position,transform.position));
            //mi funcion de linq C:

        if (col != null)
        {
            if (col.TryGetComponent(out Iinteractable y)) y.RPC_Interact(_player);
        }
        else        
            Debug.Log("no hay interactuable cerca");       
    }
 

    void SetControllerEnabled(bool enable)
    {
        _characterControllerCustom.Controller.enabled = enable;
    }



}
