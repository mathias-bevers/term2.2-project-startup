using System;
using Code.Interaction;
using UnityEngine;

namespace Code
{
    public class Key : Pickupable
    {
        protected override void Pickup(InteractionHandler handler)
        {
            if (!handler.inventory.NoDuplicateAdd(this)) { return; }

            gameObject.SetActive(false);
		}
    }
}