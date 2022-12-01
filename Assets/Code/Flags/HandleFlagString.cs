using UnityEngine;
using UnityEngine.Events;

public class HandleFlagString : HandleFlagBase
{
    [SerializeField] UnityEvent<string> stringEvent;

    string oldValue = "";

    void Update()
    {
        string newValue = FlagHandler.Instance.GetFlagString(flagName);
        if(newValue != oldValue)
        {
            oldValue = newValue;
            stringEvent?.Invoke(newValue);
        }
    }

    public void SetFlag(string value)
    {
        FlagHandler.Instance.SetFlag(flagName, value);
    }
}
