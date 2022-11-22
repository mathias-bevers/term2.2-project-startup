using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModuleBase : MonoBehaviour
{
    [SerializeField] string _jumpInput;
    [SerializeField] string _diveInput;
    [SerializeField] string _harpoonInput;
    [SerializeField] string _harpoonShoot;
    public string jumpInput { get => _jumpInput; }
    public string diveInput { get => _diveInput; }
    public string harpoonInput { get => _harpoonInput; }
    public string harpoonShoot { get => _harpoonShoot; }

    protected Vector2 _directionalInput { get; set; }
    public Vector2 directionalInput { get => _directionalInput; }

    protected Vector2 _mouseInput { get; set; }
    public Vector2 mouseInput { get => _mouseInput; }

    public Vector2 directionalInputNormalized { get => directionalInput.normalized; }
    public Vector2 mouseInputNormalized { get => mouseInput.normalized; }
    
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
        hasSetDir = true;
    }

    protected void HandleMouse(Vector2 outcome)
    {
        _mouseInput = outcome;
        hasSetMouse = true;
    }
}
