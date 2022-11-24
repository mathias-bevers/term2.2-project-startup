using System;
using System.Linq;
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
			if (!handler.inventory.Any(pickupable => pickupable.GetType() == typeof(Key))) { return; }

			++keysUsed;

			if (keysUsed < keysNeeded) { return; }

			Open();
		}

		private void Open() { throw new NotImplementedException(); }
	}
}