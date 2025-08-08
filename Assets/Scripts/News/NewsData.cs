using UnityEngine;

namespace News
{
    [CreateAssetMenu(fileName = "NewNewsData", menuName = "News/NewsData")]
    public class NewsData : ScriptableObject
    {
        public string newsID; // 新闻ID
        public string title; // 新闻标题
        public string author; // 新闻作者
        public string content; // 新闻内容
        public Sprite icon; // 新闻图标
    }
}