using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisownChildren : MonoBehaviour
{
    
    void Start()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).SetParent(null);
        }
    }
}
