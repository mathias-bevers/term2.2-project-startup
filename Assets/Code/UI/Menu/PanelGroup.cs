using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelGroup : MonoBehaviour
{
    [SerializeField] bool alwaysSelectFirst = false;
    public PanelGroup backwardsPanel;
    //[SerializeField] bool disableOthers = true;
    [HideInInspector]
    public GameObject currentActive = null;

    [SerializeField] GameObject standardActive;

    PanelGroupHandler handler;

    /*public bool getDisableOthers
    {
        get => disableOthers;
    }*/
    bool hasRegistered = false;

    public void Register(PanelGroupHandler panelGroupHandler)
    {
        handler = panelGroupHandler;
        if (!hasRegistered)
        {
            Slider[] buttonsInChild = GetComponentsInChildren<Slider>(true);
            foreach (Slider b in buttonsInChild)
                b.onValueChanged.AddListener((f) => handler.ChangedObject());

            Toggle[] buttonsInChild2 = GetComponentsInChildren<Toggle>(true);
            foreach (Toggle b in buttonsInChild2)
                b.onValueChanged.AddListener((f) => handler.ChangedObject());

            Button[] buttonsInChild3 = GetComponentsInChildren<Button>(true);
            foreach (Button b in buttonsInChild3)
                b.onClick.AddListener(() => handler.ChangedObject());

            hasRegistered = true;
        }
    }

    public void SetActive(bool active)
    {
        if (handler == null) return;
        EventSystem cashedEventSystem = handler.getEventSystem;
        if (cashedEventSystem == null) return;

        if (active) HandleActive(cashedEventSystem);
        else HandleDeactive(cashedEventSystem);
    }

    void HandleActive(EventSystem cashedEventSystem)
    {
        ControllerHandler.Instance?.callBackOnInputTypeChange?.RemoveListener(OnCallbackChange);
        ControllerHandler.Instance?.callBackOnInputTypeChange?.AddListener(OnCallbackChange);
        if (ControllerHandler.inputMode != TypeOfInput.Controler) return;

        if (cashedEventSystem.firstSelectedGameObject == null)
            cashedEventSystem.firstSelectedGameObject = standardActive;
        if (alwaysSelectFirst)
        {
            cashedEventSystem.SetSelectedGameObject(standardActive);
        }
        else
        {
            if (currentActive != null) cashedEventSystem.SetSelectedGameObject(currentActive);
            else cashedEventSystem.SetSelectedGameObject(standardActive);
        }
    }

    void HandleDeactive(EventSystem cashedEventSystem)
    {
        ControllerHandler.Instance?.callBackOnInputTypeChange?.RemoveListener(OnCallbackChange);

        Transform cashedTransform = transform;
        GameObject cashedSelected = cashedEventSystem.currentSelectedGameObject;

        for (int i = 0; i < cashedTransform.childCount; i++)
            if (cashedTransform.GetChild(i).gameObject == cashedSelected)
                currentActive = cashedSelected;
    }

    void OnCallbackChange(TypeOfInput type)
    {
        if (handler == null) return;
        EventSystem cashedSystem = handler.getEventSystem;
        if (cashedSystem == null) return;
        if (type != TypeOfInput.Controler)
        {
            handler.getEventSystem.SetSelectedGameObject(null);
            return;
        }
        if (cashedSystem.currentSelectedGameObject != null) return;
        SetActive(true);

    }
}
