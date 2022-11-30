using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedHandler : MonoBehaviour
{

    [SerializeField] GameObject bloodPrefab;
    [SerializeField] float bloodDelayTime = 1.0f;

    float timer = 0;
    

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= bloodDelayTime)
        {
            timer -= bloodDelayTime;
            GameObject obj = Instantiate(bloodPrefab);
            obj.transform.position = transform.position;
            Destroy(obj, 4.5f);
        }
    }
}
