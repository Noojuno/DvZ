using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;
using ThreadPriority = System.Threading.ThreadPriority;

namespace VoxelEngine
{
    // This is the main class that manages all the chunks that create the voxel terrain
    // It is resonsible for loading and unloading chunks as the target transform moves around

    public class VoxelEngineManager : MonoBehaviour
    {
        // More low level voxel engine settings can be found in Chunk.cs

        private static readonly ObjectPool<Chunk> chunkPool = new ObjectPool<Chunk>(128);
        private float averageFPS;
        public Light cameraLight;
        private readonly Dictionary<Vector3i, Chunk> chunkMap = new Dictionary<Vector3i, Chunk>();
        private readonly Queue<Vector3i> chunkMeshQueue = new Queue<Vector3i>();
        private readonly ChunkQueue chunkQueue = new ChunkQueue();
        private readonly Stack<Chunk> chunkUnloadStack = new Stack<Chunk>();
        private float deltaTimeFPS;

        public Light directionalLight;
        public float loadDistance = 400f;
        public int maxThreads = 8;
        private int meshesLastFrame;
        public Material meshMaterial;
        public bool showDebugInfo = true;
        public float targetFPS = 100f;
        public Transform targetTransform;
        public TerrainGeneratorBase terrainGenerator;
        private int threadCount;
        public float unloadDistanceModifier = 1.2f;
        private int unloadTick;
        private int updateTimerLastFrame;
        public float yDistanceModifier = 1.5f;

        private int yLoadTick = -1;

        private void Start()
        {
            this.averageFPS = this.targetFPS;

            if (this.showDebugInfo)
                Debug.Log("FastNoiseSIMD level: " + FastNoiseSIMD.GetSIMDLevel());

            this.ResetAll();
        }

        // Draw debug info and terrain generator buttons
        private void OnGUI()
        {
            var labelSpacing = 18;
            var rect = new Rect(4, 0, 300, 20);

            if (this.showDebugInfo)
            {
                GUI.Label(rect, "Pooled Chunks: " + chunkPool.Count);
                rect.y += labelSpacing;
                GUI.Label(rect, "Pooled Chunk GameObjects: " + Chunk.chunkGameObjectPool.Count);
                rect.y += labelSpacing;
                GUI.Label(rect, "Chunks Loaded: " + this.chunkMap.Count);
                rect.y += labelSpacing;
                GUI.Label(rect, "Chunk Queue: " + this.chunkQueue.Count);
                rect.y += labelSpacing;
                GUI.Label(rect, "Chunk Mesh Queue: " + this.chunkMeshQueue.Count);
                rect.y += labelSpacing;
                GUI.Label(rect, "Meshes Last Frame: " + this.meshesLastFrame);
                rect.y += labelSpacing;
                GUI.Label(rect, "Update Time Last Frame: " + this.updateTimerLastFrame + "ms");
                rect.y += labelSpacing;
                GUI.Label(rect, "Thread Count: " + this.threadCount);
                rect.y += labelSpacing;
                GUI.Label(rect, "FPS: " + string.Format("{0:0.0}", this.averageFPS));
            }

            rect = new Rect(Screen.width - 172, 2, 170, 20);
            labelSpacing = 22;

            if (GUI.Button(rect, "Grass Hills"))
            {
                this.terrainGenerator = FindObjectOfType<TerrainGenerator_GrassLand>();
                this.ResetAll();
            }

            rect.y += labelSpacing;

            if (GUI.Button(rect, "Alien Planet"))
            {
                this.terrainGenerator = FindObjectOfType<TerrainGenerator_AlienPlanet>();
                this.ResetAll();
            }

            rect.y += labelSpacing;

            if (GUI.Button(rect, "Cracked Surface"))
            {
                this.terrainGenerator = FindObjectOfType<TerrainGenerator_CrackedSurface>();
                this.ResetAll();
            }

            rect.y += labelSpacing;

            if (GUI.Button(rect, "Desert (SIMD)"))
            {
                this.terrainGenerator = FindObjectOfType<TerrainGeneratorSIMD_Desert>();
                this.ResetAll();
            }

            rect.y += labelSpacing;

            if (GUI.Button(rect, "Floating Islands (SIMD)"))
            {
                this.terrainGenerator = FindObjectOfType<TerrainGeneratorSIMD_FloatingIslands>();
                this.ResetAll();
            }

            rect.y += labelSpacing;

            if (GUI.Button(rect, "Caves (SIMD)"))
            {
                this.terrainGenerator = FindObjectOfType<TerrainGeneratorSIMD_Caves>();
                this.ResetAll(true);
            }
        }

        private void ResetAll(bool useCameraLight = false)
        {
            this.UnloadAllChunks();
            this.targetTransform.position = new Vector3(0, 50, 0);

            if (this.cameraLight && this.directionalLight)
            {
                this.cameraLight.enabled = useCameraLight;
                this.directionalLight.enabled = !useCameraLight;
            }
        }

        private void Update()
        {
            this.deltaTimeFPS += (Time.deltaTime - this.deltaTimeFPS) * 0.1f;

            this.averageFPS = Mathf.Lerp(this.averageFPS, 1f / this.deltaTimeFPS, 0.05f);

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        // Uses called in late update since it is called after corountine updates allowing it to start new threads if they have just finished
        // Not using fixed update so that the updates will speed up/slow down based on PC performance
        private void LateUpdate()
        {
            var updateTimer = new Stopwatch();
            updateTimer.Start();

            this.UpdateLoadingQueue();

            this.CheckUnloadChunks();

            this.LoadChunksFromQueue();

            this.MeshChunksFromQueue(updateTimer);

            // For debug info
            this.updateTimerLastFrame = (int) updateTimer.ElapsedMilliseconds;
        }

        private void UpdateLoadingQueue()
        {
            // All distance checks use the distance squared since it saves calulcating a square root for every distance
            var loadDistanceSq = this.loadDistance * this.loadDistance;
            // Load distances in chunks to know how far to extend the for loop from the player's chunk
            var loadDistanceChunk = ((Mathf.CeilToInt(this.loadDistance) - Chunk.SIZE2) >> Chunk.BIT_SIZE) + 1;
            var loadDistanceChunkY = Mathf.CeilToInt(loadDistanceChunk * this.yDistanceModifier);

            // How much to sections to stagger the chunk location checking
            const int yCheckDelay = 8;

            var chunkPos = new Vector3i();
            var chunkRealPos = new Vector3();
            var targetChunk = new Vector3i(
                Mathf.RoundToInt(this.targetTransform.position.x) >> Chunk.BIT_SIZE,
                Mathf.RoundToInt(this.targetTransform.position.y) >> Chunk.BIT_SIZE,
                Mathf.RoundToInt(this.targetTransform.position.z) >> Chunk.BIT_SIZE);

            // yLoadTick staggers chunk location checking to reduce time spent each frame
            for (var y = this.yLoadTick - loadDistanceChunkY; y < loadDistanceChunkY; y += yCheckDelay)
            {
                chunkPos.y = targetChunk.y + y;
                chunkRealPos.y = ((y + targetChunk.y) << Chunk.BIT_SIZE) + Chunk.SIZE2;

                for (var x = -loadDistanceChunk; x < loadDistanceChunk; x++)
                {
                    chunkPos.x = targetChunk.x + x;
                    chunkRealPos.x = ((x + targetChunk.x) << Chunk.BIT_SIZE) + Chunk.SIZE2;

                    for (var z = -loadDistanceChunk; z < loadDistanceChunk; z++)
                    {
                        chunkPos.z = targetChunk.z + z;

                        // Don't try to queue the chunk location if is already loaded or already in queue
                        if (this.chunkMap.ContainsKey(chunkPos) || this.chunkQueue.Contains(chunkPos))
                            continue;

                        chunkRealPos.z = ((z + targetChunk.z) << Chunk.BIT_SIZE) + Chunk.SIZE2;

                        var distanceSq = this.ScaledTargetDistanceSq(chunkRealPos);

                        if (distanceSq < loadDistanceSq) this.chunkQueue.Enqueue(distanceSq, chunkPos);
                    }
                }
            }

            // Increment the yLoadTick so that different locations will be checked next frame
            if (++this.yLoadTick >= yCheckDelay) this.yLoadTick = 0;
        }

        private void CheckUnloadChunks()
        {
            var unloadDistanceSq = this.loadDistance * this.loadDistance * this.unloadDistanceModifier *
                                   this.unloadDistanceModifier;

            // Unloading sections stagger must be (2^n)-1
            const int unloadTickMax = 31;

            // Check if chunk is in stagger section then if it is outside the unload distance
            foreach (var chunk in this.chunkMap.Values.Where(chunk =>
                (chunk.chunkPos.y & unloadTickMax) != this.unloadTick &&
                this.ScaledTargetDistanceSq(chunk.realPos) > unloadDistanceSq))
                this.chunkUnloadStack.Push(chunk);

            if (++this.unloadTick > unloadTickMax) this.unloadTick = 0;

            // Unload chunks outside the foreach to avoid removing elements causing errors
            while (this.chunkUnloadStack.Count != 0) this.UnloadChunk(this.chunkUnloadStack.Pop());
        }

        private void LoadChunksFromQueue()
        {
            var chunkPos = new Vector3i();

            var adjustedMaxThreads = Mathf.RoundToInt(this.maxThreads - this.chunkMeshQueue.Count * 0.2f);

            while (this.threadCount < adjustedMaxThreads)
            {
                // Get the closest chunk location from the queue if one exists
                if (!this.chunkQueue.Dequeue(out chunkPos))
                    break;

                // Threaded
                this.StartCoroutine(this.LoadChunkThreaded(chunkPos));

                // Not threaded
                //LoadChunkThreaded(chunkPos);
            }
        }

        private void MeshChunksFromQueue(Stopwatch updateTimer)
        {
            // For debug info
            this.meshesLastFrame = 0;

            // Allow more time meshing if above target FPS
            var milliMax = Mathf.RoundToInt(this.averageFPS - this.targetFPS);

            while (this.chunkMeshQueue.Count > 0)
            {
                Chunk chunk;

                // Try and get the chunk from it's postion (it may have been unloaded since it was added to queue)
                if (!this.chunkMap.TryGetValue(this.chunkMeshQueue.Dequeue(), out chunk))
                    continue;

                // This should always be true, but adjacent chunks may have unloaded since being added to queue
                if (chunk.CanBuildMesh())
                {
                    chunk.BuildMesh();
                    this.meshesLastFrame++;
                }
                else
                {
                    continue;
                }

                // Stop meshing if too long has been spent updating this frame
                // This is at the end of the loop to ensure at least 1 mesh will generate per frame
                if (updateTimer.ElapsedMilliseconds >= milliMax)
                    break;
            }
        }

        // Get distance squared to target using the yDistanceModifier
        public float ScaledTargetDistanceSq(Vector3 realPos)
        {
            return new Vector3(this.targetTransform.position.x - realPos.x,
                (this.targetTransform.position.y - realPos.y) * this.yDistanceModifier,
                this.targetTransform.position.z - realPos.z).sqrMagnitude;
        }

        public void LoadChunk(Vector3i chunkPos)
        {
            var chunk = chunkPool.Get();
            chunk.Setup(chunkPos, this);

            // Skip generating if outside terrain generator bounds
            if (chunk.CheckTerrainBounds())
                chunk.GenerateVoxelData();

            // Notify adjacent chunks this chunk can be used for meshing
            chunk.FillAdjChunks();
            // Added the chunk to the dictionary
            this.chunkMap.Add(chunkPos, chunk);
            // Mark the chunk position as complete and remove from the queue
            this.chunkQueue.Remove(chunkPos);
        }

        public IEnumerator LoadChunkThreaded(Vector3i chunkPos)
        {
            var chunk = chunkPool.Get();
            chunk.Setup(chunkPos, this);

            // Skip generating if outside terrain generator bounds
            if (chunk.CheckTerrainBounds())
            {
                // Start a new thread to generate the voxel data
                this.threadCount++;
                var done = false;
                var thread = new Thread(() =>
                {
                    chunk.GenerateVoxelData();
                    done = true;
                })
                {
                    Priority = ThreadPriority.BelowNormal
                };

                thread.Start();

                // Corountine waits for the thread to finish before continuing on the main thread
                while (!done)
                    yield return null;

                this.threadCount--;
            }

            // Notify adjacent chunks this chunk can be used for meshing
            chunk.FillAdjChunks();

            // Added the chunk to the dictionary
            this.chunkMap.Add(chunkPos, chunk);

            // Mark the chunk position as complete and remove from the queue
            // This is needed for threaded loading as there is a delay between dequeuing and it being added to the chunkMap
            this.chunkQueue.Remove(chunkPos);
        }

        // Clear the chunkMap and all queues
        // Use this if changing terrain/meshing to load with updated values
        public void UnloadAllChunks()
        {
            // Stop all threaded chunk loading and reset thread counter
            this.StopAllCoroutines();
            this.threadCount = 0;

            foreach (var chunk in this.chunkMap.Values) this.chunkUnloadStack.Push(chunk);

            while (this.chunkUnloadStack.Count != 0) this.UnloadChunk(this.chunkUnloadStack.Pop());

            this.chunkQueue.Clear();
            this.chunkMeshQueue.Clear();
        }

        public void UnloadChunk(Chunk chunk)
        {
            this.chunkMap.Remove(chunk.chunkPos);

            // Try to add the chunk object to the pool, if not destroy it
            if (chunkPool.Add(chunk))
                chunk.Clean();
            else
                chunk.Destroy();
        }

        // Try and get a chunk, returns null if chunk is not loaded
        public Chunk GetChunk(Vector3i chunkPos)
        {
            Chunk chunk;
            this.chunkMap.TryGetValue(chunkPos, out chunk);
            return chunk;
        }

        // Returns a chunk, if the chunk is not loaded this will throw an exeption
        public Chunk GetChunkUnsafe(Vector3i chunkPos)
        {
            return this.chunkMap[chunkPos];
        }

        // Used by chunks to queue themselves for meshing
        public void QueueChunkMeshing(Vector3i chunkPos)
        {
            this.chunkMeshQueue.Enqueue(chunkPos);
        }
    }
}