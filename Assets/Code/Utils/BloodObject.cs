using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodObject : MonoBehaviour, IPoolObject
{
    [SerializeField] float timeToStayAlive;
    public bool getIsAlive { get => timer < timeToStayAlive; }
    [SerializeField] ParticleSystem system;

    float timer;

    public void OnCreatedInPool()
    {
        timer = 0;
        gameObject.SetActive(false);
    }

    public void OnGettingFromPool()
    {
        timer = 0;
        gameObject.SetActive(true);
        system.Stop();
        system.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }


}
