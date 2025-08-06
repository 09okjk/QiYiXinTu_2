using UnityEngine;

namespace PlayerCharacter
{
    public class PlayerManager:MonoBehaviour
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
        }

        private void Start()
        {
            if (player == null)
            {
                Debug.LogError("PlayerManager: Player reference is not set.");
                return;
            }

            if (playerSpawnPoint != null)
            {
                player.transform.position = playerSpawnPoint.position;
                player.transform.rotation = playerSpawnPoint.rotation;
            }
            else
            {
                Debug.LogWarning("PlayerManager: Player spawn point is not set. Using default position.");
            }
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}