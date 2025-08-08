using System;
using News;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.News
{
    public class NewsSlot: MonoBehaviour
    {
        public NewsData newsData; // 关联的新闻数据
        public TextMeshProUGUI titleText; // 新闻标题文本
        public Image iconImage; // 新闻图标
        public Button readButton; // 阅读按钮
        
        private NewsPanelUI newsPanelUI; // 关联的新闻面板UI

        public void SetNewsData(NewsData data, NewsPanelUI panelUI)
        {
            newsPanelUI = panelUI; // 关联新闻面板UI
            newsData = data;
            if (newsData != null)
            {
                titleText.text = newsData.title;
                iconImage.sprite = newsData.icon;
                readButton.onClick.AddListener(OnReadButtonClicked);
            }
        }

        private void OnReadButtonClicked()
        {
            newsPanelUI.newsIcon.sprite = newsData.icon; // 设置新闻图标
            newsPanelUI.newsTitle.text = newsData.title; // 设置新闻标题
            newsPanelUI.newsAuthor.text = newsData.author; // 设置新闻作者
            newsPanelUI.newsDescription.text = newsData.content; // 设置新闻内容
        }
    }
}