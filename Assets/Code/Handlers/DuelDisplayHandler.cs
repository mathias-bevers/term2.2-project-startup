using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelDisplayHandler : MonoBehaviour
{
   public void OnGetValue(bool value)
    {
        if (value) return;

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
    }
}
