using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputModuleBase))]
[RequireComponent(typeof(CameraMovement))]
public class MovementPlayer : MonoBehaviour
{
    InputModuleBase _inputModule;
    CharacterController _controller;
    CameraMovement _cameraMovement;

    [SerializeField] bool iGetMotionSick = false;
    [SerializeField] Image waterEffect;
    [SerializeField] Transform visualModel;
    [SerializeField] float hoverDistanceUnderWater = 1;
    [SerializeField] float smoothingDistance = 0.5f;
    [SerializeField] float smoothingTimer = 0.7f;
    [SerializeField] float bobberOffset = 0.2f;
    [SerializeField] float bobberSpeed = 1.25f;
    [SerializeField] float swimmingSpeed = 3;


    public bool isInWater { get => _isInWater; }
    public InputModuleBase inputModule { get => _inputModule; }
    public CharacterController controller { get => _controller; }
    public CameraMovement cameraMovement { get => _cameraMovement; }

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
        _controller = GetComponent<CharacterController>();
        _inputModule = GetComponent<InputModuleBase>();
        _cameraMovement = GetComponent<CameraMovement>();
        if (WaterHandler.Instance == null) return;
        _isInWater = transform.position.y < hoveredWaterDistanceCompensated;
        _lastIsInWater = !_isInWater;
        if (cameraMovement.playerCamera != null) cameraY = cameraMovement.playerCamera.transform.localPosition.y;
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
            Vector3 inputDirection = swimmingSpeed * _inputModule.directionalInputNormalized * Time.deltaTime;
            AddToMovement(transform.forward, inputDirection.y);
            AddToMovement(transform.right, inputDirection.x);
            HandleMotionSickness();
        }
        else
        {
            float inputX = _inputModule.directionalInput.x * Time.deltaTime;
            float inputY = _inputModule.directionalInput.y * Time.deltaTime;
            float thirdSwimSpeed = swimmingSpeed / 3;

            if (_inputModule.directionalInput.y < 0) AddToMovement(transform.forward, thirdSwimSpeed * inputY);
            else if (_inputModule.directionalInput.y > 0) AddToMovement(cameraMovement.playerCamera.transform.forward, swimmingSpeed * inputY);

            AddToMovement(transform.right, thirdSwimSpeed * 2 * inputX);
        }


        canBobber = !Input.GetButton(_inputModule.diveInput);

        if (!canBobber)                                                                                                     SetMovementY(-swimmingSpeed * Time.deltaTime);
        if (Input.GetButton(_inputModule.jumpInput) && controller.transform.position.y < WaterHandler.Instance.waterLevel)  SetMovementY(swimmingSpeed * Time.deltaTime);

        _controller.Move(movementThisFrame);
    }
    public void LookAtProper(Transform point)
    {
        LookAtProper(point, Vector3.zero);
    }

    public void LookAtProper(Transform point, Vector3 offset)
    {
        Vector3 direction = point.position - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        Quaternion slerpRot = Quaternion.Slerp(transform.rotation, targetRot, 10000 * Time.deltaTime);
        float yRot = slerpRot.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRot, transform.rotation.eulerAngles.z);

        Vector3 cameraDirection = point.position - cameraMovement.playerCamera.transform.position;
        Quaternion targetRotCam = Quaternion.LookRotation(cameraDirection);
        Quaternion camSlerpRot = Quaternion.Slerp(cameraMovement.playerCamera.transform.rotation, targetRotCam, 10000 * Time.deltaTime);
        float xRot = camSlerpRot.eulerAngles.x;
        cameraMovement.playerCamera.transform.rotation = Quaternion.Euler(xRot, cameraMovement.playerCamera.transform.rotation.eulerAngles.y, cameraMovement.playerCamera.transform.rotation.eulerAngles.z);
    }
    void HandleCameraMovement()
    {
        cameraMovement.MoveCamera(_inputModule.mouseInput);

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
            SetYAxis(cameraMovement.playerCamera.transform, transform.position.y + cameraY);
        }
        if (!iGetMotionSick) return;
        if (requiresSmoothing) return;
        if (cameraMovement.playerCamera == null) return;
        SetYAxis(cameraMovement.playerCamera.transform, hoveredWaterDistance + cameraY - bobberOffset);
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
    void SetYAxis(float yAxis) => _controller.Move(new Vector3(0,yAxis - _controller.transform.position.y, 0));
    float hoveredWaterDistance => WaterHandler.Instance.waterLevel - hoverDistanceUnderWater;
    float hoveredWaterDistanceCompensated => hoveredWaterDistance - smoothingDistance;
}
