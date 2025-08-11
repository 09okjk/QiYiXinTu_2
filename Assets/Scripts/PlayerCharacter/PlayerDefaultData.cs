using UnityEngine;

namespace PlayerCharacter
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDefaultData")]
    public class PlayerDefaultData:ScriptableObject
    {
        public string playerName;
        public int Health = 5;
        public int Stamina = 5;
        public int Mana = 5;
        public float moveSpeed = 5f;
    }
}