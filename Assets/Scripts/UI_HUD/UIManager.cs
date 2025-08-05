using System.Collections.Generic;
using UnityEngine;

namespace UI_HUD
{
    public class UIManager: MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        public List<PanelUI> panels;
        public PanelUI currentPanel;
        
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
        
        /// <summary>
        /// 显示指定名称的面板
        /// </summary>
        /// <param name="panelObjectName">面板对象的名称</param>
        public void ShowPanel(string panelObjectName)
        {
            PanelUI panel = panels.Find(p => p.name == panelObjectName);
            if (panel != null)
            {
                if (currentPanel != null)
                {
                    currentPanel.Hide();
                }
                currentPanel = panel;
                currentPanel.gameObject.SetActive(true);
                currentPanel.Show();
            }
            else
            {
                Debug.LogWarning($"Panel {panelObjectName} not found.");
            }
        }
    }
}