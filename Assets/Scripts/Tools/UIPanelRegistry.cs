using System.Collections.Generic;
using UI_HUD;
using UnityEngine;

namespace Tools
{
    public class UIPanelRegistry:MonoBehaviour
    {
        private static readonly Dictionary<string, PanelUI> panels = new Dictionary<string, PanelUI>();
        
        public static void Register(PanelUI panel)
        {
            string key = panel.GetType().Name;
            if (!panels.ContainsKey(key))
            {
                panels[key] = panel;
            }
        }
        
        public static void Unregister(PanelUI panel)
        {
            panels.Remove(panel.GetType().Name);
        }
        
        public static PanelUI GetPanel(string panelName)
        {
            panels.TryGetValue(panelName, out PanelUI panel);
            return panel;
        }
    }
}