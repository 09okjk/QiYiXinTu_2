using Tools;
using UnityEngine;

namespace NpcCharacter
{
    public class Npc:MonoBehaviour
    {
        private NpcGameData _gameData;
        private Transform _followTarget;
        
        private NpcManager _npcManager;
        
        private void Awake()
        {
            _npcManager = NpcManager.Instance;
            if (_npcManager == null)
            {
                LoggerManager.Instance.LogError("NpcManager instance not found!");
            }
        }
        
        public void Initialize(NpcGameData gameData)
        {
            _gameData = gameData;
            transform.position = gameData.position;
            transform.eulerAngles = gameData.rotation;
            gameObject.name = gameData.npcName;
            
            gameObject.SetActive(gameData.isActive);
        }

        // 重新计算位置，朝向，移动速度和跟随对象等
        public void UpdateStatus()
        {
            // 设置跟随目标
            _followTarget = _npcManager.GetFollowTarget(_gameData.ID).transform;
        }
    }
}