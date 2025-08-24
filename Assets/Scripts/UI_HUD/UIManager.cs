using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD
{
    public class UIManager: MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        // public List<PanelUI> panels;
        public PanelUI currentPanel;
        
        [Header("Confirm Dialog")]
        [SerializeField] private GameObject confirmDialogPanel;
        [SerializeField] private TextMeshProUGUI confirmTitleText;
        [SerializeField] private Image confirmImage;
        [SerializeField] private TextMeshProUGUI confirmMessageText;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;
        
        [Header("InputField Window")]
        [SerializeField] private GameObject inputFieldWindow;
        [SerializeField] private TextMeshProUGUI inputFieldTitleText;
        [SerializeField] private TextMeshProUGUI inputFieldMessageText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button inputFieldConfirmButton;
        
        public event Action<bool> OnPopWindowEvent;

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
        /// <param name="panelTypeName">面板对象的名称</param>
        public void ShowPanel(string panelTypeName)
        {
            PanelUI panel = UIPanelRegistry.GetPanel(panelTypeName);
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
                LoggerManager.Instance.LogWarning($"Panel {panelTypeName} not found.");
            }
        }
        
        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="message">对话框消息</param>
        /// <param name="sprite">图片</param>
        /// <param name="onYes">点击确认时执行的操作</param>
        /// <param name="onNo">点击取消时执行的操作</param>
        public void ShowConfirmDialog(string title, string message,Sprite sprite = null, Action onYes = null, Action onNo = null)
        {
            if (!confirmDialogPanel)
            {
                LoggerManager.Instance.LogError("Confirm dialog panel not assigned!");
                return;
            }
            
            // 隐藏按钮
            confirmYesButton.gameObject.SetActive(false);
            confirmNoButton.gameObject.SetActive(false);
            confirmImage.gameObject.SetActive(false);
            
            confirmTitleText.text = title;
            confirmMessageText.text = message;
            
            // 清除之前的监听器 防止重复调用
            confirmYesButton.onClick.RemoveAllListeners();
            confirmNoButton.onClick.RemoveAllListeners();

            // 设置图片
            if (sprite != null)
            {
                confirmImage.gameObject.SetActive(true);
                confirmImage.sprite = sprite;
            }
 
            if (onYes != null)
            {
                confirmYesButton.gameObject.SetActive(true);
            }
            
            if (onNo != null)
            {
                confirmNoButton.gameObject.SetActive(true);
            }
            
            // 添加新的监听器
            confirmYesButton.onClick.AddListener(() => 
            {
                onYes?.Invoke();
                confirmDialogPanel.SetActive(false);
                OnPopWindowEvent?.Invoke(confirmDialogPanel.activeSelf);
            });
            
            confirmNoButton.onClick.AddListener(() => 
            {
                onNo?.Invoke();
                confirmDialogPanel.SetActive(false);
                OnPopWindowEvent?.Invoke(confirmDialogPanel.activeSelf);
            });
            
            confirmDialogPanel.SetActive(true);
            OnPopWindowEvent?.Invoke(confirmDialogPanel.activeSelf);
        }

        /// <summary>
        /// 显示输入框窗口
        /// </summary>
        /// <param name="title">弹窗标题</param>
        /// <param name="message">输入提示</param>
        /// <param name="onConfirm">确认按钮</param>
        /// <returns></returns>
        public Task InputFieldWindow(string title, string message, Action<string> onConfirm)
        {
            if (!inputFieldWindow)
            {
                LoggerManager.Instance.LogError("Input field window not assigned!");
                return Task.CompletedTask;
            }
            inputFieldTitleText.text = title;
            inputFieldMessageText.text = message;
            
            inputFieldWindow.SetActive(true);
            
            // 清除之前的监听器 防止重复调用
            inputFieldConfirmButton.onClick.RemoveAllListeners();
            
            // 添加新的监听器
            inputFieldConfirmButton.onClick.AddListener(() => 
            {
                string inputText = inputField.text;
                onConfirm?.Invoke(inputText);
                inputFieldWindow.SetActive(false);
                OnPopWindowEvent?.Invoke(inputFieldWindow.activeSelf);
            });
            
            OnPopWindowEvent?.Invoke(inputFieldWindow.activeSelf);
            return Task.CompletedTask;
        }
    }
}