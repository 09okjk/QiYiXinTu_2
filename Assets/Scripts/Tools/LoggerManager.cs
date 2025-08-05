using UnityEngine;
using UnityEngine.Serialization;

namespace Tools
{
    public class LoggerManager:MonoBehaviour
    {
        public static LoggerManager Instance { get; private set; }
        public bool isDebugMode = true; // 是否启用调试模式

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

        public void Log(object message)
        {
            if (!isDebugMode) return; // 如果不是调试模式，则不输出日志
            Debug.Log(message);
        }

        public void LogWarning(object message)
        {
            if (!isDebugMode) return; // 如果不是调试模式，则不输出警告日志
            Debug.LogWarning(message);
        }

        public void LogError(object message)
        {
            if (!isDebugMode) return; // 如果不是调试模式，则不输出错误日志
            Debug.LogError(message);
        }
        
    }
}