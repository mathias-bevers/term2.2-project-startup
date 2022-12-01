using UnityEngine;
using UnityEngine.Events;

public class HandleFlagInt : HandleFlagBase
{
    [SerializeField] UnityEvent<int> intEvent;

    int oldValue = 0;

    void Update()
    {
        int newValue = FlagHandler.Instance.GetFlagInt(flagName);
        if (newValue != oldValue)
        {
            oldValue = newValue;
            intEvent?.Invoke(newValue);
        }
    }

    public void SetFlag(int value)
    {
        FlagHandler.Instance.SetFlag(flagName, value);
    }
}
