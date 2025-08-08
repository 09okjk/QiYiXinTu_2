using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class GameManager:MonoBehaviour
    {
        public static GameManager Instance {get; private set;}
        [SerializeField] private InputActionAsset inputActions;

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
            EventManager.Instance.OnUIPanelOpened += OnUIPanelOpened;
            EventManager.Instance.OnQuestCompleted += OnQuestCompleted;
            EventManager.Instance.OnQuestFailed += OnQuestFailed;
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

        private void OnUIPanelOpened(bool isOpened)
        {
            if(isOpened)
            {
                inputActions.FindActionMap("Player").Disable();
                inputActions.FindActionMap("UI").Enable();
                PauseGame();
            }
            else
            {
                inputActions.FindActionMap("Player").Enable();
                inputActions.FindActionMap("UI").Disable();
                ResumeGame();
            }
        }

        private void OnQuestCompleted(string questID)
        {
            throw new NotImplementedException();
        }

        private void OnQuestFailed(string questID)
        {
            throw new NotImplementedException();
        }
    }
}
