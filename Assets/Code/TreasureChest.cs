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
			OnHover[] hovers = FindObjectsOfType<OnHover>(true);
			foreach (OnHover hover in hovers) {
				hover.gameObject.SetActive(true);
					}
        }

		private void Open() { onCompletion?.Invoke(); }
	}
}