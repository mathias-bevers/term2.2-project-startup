using UnityEngine;

namespace Code.Interaction
{
	public abstract class Pickupable : MonoBehaviour, IInteractable
	{
		public void Interact(InteractionHandler handler)
		{
			Pickup(handler);
		}

		protected virtual void Pickup(InteractionHandler handler)
		{
			handler.inventory.Add(this);
			Destroy(gameObject);
		}
	}
}