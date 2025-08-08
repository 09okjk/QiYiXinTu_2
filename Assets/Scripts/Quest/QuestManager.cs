using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class QuestGameData
    {
        public string questID; 
        public string questName; 
        public string description; 
        public QuestState state = QuestState.NotStarted;
    }
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }
        private Dictionary<string, QuestData> questDatas = new Dictionary<string, QuestData>();
        public Dictionary<string, QuestGameData> acceptedQuests = new Dictionary<string, QuestGameData>();
        public Dictionary<string, QuestGameData> finishedQuests = new Dictionary<string, QuestGameData>();
        
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
            var questDataAssets = Resources.LoadAll<QuestData>("ScriptableObjects/Quests");
            foreach (var questData in questDataAssets)
            {
                if (!questDatas.ContainsKey(questData.questID))
                {
                    questDatas[questData.questID] = questData;
                }
            }
        }
        
        public void SetQuestGameDatas(Dictionary<string, QuestGameData> acceptQuests, Dictionary<string, QuestGameData> finishQuests)
        {
            this.acceptedQuests = acceptQuests;
            this.finishedQuests = finishQuests;
        }
        
        public Dictionary<string, QuestGameData> GetAcceptQuests()
        {
            return acceptedQuests;
        }
        
        public Dictionary<string, QuestGameData> GetFinishedQuests()
        {
            return finishedQuests;
        }
        
        public void AcceptQuest(string questID)
        {
            if (questDatas.TryGetValue(questID, out var questData))
            {
                if (!acceptedQuests.ContainsKey(questID))
                {
                    var questGameData = new QuestGameData
                    {
                        questID = questData.questID,
                        questName = questData.questName,
                        description = questData.description,
                        state = QuestState.InProgress
                    };
                    acceptedQuests[questID] = questGameData;
                }
            }
            else
            {
                LoggerManager.Instance.LogWarning($"Quest with ID {questID} not found.");
            }
        }
        
        public void UpdateQuestData(string questID,bool isCompleted)
        {
            if (acceptedQuests.TryGetValue(questID, out var questGameData))
            {
                finishedQuests[questID] = questGameData;
                acceptedQuests.Remove(questID);
                if (isCompleted)
                {
                    questGameData.state = QuestState.Completed;
                }
                else
                {
                    questGameData.state = QuestState.Failed;
                }
            }
        }

        public QuestState GetQuestState(string questID)
        {
            if (acceptedQuests.TryGetValue(questID, out var questGameData))
            {
                return questGameData.state;
            }
            else if (finishedQuests.TryGetValue(questID, out var finishedQuestGameData))
            {
                return finishedQuestGameData.state;
            }

            return QuestState.NotStarted; // 如果没有找到，返回未开始状态
        }
    }
}