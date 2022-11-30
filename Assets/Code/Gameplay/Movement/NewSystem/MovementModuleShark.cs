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

    Transform _transform => getTransform;

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

        Vector3 movement;
        if (_transform.position.y > WaterHandler.Instance.waterLevel - killer.maxUnderwater)
        {
            float angle = 0;
            if (Vector3.Dot(Vector3.up, killer.cameraRig.transform.forward) <= -0.5f)
                angle = lastAngleX;
            _transform.rotation = FromAngles(lastAngle, angle);
            movement = _transform.forward;

            if (movement.y > 0)
                movement.y = 0;
        }
        else
        {
            _transform.rotation = FromAngles(lastAngle, lastAngleX);
            movement = _transform.forward;
        }

        Move(movement * killer.movementSpeedPerc * Time.deltaTime * speed );
    }

    Quaternion FromAngles(float angleFromUp, float angleFromRight)
    {
        Quaternion angleQuat = Quaternion.AngleAxis(angleFromUp, Vector3.up);
        Quaternion angleForwardQuat = Quaternion.AngleAxis(angleFromRight, Vector3.right);
        Quaternion actualQuat = angleQuat * angleForwardQuat;
        Quaternion quat = Quaternion.RotateTowards(_transform.rotation, actualQuat, maxRotationSpeed * Time.deltaTime);
        quat.eulerAngles = new Vector3(quat.eulerAngles.x, quat.eulerAngles.y, 0);
        return quat;
    }
}
