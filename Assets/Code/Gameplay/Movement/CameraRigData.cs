using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigData : MonoBehaviour
{

    [SerializeField] Camera _camera;
    public new Camera camera { get { return _camera; } }
    [SerializeField] Transform _cameraHolder;
    public Transform cameraHolder { get { return _cameraHolder; } }

    bool _camIsNull = false;
    public bool hasCamera { get { return !_camIsNull; } }

    [SerializeField]
    Transform _farPoint;

    public Transform farPoint { get { return _farPoint; } }

    private void Update()
    {
        _camIsNull = _camera == null;
    }
}
