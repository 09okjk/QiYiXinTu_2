using System;
using Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.Save
{
    public class SaveSlotUI:MonoBehaviour
    {
        private SaveSlotData _saveSlotData;
        
        public Button saveButton;
        public Button loadButton;
        public Button deleteButton;
        public TextMeshProUGUI slotIndex;
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI lastPlayedTime;
        public TextMeshProUGUI playTime;

        private void Start()
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
            loadButton.onClick.AddListener(OnLoadButtonClicked);
            deleteButton.onClick.AddListener(OnDeleteButtonClicked);    
        }

        public void Setup(SaveSlotData saveSlotData)
        {
            _saveSlotData = saveSlotData;
            slotIndex.text = $"{_saveSlotData.slotIndex}";
            playerName.text = _saveSlotData.hasData ? _saveSlotData.playerName : "Empty";
            lastPlayedTime.text = _saveSlotData.hasData ? _saveSlotData.lastPlayedTime : "--";
            playTime.text = _saveSlotData.hasData ? _saveSlotData.playTime : "--";
        }
        
        private void OnSaveButtonClicked()
        {
            SaveManager.Instance.SaveToSlot(_saveSlotData.slotIndex);
        }

        private void OnLoadButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnDeleteButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}