using UnityEngine;

[CreateAssetMenu(fileName = "Input Settings", menuName= "Input/Input Settings", order = 0)]
public class InputSettings : ScriptableObject
{
    [SerializeField] string _horizontalMovement;
    [SerializeField] string _verticalMovement;
    [SerializeField] string _horizontalCamera;
    [SerializeField] string _verticalCamera;
    [SerializeField] string _jumpInput;
    [SerializeField] string _diveInput;
    [SerializeField] string _actionInput1;
    [SerializeField] string _actionInput2;
    
    public string horizontalMovement { get => _horizontalMovement; }
    public string verticalMovement { get => _verticalMovement; }
    public string horizontalCamera { get => _horizontalCamera; }
    public string verticalCamera { get => _verticalCamera; }
    public string jumpInput { get => _jumpInput; }
    public string diveInput { get => _diveInput; }
    public string actionInput1 { get => _actionInput1; }
    public string actionInput2 { get => _actionInput2; }

    public bool IsActive()
    {
        if (new Vector2(Input.GetAxisRaw(horizontalMovement), Input.GetAxisRaw(verticalMovement)).magnitude > 0.15f)
            return true;
        if (new Vector2(Input.GetAxisRaw(horizontalCamera), Input.GetAxisRaw(verticalCamera)).magnitude > 0.15f)
            return true;
        if (Input.GetButton(jumpInput) || Input.GetButton(diveInput) || Input.GetButton(actionInput1) || Input.GetButton(actionInput2))
            return true;

        return false;
    }
}
