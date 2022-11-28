using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorHandler : MonoBehaviour
{

    [ReadOnly]
    [SerializeField]
    List<Survivor> registeredSurvivors = new List<Survivor>();

    public void RegisterSurvivor(Survivor survivor)
    {
        if (!registeredSurvivors.Contains(survivor))
        {
            registeredSurvivors.Add(survivor);
            FindObjectOfType<TargetDisplayHandler>()?.RegisterSurvivor(survivor);
        }
    }

    public void DeregisterSurvivor(Survivor survivor) 
    {
        registeredSurvivors.Remove(survivor);
        FindObjectOfType<TargetDisplayHandler>()?.DeregisterSurvivor(survivor);
    }
}
