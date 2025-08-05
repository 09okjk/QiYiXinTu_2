using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI_HUD.Dialogue
{
    public class DialoguePanelUI:PanelUI
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private Transform choiceButtonContainer;
        [SerializeField] private GameObject playerDialoguePanel; 
        [SerializeField] private Image playerImage; 
        [SerializeField] private TextMeshProUGUI playerNameText; 
        [SerializeField] private TextMeshProUGUI playerDialogueText;
        [SerializeField] private GameObject nPCDialoguePanel; 
        [SerializeField] private Image nPCImage; 
        [SerializeField] private TextMeshProUGUI nPCNameText; 
        [SerializeField] private TextMeshProUGUI nPCDialogueText;
        [SerializeField] private GameObject systemDialoguePanel; 
        [SerializeField] private TextMeshProUGUI systemNameText; 
        [SerializeField] private TextMeshProUGUI systemDialogueText;

        private DialogueGameData _dialogueGameData;
        private int _currentDialogueNodeIndex;
        private List<DialogueNode> _currentDialogueNodes;
        private TextMeshProUGUI _currentText;
        private bool _isTyping;
        private void Start()
        {
        }
        
        public override void Show()
        {
            base.Show();
            _dialogueGameData = DialogueManager.Instance.GetCurrentDialogue();
            InitDialogueUI();
        }

        private void InitDialogueUI()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            _currentDialogueNodeIndex = 0;
            if (_dialogueGameData == null)
            {
                Debug.LogWarning("对话数据未初始化或为空");
            }
            else
            {
                _currentDialogueNodes = _dialogueGameData.nodes;
                if (_currentDialogueNodes.Count > 0)
                {
                    UpdateDialogueUI();
                }
                else
                {
                    Debug.LogWarning("对话节点列表为空");
                }
            }
        }

        private void OnContinueButtonClicked()
        {
            if (_isTyping)
            {
                _isTyping = false;
                return;
            }
            UpdateDialogueUI();
        }

        private void UpdateDialogueUI()
        {
            choiceButtonPrefab.SetActive(false);
            playerDialoguePanel.SetActive(false);
            nPCDialoguePanel.SetActive(false);
            systemDialoguePanel.SetActive(false);
            if (_dialogueGameData == null)
            {
                Debug.LogWarning("对话数据未初始化或为空");
                return;
            }
            var currentNode = _currentDialogueNodes[_currentDialogueNodeIndex];
            switch (currentNode.speaker.speakerType)
            {
                case SpeakerType.Player:
                    playerDialoguePanel.SetActive(true);
                    playerImage.sprite = Resources.Load<Sprite>($"Art/Player/{currentNode.speaker.speakerID}_{currentNode.speaker.emotion.ToString()}");
                    playerNameText.text = currentNode.speaker.speakerName!= "???" ? PlayerManager.Instance.player.playerData.playerName : currentNode.speaker.speakerName;
                    _currentText = playerDialogueText;
                    break;
                case SpeakerType.Npc:
                    nPCDialoguePanel.SetActive(true);
                    nPCImage.sprite = Resources.Load<Sprite>($"Art/NPC/{currentNode.speaker.speakerID}_{currentNode.speaker.emotion.ToString()}");
                    nPCNameText.text = currentNode.speaker.speakerName;
                    _currentText = nPCDialogueText;
                    break;
                case SpeakerType.System:
                    systemDialoguePanel.SetActive(true);
                    systemNameText.text = currentNode.speaker.speakerName;
                    _currentText = systemDialogueText;
                    break;
            }
            
            // 使用携程实现打字机效果
            StartCoroutine(TypeSentence(currentNode.text,FinishTyping));
            
        }

        /// <summary>
        /// 打字机效果显示文本
        /// </summary>
        /// <param name="currentNodeText"> 当前节点文本</param>
        /// <param name="onComplete"> 完成回调</param>
        /// <returns> IEnumerator</returns>
        private IEnumerator TypeSentence(string currentNodeText,Action onComplete = null)
        {
            _isTyping = true;
            _currentText.text = "";
            foreach (char letter in currentNodeText)
            {
                if (!_isTyping)
                {
                    _currentText.text = currentNodeText;
                    break;
                }
                _currentText.text += letter;
                // 等待一段时间以实现打字机效果
                yield return new WaitForSecondsRealtime(0.02f);
            }
            _isTyping = false;
            onComplete?.Invoke();
        }

        private void FinishTyping()
        {
            // 清除任何现有的选择按钮
            foreach (Transform child in choiceButtonContainer)
            {
                Destroy(child.gameObject);
            }
            if (_currentDialogueNodes[_currentDialogueNodeIndex].choices.Count > 0)
            {
                // 创建选择按钮，每个按钮对应一个选择
                for (int i = 0; i < _currentDialogueNodes[_currentDialogueNodeIndex].choices.Count; i++)
                {
                    DialogueChoice choice = _currentDialogueNodes[_currentDialogueNodeIndex].choices[i];
                    GameObject buttonGo = Instantiate(choiceButtonPrefab, choiceButtonContainer);

                    // 设置按钮文本和点击事件
                    Button button = buttonGo.GetComponent<Button>();
                    TextMeshProUGUI buttonText = buttonGo.GetComponentInChildren<TextMeshProUGUI>();

                    buttonText.text = choice.text;

                    int choiceIndex = i; // 需要捕获索引以供lambda使用
                    button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                }
            }
        }

        private void OnChoiceSelected(int choiceIndex)
        {
            string nextNodeID = _currentDialogueNodes[_currentDialogueNodeIndex].choices[choiceIndex].nextNodeID;

            foreach (Transform child in choiceButtonContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
