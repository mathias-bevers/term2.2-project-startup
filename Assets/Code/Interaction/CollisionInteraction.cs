using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum CallbackType
{
    Enter,
    Stay,
    Leave
}

[RequireComponent(typeof(Collider))]
public class CollisionInteraction : MonoBehaviour
{

    List<ICollisionInteraction> interactions = new List<ICollisionInteraction>();

    public void RegisterCallback(ICollisionInteraction interaction)
    {
        if (!interactions.Contains(interaction)) interactions.Add(interaction);
    }

    public void DeregisterCallback(ICollisionInteraction interaction)
    {
        if (interactions.Contains(interaction)) interactions.Remove(interaction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Handle(collision, CallbackType.Enter);
    }

    private void OnCollisionExit(Collision collision)
    {
        Handle(collision, CallbackType.Leave);
    }

    private void OnCollisionStay(Collision collision)
    {
        Handle(collision, CallbackType.Stay);
    }

    private void OnTriggerEnter(Collider other)
    {
        Handle(other, CallbackType.Enter);
    }

    private void OnTriggerStay(Collider other)
    {
        Handle(other, CallbackType.Stay);
    }

    private void OnTriggerExit(Collider other)
    {
        Handle(other, CallbackType.Leave);
    }

    void Handle(Collider collider, CallbackType type)
    {
        foreach (ICollisionInteraction interaction in interactions)
        {
            if (!SatisfiesConditions(collider.gameObject, interaction)) continue;
            if (CallbackType.Enter == type) interaction.OnTriggerEnter(collider);
            if (CallbackType.Stay == type) interaction.OnTriggerStay(collider);
            if (CallbackType.Leave == type) interaction.OnTriggerExit(collider);
        }
    }

    void Handle(Collision collision, CallbackType type)
    {
        foreach (ICollisionInteraction interaction in interactions)
        {
            if (!SatisfiesConditions(collision.gameObject, interaction)) continue;
            if (CallbackType.Enter == type) interaction.OnCollisionEnter(collision);
            if (CallbackType.Stay == type) interaction.OnCollisionStay(collision);
            if (CallbackType.Leave == type) interaction.OnCollisionExit(collision);
        }
    }

    bool LayerCheck(LayerMask interactionLayers, int layer)
    {
        return (interactionLayers.value & (1 << layer)) > 0;
    }

    bool SatisfiesConditions(GameObject _gameObject, ICollisionInteraction interaction)
    {
        if (!LayerCheck(interaction.collidesWithLayers, _gameObject.layer)) return false;

        return true;
    }
}
