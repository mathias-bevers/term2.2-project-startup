using UnityEngine;
using UnityEngine.Events;

public class HandleFlagBool : HandleFlagBase
{
    [SerializeField] UnityEvent<bool> boolEvent;

    bool oldValue = false;

    void Update()
    {
        bool newValue = FlagHandler.Instance.GetFlagBool(flagName);
        if (newValue != oldValue)
        {
            oldValue = newValue;
            boolEvent?.Invoke(newValue);
        }
    }

    public void SetFlag(bool value)
    {
        FlagHandler.Instance.SetFlag(flagName, value);
    }
}
