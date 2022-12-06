using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameAnimation : MonoBehaviour
{
    [SerializeField] Canvas[] canvasses;

    void Start()
    {
        for(int i = 0; i < canvasses.Length; i++)
        {
            canvasses[i].enabled = false;
        }
    }

    void Update()
    {
        
    }
}
