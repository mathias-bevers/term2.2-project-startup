using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Survivor))]
public class Oxygen : MonoBehaviour
{

    Survivor survivor;

    [SerializeField] float maxOxygen;
    [SerializeField] float currentOxygen;
    [SerializeField] float depleteOxygenPerSecond;

    public float getMaxOxygen { get => maxOxygen; }
    public float getCurrentOxygen { get => currentOxygen; }
    public float getDepleteOxygenPerSecond { get => depleteOxygenPerSecond; }


    bool _bigDepletion;
    public bool bigDepletion { get => _bigDepletion; }

    private void Start()
    {
        survivor = GetComponent<Survivor>();
        currentOxygen = maxOxygen;
    }

    private void Update()
    {
        if (survivor.getTransform.position.y > WaterHandler.Instance.waterLevel - survivor.hoverUnderWater - 0.5f) return;
        currentOxygen -= depleteOxygenPerSecond * Time.deltaTime;
        if (currentOxygen < 0) survivor.KillEntity();
    }

    public void RemoveChunk(float amount)
    {
        currentOxygen -= amount;
        _bigDepletion = true;
    }

    private void LateUpdate()
    {
        _bigDepletion = false;
    }
}
