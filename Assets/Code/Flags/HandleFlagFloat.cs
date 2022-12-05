using System;
using UnityEngine;
using UnityEngine.Events;



public class HandleFlagFloat : HandleFlagBase
{
    [SerializeField] private float defaultValue;
    [SerializeField] UnityEvent<float> floatEvent;

    float oldValue = 0;

    void Update()
    {
        float newValue = FlagHandler.Instance.GetFlagFloat(flagName) ?? defaultValue;

        if (newValue != oldValue)
        {
            oldValue = newValue;
            floatEvent?.Invoke(newValue);
        }
    }

#pragma warning disable IDE0049
//Reason:
//Single is a MUST HAVE in this case
//because unity cannot track it otherwise
    public void SetFlag(Single value)
    {
        FlagHandler.Instance.SetFlag(flagName, value, writable);
    }
}
