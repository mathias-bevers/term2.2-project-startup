using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class PerkHandler : MonoBehaviour
{
    Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        
    }
}
