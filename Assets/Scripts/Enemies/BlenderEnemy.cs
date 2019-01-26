namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Util;
    using HomeTakeover.Character;

    public class BlenderEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;

        //Attack Distance 
        public float flipTime;

        //Attack Distance 
        public float enemyAttackRange = 2f;

        //Player is in Range
        public bool inRange = false;


        public GameObject bullet;
        int ammocount = 5;


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
            float playerDistance = Vector2.Distance((Vector2)this.gameObject.transform.position, v2);



            if (playerDistance <= enemyAttackRange)
            {
                inRange = true;
            }
            else
            {
                inRange = false;
            }

            if (inRange)
            {
                timer += Time.deltaTime;
                if (timer > flipTime)
                {
                    timer = 0;
                    while (ammocount > 0)
                    {
                        Fire();
                        ammocount--;
                    }
                    ammocount = 5;
                }
            }
        }
        /*
        Method to flip the sprite
        */
        private void Flip()
        {
            if (!facing)
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
        }
        /*
        Base on the flip time the enemy will either flip or target the player
        */
        protected override void Run()
        {

            if (inRange)
            {
                //this.Target();
                //shoot
            }


            rgbd2d.velocity = new Vector2(speed, rgbd2d.velocity.y);
        }

        //Fire Bullets!
        private void Fire()
        {
            GameObject temp = GameObject.Instantiate<GameObject>(bullet);
            temp.transform.position = this.gameObject.transform.position;
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(1,1) * 8.0f;
        }

    }
}
