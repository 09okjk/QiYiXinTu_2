using System.Collections.Generic;
using Core;
using Save;
using UnityEngine;

namespace Tools
{
    public class SaveRegistry:MonoBehaviour
    {
        private static readonly Dictionary<string, ISaveable> saveables = new Dictionary<string, ISaveable>();

        public static void Register(ISaveable saveable)
        {
            string key = saveable.GetType().Name;
            if (!saveables.ContainsKey(key))
            {
                saveables[key] = saveable;
            }
        }

        public static void Unregister(ISaveable saveable)
        {
            saveables.Remove(saveable.GetType().Name);
        }

        public static Dictionary<string, object> SaveAll()
        {
            var data = new Dictionary<string, object>();
            foreach (var kvp in saveables)
            {
                data[kvp.Key] = kvp.Value.SaveData();
            }
            return data;
        }

        public static void LoadAll(Dictionary<string, object> data)
        {
            foreach (var kvp in saveables)
            {
                if (data.TryGetValue(kvp.Key, out object d))
                {
                    kvp.Value.LoadData(d);
                }
            }
        }
    }
}