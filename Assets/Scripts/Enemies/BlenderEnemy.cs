namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Util;
    using HomeTakeover.Character;
    using System;

    public class BlenderEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;

        //Attack Distance 
        public float flipTime;

        //Attack Distance 
        public float enemyAttackRange = 2f;

        //Player is in Range
        public bool inRange = false;

        //Shooting related variables
        public float shotPower = 2f;
        public float powerShotPower = 7f;
        public Furniture.FurniturePool.FurnitureTypes bullet;
        public int ammocount = 3;
        int initialAmmoCount;

        public GameObject blenderSprite;

        public ParticleSystem particles;

        public Animator AC;

        //If enemy should be using extra powerful horizontal shot.
        public bool powerShot = false;

        private float timer;
        [SerializeField] private bool right;


        protected override void Init()
        {
            timer = 0;
            right = true;
            initialAmmoCount = ammocount;
        }

        /*
        Checks a circle collider for player Tag to confimr if player is in Range 
        */
        protected override void Attack()
        {
            Vector2 v2 = this.gameObject.transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(v2, enemyAttackRange);
            float playerDistance = Math.Abs(Vector2.Distance((Vector2)PlayerController.instance.gameObject.transform.position, v2));
            if(blenderSprite != null)
            {
                int shouldReverse = 1;
                if (right) { shouldReverse *= -1; }
                if (powerShot)
                {
                    blenderSprite.transform.rotation = Quaternion.Euler(0, 0, shouldReverse * 90);
                }
                else
                {
                    blenderSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }


            bool inRange = playerDistance <= enemyAttackRange;

            if (inRange)
            {
                timer += Time.deltaTime;
                if (flipTime - timer < .5f)
                {
                    blenderSprite.GetComponent<SpriteRenderer>().color = new Color(255 - (flipTime - timer) * 255, 0, 0);
                }
                else
                {
                    blenderSprite.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                }
                if (timer > flipTime)
                {
                    timer = 0;
                    while (ammocount > 0)
                    {
                        Fire();
                        ammocount--;
                    }
                    ammocount = initialAmmoCount;
                    powerShot = UnityEngine.Random.Range(0, 10) > 5;
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
            powerShot = UnityEngine.Random.Range(0, 10) > 5;
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

            IsFacing();
            if (facing)
            {
                if (!powerShot)
                {
                    rgbd2d.velocity = new Vector2(speed, rgbd2d.velocity.y);
                    AC.SetFloat("Vel", 1.0f);
                }
                else
                {
                    Vibrate();
                    AC.SetFloat("Vel", 0);
                }
            }
            else
            {
                timer += Time.deltaTime;
                if(timer >= flipTime)
                {
                    Flip();
                    timer = 0;
                }

            }
            
        }

        //Fire Bullets!
        private void Fire()
        {
            float playerDistance = Math.Abs(Vector2.Distance((Vector2)PlayerController.instance.gameObject.transform.position, this.gameObject.transform.position));

            int shouldReverse = -1;
            if (right) { shouldReverse *= -1; }

            GameObject temp = Furniture.FurniturePool.Instance.GetFurniture(bullet);
            if (temp != null)
            {
                temp.transform.position = this.gameObject.transform.position;

                if (powerShot)
                    temp.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * shouldReverse, 0) * playerDistance * powerShotPower;
                else
                    temp.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * shouldReverse, 1) * playerDistance * shotPower;
            }
            particles.Emit(1);
        }

        public void IsFacing()
        {
            if(right && PlayerController.instance.gameObject.transform.position.x >= this.transform.position.x)
            {
                facing = true;
            }
            else if (!right && PlayerController.instance.gameObject.transform.position.x <= this.transform.position.x)
            {
                facing = true;
            }
            else
            {
                facing = false;
            }
        }

        public void Vibrate()
        {
            if(blenderSprite != null)
            {
                blenderSprite.transform.localPosition = new Vector3(0 + UnityEngine.Random.Range(0,.3f), 0 + UnityEngine.Random.Range(0, .3f), 0);
            }
            else
            {
                blenderSprite.transform.localPosition = Vector3.zero;
            }
        }

    }
}
