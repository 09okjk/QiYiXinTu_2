using System;
using System.Collections.Generic;
using Core;
using Tools;
using UI_HUD;
using UnityEngine;

namespace Dialogue
{
    public class DialogueGameData
    {
        public string dialogueID;
        public DialogueState state = DialogueState.WithOutStart; // 新增：对话状态
        public string currentNodeID;
        public ThresholdCondition conditions; // 新增：条件容器
        public List<DialogueNode> nodes = new List<DialogueNode>();
    }
    
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        
        private DialogueGameData _currentDialogue;
        private Dictionary<string, bool> _dialogueStates = new Dictionary<string, bool>();
        private Dictionary<string, DialogueGameData> _dialogueDatas = new Dictionary<string, DialogueGameData>();
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

        private void InitializeDialogueData()
        {
            try
            {
                LoggerManager.Instance.Log("开始初始化对话数据");
                var dialogueDatas = Resources.LoadAll<DialogueData>("ScriptableObjects/Dialogues");

                foreach (var dialogueData in dialogueDatas)
                {
                    if (dialogueData != null)
                    {
                        var gameData = new DialogueGameData
                        {
                            dialogueID = dialogueData.dialogueID,
                            state = dialogueData.state,
                            currentNodeID = dialogueData.currentNodeID,
                            conditions = dialogueData.conditions,
                            nodes = new List<DialogueNode>(dialogueData.nodes)
                        };

                        _dialogueDatas[dialogueData.dialogueID] = gameData;
                        _dialogueStates[dialogueData.dialogueID] = false; // 初始化对话状态为未完成
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Instance.LogError("初始化对话数据时发生错误: " + ex.Message);
            }
        }

        private void SaveDialogueData(Dictionary<string, DialogueGameData> dialogueGameDatas)
        {
            if (dialogueGameDatas == null || dialogueGameDatas.Count == 0)
            {
                LoggerManager.Instance.LogWarning("没有对话数据需要保存");
                return;
            }

            try
            {
                _dialogueDatas = new Dictionary<string, DialogueGameData>(dialogueGameDatas);
            }
            catch (Exception ex)
            {
                LoggerManager.Instance.LogError("保存对话数据时发生错误: " + ex.Message);
            }
        }
        
        private Dictionary<string, DialogueGameData> GetDialogueDatas()
        {
            if (_dialogueDatas.Count == 0)
            {
                InitializeDialogueData();
            }
            return _dialogueDatas;
        }
        
        public DialogueGameData GetDialogueDataByID(string dialogueID)
        {
            if (_dialogueDatas.Count == 0)
            {
                InitializeDialogueData();
            }

            if (_dialogueDatas.TryGetValue(dialogueID, out var dialogueData))
            {
                return dialogueData;
            }
            else
            {
                LoggerManager.Instance.LogWarning($"对话数据 {dialogueID} 不存在");
                return null;
            }
        }
        
        public DialogueGameData GetCurrentDialogue()
        {
            if (_currentDialogue == null)
            {
                LoggerManager.Instance.LogWarning("当前对话未设置");
                return null;
            }
            return _currentDialogue;
        }
        
        private bool CheckDialogueCanStart(string dialogueID)
        {
            int conditionCount = 0;
            if (_dialogueDatas.TryGetValue(dialogueID, out var dialogueGameData))
            {
                // 检查对话状态
                if (dialogueGameData.state == DialogueState.Finished)
                {
                    LoggerManager.Instance.LogWarning($"对话 {dialogueID} 已经完成，无法重新开始");
                    return false;
                }
                
                // 检查条件是否满足
                foreach (var dialogueCondition in dialogueGameData.conditions.conditions)
                {
                    if (dialogueCondition is QuestCompletedCondition questCompletedCondition)
                    {
                        // 检查任务是否完成
                        foreach (var questID in questCompletedCondition.questIDs)
                        {
                            // if (!QuestManager.Instance.IsQuestCompleted(questID))
                            // {
                            //     return false; // 有未完成的任务，条件不满足
                            // }
                        }
                        conditionCount++;
                    }else if (dialogueCondition is ItemAcquiredCondition itemAcquiredCondition)
                    {
                        // 检查物品是否拥有
                        foreach (var itemCondition in itemAcquiredCondition.itemConditions)
                        {
                            // if(Inventory.Instance.GetItemCount(itemCondition.itemID) < itemCondition.requiredCount)
                            //     return false; // 物品数量不足，条件不满足
                        }
                        conditionCount++;
                    }else if (dialogueCondition is SceneNameCondition sceneNameCondition)
                    {
                        // 检查当前场景是否匹配

                        conditionCount++;
                    }else if (dialogueCondition is NpcCheckCondition npcCheckCondition)
                    {
                        // 检查NPC状态
                        // if (!NpcManager.Instance.IsNpcAvailable(npcCheckCondition.npcID))
                        // {
                        //     return false; // NPC不可用，条件不满足
                        // }
                        conditionCount++;
                    }else if (dialogueCondition is DialogueCompletedCondition dialogueCompletedCondition)
                    {
                        // 检查其他对话是否完成
                        // foreach (var completedDialogueID in dialogueCompletedCondition.dialogueIDs)
                        // {
                        //     if (!_dialogueStates.TryGetValue(completedDialogueID, out var isCompleted) || !isCompleted)
                        //     {
                        //         return false; // 有未完成的对话，条件不满足
                        //     }
                        // }

                        conditionCount++;
                    }else if (dialogueCondition is EnemyClearedCondition enemyClearedCondition)
                    {
                        // 检查敌人是否清除
                        // foreach (var enemyGroupID in enemyClearedCondition.enemyGroupIDs)
                        // {
                        //     if (!EnemyManager.Instance.IsEnemyGroupCleared(enemyGroupID))
                        //     {
                        //         return false; // 有未清除的敌人，条件不满足
                        //     }
                        // }
                        conditionCount++;
                    }
                }
                // 检查条件数量是否满足
                if (conditionCount < dialogueGameData.conditions.requiredCount)
                {
                    LoggerManager.Instance.LogWarning($"对话 {dialogueID} 条件未满足，所需数量：{dialogueGameData.conditions.requiredCount}，实际数量：{conditionCount}");
                    return false;
                }
                return true;
            }
            else
            {
                LoggerManager.Instance.LogWarning($"对话状态 {dialogueID} 不存在");
                return false;
            }
        }
        
        public void StartDialogue(string dialogueID)
        {
            if (CheckDialogueCanStart(dialogueID))
            {
                LoggerManager.Instance.Log($"开始对话： {dialogueID}");
                _currentDialogue = GetDialogueDataByID(dialogueID);
                UIManager.Instance.ShowPanel("DialoguePanel");
            }
            else
            {
                LoggerManager.Instance.LogError($"对话 {dialogueID} 不存在或已完成或条件为达成");
            }
        }
        
        public void FinishDialogue(string dialogueID)
        {
            if (_dialogueDatas.TryGetValue(dialogueID, out var data))
            {
                data.state = DialogueState.Finished;
                LoggerManager.Instance.Log($"对话 {dialogueID} 已完成");
                EventManager.Instance.TriggerDialogueFinished(dialogueID);
            }
            else
            {
                LoggerManager.Instance.LogError($"对话 {dialogueID} 不存在");
            }
        }
        
    }
}
