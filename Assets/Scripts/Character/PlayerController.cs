namespace HomeTakeover.Character
{
    using UnityEngine;
    using System.Collections;
    using HomeTakeover.Util;
    using HomeTakeover.Furniture;
    using System.Collections.Generic;

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rgdb;

        /// <summary>
        /// How fast the player moves
        /// </summary>
        public float speed;
        /// <summary>
        /// How much force the player jumps with
        /// </summary>
        public float jumpspeed;

        /// <summary>
        /// Extra transforms on the edge of the player used to raycast down to detect whether or not we are grounded.
        /// </summary>
        public Transform left, right;
        private List<Transform> raycastCheckPoints;
        private float raycastDistance = 0.1f;
        /// <summary>
        /// The layer mask used to check for the ground beneath the player
        /// </summary>
        public LayerMask groundCheckLayerMask;
        public LayerMask platformCheckLayerMask;

        private bool canJumpAgainInAir = true;

        /// <summary>
        /// The reticule that follows the mouse. Also used as the origin for an OverlapCircle check when attempting to grab items.
        /// </summary>
        public Transform reticule;
        /// <summary>
        /// Empty transform at the end of the arm sprite
        /// </summary>
        public Transform defaultArmEndPos;
        /// <summary>
        /// Holds the pivot point that the arm rotates around
        /// </summary>
        public Transform armHolderTransform;
        /// <summary>
        /// The visual component of the arm sprite
        /// </summary>
        public SpriteRenderer armSprite;
        /// <summary>
        /// The initial size of the arm, used to reset it after a stretch;
        /// </summary>
        Vector2 armSpriteInitSize;

        /// <summary>
        /// The transform that picked up objects get parented to.
        /// </summary>
        public Transform objectPivotPoint;
        private float initObjectPivotPointY;

        /// <summary>
        /// The radius used to check for grabbable objects around the player's hand when they try to grab things.
        /// </summary>
        public float armGrabRadius;
        /// <summary>
        /// The layer mask used to check for grabbable furniture when the player clicks
        /// </summary>
        public LayerMask furnitureGrabLayerMask;

        /// <summary>
        /// The furniture item currently in the player's hand. Null if empty
        /// </summary>
        public Furniture heldItem = null;

        /// <summary>
        /// How much force to apply to thrown objects
        /// </summary>
        public float throwForce;

        /// <summary>
        /// The player animator
        /// </summary>
        public Animator anim;

        /// <summary>
        /// Timer for lerping arm to reach out to reticule when grabbing things
        /// </summary>
        private float armReturnTimer = 0.0f, maxArmReturnTimer = 0.225f;

        /// <summary>
        /// Timer for lerping arm to match weapon melee attack timing
        /// </summary>
        public float armMeleeTimer, maxArmMeleeTimer;

        private Collider2D collider;

        /// <summary>
        /// Singleton
        /// </summary>
        public static PlayerController instance;

        private int AttackId = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            rgdb = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            raycastCheckPoints = new List<Transform>();
            raycastCheckPoints.Add(left);
            raycastCheckPoints.Add(transform);
            raycastCheckPoints.Add(right);

            armSpriteInitSize = armSprite.size;
            initObjectPivotPointY = objectPivotPoint.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (heldItem != null && heldItem.gameObject.activeSelf == false)
            {
                heldItem = null;
            }
            Move();
            UpdateArm();
            UpdateAnimations();
        }

        void TakeDamage(int damage)
        {
            // take damage from enemy health -= damage
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
            else if (armMeleeTimer > 0)
            {
                armMeleeTimer = Mathf.Max(0, armMeleeTimer -= Time.deltaTime);
                Vector3 pos = Vector3.Lerp(defaultArmEndPos.transform.position, defaultArmEndPos.transform.position + new Vector3(0,1.5f,0), armMeleeTimer / maxArmMeleeTimer);
                UpdateArmStretch(armSprite, armHolderTransform.position, pos);
            }
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Attack))
            {
                if (heldItem == null)
                {
                    armReturnTimer = maxArmReturnTimer;
                    Furniture tempFurn = AttemptToGrabFurniture();
                    if (tempFurn == null)
                    {
                        ArmAttack();
                    }
                    else
                    {
                        Grab(tempFurn);
                    }
                }
                else
                {
                    heldItem.OnUse();
                }
            }
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Throw) && heldItem != null)
            {
                ThrowItem();
            }
        }

        public void MeleeWeaponPunch(float duration)
        {
            armMeleeTimer = duration;
            maxArmMeleeTimer = duration;

            armMeleeTimer = maxArmMeleeTimer;
        }

        void Grab(Furniture tempFurn)
        {
            heldItem = tempFurn;
            tempFurn.OnPickup(objectPivotPoint);
        }

        void ThrowItem()
        {
            Vector2 direction = new Vector2(reticule.transform.position.x - armHolderTransform.position.x, reticule.transform.position.y - armHolderTransform.position.y);
            heldItem.OnThrow(armHolderTransform, direction, throwForce, rgdb.velocity);
            heldItem = null;
        }

        /// <summary>
        /// When the player clicks, performs a radial raycast around the reticule and populates a list of furniture that falls within it.
        /// Returns the item closest to the reticule
        /// </summary>
        /// <returns> The closest furniture item in range of the player's reticule </returns>
        private Furniture AttemptToGrabFurniture()
        {
            Furniture furn = null;
            float closestDistance = float.MaxValue;
            Collider2D[] cols = Physics2D.OverlapCircleAll(reticule.transform.position, armGrabRadius, furnitureGrabLayerMask);
            foreach (Collider2D col in cols)
            {
                if (Vector2.Distance(col.transform.position, reticule.transform.position) < closestDistance) {
                    furn = col.GetComponent<Furniture>();
                }
            }
            return furn;
        }

        /// <summary>
        /// If the player fails to grab any furniture, perform a weak punch attack.
        /// </summary>
        private void ArmAttack()
        {
            //TODO: weak attack
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
            objectPivotPoint.transform.localPosition = new Vector3(0, (initObjectPivotPointY + distance) *-1, 0);
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
                hit = Physics2D.Raycast(trans.position, trans.TransformDirection(Vector2.down), raycastDistance, groundCheckLayerMask | platformCheckLayerMask);
                if (hit.collider != null)
                {
                    canJumpAgainInAir = true;
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

            if (CustomInput.BoolHeld(CustomInput.UserInput.Down)
                        )
            {
                StartCoroutine("Fall");
            } 
            
            vel = new Vector2(x*speed*Time.deltaTime, rgdb.velocity.y);
            rgdb.velocity = vel;

            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Jump))
            {
                if (IsGrounded())
                {
                    vel = new Vector2(vel.x, jumpspeed);
                }
                else if (canJumpAgainInAir == true)
                {
                    canJumpAgainInAir = false;
                    vel = new Vector2(vel.x, jumpspeed);
                }
            }

            rgdb.velocity = vel;
        }

        public int RequestAttackId()
        {
            return AttackId++;
        }

        /// <summary> Makes the character fall by disabling then enabling the collider 
        /// Contact filter is broken, aaaaaaah
        /// <summary/>
        private IEnumerator Fall() {
            ContactPoint2D[] contacts = new ContactPoint2D[5];
            collider.GetContacts(contacts);
            foreach (ContactPoint2D contact in contacts)
            {
                if (contact.collider != null && contact.collider.gameObject.name.Equals("PlatformMap")) 
                {
                    Physics2D.IgnoreCollision(collider, contact.collider, true);
                }
            }
            yield return new WaitForSeconds(0.3f);
            foreach (ContactPoint2D contact in contacts)
            {
                if (contact.collider != null)
                    Physics2D.IgnoreCollision(collider, contact.collider, false);
            }
        }
    }
}
