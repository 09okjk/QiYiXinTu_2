using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Level.InitializeScene
{
    public class TitleScene:MonoBehaviour
    {
        public GameObject titleWords;
        public Slider loadingBar;

        private void Start()
        {
            titleWords.SetActive(true);
            loadingBar.gameObject.SetActive(false); 
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                titleWords.SetActive(false);
                loadingBar.gameObject.SetActive(true);
                EnterGame();
            }
        }

        private void EnterGame()
        {
            loadingBar.value = 1;
            GameManager.Instance.ChangeScene("MainMenu");
        }
    }
}