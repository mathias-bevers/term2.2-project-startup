using System;
using Code.Interaction;
using UnityEngine;

namespace Code
{
	public class TreasureChest : MonoBehaviour, IInteractable
	{
		[SerializeField, PlusMinus] private int keysNeeded = 3;

		private int keysUsed = 0;

		public void Interact(InteractionHandler handler)
		{
            if (handler.inventory.HasType<Key>())
            {
				Debug.LogWarning($"{handler.gameObject.name} doesn't have a key to open this chest.");
				return;
            }

            ++keysUsed;
			handler.inventory.Remove<Pickupable>();

			if (keysUsed < keysNeeded) { return; }

			Open();
		}

        public void OnHover(InteractionHandler handler)
        {
            //TODO: implement
        }

		private void Open() { throw new NotImplementedException(); }
	}
}