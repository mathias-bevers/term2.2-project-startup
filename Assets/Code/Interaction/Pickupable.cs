using System;
using UnityEngine;

namespace Code.Interaction
{
    public abstract class Pickupable : MonoBehaviour, IInteractable
    {
        public GameObject worldPrefab;

        public void Interact(InteractionHandler handler) { Pickup(handler); }

        public void OnHover(InteractionHandler handler)
        {
            Debug.Log($"Hovering {name}");
            //TODO: implement
        }

        protected virtual void Awake()
        {
            if (worldPrefab == null) { throw new NullReferenceException("worldPrefab variable should be set in the inspector."); }
        }

        protected virtual void Pickup(InteractionHandler handler) { }
    }
}