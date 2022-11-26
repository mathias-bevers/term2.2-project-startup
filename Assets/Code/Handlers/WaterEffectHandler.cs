using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class WaterEffectHandler : MonoBehaviour
{
    Camera cam;

    bool inWater = false;
    bool lastInWater = false;

    private void Start()
    {

        cam = GetComponent<Camera>();
        inWater = cam.transform.position.y < WaterHandler.Instance.transform.position.y;
        lastInWater = !inWater;

    }

    void Update()
    {
        if (cam == null) return;

        SetInWater();
    }

    void SetInWater()
    {
        inWater = cam.transform.position.y < WaterHandler.Instance.transform.position.y;
        if (lastInWater != inWater)
        {
            WaterToggleCallback();
            lastInWater = inWater;
        }
    }

    void WaterToggleCallback()
    {
        UniversalAdditionalCameraData data = cam.GetUniversalAdditionalCameraData();
    }
}
