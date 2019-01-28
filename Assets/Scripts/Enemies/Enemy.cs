namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Character;
    using UI;
    using Util.ObjectPooling;
    

    public abstract class Enemy : MonoBehaviour, IPoolable
    {
        public EnemyPool.EnemyTypes type;
        public int maxHealth;
        public float speed = 3f;
        public Furniture.FurniturePool.FurnitureTypes deathItem;
        public bool facing = false;
        public EnemyHealthBar healthBar;
        public Vector3 impuluse;

        [SerializeField]
        private int referenceIndex = 0;

        protected int health;

        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion q;
        private int attackID = -1;

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Enemy enemy = Instantiate<Enemy>(this);
            enemy.referenceIndex = referenceIndex;
            return enemy;
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
            health = maxHealth;
            this.healthBar.Percent = 1;
            Init();
        }

        public void ReInitialize()
        {
            this.gameObject.SetActive(true);
            health = maxHealth;
            this.healthBar.Percent = 1;
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
            Run();
            Attack();
        }

        /*
        Rotates sprite to face player
        */
      

        /*
        Method to confirm if enemy is facing player withing 10 deg angel
        */
        public void IsFacing()
        {
            float angle = 10;
            if (Vector3.Angle(PlayerController.instance.gameObject.transform.forward, transform.position - PlayerController.instance.gameObject.transform.position) < angle)
            {
                facing = true;
            }
            else
            {
                facing = false;
            }
        }


        public void TakeDamage()
        {
            health--;
            float percent = (this.health / (float)this.maxHealth);
            if (percent < 0)
                percent = 0;
            this.healthBar.Percent = percent;

            if (health <= 0)
                Die();
        }
        /*
       Collider behavoirs for Player and PlayerAttack
        */
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "furnitureAttack" || collision.gameObject.tag == "PlayerBullet")
            {
                collision.gameObject.GetComponentInChildren<DamageDealer>().hitEnemy = true;

                if (attackID != collision.gameObject.GetComponentInChildren<DamageDealer>().attackId)
                {
                    TakeDamage();
                    attackID = collision.gameObject.GetComponentInChildren<DamageDealer>().attackId;
                }
                if (health <= 0)
                    Die();
            }
            if (collision.gameObject.tag == "Player")
            {
                //Melee
            }
        }

        /*
        Abstract Methods
        */
        protected abstract void Attack();
        protected abstract void Init();
        protected abstract void Run();

        /*
        Method for handling death
        */
        protected void Die()
        {
            GameObject item = Furniture.FurniturePool.Instance.GetFurniture(deathItem);
            if(item != null)
                item.transform.position = this.transform.position;
            EnemyPool.Instance.ReturnEnemy(this.type, this.gameObject);
        }
    }
}
