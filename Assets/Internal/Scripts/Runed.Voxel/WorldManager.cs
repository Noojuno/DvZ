using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;

        public static World Active => WorldManager.Instance._activeWorld;

        public List<World> Worlds = new List<World>();

        private World _activeWorld;

        void Awake()
        {
            WorldManager.Instance = this;
            this.Worlds = new List<World>();

            this._activeWorld = new World(6969);
            this.RegisterWorld(this._activeWorld);
        }

        void Start()
        {

        }

        public void RegisterWorld(World world)
        {
            this.Worlds.Add(world);
        }

        void Update()
        {
            foreach (var world in this.Worlds)
            {
                world.Update();
            }
        }
    }
}
