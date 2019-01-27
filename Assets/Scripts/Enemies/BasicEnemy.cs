namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Character;

    public class BasicEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;

        //Attack Distance 
        public float flipTime;

        //Attack Distance 
        public float stunTime;

        //Attack Distance 
        public float enemyAttackRange = 2f;

        //Player is in Range
        public bool inRange = false;

        private float timer;
        private bool right;
        
        
        protected override void Init()
        {
            timer = 0;
            right = false;
        }

        /*
        Checks a circle collider for player Tag to confimr if player is in Range 
        */
        protected override void Attack()
        {
            Vector2 v2 = this.gameObject.transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(v2, enemyAttackRange);
            float playerDistance = Vector2.Distance((Vector2)PlayerController.instance.transform.position, v2);

            if (playerDistance <= enemyAttackRange)
            {
                inRange = true;
                
            }
            else
            {
                inRange = false;
            }
        }
        /*
        Method to flip the sprite
        */
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
        /*
        Base on the flip time the enemy will either flip or target the player
        */
        protected override void Run()
        {

            if ((timer += Time.deltaTime) > stunTime)
            {
                this.GetComponent<Rigidbody2D>().drag = 0;
            }

            if ((timer += Time.deltaTime) > flipTime )
            {
                if (inRange)
                {
                    timer = 0;
                      //shoot
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
