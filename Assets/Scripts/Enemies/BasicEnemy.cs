namespace HomeTakeover.Enemies
{
    using UnityEngine;

    public class BasicEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;
        public LayerMask layer;
        public float flipTime;
        public float enemyAttackRange = 2f;
        public bool inRange = false;

        private float timer;
        private bool right;
        
        
        protected override void Init()
        {
            timer = 0;
            right = false;
        }


        protected override void Attack()
        {
             Vector2 v2 = this.gameObject.transform.position;
             Collider2D[] hitColliders = Physics2D.OverlapCircleAll(v2, enemyAttackRange);
     
            for(int i = 0; hitColliders.Length > i; i++)
                if ( hitColliders[i].gameObject.tag == "Player")
                {
                    inRange = true;
                }
                else
                {
                    inRange = false;
                }
        }
        private void Flip()
        {
            right = !right;
            speed = -speed;

            if (right)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                speed = speed * Mathf.Sign(speed);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                speed = speed * -Mathf.Sign(speed);
            }
        }
        protected override void Run()
        {
            if((timer += Time.deltaTime) > flipTime )
            {
                if (inRange)
                {
                    timer = 0;
                    this.Target();
                }
                else
                {
                    timer = 0;
                    Flip();
                }
            }

            rgbd2d.velocity = new Vector2(speed, rgbd2d.velocity.y);
        }


    }
}
