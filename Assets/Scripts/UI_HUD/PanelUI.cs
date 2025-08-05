using Core;
using UnityEngine;

namespace UI_HUD
{
    public class PanelUI: MonoBehaviour,IUIPanel
    {
        public virtual void Show()
        {
            GameManager.Instance.PauseGame();
        }

        public virtual void Hide()
        {
            GameManager.Instance.ResumeGame();
            gameObject.SetActive(false);
        }
    }
}