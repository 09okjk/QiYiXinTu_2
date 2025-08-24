using System;
using Core;
using Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerCharacter
{
    [Serializable]
    public class PlayerData
    {
        public string playerName;
        public int Health = 5;
        public int Stamina = 5;
        public int Mana = 5;
        public float moveSpeed = 5f;
        public Vector3 position;
        public Vector3 rotation;
    }
    public class Player:MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        
        public void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                LoggerManager.Instance.LogError("Player name cannot be empty.");
                return;
            }
            playerData.playerName = name;
        }
        
        public string GetPlayerName()
        {
            return playerData.playerName;
        }
        
        public void SetData(PlayerData data)
        {
            if (data == null)
            {
                LoggerManager.Instance.LogError("Player data cannot be null.");
                return;
            }
            playerData = data;
        }
        
        public PlayerData GetPlayerData()
        {
            return playerData;
        }
    }
}