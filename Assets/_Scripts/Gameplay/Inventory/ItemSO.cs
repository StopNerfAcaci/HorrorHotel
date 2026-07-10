using UnityEngine;

namespace Gameplay.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Item", fileName = "NewItemSO")]
    public class ItemSO : ScriptableObject
    {
        public string itemId;
        public string displayName;
        public Category itemType = Category.Consumable;
        [TextArea] public string description;
        public Sprite icon;
        public int maxStack = 99;
    }
}