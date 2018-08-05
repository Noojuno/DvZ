using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runed.Voxel
{
    public static class WorldManager
    {
        public static List<World> Worlds = new List<World>();
        public static World Current;

        /* public static void RegisterWorld(World world)
        {
            WorldManager.Worlds.Add(world);
        } */

        public static void Update()
        {
            /* foreach (var world in WorldManager.Worlds)
            {
                world.Update();
            } */

            WorldManager.Current.Update();
        }
    }
}
