using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerCharacter;
using Tools;
using UnityEngine;

namespace Save
{
    [System.Serializable]
    public class SaveSlotData
    {
        public int slotIndex;
        public bool hasData;
        public string playerName;
        public string lastPlayedTime;
        public string playTime;
    }
    
    [System.Serializable]
    public class SaveMetadata
    {
        public int slotIndex;
        public string lastPlayed;
        public string playTime;
        public string playerName;
        public string sceneName;
    }
    
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;
        public SaveSettings settings;

        // 私有状态
        private float autoSaveTimer;
        private int lastSlotIndex = -1;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (settings == null)
            {
                settings = Resources.Load<SaveSettings>("ScriptableObjects/SaveSettings");
                if (settings == null)
                {
                    LoggerManager.Instance.LogError("未找到 SaveSettings 资源！");
                    enabled = false;
                    return;
                }
            }
        }

        private void Update()
        {
            if (settings.enableAutoSave && settings.autoSaveInterval > 0)
            {
                autoSaveTimer += Time.deltaTime;
                if (autoSaveTimer >= settings.autoSaveInterval)
                {
                    AutoSave();
                    autoSaveTimer = 0f;
                }
            }
        }

        public int GetLastSlotIndex()
        {
            return lastSlotIndex;
        }
        
        // =============================
        // 📥 路径管理
        // =============================
        
        private string GetSaveFolderPath()
        {
            return Path.Combine(Application.persistentDataPath, settings.saveFolder);
        }

        private string GetSaveFilePath(int slotIndex)
        {
            string folder = GetSaveFolderPath();
            string fileName = slotIndex == 0 
                ? settings.autoSaveFileName 
                : $"{settings.saveFilePrefix}{slotIndex}";
            return Path.Combine(folder, $"{fileName}.json");
        }

        private string GetMetaFilePath(int slotIndex)
        {
            return GetSaveFilePath(slotIndex) + ".meta";
        }
        
        // =============================
        // 💾 保存到指定槽位
        // =============================

        public void SaveToSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > settings.maxSlotCount)
            {
                LoggerManager.Instance.LogError($"[SaveSystem] 无效槽位：{slotIndex}");
                return;
            }

            // 创建目录
            string path = GetSaveFilePath(slotIndex);
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            // 保存主数据
            var allData = SaveRegistry.SaveAll();
            var wrapper = new SaveDataWrapper { data = allData };
            string json = JsonConvert.SerializeObject(wrapper,  new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All  // 保留类型信息
            });
            File.WriteAllText(path, json);

            LoggerManager.Instance.Log($"[SaveSystem] 已保存到槽位 {slotIndex}：{path}");

            // 保存元数据（用于菜单显示）
            if (settings.saveMetadata)
            {
                SaveSlotMetadata(slotIndex);
            }

            EventManager.Instance.TriggerGameSaved();
        }

        // =============================
        // 📂 加载指定槽位
        // =============================

        public bool LoadFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > settings.maxSlotCount)
            {
                LoggerManager.Instance.LogError($"[SaveSystem] 无效槽位：{slotIndex}");
                return false;
            }

            string path = GetSaveFilePath(slotIndex);
            if (!File.Exists(path))
            {
                LoggerManager.Instance.LogWarning($"[SaveSystem] 存档不存在：{path}");
                return false;
            }

            string json = File.ReadAllText(path);
            try
            {
                var wrapper = JsonConvert.DeserializeObject<SaveDataWrapper>(json,new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                SaveRegistry.LoadAll(wrapper.data);
                LoggerManager.Instance.Log($"[SaveSystem] 成功加载槽位 {slotIndex}");
                EventManager.Instance.TriggerGameLoaded();
                return true;
            }
            catch (JsonException e)
            {
                LoggerManager.Instance.LogError($"[SaveSystem] JSON 解析失败：{e.Message}");
                return false;
            }
        }

        // =============================
        // 🧹 删除存档
        // =============================

        public void DeleteSave(int slotIndex)
        {
            string path = GetSaveFilePath(slotIndex);
            string metaPath = GetMetaFilePath(slotIndex);

            if (File.Exists(path)) File.Delete(path);
            if (File.Exists(metaPath)) File.Delete(metaPath);

            LoggerManager.Instance.Log($"[SaveSystem] 已删除槽位 {slotIndex} 的存档");
        }

        // =============================
        // 🕒 自动保存
        // =============================

        public void AutoSave()
        {
            SaveToSlot(0);
        }

        public DateTime? GetAutoSaveTime()
        {
            string path = GetSaveFilePath(0);
            try
            {
                return File.Exists(path) ? File.GetLastWriteTime(path) : (DateTime?)null;
            }
            catch (System.Exception e)
            {
                LoggerManager.Instance.LogError($"无法读取自动存档时间：{e.Message}");
                return null;
            }
        }

        // =============================
        // 📊 元数据管理（用于 UI 显示）
        // =============================

        private void SaveSlotMetadata(int slotIndex)
        {
            var meta = new SaveMetadata
            {
                slotIndex = slotIndex,
                lastPlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                playTime = "1h 30m", // 可从 GameManager 获取
                playerName = PlayerManager.Instance?.GetPlayer()?.name ?? "未知",
                sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            };

            string metaPath = GetMetaFilePath(slotIndex);
            string json = JsonConvert.SerializeObject(meta, Formatting.Indented);
            File.WriteAllText(metaPath, json);
        }

        public SaveSlotData GetSlotData(int slotIndex)
        {
            var data = new SaveSlotData
            {
                slotIndex = slotIndex,
                hasData = HasSaveInSlot(slotIndex)
            };

            if (data.hasData)
            {
                string path = GetSaveFilePath(slotIndex);
                DateTime time = File.GetLastWriteTime(path);
                data.lastPlayedTime = time.ToString("yyyy-MM-dd HH:mm"); // 或 "MM/dd HH:mm"

                // 读取 .meta 文件获取角色信息（可选）
                if (settings.saveMetadata)
                {
                    string metaPath = GetMetaFilePath(slotIndex);
                    if (File.Exists(metaPath))
                    {
                        string metaJson = File.ReadAllText(metaPath);
                        try
                        {
                            SaveMetadata meta = JsonConvert.DeserializeObject<SaveMetadata>(metaJson);
                            data.playerName = meta.playerName;
                        }
                        catch (JsonException e)
                        {
                            LoggerManager.Instance.LogError($"[SaveSystem] 解析元数据失败：{e.Message}");
                        }
                    }
                }
            }
            else
            {
                data.lastPlayedTime = "空";
                data.playerName = "-";
            }

            return data;
        }

        public List<SaveSlotData> GetAllSlotData()
        {
            var list = new List<SaveSlotData>();
            for (int i = 1; i <= settings.maxSlotCount; i++)
            {
                var slotData = GetSlotData(i);
                list.Add(slotData);
                // 找到lastPlayedTime最新数据
                if (slotData.hasData)
                {
                    if (lastSlotIndex == -1)
                    {
                        lastSlotIndex = i;
                    }
                    else
                    {
                        DateTime currentLastTime = DateTime.Parse(list[lastSlotIndex - 1].lastPlayedTime);
                        DateTime newTime = DateTime.Parse(slotData.lastPlayedTime);
                        if (newTime > currentLastTime)
                        {
                            lastSlotIndex = i;
                        }
                    }
                }
            }
            return list;
        }

        // =============================
        // 🔍 查询
        // =============================

        public bool HasSaveInSlot(int slotIndex)
        {
            return File.Exists(GetSaveFilePath(slotIndex));
        }
    }

    // 用于包装 Dictionary<string, object>，因为 Json.NET 不能直接序列化 object 类型的 Dictionary
    [System.Serializable]
    public class SaveDataWrapper
    {
        public Dictionary<string, object> data;
    }
}