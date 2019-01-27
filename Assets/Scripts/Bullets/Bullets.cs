namespace HomeTakeover.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HomeTakeover.Character;
    using UI;
    using Util.ObjectPooling;

    public class Bullets : MonoBehaviour, IPoolable
    {
        public BulletPool.BulletTypes type;
        public float speed = 3f;
        public bool gravity = false;
       // public Furniture.FurniturePool.FurnitureTypes deathItem;
        public bool facing = false;
        private Transform spriteTransform;
        private Vector3 originalScale;
        private bool isScaled;
        public Rigidbody2D rgbd;
        private Vector3 vectorToTarget;
        public Vector3 impuluse;

        /// <summary>
        /// Collider for physics/taking damage
        /// </summary>
        public BoxCollider2D hitbox;

        [SerializeField]
        private int referenceIndex = 0;

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Bullets bullet = Instantiate<Bullets>(this);
            bullet.referenceIndex = referenceIndex;
            return bullet;
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

        private void Update()
        {
          
        }
         
        public void IsFacing()
        {
         //Spawn Direction
        }

        /*
       Collider behavoirs for Player and PlayerAttack
        */
       void OnTriggerEnter2D(Collider2D collision)
        {
            rgbd.velocity = Vector3.zero;
          
            if (collision.gameObject.layer == LayerMask.NameToLayer("enemies"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage();
            }
            BulletPool.Instance.ReturnBullet(this.type, this.gameObject);
        }
        void Init()
        {
            rgbd = GetComponent<Rigidbody2D>();
            if (!gravity)
            {
                rgbd.gravityScale = 0.0f;
            }
            hitbox.enabled = true;
            spriteTransform = gameObject.GetComponentInChildren<SpriteRenderer>().transform;
            originalScale = spriteTransform.localScale;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

   
    }
}