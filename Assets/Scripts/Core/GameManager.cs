using System;
using UnityEngine;

namespace Core
{
    public class GameManager:MonoBehaviour
    {
        public static GameManager Instance {get; private set;}

        #region Events

        public event Action<string> OnDialogueFinished;

        #endregion
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
            // 初始化游戏状态
            // InitializeGame();
            
            EventManager.Instance.OnDialogueFinished += OnDialogueCompleted;
        }
        
        public void PauseGame()
        {
            Time.timeScale = 0f; // 暂停游戏
            Debug.Log("Game Paused");
        }
        public void ResumeGame()
        {
            Time.timeScale = 1f; // 恢复游戏
            Debug.Log("Game Resumed");
        }
        
        private void OnDialogueCompleted(string dialogueID)
        {
            //Debug.Log($"Dialogue {dialogueID} completed.");
        }
    }
}