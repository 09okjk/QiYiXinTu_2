using TMPro;
using Tools;
using UnityEngine;

namespace UI_HUD.Quest
{
    public class QuestUI:MonoBehaviour
    {
        public TextMeshProUGUI questDescriptionText;
        
        public void SetQuestDescription(string description)
        {
            if (questDescriptionText != null)
            {
                questDescriptionText.text = description;
            }
            else
            {
                LoggerManager.Instance.LogError("Quest Description Text is not assigned in the QuestUI component.");
            }
        }
    }
}