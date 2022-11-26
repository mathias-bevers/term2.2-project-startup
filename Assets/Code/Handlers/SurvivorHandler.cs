using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorHandler : InstancedSingleton<SurvivorHandler>
{

    [ReadOnly]
    [SerializeField]
    List<Survivor> registeredSurvivors = new List<Survivor>();

    public void RegisterSurvivor(Survivor survivor)
    {
        if (!registeredSurvivors.Contains(survivor))
        {
            registeredSurvivors.Add(survivor);
            TargetDisplayHandler.instance.RegisterSurvivor(survivor);
        }
    }

    public void DeregisterSurvivor(Survivor survivor) 
    {
        registeredSurvivors.Remove(survivor);
        TargetDisplayHandler.instance.DeregisterSurvivor(survivor);
    }
}
