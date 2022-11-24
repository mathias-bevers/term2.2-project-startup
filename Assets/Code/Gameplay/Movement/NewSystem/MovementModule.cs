using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModule : MonoBehaviour
{
    public CharacterController controller;

    internal bool hasController { get; private set; }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    private void Update()
    {
        hasController = controller != null;
        Tick();
    }

    protected virtual void Tick()
    {
        
    }
}
