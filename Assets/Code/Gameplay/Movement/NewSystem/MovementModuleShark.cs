using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Killer))]
public class MovementModuleShark : MovementModuleControlled
{
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float acceleration = 3f;
    [SerializeField] float maxRotationSpeed = 150;
    Vector2 inputDirection = Vector2.zero;
    Killer killer;

    Transform _transform => controller.transform;

    protected override void OnStart()
    {
        base.OnStart();
        killer = GetComponent<Killer>();

    }

    protected override void Tick()
    {
        base.Tick();
        if (!hasController) return;
        HandleInput();
        Move();
    }

    void HandleInput()
    {
        if (!hasInputModule) return;
        inputDirection = inputModule.directionalInput;

    }

    private void Move()
    {
        Vector3 direction = killer.cameraRig.forward;
        if(_transform.position.y > WaterHandler.Instance.waterLevel - killer.maxUnderwater)
            if(direction.y > 0)
                direction = new Vector3(direction.x, 0, direction.z);

        if (inputDirection.magnitude > 0)
        {
            Quaternion quat = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(direction), maxRotationSpeed * Time.deltaTime);
            _transform.rotation = quat;


            controller.Move(_transform.forward * Time.deltaTime * maxSpeed);

        }

    }
}
