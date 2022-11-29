using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PanelGroupHandler : MonoBehaviour
{
    [SerializeField] public UnityEvent onChangeObject;
    [SerializeField] EventSystem unityEventSystem;
    [SerializeField] List<PanelGroup> panelGroups = new List<PanelGroup>();
    [SerializeField] string backButton = "CancelJoy";

    internal PanelGroup currentActive = null;

    public EventSystem getEventSystem
    {
        get => unityEventSystem;
    }

    private void Start()
    {
        PanelGroup[] panels = GetComponentsInChildren<PanelGroup>();
        for(int i = 0; i < panels.Length; i++)
            if (!panelGroups.Contains(panels[i]))
                panelGroups.Add(panels[i]);
        
        SetActive(0);
    }

    private void Update()
    {
        UpdateButtonMappings();
        if (Input.GetButtonDown(backButton)) OnBack();
        
    }

    public virtual void OnBack()
    {
        if (currentActive == null) return;
        if (currentActive.backwardsPanel == null) return;
        SetActive(currentActive.backwardsPanel);
        ChangedObject();
    }
    public void ChangedObject()
    {
        onChangeObject?.Invoke();
    }


    void UpdateButtonMappings()
    {
        StandaloneInputModule module = ((StandaloneInputModule)getEventSystem.currentInputModule);
        InputSettings settings = ControllerHandler.Instance.getActiveSettings;
        if (settings == null) return;
        module.horizontalAxis = settings.axisHorizontal;
        module.verticalAxis = settings.axisVertical;
    }

    public void RegisterFocus()
    {
        if (getEventSystem == null) return;
        if (getEventSystem.currentSelectedGameObject == null)
        {
            if (currentActive == null) return;
            getEventSystem.SetSelectedGameObject(currentActive.currentActive);
            
        }
    }

    public void SetActive(PanelGroup panelGroup)
    {
        for (int i = 0; i < panelGroups.Count; i++)
        {
            if (panelGroups[i] == panelGroup)
            {
                SetActive(i);
                return;
            }
        }
    }

    public void SetActive(int panelGroup)
    {
        if (panelGroups.Count <= 0) return;
        if (panelGroup >= panelGroups.Count) return;

        if (currentActive != null) currentActive.SetActive(false);

        for (int i = 0; i < panelGroups.Count; i++)
        {
            panelGroups[i].Register(this);
            panelGroups[i].gameObject.SetActive(i == panelGroup);
        }

        currentActive = panelGroups[panelGroup];
        if (currentActive == null) return;
        currentActive.SetActive(true);
    }
}
