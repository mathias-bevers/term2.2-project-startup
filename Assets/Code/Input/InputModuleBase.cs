using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModuleBase : MonoBehaviour
{
    [SerializeField] string _movementAxisHorizontal;
    [SerializeField] string _movementAxisVertical;
    [SerializeField] string _cameraAxisHorizontal;
    [SerializeField] string _cameraAxisVertical;
    [SerializeField] string _jumpInput;
    [SerializeField] string _diveInput;
    [SerializeField] string _actionButton1;
    [SerializeField] string _actionButton2;
    public string jumpInput { get => _jumpInput; }
    public string diveInput { get => _diveInput; }
    public string actionButton1 { get => _actionButton1; }
    public string actionButton2 { get => _actionButton2; }
    public string movementAxisHorizontal { get => _movementAxisHorizontal; }
    public string movementAxisVertical { get => _movementAxisVertical; }
    public string cameraAxisHorizontal { get => _cameraAxisHorizontal; }
    public string cameraAxisVertical { get => _cameraAxisVertical; }

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

    protected void Update()
    {
        HandleDirection();
        HandleMouse();
        OnUpdate();
        if (!hasSetDir) Debug.LogError("You MUST call base.HandleDirection(Vector2)");
        if (!hasSetMouse) Debug.LogError("You MUST call base.HandleMouse(Vector2)");
    }

    bool hasSetDir = false;
    bool hasSetMouse = false;

    protected virtual void RegisterDictionary() { }
    protected virtual void OnUpdate() { }
    protected virtual void HandleDirection() { }
    protected virtual void HandleMouse() { }

    protected void HandleDirection(Vector2 outcome)
    {
        _directionalInput = outcome;
        SetAngle(ref _directionAngle, outcome);
        hasSetDir = true;
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
        hasSetMouse = true;
    }
}
