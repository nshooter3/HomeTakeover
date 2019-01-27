namespace HomeTakeover.Furniture
{
    using UnityEngine;
    using HomeTakeover.Character;
    using SpriteGlow;
    using Util.ObjectPooling;
    using UI;

    public abstract class Furniture : MonoBehaviour, IPoolable
    {
        public FurniturePool.FurnitureTypes type;
        public GameObject pivotPoint;

        [SerializeField]
        private int referenceIndex = 0;

        /// <summary>
        /// Whether or not this object is being held by the player
        /// </summary>
        public bool held = false;

        /// <summary>
        /// Dictates whether or not you can attack remotely with the reticule, or you just hold the object in front of you. Heavier objects cannot be rotated
        /// </summary>
        public bool isOversizedObject = false;

        /// <summary>
        /// The health of this weapon before it breaks
        /// </summary>
        public int maxDurability;
        private int durability;

        public bool isBroken = false;
        public float breakTimer = 0, maxBreakTimer = 0.25f;

        /// <summary>
        /// How much damage this object deals as a thrown projectile
        /// </summary>
        public int ThrowDamage;

        /// <summary>
        /// Collider for physics/taking damage
        /// </summary>
        public BoxCollider2D hitbox;
        /// <summary>
        /// Trigger collider for dealing damage
        /// </summary>
        public BoxCollider2D hurtbox;

        private Rigidbody2D rgbd;
        private Transform spriteTransform;
        private Vector3 originalScale;
        private bool isScaled;

        public bool thrown = false;

        /// <summary>
        /// How much to z rotate weapon by when held
        /// </summary>
        public Vector3 rotationOffset;

        [SerializeField]
        private EnemyHealthBar healthBar;


        public IPoolable SpawnCopy(int referenceIndex)
        {
            Furniture furniture = Instantiate<Furniture>(this);
            furniture.referenceIndex = referenceIndex;
            return furniture;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public void Initialize()
        {
            Init();
        }

        public void ReInitialize()
        {
            this.gameObject.SetActive(true);
            Init();
        }

        public void Deallocate()
        {
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Gets called when the object is spawned.
        /// </summary>
        public void Init()
        {
            if (healthBar == null)
            {
                healthBar = GetComponentInChildren<EnemyHealthBar>();
            }
            this.healthBar.Percent = 1;
            rgbd = GetComponent<Rigidbody2D>();
            durability = maxDurability;
            hitbox.enabled = true;
            hurtbox.enabled = false;
            spriteTransform = gameObject.GetComponentInChildren<SpriteRenderer>().transform;
            originalScale = spriteTransform.localScale;
            isBroken = false;
        }

        public void TakeDurabilityDamage(int damage)
        {
            durability -= damage;
            float percent = (this.durability / (float)this.maxDurability);
            if (percent < 0)
                percent = 0;
            this.healthBar.Percent = percent;
            if (durability <= 0)
            {
                OnDrop(transform);
                isBroken = true;
                breakTimer = maxBreakTimer;
            }
        }

        /// <summary>
        /// Called when the player picks up this weapon
        /// </summary>
        /// <param name="handPivot"> The empty transform at the end of the player's arm where we parent held items </param>
        public void OnPickup(Transform handPivot)
        {
            if (!isBroken)
            {
                rgbd.bodyType = RigidbodyType2D.Kinematic;
                transform.parent = handPivot;
                transform.localPosition = Vector2.zero;
                transform.localEulerAngles = rotationOffset;
                rgbd.velocity = Vector3.zero;
                rgbd.angularVelocity = 0;
                hitbox.enabled = false;
            }
        }

        /// <summary>
        /// Called when the player drops this weapon
        /// </summary>
        /// <param name="dropCoordinates">Where to drop the weapon</param>
        public void OnDrop(Transform dropCoordinates)
        {
            rgbd.bodyType = RigidbodyType2D.Dynamic;
            transform.parent = null;
            transform.position = dropCoordinates.position;
            hitbox.enabled = true;
        }

        /// <summary>
        /// Called when the player throws this weapon
        /// </summary>
        /// <param name="dropCoordinates"> where to release the weapon </param>
        /// <param name="direction"> which direction to throw the weapon </param>
        /// <param name="magnitude"> how much force to throw the weapon with </param>
        public void OnThrow(Transform dropCoordinates, Vector2 direction, float magnitude, Vector2 playerVelocity)
        {
            OnDrop(dropCoordinates);
            rgbd.velocity = direction * magnitude + playerVelocity;
            hitbox.enabled = true;
            thrown = true;
            OnThrowChild();
        }

        public abstract void OnThrowChild();

        /// <summary>
        /// Turns on/off the trigger collider that damages enemies upon entry
        /// </summary>
        /// <param name="enabled"> what to set our hurtbox trigger to </param>
        public void ToggleHurtBox(bool enabled)
        {
            hurtbox.enabled = enabled;
        }
        /// <summary>
        /// Restores this object's durability when another object gets consumed
        /// </summary>
        /// <param name="consumedObject"></param>
        public void OnGetRepaired(int healAmount)
        {
            durability = Mathf.Min(maxDurability, durability + healAmount);
        }

        void OnMouseOver()
        {
            if (!isBroken)
            {
                if (!isScaled && PlayerController.instance.heldItem == null)
                {
                    originalScale = spriteTransform.localScale;
                    spriteTransform.localScale = originalScale * 1.2f;
                    spriteTransform.gameObject.GetComponent<SpriteGlowEffect>().OutlineWidth = 2;
                    isScaled = true;
                }
            }
        }

        void OnMouseExit()
        {
            if (!isBroken)
            {
                spriteTransform.localScale = originalScale;
                spriteTransform.gameObject.GetComponent<SpriteGlowEffect>().OutlineWidth = 0;
                isScaled = false;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("bullet"))
            {
                TakeDurabilityDamage(1);
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.layer == LayerMask.NameToLayer("platform"))
            {
                if (thrown)
                {
                    thrown = false;
                    ToggleHurtBox(false);
                }
            }
        }

        /// <summary>
        /// What do when this weapon attacks? You decide!
        /// </summary>
        public abstract void OnUse();
    }
}
