using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterInputHandler : NetworkBehaviour
{
    DebugableObject _debug;
    float _movementInput;
    NetworkPlayer player;
    bool _isJumpPressed;
    bool interactPressed;

    float interactRadius = 15f;

    NetworkInputData _inputData;
    private void Awake()
    {
        
        _inputData = new NetworkInputData();
        player = GetComponent<NetworkPlayer>();
    }
    void Start()
    {
        
    }

    void Update()
    {

        _movementInput = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            NearestInteractable();
        }
    }

    void NearestInteractable()
    {
        Collider col = Physics.OverlapSphere(transform.position, interactRadius)
            .Where(x => x.GetComponent<Iinteractable>() != null)
            .OrderByDescending(x => Vector3.Distance(x.transform.position, transform.position))
            .First();

        if (col.TryGetComponent(out Iinteractable y)) y.Interact(player);
    }

    public NetworkInputData GetInputs()
    {
        Debug.Log("Doy mis Inputs");
        _inputData.movementInput = _movementInput;
  

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;       

        _inputData.isInteractPressed = interactPressed;
        interactPressed =false;

        return _inputData;
    }
}
