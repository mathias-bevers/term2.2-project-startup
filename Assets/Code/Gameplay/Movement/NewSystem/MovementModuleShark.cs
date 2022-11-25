using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Killer))]
public class MovementModuleShark : MovementModuleControlled
{
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float acceleration = 3f;
    [SerializeField] float deceleration = 5f;
    [SerializeField] float maxRotationSpeed = 150;
    Vector2 inputDirection = Vector2.zero;
    Killer killer;

    Transform _transform => controller.transform;

    float speed = 0;

    float lastAngle = 0;
    float lastAngleX = 0;

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

        if (inputDirection.magnitude > 0.1f)
        {
            lastAngle = inputModule.directionAngle + killer.cameraRig.transform.eulerAngles.y;
            lastAngleX = killer.cameraRig.transform.localEulerAngles.x;
        }
    }

    private void Move()
    {
        speed = Mathf.Clamp(speed += (inputDirection.magnitude > 0.1f ? acceleration : -deceleration) * Time.deltaTime, -0.1f, maxSpeed);
        if (speed < 0) return;

        Quaternion angleQuat = Quaternion.AngleAxis(lastAngle, Vector3.up);
        Quaternion angleForwardQuat = Quaternion.AngleAxis(lastAngleX, Vector3.right);
        Quaternion actualQuat = angleQuat * angleForwardQuat;
        Quaternion quat = Quaternion.RotateTowards(_transform.rotation, actualQuat, maxRotationSpeed * Time.deltaTime);
        quat.eulerAngles = new Vector3(quat.eulerAngles.x, quat.eulerAngles.y, 0);
        _transform.rotation = quat;

        Vector3 movement = _transform.forward;

        if (_transform.position.y + movement.y > WaterHandler.Instance.waterLevel - killer.maxUnderwater)
            movement.y = 0;

        //Move(movement, speed);

        controller.Move(movement * Time.deltaTime * speed);
    }
}
