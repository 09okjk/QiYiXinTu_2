using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "NewQuestData", menuName = "Quest/QuestData")]
    public class QuestData: ScriptableObject
    {
        public string questID; 
        public string questName; 
        public string description; 
        public QuestState state = QuestState.NotStarted;
    }

    public enum QuestState
    {   
        NotStarted, // 未开始
        InProgress, // 进行中
        Completed, // 已完成
        Failed // 失败
    }
}