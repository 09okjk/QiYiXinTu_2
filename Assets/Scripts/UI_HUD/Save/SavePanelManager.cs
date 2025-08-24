using System.Collections.Generic;
using Save;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.Save
{
    public class SavePanelManager:PanelUI
    {
        public Button closeButton;
        public GameObject saveSlotPrefab; // 预制体
        public Transform saveSlotContainer; // 容器
        
        private void Start()
        {
            closeButton.onClick.AddListener(Hide);
        }
        
        public override void Show()
        {
            base.Show();
            gameObject.SetActive(true);
            // 这里可以添加加载存档槽的逻辑
            
            RefreshSaveSlots();
        }

        private void RefreshSaveSlots()
        {
            // 清空现有的存档槽
            foreach (Transform child in saveSlotContainer)
            {
                Destroy(child.gameObject);
            }

            List<SaveSlotData> slots = SaveManager.Instance.GetAllSlotData();
            
            foreach (var slot in slots)
            {
                GameObject slotObj = Instantiate(saveSlotPrefab, saveSlotContainer);
                SaveSlotUI slotUI = slotObj.GetComponent<SaveSlotUI>();
                if (slotUI != null)
                {
                    slotUI.Setup(slot);
                }
                else
                {
                    Debug.LogError("SaveSlotPrefab 缺少 SaveSlotUI 组件！");
                }
            }
        }
    }
}