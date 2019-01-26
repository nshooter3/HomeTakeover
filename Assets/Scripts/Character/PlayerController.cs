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

        public Transform reticule;
        public Transform defaultArmEndPos;
        public Transform armHolderTransform;
        public SpriteRenderer armSprite;
        Vector2 armSpriteInitSize;

        public Animator anim;

        private float armReturnTimer = 0.0f, maxArmReturnTimer = 0.25f;

        public LayerMask layerMask;

        private bool grounded = true;

        private void Start()
        {
            rgdb = GetComponent<Rigidbody2D>();
            raycastCheckPoints = new List<Transform>();
            raycastCheckPoints.Add(left);
            raycastCheckPoints.Add(transform);
            raycastCheckPoints.Add(right);

            armSpriteInitSize = armSprite.size;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            UpdateArm();
            UpdateAnimations();
        }

        void UpdateArm()
        {
            UpdateArmRotation();
            if (armReturnTimer > 0)
            {
                armReturnTimer = Mathf.Max(0, armReturnTimer -= Time.deltaTime);
                Vector3 pos = Vector3.Lerp(defaultArmEndPos.transform.position, reticule.transform.position, armReturnTimer / maxArmReturnTimer);
                UpdateArmStretch(armSprite, armHolderTransform.position, pos);
            }
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Attack))
            {
                armReturnTimer = maxArmReturnTimer;
            }
        }

        void UpdateArmRotation()
        {
            // Get Angle in Radians
            float AngleRad = Mathf.Atan2(reticule.transform.position.y - armHolderTransform.transform.position.y, reticule.transform.position.x - armHolderTransform.transform.position.x);
            // Get Angle in Degrees
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            // Rotate Object
            armHolderTransform.transform.rotation = Quaternion.Euler(0, 0, AngleDeg + 90);
        }

        public void UpdateArmStretch(SpriteRenderer sprite, Vector2 initialPosition, Vector2 finalPosition)
        {
            float distance = Vector2.Distance(initialPosition, finalPosition);
            float yScale = distance * 1.6f;
            sprite.size = new Vector2(armSpriteInitSize.x - 0.15f*(armReturnTimer / maxArmReturnTimer), armSpriteInitSize.y * yScale);
            float ypos = Mathf.Lerp(0, -0.25f, armReturnTimer / maxArmReturnTimer);
            if (distance <= 2.5f)
            {
                ypos = 0;
            }
            sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, ypos, sprite.transform.localPosition.z);
        }

        /// <summary> True if the player is pressing a movement key. </summary>
        private bool IsMoving()
        {
            return CustomInput.BoolHeld(CustomInput.UserInput.Left) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Right);
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

        private void UpdateAnimations()
        {
            anim.SetBool("IsMoving", IsMoving());
            anim.SetBool("IsGrounded", IsGrounded());
        }

        private void Move()
        {
            Vector2 vel;
            int x;

            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
            {
                x = -1;
                transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x)*-1, transform.localScale.y, transform.localScale.z);
            }
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
            {
                x = 1;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                x = 0;
            }
            
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
