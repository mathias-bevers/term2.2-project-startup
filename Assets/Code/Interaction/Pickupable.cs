using System;
using UnityEngine;

namespace Code.Interaction
{
    public abstract class Pickupable : MonoBehaviour, IInteractable
    {
        public void Interact(InteractionHandler handler) { Pickup(handler); }

        public void OnHover(InteractionHandler handler)
        {
            OnHover[] onHovers = FindObjectsOfType<OnHover>(true);
            foreach(OnHover onHover in onHovers)
                onHover.gameObject.SetActive(true);
        }
        protected virtual void Pickup(InteractionHandler handler) { }
    }
}