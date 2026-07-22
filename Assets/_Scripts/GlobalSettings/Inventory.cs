using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalSettings
{
    
    [CreateAssetMenu(fileName = "Inventory", menuName = "GlobalSettings/Inventory")]
    public class Inventory : GlobalSettingsBase<Inventory>
    {
        [Serializable]
        public struct KeyItemEntry
        {
            public ItemSO item;
            public bool startsCollected;
        }
        
        [SerializeField] private KeyItemEntry[] keyItems;
        private Dictionary<ItemSO, bool> itemFlagDict = new();

        private const string KeyPrefix = "KeyItem_";

        public static Inventory Get()
        {
            return GlobalSettingsBase<Inventory>.Get("Inventory");
        }
        
        public void LoadInventory()
        {
            itemFlagDict.Clear();
            foreach (var entry in keyItems)
            {
                if (entry.item == null) continue;

                bool defaultValue = entry.startsCollected;
                bool collected = PlayerPrefs.GetInt(KeyPrefix + entry.item.ItemId, defaultValue ? 1 : 0) == 1;
                itemFlagDict[entry.item] = collected;
            }
            Debug.Log("Inventory loaded");
        }
        
        [RuntimeInitializeOnLoadMethod]
        void PreLoad()
        {
            GlobalSettingsBase<Inventory>.StartPreloadAddressable("Inventory");
        }
        public static void UnLoad()
        {
            GlobalSettingsBase<Inventory>.StartUnload();
        }
        public void AddItem(ItemSO item)
        {
            if (itemFlagDict.ContainsKey(item))
            {
                itemFlagDict[item] = true;
                PlayerPrefs.SetInt(KeyPrefix + item.ItemId, 1);
                PlayerPrefs.Save();
            }
        }

        public bool HasAllKeyItems()
        {
            foreach (var kvp in itemFlagDict)
            {
                if (!kvp.Value) return false;
            }
            return true;
        }
        [ContextMenu("Clear All KeyItems")]
        public void ResetInventory()
        {
            foreach (var entry in keyItems)
            {
                if (entry.item == null || entry.startsCollected) continue;
                PlayerPrefs.DeleteKey(KeyPrefix + entry.item.ItemId);
            }
        }

        public bool CheckHasKey(ItemSO item)
        {
            return itemFlagDict.ContainsKey(item) && itemFlagDict[item];
        }
    }
    
}