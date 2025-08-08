using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace News
{
    public class NewsManager:MonoBehaviour
    {
        public static NewsManager Instance { get; private set; }
        private Dictionary<string, NewsData> newsData = new Dictionary<string, NewsData>();
        public Dictionary<string, NewsData> KnownNews = new Dictionary<string, NewsData>();
        private string clickedNewsID = string.Empty;
        
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

        private void Start()
        {
            var newsDatas = Resources.LoadAll<NewsData>("ScriptableObjects/News");
            foreach (var newsDataItem in newsDatas)
            {
                if (!newsData.ContainsKey(newsDataItem.newsID))
                {
                    newsData[newsDataItem.newsID] = newsDataItem;
                }
            }
        }
        
        public void SetKnownNews(Dictionary<string, NewsData> knownNews)
        {
            KnownNews = knownNews;
        }
        
        public Dictionary<string, NewsData> GetKnownNews()
        {
            return KnownNews;
        }
        
        public NewsData GetNewsData(string newsID)
        {
            if (newsData.TryGetValue(newsID, out var data))
            {
                return data;
            }
            LoggerManager.Instance.LogWarning($"News with ID {newsID} not found.");
            return null;
        }
        
        public void AddNews(string newsID)
        {
            if (newsData.TryGetValue(newsID, out var data))
            {
                if (!KnownNews.ContainsKey(newsID))
                {
                    KnownNews[newsID] = data;
                }
                else
                {
                    LoggerManager.Instance.LogWarning($"News with ID {newsID} already exists in known news.");
                }
            }
            else
            {
                LoggerManager.Instance.LogWarning($"News with ID {newsID} not found in news data.");
            }
        }
        
        public void ClickedNews(string newsID)
        {
            if (newsData.ContainsKey(newsID))
            {
                clickedNewsID = newsID;
                LoggerManager.Instance.Log($"Clicked on news with ID: {newsID}");
            }
            else
            {
                LoggerManager.Instance.LogWarning($"News with ID {newsID} not found.");
            }
        }
        
        public string GetClickedNewsID()
        {
            if (!string.IsNullOrEmpty(clickedNewsID))
            {
                return clickedNewsID;
            }
            LoggerManager.Instance.LogWarning("No news has been clicked yet.");
            return string.Empty;
        }
    }
}
