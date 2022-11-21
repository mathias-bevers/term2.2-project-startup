using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputModuleBase))]
public class PlayerSwim : MonoBehaviour
{
    InputModuleBase inputModule;
    CharacterController controller;

    bool lastIGetMotionSick = false;
    [SerializeField] bool iGetMotionSick = false;

    [SerializeField] Transform visualModel;
    [SerializeField] Camera playerCamera;
    bool _isInWater = false;
    bool _lastIsInWater = false;
    public bool isInWater { get => _isInWater; }

    [SerializeField] float hoverDistanceUnderWater = 1;
    [SerializeField] float smoothingDistance = 0.5f;
    [SerializeField] float smoothingTimer = 0.7f;
    [SerializeField] float bobberOffset = 0.2f;
    [SerializeField] float bobberSpeed = 1.25f;

    float cameraY = 0;

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

        if (!isInWater)
        {
            HandleWaterBobber();



            HandleMotionSickness();
        }
        
        controller.Move(transform.forward * (3 * inputModule.directionalInput.y * Time.deltaTime));
        controller.Move(transform.right * (3 * inputModule.directionalInput.x * Time.deltaTime));
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


    float distancePW = 0;
    void OnWaterStateToggle()
    {
        if (!isInWater) 
        {
            timer = 0;
            requiresSmoothing = true;
            distancePW = hoveredWaterDistance - transform.position.y;
        }
    }
    
    float bobberYOffset = 0;
    float timer;
    bool requiresSmoothing = true;
    
    void HandleWaterBobber()
    {
       
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

    void SetYAxis(Transform obj, float yAxis) => obj.position = new Vector3(obj.position.x, yAxis, obj.position.z);
    void SetYAxis(float yAxis) => controller.Move(new Vector3(0,yAxis - controller.transform.position.y, 0));
    float hoveredWaterDistance => WaterHandler.Instance.waterLevel - hoverDistanceUnderWater;
    float hoveredWaterDistanceCompensated => hoveredWaterDistance - smoothingDistance;
}
