using UnityEngine;


    [CreateAssetMenu(menuName = "Inventory/Item", fileName = "NewItemSO")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] int itemId;
        public int ItemId => itemId;
        public string displayName;
        public Category itemType = Category.Consumable;
        [TextArea] public string description;
        public Sprite icon;
    }