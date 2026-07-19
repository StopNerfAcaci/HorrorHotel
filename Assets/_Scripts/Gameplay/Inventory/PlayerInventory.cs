using System;
using System.Collections.Generic;
using GlobalSettings;
using UnityEngine;

namespace Gameplay.Inventory
{
    public class PlayerInventory
    {
        private List<ItemSO> itemList;
        
        public PlayerInventory(List<ItemSO> itemList)
        {
            this.itemList = itemList;
        }
        
        public class Builder
        {
            private readonly PlayerInventory _inventory;

            public Builder()
            {
                _inventory = new PlayerInventory(new List<ItemSO>());
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

        public void AddItem(ItemSO item)
        {
            if (item == null) return;
            itemList.Add(item);
            Debug.Log($"[Inventory] Added {item.displayName}");
        }

        public void RemoveItem(ItemSO item)
        {
            if (item == null) return;
            itemList.Remove(item);
        }
        public void Clear() =>  itemList.Clear();
    }
}