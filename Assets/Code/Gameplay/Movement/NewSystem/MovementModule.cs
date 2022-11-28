using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementModule : MonoBehaviour
{
    public Transform getTransform { get => controller.transform; }
    CharacterController controller;
    public CharacterController setController { set => controller = value; }

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


    internal void Move(Vector3 direction, float speed)
    {
        controller?.Move(direction.normalized * speed * Time.deltaTime);
    }

    public void Move(Vector3 direction)
    {
        controller.Move(direction);
    }
}
