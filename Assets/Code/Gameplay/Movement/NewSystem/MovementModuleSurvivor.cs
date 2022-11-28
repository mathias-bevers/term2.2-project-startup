using Code;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Survivor))]
public class MovementModuleSurvivor : MovementModuleControlled
{
    Survivor survivor;
    Transform _transform => controller.transform;

    [SerializeField] bool iGetMotionSick = false;
    //[SerializeField] float hoverDistanceUnderWater = 1;
    [SerializeField] float smoothingDistance = 0.5f;
    [SerializeField] float smoothingTimer = 0.7f;
    [SerializeField] float bobberOffset = 0.2f;
    [SerializeField] float bobberSpeed = 1.25f;
    [SerializeField] float swimmingSpeed = 7;

    public bool isInWater { get => _isInWater; }

    float cameraY;
    float bobberYOffset = 0;
    float timer = 0;
    float distancePW = 0;

    bool requiresSmoothing = true;
    bool canBobber = true;
    bool lastIGetMotionSick = false;

    bool _isInWater = false;
    bool _lastIsInWater = false;

    Vector3 movementThisFrame = Vector3.zero;

    bool divingFromCamera = false;

    protected override void OnStart()
    {
        base.OnStart();
        survivor = GetComponent<Survivor>();

        _isInWater = transform.position.y < hoveredWaterDistanceCompensated;
        _lastIsInWater = !_isInWater;
        cameraY = survivor.cameraFollowPoint.localPosition.y;
    }

    protected override void Tick()
    {
        base.Tick();
        HandleWaterState();
        HandleMovement();
    }

    void HandleWaterState()
    {
        _isInWater = transform.position.y < hoveredWaterDistanceCompensated;
        if (_lastIsInWater != _isInWater)
        {
            _lastIsInWater = _isInWater;
            OnWaterStateToggle();
        }
    }
    void OnWaterStateToggle()
    {
        if (!isInWater)
        {
            timer = 0;
            requiresSmoothing = true;
            distancePW = hoveredWaterDistance - transform.position.y;
        }
    }

    void HandleMovement()
    {
        _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, survivor.cameraRig.transform.eulerAngles.y, _transform.eulerAngles.z);

        movementThisFrame = Vector3.zero;

        if (!isInWater) HandleMovementOutOfWater();
        else HandleMovementInWater();

        canBobber = !inputModule.OnButton(InputType.Dive);
        if (divingFromCamera) { canBobber = false; divingFromCamera = false; }

        if (!canBobber) SetMovementY(-swimmingSpeed * Time.deltaTime);
        if (inputModule.OnButton(InputType.Jump) && controller.transform.position.y < hoveredWaterDistance) SetMovementY(swimmingSpeed * Time.deltaTime);


       // Move(movementThisFrame, movementThisFrame.magnitude);
        controller.Move(movementThisFrame);
    }
    void HandleMovementOutOfWater()
    {
        HandleWaterBobber();
        Vector3 inputDirection = swimmingSpeed * inputModule.directionalInputNormalized * Time.deltaTime;
        AddToMovement(transform.forward, inputDirection.y);
        AddToMovement(transform.right, inputDirection.x);
        HandleMotionSickness();
        if(inputDirection.y > 0.1f)
        divingFromCamera = Vector3.Dot(survivor.cameraRig.forward, Vector3.up) < -0.5f;
    }

    void HandleMotionSickness()
    {
        if (lastIGetMotionSick != iGetMotionSick)
        {
            lastIGetMotionSick = iGetMotionSick;
            SetYAxis(survivor.cameraFollowPoint, transform.position.y + cameraY);
        }
        if (!iGetMotionSick) return;
        if (requiresSmoothing) return;
        SetYAxis(survivor.cameraFollowPoint, hoveredWaterDistance + cameraY - bobberOffset);
    }

    void HandleMovementInWater()
    {
        float inputX = inputModule.directionalInput.x * Time.deltaTime;
        float inputY = inputModule.directionalInput.y * Time.deltaTime;

        float thirdSwimSpeed = swimmingSpeed / (float)3;

        if (inputModule.directionalInput.y < 0) AddToMovement(transform.forward, thirdSwimSpeed * inputY);
        else if (inputModule.directionalInput.y > 0) AddToMovement(survivor.cameraRig.forward, swimmingSpeed * inputY);

        AddToMovement(transform.right, thirdSwimSpeed * 2 * inputX);
    }

    void HandleWaterBobber()
    {
        if (!canBobber) return;
        if (requiresSmoothing)
        {
            HandleSmoothedWaterBobber();
            return;
        }
        timer += Time.deltaTime * bobberSpeed;
        bobberYOffset = Mathf.Abs(bobberOffset * Mathf.Sin(timer));
        SetYAxis(hoveredWaterDistance - bobberYOffset);
    }

    void HandleSmoothedWaterBobber()
    {
        timer += Time.deltaTime;
        float mapped = Utils.Map(timer, 0, smoothingTimer, 0, distancePW);
        SetYAxis(hoveredWaterDistance - (distancePW - mapped));
        if (timer >= smoothingTimer)
        {
            requiresSmoothing = false;
            timer = 0;
        }
    }


    void AddToMovement(Vector3 direction, float power) => movementThisFrame += direction * power * survivor.movementSpeedPerc;
    void SetMovementY(float y) => movementThisFrame = new Vector3(movementThisFrame.x, y, movementThisFrame.z);
    void SetYAxis(Transform obj, float yAxis) => obj.position = new Vector3(obj.position.x, yAxis, obj.position.z);
    void SetYAxis(float yAxis) => controller.Move(new Vector3(0, yAxis - controller.transform.position.y, 0));
    float hoveredWaterDistance => WaterHandler.Instance.waterLevel - survivor.hoverUnderWater;
    float hoveredWaterDistanceCompensated => hoveredWaterDistance - smoothingDistance;
}
