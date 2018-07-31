using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    public class Test : MonoBehaviour
    {
        public int a = 0;

        public void Update()
        {
            if (this.a == 100)
            {
                BlockManager.Initialize();
                Debug.Log(BlockManager.Export());
                this.a = 0;
            }
        }
    }
}
