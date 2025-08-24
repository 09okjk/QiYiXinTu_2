using Core;
using Tools;
using UnityEngine;

namespace UI_HUD
{
    public class PanelUI: MonoBehaviour,IUIPanel
    {
        protected virtual void Awake()
        {
            UIPanelRegistry.Register(this);
        }
        
        protected virtual void OnDestroy()
        {
            UIPanelRegistry.Unregister(this);
        }
        
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