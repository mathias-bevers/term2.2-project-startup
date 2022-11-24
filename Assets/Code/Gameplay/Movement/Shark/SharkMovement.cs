using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputModuleBase))]
[RequireComponent(typeof(CharacterController))]
public class SharkMovement : MonoBehaviour
{
    InputModuleBase inputModule;
    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputModule = GetComponent<InputModuleBase>();
    }
    
    void Update()
    {
        Vector2 directionInput = inputModule.directionalInput;
    }
}
