using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedHandler : MonoBehaviour
{

    [SerializeField] BloodObject bloodPrefab;
    [SerializeField] float bloodDelayTime = 1.0f;
    Pool<BloodObject> bloodPool;

    float timer = 0;
    
    List<BloodObject> bloodObjects = new List<BloodObject>();

    private void Start()
    {
        bloodPool = Pool.Create(bloodPrefab, 20);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= bloodDelayTime)
        {
            timer -= bloodDelayTime;
            BloodObject obj = bloodPool.Get();
            obj.transform.position = transform.position;
            bloodObjects.Add(obj);
        }

        for(int i = bloodObjects.Count - 1; i >= 0; i--)
        {
            if (!bloodObjects[i].getIsAlive)
            {
                bloodPool.Take(bloodObjects[i]);
                bloodObjects.RemoveAt(i);
            }
        }
    }
}
