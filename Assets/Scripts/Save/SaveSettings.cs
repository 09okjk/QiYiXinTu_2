using UnityEngine;

namespace Save
{
    [CreateAssetMenu(fileName = "SaveSettings", menuName = "Save System/Save Settings")]
    public class SaveSettings : ScriptableObject
    {
        [Header("📁 路径设置")]
        public string saveFolder = "Saves";
        public string saveFilePrefix = "save_";       // save_1.json
        public string autoSaveFileName = "auto_save"; // auto_save.json

        [Header("💾 槽位设置")]
        public int maxSlotCount = 3;

        [Header("⏱️ 自动保存")]
        public bool enableAutoSave = true;
        public float autoSaveInterval = 300f; // 单位：秒（默认 5 分钟）

        [Header("⚙️ 高级")]
        public bool saveMetadata = true; // 是否生成 .meta 文件用于 UI 显示
    }
}