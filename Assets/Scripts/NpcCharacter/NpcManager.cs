using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace NpcCharacter
{
    [Serializable]
    public class NpcGameData
    {
        public string ID;
        public string npcName;
        public float moveSpeed = 3f;
        public bool isFollowPlayer = false;
        public bool isActive = false;
        public Vector3 position;
        public Vector3 rotation;
    }
    public class NpcManager:MonoBehaviour
    {
        public static NpcManager Instance { get; private set; }
        public GameObject npcPrefab;
        private Dictionary<string, NpcGameData> npcDataDict = new Dictionary<string, NpcGameData>();
        private ObjectPool<Npc> _npcPool;
        private List<Npc> _activeNpcs = new List<Npc>();
        private List<string> _followNpcIds = new List<string>();
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
            var npcComponent = npcPrefab.GetComponent<Npc>();
            _npcPool = new ObjectPool<Npc>(npcComponent, transform, 10);
        }

        private void Start()
        {
            var npcDataArray = Resources.LoadAll<NpcData>("ScriptableObjects/Npcs");
            foreach (var npcData in npcDataArray)
            {
                NpcGameData gameData = new NpcGameData
                {
                    ID = npcData.ID,
                    npcName = npcData.npcName,
                    moveSpeed = npcData.moveSpeed,
                    isFollowPlayer = npcData.isFollowPlayer,
                    isActive = npcData.isActive,
                    position = npcData.position,
                    rotation = npcData.rotation
                };
                npcDataDict[npcData.ID] = gameData;
            }
        }

        public void InitializeNpcs()
        {
            foreach (var npcDataValue in npcDataDict.Values)
            {
                var npcObject = _npcPool.Get();
                npcObject.Initialize(npcDataValue);
                _activeNpcs.Add(npcObject);
                npcObject.transform.SetParent(transform, false);
            }
        }
        
        public void SaveNpcDatas(Dictionary<string, NpcGameData> data)
        {
            npcDataDict = new Dictionary<string, NpcGameData>(data);
        }
        
        public Dictionary<string, NpcGameData> GetNpcDatas()
        {
            return new Dictionary<string, NpcGameData>(npcDataDict);
        }

        public void ActivateNpc(string npcId)
        {
            var npc = npcDataDict[npcId];
            if (npc != null)
            {
                var npcObject = _npcPool.Get();
                npcObject.Initialize(npc);
                _activeNpcs.Add(npcObject);
                npcObject.transform.SetParent(transform, false);
            }
            AddIntoFollowList(npcId);
            UpdateNpcs();
        }
        
        public void DeactivateNpc(string npcId)
        {
            var npc = _activeNpcs.Find(n => n.gameObject.name == npcId);
            if (npc != null)
            {
                _npcPool.Return(npc);
                _activeNpcs.Remove(npc);
            }
            RemoveFromFollowList(npcId);
            UpdateNpcs();
        }

        public void DeleteNpc(string npcId)
        {
            DeactivateNpc(npcId);
            if (!npcDataDict.Remove(npcId))
            {
                LoggerManager.Instance.LogWarning($"Npc with ID {npcId} not found in data dictionary.");
            }
        }

        public void UpdateNpcs()
        {
            foreach (var activeNpc in _activeNpcs)
            {
                activeNpc.UpdateStatus();
            }
        }

        #region Follow Npc

        private void AddIntoFollowList(string npcId)
        {
            if (!_followNpcIds.Contains(npcId))
            {
                _followNpcIds.Add(npcId);
            }
        }
        
        public void RemoveFromFollowList(string npcId)
        {
            if (_followNpcIds.Contains(npcId))
            {
                _followNpcIds.Remove(npcId);
            }
            // 将队列后面的Npc依次前移
            for (int i = 0; i < _followNpcIds.Count; i++)
            {
                var followNpcId = _followNpcIds[i];
                var npc = _activeNpcs.Find(n => n.gameObject.name == followNpcId);
                if (npc != null)
                {
                    npc.UpdateStatus();
                }
            }
        }

        #endregion
    }
}