using System.Linq;
using Code.Interaction;
using UnityEngine;

namespace Code
{
	public class Key : Pickupable
	{
		protected override void Pickup(InteractionHandler handler)
		{
			if (handler.inventory.Any(pickupable => pickupable.GetType() == typeof(Key)))
			{
				Debug.LogWarning($"{handler.gameObject.name} already has a key in its inventory");
				return;
			}

			base.Pickup(handler);
		}
	}
}