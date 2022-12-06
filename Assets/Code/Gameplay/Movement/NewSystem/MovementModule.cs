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

    float _speed;
    public float getSpeed { get => _speed; }

    List<float> speedsGotten = new List<float>();

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
        speedsGotten.Add(speed);
        controller?.Move(direction.normalized * speed * Time.deltaTime);
    }

    public void Move(Vector3 direction)
    {
        speedsGotten.Add(direction.magnitude);
        controller.Move(direction);
    }

    private void FixedUpdate()
    {
        if (speedsGotten.Count == 0) return;
        float internalSpeed = 0;
        for(int i = 0; i < speedsGotten.Count; i++)
        {
            internalSpeed += speedsGotten[i];
        }   
        _speed = internalSpeed;
        speedsGotten.Clear();
    }
}
