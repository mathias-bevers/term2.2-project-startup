using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Code.Interaction
{
    public class Inventory
    {
        private List<Pickupable> inventory = new();
        private string previousOutput = string.Empty;

        public void Add(Pickupable item)
        {
            if (item == null) { throw new NoNullAllowedException(); }

            inventory.Add(item);
        }


        /// <summary>
        ///     Adds a <see cref="Pickupable" /> if the type does not exist in the inventory. Returns <see langword="true" />
        ///     if the <paramref name="item" /> has been added, otherwise <see langword="false" /> is returned.
        /// </summary>
        /// <param name="item"><see cref="Pickupable" /> to add to the inventory.</param>
        /// <returns>
        ///     <see langword="true" /> if the <see cref="Pickupable" /> is added successfully, <see langword="false" /> if it
        ///     has not been added.
        /// </returns>
        /// <exception cref="NoNullAllowedException" />
        public bool NoDuplicateAdd(Pickupable item)
        {
            if (item == null) { throw new NoNullAllowedException(); }

            if (HasType(item)) { return false; }

            inventory.Add(item);
            return true;
        }

        public void Remove(Pickupable item)
        {
            if (item == null) { throw new NoNullAllowedException(); }

            inventory.Remove(item);
        }

        /// <summary>
        ///     Removes the first <see cref="Pickupable" /> in the inventory collection of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">Type of <see cref="Pickupable" /> to remove</typeparam>
        public Pickupable Remove<T>() where T : Pickupable
        {
            Pickupable toRemove = inventory.Find(item => item.GetType() == typeof(T));

            if (toRemove == null) { return null; }

            inventory.Remove(toRemove);
            return toRemove;
        }

        public bool HasType<T>() where T : Pickupable { return inventory.Any(pickupable => pickupable.GetType() == typeof(T)); }

        public bool HasType<T>(T type) where T : Pickupable
        {
            if (inventory == null) { Debug.Break(); }

            return inventory.Any(pickupable => pickupable.GetType() == type.GetType());
        }

        public void Drop<T>(Vector3 dropPoint) where T : Pickupable
        {
            Pickupable toDrop = Remove<T>();

            if (toDrop == null) { return; }

            toDrop.transform.position = dropPoint;
            toDrop.gameObject.SetActive(true);
        }
    }
}