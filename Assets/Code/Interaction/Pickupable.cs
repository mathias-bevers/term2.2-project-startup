using UnityEngine;

namespace Code.Interaction
{
    public abstract class Pickupable : MonoBehaviour, IInteractable
    {
        public void Interact(InteractionHandler handler) { Pickup(handler); }

        public void OnHover(InteractionHandler handler)
        {
            Debug.Log($"Hovering {name}");
            //TODO: implement
        }

        protected virtual void Pickup(InteractionHandler handler)
        {
            handler.inventory.Add(this);
            Destroy(gameObject);
        }
    }
}