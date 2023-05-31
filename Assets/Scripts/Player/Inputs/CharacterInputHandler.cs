using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    float _movementInput;

    bool _isJumpPressed;  

    NetworkInputData _inputData;

    void Start()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        _movementInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }       
    }

    public NetworkInputData GetInputs()
    {
        _inputData.movementInput = _movementInput;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;       

        return _inputData;
    }
}
