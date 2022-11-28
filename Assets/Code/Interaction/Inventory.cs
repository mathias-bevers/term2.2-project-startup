using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Interaction
{
    public class Inventory
    {
        private List<InventoryItem> inventory = new();
        private string previousOutput = string.Empty;

        public void Add(Pickupable pickupable)
        {
            if (pickupable == null) { throw new NoNullAllowedException(); }

            inventory.Add(new InventoryItem(pickupable.name, pickupable.worldPrefab));
        }


        /// <summary>
        ///     Adds a <see cref="Pickupable" /> if the type does not exist in the inventory. Returns <see langword="true" />
        ///     if the <paramref name="pickupable" /> has been added, otherwise <see langword="false" /> is returned.
        /// </summary>
        /// <param name="pickupable"><see cref="Pickupable" /> to add to the inventory.</param>
        /// <returns>
        ///     <see langword="true" /> if the <see cref="Pickupable" /> is added successfully, <see langword="false" /> if it
        ///     has not been added.
        /// </returns>
        /// <exception cref="NoNullAllowedException" />
        public bool NoDuplicateAdd(Pickupable pickupable)
        {
            if (pickupable == null) { throw new NoNullAllowedException(); }

            if (HasType(pickupable)) { return false; }

            inventory.Add(new InventoryItem(pickupable.name, pickupable.worldPrefab));
            return true;
        }

        public void Remove(Pickupable pickupable)
        {
            if (pickupable == null) { throw new NoNullAllowedException(); }

            InventoryItem toRemove = inventory.Find(inventoryItem => inventoryItem.name == pickupable.name);
            inventory.Remove(toRemove);
        }

        /// <summary>
        ///     Removes the first <see cref="Pickupable" /> in the inventory collection of type <typeparamref name="T" />
        /// </summary>
        public InventoryItem Remove<T>() where T : Pickupable
        {
            InventoryItem toRemove = inventory.Find(inventoryItem => inventoryItem.name == GetPickupableName<T>());

            if (toRemove == null) { return null; }

            inventory.Remove(toRemove);
            return toRemove;
        }

        public bool HasType<T>() where T : Pickupable
        {
            return inventory.Any(inventoryItem => inventoryItem.name == GetPickupableName<T>());
        }

        public bool HasType<T>(T type) where T : Pickupable { return inventory.Any(inventoryItem => inventoryItem.name == type.name); }

        public void Drop<T>(Vector3 dropPoint) where T : Pickupable
        {
            InventoryItem toDrop = Remove<T>();

            if (toDrop == null) { return; }

            Object.Instantiate(toDrop.worldPrefab, dropPoint, Quaternion.identity);
            Debug.Log("Drop success");
        }

        private string GetPickupableName<T>() where T : Pickupable
        {
            PropertyInfo namePropertyInfo = typeof(T).GetProperty("name");

            if (namePropertyInfo == null) { throw new NullReferenceException("Could not find property \'name\'"); }

            if (namePropertyInfo.PropertyType != typeof(string))
            {
                throw new InvalidCastException($"Property \'name\' is not a string, but a {namePropertyInfo.PropertyType}");
            }


            T instance = Activator.CreateInstance<T>();
            string propertyValue = Convert.ToString(namePropertyInfo.GetValue(instance));

            return propertyValue;
        }
    }

    public class InventoryItem
    {
        public GameObject worldPrefab { get; }
        public string name { get; }

        public InventoryItem(string name, GameObject worldPrefab)
        {
            this.name = name;
            this.worldPrefab = worldPrefab;
        }
    }
}