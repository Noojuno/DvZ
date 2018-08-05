using Runed.Voxel;
using UnityEngine;

namespace DvZ.Core
{
    public class Test : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                BlockManager.Initialize();
                Debug.Log(BlockManager.Export());
            }
        }
    }
}
