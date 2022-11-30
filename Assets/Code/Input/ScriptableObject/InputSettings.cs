using Code.Utils;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Settings", menuName= "Input/Input Settings", order = 0)]
public class InputSettings : ScriptableObject
{
   
    [SerializeField, StringInList("GetAllAxes")] string _horizontalMovement;
    [SerializeField, StringInList("GetAllAxes")] string _verticalMovement;
    [SerializeField, StringInList("GetAllAxes")] string _horizontalCamera;
    [SerializeField, StringInList("GetAllAxes")] string _verticalCamera;
    [SerializeField, StringInList("GetAllAxes")] string _jumpInput;
    [SerializeField, StringInList("GetAllAxes")] string _diveInput;
    [SerializeField, StringInList("GetAllAxes")] string _actionInput1;
    [SerializeField, StringInList("GetAllAxes")] string _actionInput2;
    [SerializeField, StringInList("GetAllAxes")] string _axisHorizontal;
    [SerializeField, StringInList("GetAllAxes")] string _axisVertical;
    
    public string horizontalMovement { get => _horizontalMovement; }
    public string verticalMovement { get => _verticalMovement; }
    public string horizontalCamera { get => _horizontalCamera; }
    public string verticalCamera { get => _verticalCamera; }
    public string jumpInput { get => _jumpInput; }
    public string diveInput { get => _diveInput; }
    public string actionInput1 { get => _actionInput1; }
    public string actionInput2 { get => _actionInput2; }
    public string axisHorizontal { get => _axisHorizontal; }
    public string axisVertical { get => _axisVertical; }

    public bool IsActive()
    {
        if (new Vector2(Input.GetAxisRaw(horizontalMovement), Input.GetAxisRaw(verticalMovement)).magnitude > 0.15f)
            return true;
        if (new Vector2(Input.GetAxisRaw(horizontalCamera), Input.GetAxisRaw(verticalCamera)).magnitude > 0.05f)
            return true;
        if (new Vector2(Input.GetAxisRaw(axisHorizontal), Input.GetAxisRaw(axisVertical)).magnitude > 0.15f)
            return true;
        if (Input.GetButton(jumpInput) || Input.GetButton(diveInput) || Input.GetButton(actionInput1) || Input.GetButton(actionInput2))
            return true;

        return false;
    }

    public bool IsDown(InputSettingsEnum setting)
    {
        switch (setting)
        {
            case InputSettingsEnum.HorizontalMovement:
                return Input.GetAxisRaw(horizontalMovement) > 0.19f;
            case InputSettingsEnum.VerticalMovement:
                return Input.GetAxisRaw(verticalMovement) > 0.19f;
            case InputSettingsEnum.VerticalCamera:
                return Input.GetAxisRaw(verticalCamera) > 0.19f;
            case InputSettingsEnum.HorizontalCamera:
                return Input.GetAxisRaw(horizontalCamera) > 0.19f;
            case InputSettingsEnum.AxisHorizontal:
                return Input.GetAxisRaw(axisHorizontal) > 0.19f;
            case InputSettingsEnum.AxisVertical:
                return Input.GetAxisRaw(axisVertical) > 0.19f;
            case InputSettingsEnum.JumpInput:
                return Input.GetButton(jumpInput);
            case InputSettingsEnum.DiveInput:
                return Input.GetButton(diveInput);
            case InputSettingsEnum.ActionButton1:
                return Input.GetButton(actionInput1);
            case InputSettingsEnum.ActionButton2:
                return Input.GetButton(actionInput2);
            case InputSettingsEnum.None:
            default:
                return false;
        }
    }
}

public enum InputSettingsEnum
{
    None,
    HorizontalMovement,
    VerticalMovement,
    HorizontalCamera,
    VerticalCamera,
    JumpInput,
    DiveInput,
    ActionButton1,
    ActionButton2,
    AxisHorizontal,
    AxisVertical,
}
