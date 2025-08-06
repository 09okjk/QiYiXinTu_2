using UnityEngine;

namespace PlayerCharacter
{
    public class PlayerDefaultData:ScriptableObject
    {
        public string playerName;
        public int Health = 5;
        public int Stamina = 5;
        public int Mana = 5;
        public float moveSpeed = 5f;
    }
}