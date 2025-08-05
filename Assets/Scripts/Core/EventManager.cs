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

        #region MyRegion

        public event Action<string> OnDialogueFinished;
        public void TriggerDialogueFinished(string dialogueID)
        {
            OnDialogueFinished?.Invoke(dialogueID);
        }

        #endregion
    }
}