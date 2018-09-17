using System.Collections.Generic;
using UnityEngine;

namespace Runed.Utilities
{
    public class Pool
    {
        private readonly GameObject prefab;
        private readonly Stack<GameObject> inactive;
        private int nextId = 0;

        public Pool(GameObject prefab, int count=1)
        {
            this.prefab = prefab;
            this.inactive = new Stack<GameObject>(count);
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject newGameObject;

            if (inactive.Count == 0)
            {
                newGameObject = GameObject.Instantiate(this.prefab, position, rotation);
                newGameObject.name = prefab.name + " (" + (nextId++) + ")";

                return newGameObject;
            }
            else
            {
                newGameObject = inactive.Pop();

                if (newGameObject == null)
                {
                    return Spawn(position, rotation);
                }
            }

            newGameObject.transform.position = position;
            newGameObject.transform.rotation = rotation;
            newGameObject.SetActive(true);

            return newGameObject;
        }

        public void Despawn(GameObject gameObject)
        {
            gameObject.SetActive(false);

            inactive.Push(gameObject);
        }
    }
}
