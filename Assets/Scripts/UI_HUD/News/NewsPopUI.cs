using News;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.News
{
    public class NewsPopUI:PanelUI
    {
        public Image newsIcon; // 新闻图标
        public TextMeshProUGUI newsTitle; // 新闻标题文本
        public TextMeshProUGUI newsAuthor; // 新闻作者文本
        public TextMeshProUGUI newsDescription; // 新闻描述文本
        public Button closeButton; // 关闭按钮
        
        private void Start()
        {
            closeButton.onClick.AddListener(Hide); // 绑定关闭按钮事件
        }
        
        public override void Show()
        {
            base.Show(); // 调用基类的Show方法
            string clickedNewsID = NewsManager.Instance.GetClickedNewsID();
            if (!string.IsNullOrEmpty(clickedNewsID))
            {
                ShowNews(clickedNewsID); // 显示被点击的新闻
            }
            else
            {
                LoggerManager.Instance.LogWarning("No news ID clicked to show.");
            }
        }

        private void ShowNews(string newsID)
        {
            var newsData = NewsManager.Instance.GetNewsData(newsID);
            if (newsData != null)
            {
                newsIcon.sprite = newsData.icon; // 设置新闻图标
                newsTitle.text = newsData.title; // 设置新闻标题
                newsAuthor.text = newsData.author; // 设置新闻作者
                newsDescription.text = newsData.content; // 设置新闻内容
            }
            else
            {
                LoggerManager.Instance.LogWarning($"News with ID {newsID} not found.");
            }
        }
    }
}