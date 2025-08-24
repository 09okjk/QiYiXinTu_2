using Core;
using Save;
using UnityEngine;

namespace UI_HUD
{
    public class MenuUIManager:MonoBehaviour
    {
        public GameObject settingsPanel;
        public GameObject savePanel;
        
        private void Start()
        {
            settingsPanel.SetActive(false);
            savePanel.SetActive(false);
        }
        
        public void OpenSettingsPanel()
        {
            UIManager.Instance.ShowPanel("SettingPanel");
        }
        public void OpenSavePanel()
        {
            UIManager.Instance.ShowPanel("SavePanel");
        }

        public void NewGame()
        {
            GameStateManager.Instance.SetFlag("isNewGame", true);
            GameManager.Instance.ChangeScene("GameScene");
        }

        public void ContinueGame()
        {
            if(SaveManager.Instance.LoadFromSlot(SaveManager.Instance.GetLastSlotIndex()))
            {
                GameManager.Instance.ChangeScene("GameScene");
            }
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}