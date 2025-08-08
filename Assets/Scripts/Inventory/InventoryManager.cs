using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager:MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        public Dictionary<string, int> PlayerInventory { get; private set; } = new Dictionary<string, int>();
        private Dictionary<string, ItemData> Inventory = new Dictionary<string, ItemData>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            var itemDatas = Resources.LoadAll<ItemData>("ScriptableObjects/Items");
            foreach (var itemData in itemDatas)
            {
                if (!Inventory.ContainsKey(itemData.itemID))
                {
                    Inventory[itemData.itemID] = itemData;
                }
            }
        }
        
        public void SetPlayerInventory(Dictionary<string, int> inventory)
        {
            PlayerInventory = inventory;
        }
        
        public Dictionary<string, int> GetPlayerInventory()
        {
            return PlayerInventory;
        }
        
        public ItemData GetItemData(string itemID)
        {
            if (Inventory.TryGetValue(itemID, out var itemData))
            {
                return itemData;
            }
            LoggerManager.Instance.LogWarning($"Item with ID {itemID} not found in inventory.");
            return null;
        }
        
        public void AddItem(string itemID, int quantity = 1)
        {
            if (Inventory.TryGetValue(itemID, out var itemData))
            {
                if (PlayerInventory.ContainsKey(itemID))
                {
                    PlayerInventory[itemID] += quantity;
                }
                else
                {
                    PlayerInventory[itemID] = quantity;
                }
                LoggerManager.Instance.Log($"Added {quantity} of {itemData.itemName} to inventory.");
            }
            else
            {
                LoggerManager.Instance.LogWarning($"Item with ID {itemID} not found in inventory.");
            }
        }
        
    }
}