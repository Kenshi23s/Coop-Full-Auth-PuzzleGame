using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class CharacterInputHandler : MonoBehaviour
{
    DebugableObject _debug;
    float _movementInput;

    bool _isJumpPressed;
    bool interactPressed;

    NetworkInputData _inputData;
    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        _inputData = new NetworkInputData();
    }
    void Start()
    {
        
    }

    void Update()
    {
       
        _debug.Log(_movementInput.ToString());
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            interactPressed = true;
        }
    }

    public NetworkInputData GetInputs()
    {
        _debug.Log("Doy mis Inputs");
        _inputData.movementInput = _movementInput;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;       

        _inputData.isInteractPressed = interactPressed;
        interactPressed =false;

        return _inputData;
    }
}
