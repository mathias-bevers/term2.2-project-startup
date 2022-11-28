using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Code.Interaction
{
    public class Inventory
    {
        public List<Pickupable> inventory;

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
        public void Remove<T>() where T : Pickupable
        {
            Pickupable toRemove = inventory.Find(item => item.GetType() == typeof(T));

            if (toRemove == null) { return; }

            inventory.Remove(toRemove);
        }

        public bool HasType<T>() where T : Pickupable { return inventory.Any(pickupable => pickupable.GetType() == typeof(T)); }

        public bool HasType<T>(T type) where T : Pickupable
        {
            return inventory.Any(pickupable => pickupable.GetType() == type.GetType());
        }
    }
}