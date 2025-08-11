using UnityEngine;

namespace NpcCharacter
{
    public class Npc:MonoBehaviour
    {
        private NpcGameData _gameData;
        private Transform _flollowTarget;
        
        public void Initialize(NpcGameData gameData)
        {
            _gameData = gameData;
            transform.position = gameData.position;
            transform.eulerAngles = gameData.rotation;
            gameObject.name = gameData.npcName;
            gameObject.SetActive(gameData.isActive);
        }

        public void UpdateStatus()
        {
            // 重新计算位置，朝向，移动速度和跟随对象等
        }
    }
}