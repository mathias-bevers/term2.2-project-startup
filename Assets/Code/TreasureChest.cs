using System;
using Code.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Code
{
	public class TreasureChest : MonoBehaviour, IInteractable
	{
		[SerializeField, PlusMinus] private int keysNeeded = 3;

		private int keysUsed = 0;

		[SerializeField] UnityEvent onCompletion;
		[SerializeField] UnityEvent<int> onAddKey;

		public void Interact(InteractionHandler handler)
		{
			Debug.Log("Interract");
            if (!handler.inventory.HasType<Key>())
            {
				Debug.LogWarning($"{handler.gameObject.name} doesn't have a key to open this chest.");
				return;
            }

            ++keysUsed;
			handler.inventory.Remove<Key>();
			onAddKey?.Invoke(keysUsed);

			if (keysUsed < keysNeeded) { return; }

			Open();
			onCompletion?.Invoke();
		}

        public void OnHover(InteractionHandler handler)
        {
            //TODO: implement
        }

		private void Open() { throw new NotImplementedException(); }
	}
}