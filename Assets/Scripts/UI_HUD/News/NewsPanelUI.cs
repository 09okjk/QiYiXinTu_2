using System;
using System.Collections.Generic;
using News;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.News
{
    public class NewsPanelUI: PanelUI
    {
        public GameObject newsItemPrefab; // 新闻项预制体
        public Transform newsContent; // 新闻内容容器
        public Image newsIcon; // 新闻图标
        public TextMeshProUGUI newsTitle; // 新闻标题文本
        public TextMeshProUGUI newsAuthor; // 新闻作者文本
        public TextMeshProUGUI newsDescription; // 新闻描述文本
        public Button closeButton; // 关闭按钮
        
        private ObjectPool<NewsSlot> _newsSlotPool; // 新闻项对象池
        private List<NewsSlot> _activeNewsSlots = new List<NewsSlot>(); // 当前激活的新闻项列表

        protected override void Awake()
        {
            var newsSlotComponent = newsItemPrefab.GetComponent<NewsSlot>();
            _newsSlotPool = new ObjectPool<NewsSlot>(newsSlotComponent, newsContent, 10);
        }

        private void Start()
        {
            closeButton.onClick.AddListener(Hide); // 绑定关闭按钮事件
        }

        public override void Show()
        {
            base.Show();
            var newsDataDic = NewsManager.Instance.GetKnownNews();
            foreach (var newsData in newsDataDic.Values)
            {
                // 创建新闻项
                var newsItem = Instantiate(newsItemPrefab, newsContent);
                var newsSlot = newsItem.GetComponent<NewsSlot>();
                if (newsSlot != null)
                {
                    // 使用对象池获取新闻项
                    newsSlot = _newsSlotPool.Get();
                    _activeNewsSlots.Add(newsSlot); // 添加到活跃列表
                    newsSlot.SetNewsData(newsData, this); // 设置新闻数据和关联的新闻面板UI
                    newsSlot.transform.SetParent(newsContent, false); // 设置父物体为新闻内容容器
                }
                else
                {
                    LoggerManager.Instance.LogWarning("NewsSlot component not found on the news item prefab.");
                }
            }
        }
    }
}