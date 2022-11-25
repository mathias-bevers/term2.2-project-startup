using System;
using Code.Interaction;
using UnityEngine;

namespace Code
{
	public class TreasureChest : MonoBehaviour, IInteractable
	{
		[SerializeField] private int keysNeeded = 3;

		private int keysUsed = 0;

		public void Interact(InteractionHandler handler)
		{
			if (!handler.hasKey)
			{
				Debug.LogWarning($"{handler.gameObject.name} doesn't have a key to open this chest.");
				return;
			}

			++keysUsed;
			handler.inventory.Remove(handler.inventory.Find(pickupable => pickupable.GetType() == typeof(Key)));

			if (keysUsed < keysNeeded) { return; }

			Open();
		}

		private void Open() { throw new NotImplementedException(); }
	}
}