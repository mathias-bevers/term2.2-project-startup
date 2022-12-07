using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetPos : MonoBehaviour
{
    private void Update()
    {
        Random.InitState(Random.Range(0, 10000000));
        transform.position = new Vector3(Random.Range(-400, 400), Random.Range(0, WaterHandler.Instance.waterLevel), Random.Range(-400, 400));
    }
}
