using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputModule))]
[RequireComponent(typeof(CharacterController))]
public class SharkMovement : MonoBehaviour
{
    InputModule inputModule;
    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputModule = GetComponent<InputModule>();
    }
    
    void Update()
    {
        Vector2 directionInput = inputModule.directionalInput;
    }
}
