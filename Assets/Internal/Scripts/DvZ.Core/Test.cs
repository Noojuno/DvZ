using Runed.Voxel;
using UnityEngine;

namespace DvZ.Core
{
    public class Test : MonoBehaviour
    {
        public Texture2D texture;
        public Texture2D tex2;

        public Vector3Int pos;

        void Awake()
        {
            BlockManager.Initialize();
            TextureManager.Initialize();
        }

        void Start()
        {
            this.texture = TextureManager.GetTexture("blocks.blocktesttwo");

            //this.tex2 = Resources.Load<Texture2D>("Textures/lapis_ore");
            
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                Debug.Log(WorldManager.Active.GetBlock(pos).Definition.Identifier);
            }
        }
    }
}
