using System;
using Core;
using Save;
using Tools;
using UnityEngine;

namespace PlayerCharacter
{
    public class PlayerManager:MonoBehaviour,ISaveable
    {
        public static PlayerManager Instance { get; private set; }
        private Player player;
        public Transform playerSpawnPoint;

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
            SaveRegistry.Register(this);
        }

        private void Start()
        {
            if (player == null)
            {
                LoggerManager.Instance.LogError("PlayerManager: Player reference is not set.");
                return;
            }

            if (playerSpawnPoint != null)
            {
                player.transform.position = playerSpawnPoint.position;
                player.transform.rotation = playerSpawnPoint.rotation;
            }
            else
            {
                LoggerManager.Instance.LogWarning("PlayerManager: Player spawn point is not set. Using default position.");
            }
        }

        private void OnDestroy()
        {
            SaveRegistry.Unregister(this);
        }

        public Player GetPlayer()
        {
            return player;
        }

        public object SaveData()
        {
            return player.GetPlayerData();
        }

        public void LoadData(object data)
        {
            if (data is PlayerData playerData)
            {
                player.SetData(playerData);
                /*if (playerSpawnPoint != null)
                {
                    player.transform.position = playerSpawnPoint.position;
                    player.transform.rotation = playerSpawnPoint.rotation;
                }*/
            }
            else
            {
                LoggerManager.Instance.LogError("PlayerManager: Invalid data type for loading player data.");
            }
        }
    }
}