using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.Inventory
{
    public class ItemSlotUI:MonoBehaviour
    {
        public ItemData itemData; // 物品数据
        public Button itemButton; // 物品按钮
        public TextMeshProUGUI itemCountText; // 物品数量文本
        public Sprite itemIcon; // 物品图标
        
        public void Initialize(ItemData data, int count)
        {
            itemData = data;
            itemIcon = data.icon;
            itemButton.GetComponent<Image>().sprite = itemIcon;
            itemCountText.text = count.ToString();
            itemButton.onClick.AddListener(OnItemClicked);
        }

        private void OnItemClicked()
        {
            throw new System.NotImplementedException();
        }
    }
}