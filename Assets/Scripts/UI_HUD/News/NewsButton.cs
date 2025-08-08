using System;
using Core;
using News;
using UnityEngine;
using UnityEngine.UI;

namespace UI_HUD.News
{
    public class NewsButton:MonoBehaviour
    {
        public string newsID;
        public Image newsIcon;
        public Button newsButton;
        public BoxCollider2D newsCollider;

        private void Start()
        {
            newsButton.onClick.AddListener(OnNewsButtonClicked);
        }

        private void OnNewsButtonClicked()
        {
            NewsManager.Instance.ClickedNews(newsID);
            UIManager.Instance.ShowPanel("NewsPopUpPanel");
        }
    }
}