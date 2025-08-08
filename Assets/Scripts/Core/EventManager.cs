using System;
using UnityEngine;

namespace Core
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

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

        #region Events

        public event Action<string> OnDialogueFinished;
        public event Action<bool> OnUIPanelOpened;
        public event Action<string> OnQuestCompleted;
        public event Action<string> OnQuestFailed;
        
        //-------------------- 触发事件 --------------------
        
        public void TriggerDialogueFinished(string dialogueID)
        {
            OnDialogueFinished?.Invoke(dialogueID);
        }
        
        public void TriggerUIPanelOpened(bool isOpened)
        {
            OnUIPanelOpened?.Invoke(isOpened);
        }
        
        public void TriggerQuestCompleted(string questID)
        {
            OnQuestCompleted?.Invoke(questID);
        }

        public void TriggerQuestFailed(string questID)
        {
            OnQuestFailed?.Invoke(questID);
        }
        #endregion
    }
}