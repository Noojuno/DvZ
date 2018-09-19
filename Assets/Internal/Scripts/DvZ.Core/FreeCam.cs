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
        private float lastHitEnd;

        public void OnGUI()
        {
            GUI.Label(new Rect(10, 150, 150, 25), $"{this.lookPos}");
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

                var hitPoint = hit.point + new Vector3(0.5f, 0.5f, 0.5f);

                if (Input.GetMouseButtonUp(0))
                {
                    var vec = new Vector3Int(Mathf.FloorToInt(hitPoint.x), Mathf.FloorToInt(hitPoint.y),
                        Mathf.FloorToInt(hitPoint.z));

                    this.lastHitStart = transform.position;
                    this.lastHitEnd = hit.distance;

                    var pos = hit.point;
                    var dir = fwd.normalized;
                    dir = Quaternion.Inverse(Quaternion.identity) * dir;

                    Vector3Int dirS = Vector3Int.RoundToInt(new Vector3(dir.x > 0 ? 1 : -1, dir.y > 0 ? 1 : -1, dir.z > 0 ? 1 : -1));
                    var dirZ = Vector3Int.FloorToInt(new Vector3(dir.x > 0 ? 1 : -1, dir.y > 0 ? 1 : -1, dir.z > 0 ? 1 : -1));

                    var aPos = new Vector3Int(ResolveBlockPos(pos.x, dirS.x), ResolveBlockPos(pos.y, dirS.y),
                        ResolveBlockPos(pos.z, dirS.z));

                    var bPos = new Vector3Int(AResolveBlockPos(pos.x, dirS.x), AResolveBlockPos(pos.y, dirS.y),
                        AResolveBlockPos(pos.z, dirS.z));

                    Debug.Log($"{hit.point} {aPos} {bPos} {dirS} {dirZ}");

                    WorldManager.Active.SetBlock(aPos, BlockManager.GetBlock("air"));
                }
            }

            if (this.lastHitEnd != null && this.lastHitStart != null)
            {
                Debug.DrawRay(this.lastHitStart, transform.TransformDirection(Vector3.forward) * (this.lastHitEnd + 0.5f),
                    Color.yellow);
            }
        }

        private static int ResolveBlockPos(float pos, int dirS)
        {
            float fPos = pos + 0.5f;
            int iPos = (int)fPos;

            if (Math.Abs(fPos - iPos) < 0.001f)
            {
                if (dirS == 1)
                    return iPos;

                return iPos - 1;
            }

            return Mathf.RoundToInt(pos);
        }

        private static int AResolveBlockPos(float pos, int dirS)
        {
            float fPos = pos + 0.5f;
            int iPos = (int)fPos;

            if (Math.Abs(fPos - iPos) < 0.001f)
            {
                if (dirS == -1)
                    return iPos;

                return iPos - 1;
            }

            return Mathf.RoundToInt(pos);
        }
    }
}