using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayCamerasHandler : Singleton<OverlayCamerasHandler>
{
    [SerializeField] List<OverlayCameraObject> overlayObjects = new List<OverlayCameraObject>();

    public Camera GetOverlayCamera(string name)
    {
        for (int i = 0; i < overlayObjects.Count; i++)
        {
            if (overlayObjects[i].name == name)
                return overlayObjects[i].overlayCamera;
        }

        return null;
    }
}
