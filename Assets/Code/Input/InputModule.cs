using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    None,
    HorizontalMovement,
    VerticalMovement,
    HorizontalCamera,
    VerticalCamera,
    Jump,
    Dive,
    ActionButton1,
    ActionButton2
}

public class InputModule : MonoBehaviour
{
    [SerializeField] InputSettings _settings;
    public InputSettings settings { get => _settings; }

    protected Vector2 _directionalInput { get; set; }
    public Vector2 directionalInput { get => ClampedDirInput(); }

    protected Vector2 _mouseInput { get; set; }
    public Vector2 mouseInput { get => _mouseInput; }

    public Vector2 directionalInputNormalized { get => directionalInput.normalized; }
    public Vector2 mouseInputNormalized { get => mouseInput.normalized; }

    float _directionAngle;
    public float directionAngle { get => _directionAngle; }

    float _mouseAngle;
    public float mouseAngle { get => _mouseAngle; }

    Vector2 ClampedDirInput()
    {
        Vector2 vec = _directionalInput;
        if (vec.magnitude > 1)
            vec = _directionalInput.normalized;
        return vec;
    }


    public bool IsDown(InputSettingsEnum setting) => enabled ? _settings.IsDown(setting) : false;

    public bool OnButtonDown(InputType inputType) => enabled ? Input.GetButtonDown(FromInputType(inputType)) : false;
    public bool OnButton(InputType inputType) => enabled ? Input.GetButton(FromInputType(inputType)) : false;
    public bool OnButtonUp(InputType inputType) => enabled ? Input.GetButtonUp(FromInputType(inputType)) : false;

    public float GetAxisRaw(InputType inputType) => enabled ? Input.GetAxisRaw(FromInputType(inputType)) : 0;
    public float GetAxis(InputType inputType) => enabled ? Input.GetAxis(FromInputType(inputType)) : 0;

    protected void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        HandleDirection(new Vector2(GetAxisRaw(InputType.HorizontalMovement), GetAxisRaw(InputType.VerticalMovement)));
        HandleMouse(new Vector2(GetAxisRaw(InputType.HorizontalCamera), -GetAxisRaw(InputType.VerticalCamera)));
    }

    protected void HandleDirection(Vector2 outcome)
    {
        _directionalInput = outcome;
        SetAngle(ref _directionAngle, outcome);
    }

    void SetAngle(ref float angle,Vector2 input)
    {
        Vector3 to = new Vector3(-input.x, 0, input.y);
        angle = Vector3.Angle(Vector3.forward, to);
        Vector3 cross = Vector3.Cross(Vector3.forward, to);
        if (cross.y > 0) angle = -angle;
    }

    protected void HandleMouse(Vector2 outcome)
    {
        _mouseInput = outcome;
        SetAngle(ref _mouseAngle, outcome);
    }


    string FromInputType(InputType type)
    {
        if (_settings == null) return string.Empty;
        switch(type)
        {
            case InputType.HorizontalMovement: return _settings.horizontalMovement;
            case InputType.VerticalMovement: return _settings.verticalMovement;
            case InputType.HorizontalCamera: return _settings.horizontalCamera;
            case InputType.VerticalCamera: return _settings.verticalCamera;
            case InputType.Jump: return _settings.jumpInput;
            case InputType.Dive: return _settings.diveInput;
            case InputType.ActionButton1: return _settings.actionInput1;
            case InputType.ActionButton2: return _settings.actionInput2;

            case InputType.None:
            default: return string.Empty;
        }
    }
}
