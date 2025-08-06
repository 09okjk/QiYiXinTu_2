using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager:MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        public Dictionary<string, ItemData> Inventory = new Dictionary<string, ItemData>();
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
        
        
    }
}