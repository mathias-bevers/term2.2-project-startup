using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OverlayCameraObject : MonoBehaviour
{
    Camera _overlayCamera;

    public Camera overlayCamera { get => _overlayCamera; }

    [SerializeField] string _overlayObjectName = "CHANGE ME!";
    public string overlayObjectName { get { return _overlayObjectName; } }
}
