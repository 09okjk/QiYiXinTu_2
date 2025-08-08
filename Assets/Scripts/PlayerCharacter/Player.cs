using System;
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
    }
}