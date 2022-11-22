using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputModuleBase))]
public class MovementPlayer : MonoBehaviour
{
    InputModuleBase inputModule;
    CharacterController controller;

    [SerializeField] bool iGetMotionSick = false;
    [SerializeField] Image waterEffect;
    [SerializeField] Transform visualModel;
    [SerializeField] Camera playerCamera;
    [SerializeField] float hoverDistanceUnderWater = 1;
    [SerializeField] float smoothingDistance = 0.5f;
    [SerializeField] float smoothingTimer = 0.7f;
    [SerializeField] float bobberOffset = 0.2f;
    [SerializeField] float bobberSpeed = 1.25f;
    [SerializeField] float swimmingSpeed = 3;

    public bool isInWater { get => _isInWater; }

    float cameraY = 0;
    float bobberYOffset = 0;
    float timer = 0;
    float distancePW = 0;

    bool requiresSmoothing = true;
    bool canBobber = true;
    bool lastIGetMotionSick = false;

    bool _isInWater = false;
    bool _lastIsInWater = false;

    Vector3 movementThisFrame = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputModule = GetComponent<InputModuleBase>();
        if (WaterHandler.Instance == null) return;
        _isInWater = transform.position.y < hoveredWaterDistanceCompensated;
        _lastIsInWater = !_isInWater;
        if (playerCamera != null) cameraY = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        if (WaterHandler.Instance == null) return;
        HandleWaterState();
        HandleCameraMovement();
        HandleMovement();
        HandleWaterOverlay();
    }

    void HandleMovement()
    {
        movementThisFrame = Vector3.zero;
        if (!isInWater)
        {
            HandleWaterBobber();
            Vector3 inputDirection = swimmingSpeed * inputModule.directionalInputNormalized * Time.deltaTime;
            AddToMovement(transform.forward, inputDirection.y);
            AddToMovement(transform.right, inputDirection.x);
            HandleMotionSickness();
        }
        else
        {
            float inputX = inputModule.directionalInput.x * Time.deltaTime;
            float inputY = inputModule.directionalInput.y * Time.deltaTime;
            float thirdSwimSpeed = swimmingSpeed / 3;

            if (inputModule.directionalInput.y < 0) AddToMovement(transform.forward, thirdSwimSpeed * inputY);
            else if (inputModule.directionalInput.y > 0) AddToMovement(playerCamera.transform.forward, swimmingSpeed * inputY);

            AddToMovement(transform.right, thirdSwimSpeed * 2 * inputX);
        }


        canBobber = !Input.GetButton(inputModule.diveInput);

        if (!canBobber)                     SetMovementY(-swimmingSpeed * Time.deltaTime);
        if (Input.GetButton(inputModule.jumpInput))    SetMovementY(swimmingSpeed * Time.deltaTime);

        controller.Move(movementThisFrame);
    }
    void HandleCameraMovement()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera == null) return;
        Vector2 mouse = inputModule.mouseInput;

        float xRot = playerCamera.transform.rotation.eulerAngles.x;
        if (xRot >= 200 && xRot <= 360 && xRot < 285) if (mouse.y < 0) mouse.y = 0;
        if (xRot >= 0 && xRot < 200 && xRot > 75) if (mouse.y > 0) mouse.y = 0;

        controller.transform.Rotate(new Vector2(0, mouse.x), Space.Self);
        playerCamera.transform.Rotate(new Vector2(mouse.y, 0), Space.Self);
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
        if(timer >= smoothingTimer)
        {
            requiresSmoothing = false;
            timer = 0;
        }
    }
    void HandleMotionSickness()
    {
        if(lastIGetMotionSick != iGetMotionSick)
        {
            lastIGetMotionSick = iGetMotionSick;
            SetYAxis(playerCamera.transform, transform.position.y + cameraY);
        }
        if (!iGetMotionSick) return;
        if (requiresSmoothing) return;
        if (playerCamera == null) return;
        SetYAxis(playerCamera.transform, hoveredWaterDistance + cameraY - bobberOffset);
    }
    void HandleWaterOverlay()
    {
        if (waterEffect != null)
            waterEffect.enabled = isInWater;
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

    void AddToMovement(Vector3 direction, float power) => movementThisFrame += direction * power;
    void SetMovementY(float y) => movementThisFrame = new Vector3(movementThisFrame.x, y, movementThisFrame.z);
    void SetYAxis(Transform obj, float yAxis) => obj.position = new Vector3(obj.position.x, yAxis, obj.position.z);
    void SetYAxis(float yAxis) => controller.Move(new Vector3(0,yAxis - controller.transform.position.y, 0));
    float hoveredWaterDistance => WaterHandler.Instance.waterLevel - hoverDistanceUnderWater;
    float hoveredWaterDistanceCompensated => hoveredWaterDistance - smoothingDistance;
}
