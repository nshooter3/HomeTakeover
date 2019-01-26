namespace HomeTakeover.Enemies
{
    using UnityEngine;

    public abstract class Enemy : MonoBehaviour
    {
        public int maxHealth;
        public GameObject deathItem;
        public GameObject player;
        protected int health;

        private bool inRange;
        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion q;
   

        private void Start()
        {
            health = maxHealth;
            Init();
        }

        private void Update()
        {
        }
        private void Target()
        {
            vectorToTarget = player.transform - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                health--;
                if (health <= 0)
                    Die();
            }
            if (other.gameObject.tag == "Player")
            {
                //Melee
            }
        }

        protected abstract void Attack();
        protected abstract void Init();
        protected abstract void Run();

        protected void Die()
        {
            GameObject item = Instantiate(deathItem);
            item.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
