using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public enum TypeOfInput
{
    None,
    Default,
    Controler
}

public class ControllerHandler : Singleton<ControllerHandler>
{
    [SerializeField]
    List<InputSettings> inputSettings;
    [SerializeField]
    List<TypeOfInput> typeOfInputs;
    static bool _lastMouseVisible = false;
    public static bool mouseVisible = true;
    public static TypeOfInput inputMode { get; private set; } = TypeOfInput.Default;
    TypeOfInput lastInputType { get; set; } = TypeOfInput.None;
    int lastCount = -1;
    public UnityEventInputType callBackOnInputTypeChange = new UnityEventInputType();

    InputSettings _activeSettings;
    public InputSettings getActiveSettings { get => _activeSettings; }

    [SerializeField] PersonalCursorUI cursor;


    private void Update()
    { 
        //I know, I know, but this is crucial :pensive:
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

        for(int i = 0; i < inputSettings.Count; i++)
        {
            if (inputSettings[i].IsActive()) { 
                inputMode = typeOfInputs[i];
                _activeSettings = inputSettings[i];
            }
        }

        if(lastInputType != inputMode || lastCount != eventSystems.Length || _lastMouseVisible != mouseVisible)
        {
            _lastMouseVisible = mouseVisible;
            lastCount = eventSystems.Length;
            lastInputType = inputMode;
            if (inputMode == TypeOfInput.Controler) {
                
                if(cursor != null)
                    cursor.gameObject.SetActive( mouseVisible && eventSystems.Length == 0);
                Cursor.visible = false; 
            }
            else
            {
                if (cursor != null)
                {
                    cursor.gameObject.SetActive(false);       
                }
                Cursor.visible = mouseVisible;
                if (!mouseVisible) Cursor.lockState = CursorLockMode.Locked;
                else Cursor.lockState = CursorLockMode.None;
            }
            callBackOnInputTypeChange?.Invoke(inputMode);
        }
    }

    public static Vector2 mousePosition {
        get {
            ControllerHandler self = Instance;
            if (!self.cursor.isActiveAndEnabled) return Input.mousePosition;
            else return self.cursor.getCursorPos;
        } }
}

[System.Serializable]
public class UnityEventInputType : UnityEvent<TypeOfInput>
{

}
