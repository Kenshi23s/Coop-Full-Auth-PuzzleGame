using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class CharacterInputHandler : MonoBehaviour
{
    DebugableObject _debug;
    float _movementInput;

    bool _isJumpPressed;  

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
    }

    public NetworkInputData GetInputs()
    {
        _debug.Log("Doy mis Inputs");
        _inputData.movementInput = _movementInput;
        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;       

        return _inputData;
    }
}
