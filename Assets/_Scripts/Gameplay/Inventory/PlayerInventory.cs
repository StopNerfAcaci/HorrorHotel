using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Inventory
{
    [Serializable]
    public class InventoryEntry
    {
        public ItemSO item;
        public int quantity;
    }
    public class PlayerInventory
    {
        private List<InventoryEntry> entries;


        public PlayerInventory(List<InventoryEntry> entries)
        {
            this.entries = entries;
        }

        public event Action<ItemSO, int> OnItemAdded;

        public class Builder
        {
            private readonly PlayerInventory _inventory;

            public Builder()
            {
                _inventory = new PlayerInventory(new List<InventoryEntry>());
            }

            public Builder WithEntryItems(List<ItemSO> items)
            {
                items.AddRange(items);
                return this;
            }

            public PlayerInventory Build()
            {
                return _inventory;
            }
        }

        public void AddItem(ItemSO item, int quantity = 1)
        {
            if (item == null || quantity <= 0) return;

            if (item.itemType != Category.Key)
            {
                var existing = entries.Find(e => e.item == item);
                if (existing != null)
                {
                    existing.quantity = Mathf.Min(existing.quantity + quantity, item.maxStack);
                    OnItemAdded?.Invoke(item, quantity);
                    return;
                }
            }

            entries.Add(new InventoryEntry { item = item, quantity = quantity });
            OnItemAdded?.Invoke(item, quantity);

            Debug.Log($"[Inventory] Added {quantity}x {item.displayName}");
        }
    }
}