using System;
using System.Collections.Generic;
using Save;
using Tools;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class GameStateData
    {
        public Dictionary<string,bool> BoolFlags = new Dictionary<string, bool>();
        public Dictionary<string,int> INTFlags = new Dictionary<string, int>();
        public Dictionary<string,float> FloatFlags = new Dictionary<string, float>();
        public Dictionary<string,string> StringFlags = new Dictionary<string, string>();
    }
    public class GameStateManager:MonoBehaviour, ISaveable
    {
        public static GameStateManager Instance;
        private GameStateData _gameStateData = new GameStateData();
        
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
            SaveRegistry.Register(this);
        }

        public object SaveData()
        {
            return _gameStateData;
        }

        public void LoadData(object data)
        {
            if (data is GameStateData loadedData)
            {
                _gameStateData = loadedData;
            }
            else
            {
                LoggerManager.Instance.LogError("GameStateManager: Failed to load data - invalid data type.");
            }
        }
        
        public void SetFlag(string key, bool value)
        {
            _gameStateData.BoolFlags[key] = value;
        }
        public void SetFlag(string key, int value)
        {
            _gameStateData.INTFlags[key] = value;
        }
        public void SetFlag(string key, float value)
        {
            _gameStateData.FloatFlags[key] = value;
        }
        public void SetFlag(string key, string value)
        {
            _gameStateData.StringFlags[key] = value;
        }

        public bool GetBoolFlag(string flagName)
        {
            if (_gameStateData.BoolFlags.TryGetValue(flagName, out bool value))
            {
                return value;
            }
            LoggerManager.Instance.LogWarning($"GameStateManager: Bool flag '{flagName}' not found. Returning false.");
            return false;
        }

        public int GetIntFlag(string flagName)
        {
            if (_gameStateData.INTFlags.TryGetValue(flagName, out int value))
            {
                return value;
            }
            LoggerManager.Instance.LogWarning($"GameStateManager: Int flag '{flagName}' not found. Returning 0.");
            return -2000;
        }
        
        public float GetFloatFlag(string flagName)
        {
            if (_gameStateData.FloatFlags.TryGetValue(flagName, out float value))
            {
                return value;
            }
            LoggerManager.Instance.LogWarning($"GameStateManager: Float flag '{flagName}' not found. Returning 0.");
            return -2000f;
        }
        
        public string GetStringFlag(string flagName)
        {
            if (_gameStateData.StringFlags.TryGetValue(flagName, out string value))
            {
                return value;
            }
            LoggerManager.Instance.LogWarning($"GameStateManager: String flag '{flagName}' not found. Returning empty string.");
            return "";
        }
    }
}