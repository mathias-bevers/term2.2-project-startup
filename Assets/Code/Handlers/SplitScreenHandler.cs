using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SplitScreenHandler : MonoBehaviour
{
    [SerializeField] CameraRig[] cameraRigs;
    [SerializeField] Canvas[] canvasses;
    [SerializeField] RawImage[] topImages;
    [SerializeField] RawImage[] bottomImages;

    RenderTexture[] renderTextures = new RenderTexture[2];

    private void Awake()
    {
        SetActive(ref canvasses, false);
        renderTextures = new RenderTexture[2];
        for (int i = 0; i < renderTextures.Length; i++)
        {
            renderTextures[i] = new RenderTexture(Screen.width, Screen.height, 32, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
            renderTextures[i].Create();
        } 
    }

    public void SetSplitScreen(bool splitScreen)
    {
        if (!splitScreen)
        {
            SetActive(ref canvasses, false);
            Loop(ref cameraRigs, (i, v) => { v.getCamera.targetTexture = null; v.getCamera.rect = new Rect(0, 0, 1, 1); });
            return;
        }
        else
        {
            SetActive(ref canvasses, true);
            Loop(ref cameraRigs, (i, v) => { v.getCamera.targetTexture = renderTextures[i]; v.getCamera.rect = new Rect(0, 0.5f * (i == 0 ? 1 : -1), 1, 1); });
            Loop(ref topImages, (i, v) => { v.texture = renderTextures[0]; });
            Loop(ref bottomImages, (i, v) => { v.texture = renderTextures[1]; });
        }
    }

    void SetActive<T>(ref T[] array, bool active) where T : Behaviour
    {
        Loop(ref array, (i, v) => v.enabled = active);
    }



    void Loop<T>(ref T[] array, Action<int, T> callback) where T : Behaviour
    {
        for(int i = 0; i < array.Length; i++)
            callback(i, array[i]);
    }
}
