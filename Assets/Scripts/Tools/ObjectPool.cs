using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class ObjectPool<T> where T : Component
    {
        private Queue<T> pool = new Queue<T>();
        private T prefab;
        private Transform parent;
        private int defaultSize;

        public ObjectPool(T prefab, Transform parent, int defaultSize = 10)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.defaultSize = defaultSize;
            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < defaultSize; i++)
            {
                T obj = Object.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            if (pool.Count > 0)
            {
                T obj = pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                return Object.Instantiate(prefab, parent);
            }
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        public void ReturnAll()
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                    {
                        Return(component);
                    }
                }
            }
        }
    }
}