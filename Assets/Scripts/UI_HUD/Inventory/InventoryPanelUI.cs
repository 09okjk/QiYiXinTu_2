using System;
using System.Collections.Generic;
using Inventory;
using Tools;
using UnityEngine;

namespace UI_HUD.Inventory
{
    public class InventoryPanelUI: PanelUI
    {
        public GameObject ItemContainer; // 物品容器
        public GameObject itemPrefab; // 物品预制件
        
        private ObjectPool<ItemSlotUI> _itemPool; // 物品UI对象池
        private List<ItemSlotUI> _activeItems = new List<ItemSlotUI>(); // 当前激活的物品UI列表

        private void Awake()
        {
            var itemUIComponent = itemPrefab.GetComponent<ItemSlotUI>();
            _itemPool = new ObjectPool<ItemSlotUI>(itemUIComponent, ItemContainer.transform, 10);
        }

        public override void Show()
        {
            base.Show();
            var inventoryItems = InventoryManager.Instance.GetPlayerInventory();
            foreach (var item in inventoryItems)
            {
                var itemData = InventoryManager.Instance.GetItemData(item.Key);
                if (itemData != null)
                {
                    var itemUI = _itemPool.Get();
                    itemUI.Initialize(itemData, item.Value);
                    _activeItems.Add(itemUI);
                    itemUI.transform.SetParent(ItemContainer.transform, false); // 设置父物体为物品容器
                }
                else
                {
                    LoggerManager.Instance.LogWarning($"Item with ID {item.Key} not found in inventory.");
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            // 将所有活跃的物品UI返回到对象池
            foreach (var itemUI in _activeItems)
            {
                _itemPool.Return(itemUI);
            }
            _activeItems.Clear();
        }
    }
}