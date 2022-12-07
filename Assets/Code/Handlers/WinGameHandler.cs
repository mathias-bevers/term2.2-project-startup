using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameHandler : MonoBehaviour
{
    [SerializeField] Killer killer;
    [SerializeField] Survivor survivor;

    void Update()
    {
        if (survivor == null) return;
        if (!survivor.hasDied) return;


    }
}
