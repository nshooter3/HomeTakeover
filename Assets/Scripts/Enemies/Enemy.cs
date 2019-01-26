namespace HomeTakeover.Enemies
{
    using UnityEngine;

    public abstract class Enemy : MonoBehaviour
    {
        public int maxHealth;
        public GameObject deathItem;

        protected int health;

        private void Start()
        {
            health = maxHealth;
            Init();
        }

        private void Update()
        {
            Run();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                health--;
                if (health <= 0)
                    Die();
            }
        }

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
