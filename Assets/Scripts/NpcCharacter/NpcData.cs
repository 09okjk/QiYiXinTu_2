using UnityEngine;

namespace NpcCharacter
{
    [CreateAssetMenu(fileName = "NpcData", menuName = "ScriptableObjects/NpcData")]
    public class NpcData:ScriptableObject
    {
        public string ID;
        public string npcName;
        public float moveSpeed = 3f;
        public bool isFollowPlayer = false;
        public bool isActive = false;
        public Vector3 position;
        public Vector3 rotation;
    }
}