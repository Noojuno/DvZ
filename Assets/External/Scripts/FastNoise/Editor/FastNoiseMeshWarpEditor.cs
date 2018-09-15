using UnityEditor;
using UnityEngine;

namespace FastNoiseC
{
    [CustomEditor(typeof(FastNoiseMeshWarp))]
    public class FastNoiseMeshWarpEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (Application.isPlaying && GUILayout.Button("Recalculate"))
            {
                FastNoiseMeshWarp fastNoiseMeshWarp = ((FastNoiseMeshWarp)target);

                fastNoiseMeshWarp.WarpAllMeshes();
            }
        }
    }

}