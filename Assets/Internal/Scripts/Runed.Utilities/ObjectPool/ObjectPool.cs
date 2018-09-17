using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runed.Utilities {
    public static class ObjectPool
    {
        public const int DEFAULT_SIZE = 3;

        private static Dictionary<GameObject, Pool> _pools;

        public static void Initialise(GameObject gameObject = null, int qty = DEFAULT_SIZE)
        {
            if (_pools == null)
            {
                _pools = new Dictionary<GameObject, Pool>();
            }

            if (gameObject != null && !_pools.ContainsKey(gameObject))
            {
                _pools[gameObject] = new Pool(gameObject, qty);
            }
        }

        public static GameObject Spawn(GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            Initialise(gameObject);

            return _pools[gameObject].Spawn(position, rotation);
        }

        public static void Despawn(GameObject gameObject)
        {
            PoolMember poolMember = gameObject.GetComponent<PoolMember>();
            if (poolMember == null)
            {
                Debug.Log("Object '" + gameObject.name + "' wasn't spawned from a pool. Destroying it instead.");
                GameObject.Destroy(gameObject);
            }
            else
            {
                poolMember.ActivePool.Despawn(gameObject);
            }
        }

        public static void Preload(GameObject gameObject, int count)
        {
            Initialise(gameObject);

            GameObject[] preloadedObjects = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                preloadedObjects[i] = Spawn(gameObject, Vector3.zero, Quaternion.identity);
            }

            for (int i = 0; i < count; i++)
            {
                Despawn(preloadedObjects[i]);
            }
        }
    }
}
