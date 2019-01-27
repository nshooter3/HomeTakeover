namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Util;
    using HomeTakeover.Character;
    

    public abstract class Enemy : MonoBehaviour
    {
        public int maxHealth;
        public float speed = 3f;
        public GameObject deathItem;
        public bool facing = false;
        public Vector2 impulse;

        protected int health;

        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion q;
        private int attackID = -1;
        private Transform playerPos;

        private void Start()
        {
            health = maxHealth;
            Init();
            playerPos = PlayerController.instance.gameObject.transform;
        }

        private void Update()
        {
            Run();
            Attack();
            IsFacing();
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
            if (Vector2.Angle(PlayerController.instance.gameObject.transform.forward, this.transform.position - PlayerController.instance.gameObject.transform.position) < angle)
            {
                facing = true;
                impulse.Set(0.0f, 5.0f);
            }
            else
            {
                facing = false;
                impulse.Set(0.0f, -5.0f);
            }
        }

        /*
       Collider behavoirs for Player and PlayerAttack
        */
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "furnitureAttack")
            {
                
                if (attackID != collision.gameObject.GetComponent<DamageDealer>().attackId)
                {
                    health -= collision.gameObject.GetComponent<DamageDealer>().damage;
                    attackID = collision.gameObject.GetComponent<DamageDealer>().attackId;
                    speed *= collision.gameObject.GetComponent<DamageDealer>().stun;
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(impulse, ForceMode2D.Impulse);
                   
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
            GameObject item = Instantiate(deathItem);
            item.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
