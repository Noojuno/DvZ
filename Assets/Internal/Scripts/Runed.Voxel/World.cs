namespace Runed.Voxel
{
    public class World
    {
        public int Seed = 0;
        public TerrainGenerator TerrainGenerator;

        public World(int seed)
        {
            this.Seed = seed;

            this.TerrainGenerator = new TerrainGenerator(this.Seed);
        }

        public void Update()
        {

        }
    }
}