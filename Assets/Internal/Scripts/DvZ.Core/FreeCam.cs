using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runed.Voxel;
using UnityEngine;

namespace DvZ.Core
{
    public class FreeCam : MonoBehaviour
    {
        [Header("Input Keys")] public KeyCode forwardKey = KeyCode.W;
        public KeyCode backKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode upKey = KeyCode.Space;
        public KeyCode downKey = KeyCode.LeftShift;
        public KeyCode fastKey = KeyCode.LeftControl;

        [Header("Camera Variables")] public float speed = 1.0f;
        public float fastMultiplier = 1.5f;

        private Vector3 lookPos;

        private Vector3 lastHitStart;
        private Vector3 lastHitEnd;
        private Vector3 lastHitEnd2;

        public void OnGUI()
        {
            GUI.Label(new Rect(10, 150, 250, 25), $"Looking at: {this.lookPos}");
        }

        public void Update()
        {
            var currSpeed = this.speed;

            if (Input.GetKey(this.fastKey)) currSpeed = this.speed * this.fastMultiplier;

            if (Input.GetKey(this.forwardKey))
            {
                this.transform.position += transform.forward * currSpeed;
            }

            if (Input.GetKey(this.backKey))
            {
                this.transform.position += -transform.forward * currSpeed;
            }

            if (Input.GetKey(this.leftKey))
            {
                this.transform.position += -transform.right * currSpeed;
            }

            if (Input.GetKey(this.rightKey))
            {
                this.transform.position += transform.right * currSpeed;
            }

            if (Input.GetKey(this.upKey))
            {
                this.transform.position += Vector3.up * currSpeed;
            }

            if (Input.GetKey(this.downKey))
            {
                this.transform.position += Vector3.down * currSpeed;
            }

            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, fwd, out RaycastHit hit, 20))
            {
                this.lookPos = hit.point;

                if (Input.GetMouseButtonUp(0))
                {
                    this.SetBlockAt(hit, BlockDefinition.Air);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    this.SetBlockAt(hit, BlockManager.GetBlock("test"), true);
                }
            }

            Debug.DrawLine(this.lastHitStart, this.lastHitEnd, Color.yellow);
        }

        public void SetBlockAt(RaycastHit hit, BlockDefinition block, bool adjacent = false)
        {
            int multiplier = adjacent ? 1 : -1;

            Vector3Int position = Vector3IntUtil.FloorFromVector3(hit.point + hit.normal * (multiplier * 0.5f));
            position.x += 1;

            this.lastHitStart = transform.position;
            this.lastHitEnd = position;

            Debug.Log($"{hit.point} {position} {hit.normal}");

            WorldManager.Active.SetBlock(position, block);
        }
    }
}