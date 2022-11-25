using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementModule : MonoBehaviour
{
    public CharacterController controller { get; set; }

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
