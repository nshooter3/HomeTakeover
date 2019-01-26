namespace HomeTakeover.Character
{
    using UnityEngine;
    using HomeTakeover.Util;
    using System.Collections.Generic;

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rgdb;

        public float speed;
        public float jumpspeed;

        public Transform left, right;
        private List<Transform> raycastCheckPoints;
        private float raycastDistance = 0.1f;

        public LayerMask layerMask;

        private bool grounded = true;

        private void Start()
        {
            rgdb = GetComponent<Rigidbody2D>();
            raycastCheckPoints = new List<Transform>();
            raycastCheckPoints.Add(left);
            raycastCheckPoints.Add(transform);
            raycastCheckPoints.Add(right);
        }

        /// <summary> True if the player is pressing a movement key. </summary>
        private bool Moving
        {
            get
            {
                return CustomInput.BoolHeld(CustomInput.UserInput.Left) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Right) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Up) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Down);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private bool IsGrounded()
        {
            RaycastHit2D hit;
            foreach (Transform trans in raycastCheckPoints)
            {
                hit = Physics2D.Raycast(trans.position, trans.TransformDirection(Vector2.down), raycastDistance, layerMask);
                if (hit.collider != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void Move()
        {
            Vector2 vel;
            int x;

            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
                x = -1;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
                x = 1;
            else
                x = 0;
            
            vel = new Vector2(x*speed*Time.deltaTime, rgdb.velocity.y);
            rgdb.velocity = vel;

            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Jump) && IsGrounded())
            {
                vel = new Vector2(vel.x, jumpspeed);
            }

            rgdb.velocity = vel;
        }
    }
}
