using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUIHandler : MonoBehaviour
{
    [SerializeField] Survivor handleFor;

    [SerializeField] UIFillerBar barToFill;


    private void LateUpdate()
    {
        if (handleFor == null || barToFill == null) return;
        barToFill.SetFillAmount(handleFor.getOxygen.getCurrentOxygen, 0, handleFor.getOxygen.getMaxOxygen, handleFor.getOxygen.bigDepletion);
    }

}
