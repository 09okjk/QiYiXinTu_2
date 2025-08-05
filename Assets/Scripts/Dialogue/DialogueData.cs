using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        public string dialogueID;
        public DialogueState state = DialogueState.WithOutStart;
        public string currentNodeID;
        
        public ThresholdCondition conditions; // 新增：条件容器
        
        public List<DialogueNode> nodes = new List<DialogueNode>();
    }
    
    // --- 逻辑容器 ---
    /// <summary>
    /// 阈值逻辑：N个条件中需要满足M个。
    /// </summary>
    [Serializable]
    public class ThresholdCondition
    {
        [Tooltip("列表中需要满足的条件数量 (M)")]
        public int requiredCount = 1;

        [SerializeReference]
        [Tooltip("条件列表 (N)")]
        public List<DialogueCondition> conditions = new List<DialogueCondition>();
        
    }

    // --- 条件基类 ---
    [Serializable]
    public abstract class DialogueCondition
    {
    }

    // --- 具体条件 ---
    // (这些类保持不变，但为了完整性在此列出)

    [Serializable]
    public class QuestCompletedCondition : DialogueCondition
    {
        public List<string> questIDs;
    }

    [Serializable]
    public class ItemAcquiredCondition : DialogueCondition
    {
        public List<ItemCondition> itemConditions = new List<ItemCondition>();
    }
    
    [Serializable]
    public class SceneNameCondition : DialogueCondition
    {
        public string sceneName;
    }

    [Serializable]
    public class NpcCheckCondition : DialogueCondition
    {
        public string npcID;
    }
    
    [Serializable]
    public class DialogueCompletedCondition : DialogueCondition
    {
        public List<string> dialogueIDs;
    }

    [Serializable]
    public class EnemyClearedCondition : DialogueCondition
    {
        public List<string> enemyGroupIDs;
    }
    
    // --- 其他数据结构 ---
    
    [Serializable]
    public class ItemCondition
    {
        public string itemID;
        public int requiredAmount;
    }

    [Serializable]
    public class DialogueNode
    {
        public string nodeID;
        public string text;
        public DialogueSpeaker speaker;
        public string nextNodeID;
        public List<DialogueChoice> choices = new List<DialogueChoice>();
        public string questID;
        public List<string> rewardIDs = new List<string>();
        public bool isFollow;
    }

    [Serializable]
    public class DialogueChoice
    {
        public string text;
        public string nextNodeID;
    }

    [Serializable]
    public class DialogueSpeaker
    {
        public string speakerID;
        public string speakerName;
        public SpeakerType speakerType = SpeakerType.Npc;
        public Emotion emotion = Emotion.Neutral;
    }

    [Serializable]
    public enum DialogueState
    {
        Finished, Ongoing, WithOutStart
    }

    [Serializable]
    public enum SpeakerType
    {
        Player, Npc, System, PlayerChoice, NpcNotice
    }

    [Serializable]
    public enum Emotion
    {
        Neutral, NeutralCamera, Happy, HappyCamera, Sad, SadCamera, Angry, AngryCamera, Surprised, SurprisedCamera, Smile, SmileCamera, SmileGlasses, Wink, WinkCamera, Confused, ConfusedCamera
    }
}