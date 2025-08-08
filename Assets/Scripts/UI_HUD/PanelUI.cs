using Core;
using UnityEngine;

namespace UI_HUD
{
    public class PanelUI: MonoBehaviour,IUIPanel
    {
        public virtual void Show()
        {
            EventManager.Instance.TriggerUIPanelOpened(true);
        }

        public virtual void Hide()
        {
            EventManager.Instance.TriggerUIPanelOpened(false);
            gameObject.SetActive(false);
        }
    }
}