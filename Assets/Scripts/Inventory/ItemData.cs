using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemID; 
        public string itemName; 
        public string description; 
        public ItemType itemType; 
        public Sprite icon;
    }

    public enum ItemType
    {
        QuestItem, 
        PuzzleItem, 
        Consumable, 
    }
}