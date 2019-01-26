namespace HomeTakeover.Enemies
{
    using UnityEngine;

    public class BasicEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;
        public float flipTime;
        public float speed;
        public float enemyAttackRange;


        private float timer;
        private bool right;
        private bool facing;
        private bool inRange;
        
        protected override void Init()
        {
            timer = 0;
            right = false;
        }


        protected override void Attack()
        {

            Collider2D[] hitColliders = Physics2D.OverlapCircle(this.transform.position, enemyAttackRange, 1 << LayerMask.NameToLayer("Player"));

            for (int i = 0; i < hitColliders.Length; i++)
            {
               if( hitColliders[i].gameObject.tag == "Player")
                {
                    inRange = true;
                }
                else
                {
                    inRange = false;
                }
            }
        }

        protected override void Run()
        {
            
            if((timer += Time.deltaTime) > flipTime )
            {
                if (this.PlayerInRange())
                {
                    Target();
                    timer = 0;
                    return;
                }

                timer = 0;
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

            rgbd2d.velocity = new Vector2(speed, rgbd2d.velocity.y);
        }


    }
}
