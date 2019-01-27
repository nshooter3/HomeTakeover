namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Util;
    using HomeTakeover.Character;
    using System;

    public class ChairEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;


        public enum ChairState { roaming, charging, attacking}

        public ChairState state = ChairState.roaming;
        //Attack Distance 
        public float chargeTime;
        private float initialChargeTime;

        //Attack Distance 
        public float enemyAttackRange = 2f;

        //Player is in Range
        public bool inRange = false;

        public Vector3 chargingRotation = new Vector3(0, 0, 90);

        public GameObject chairSprite;

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
            state = ChairState.roaming;
            initialChargeTime = chargeTime;
        }

        private void Update()
        {
            switch (state)
            {
                case ChairState.roaming:
                    Run();
                    break;

                case ChairState.charging:
                    Charge();
                    break;

                case ChairState.attacking:

                    break;

                default:

                    break;
            }
        }

        
        public void Charge()
        {
            if(timer < chargeTime)
            {
                timer += Time.deltaTime;
                float timerPercent = timer / chargeTime;
                float colorLerp = Mathf.Lerp(0, 255, timerPercent);
                chairSprite.GetComponent<SpriteRenderer>().color = new Color(255, 255 - colorLerp, 255 - colorLerp);
            }
        }

        /*
        Checks a circle collider for player Tag to confimr if player is in Range 
        */
        protected override void Attack()
        {
            



            

           
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
            IsFacing();
            float playerDistance = Math.Abs(Vector2.Distance((Vector2)PlayerController.instance.gameObject.transform.position, this.gameObject.transform.position));
            bool inRange = playerDistance <= enemyAttackRange;
            if (facing)
            {
                AC.SetFloat("Vel", 1.0f);
                rgbd2d.velocity = new Vector2(speed, rgbd2d.velocity.y);
            }
            else
            {
                Flip();
            }
            

            if(inRange)
            {
                timer = 0;
                state = ChairState.charging;
            }

        }

        //Fire Bullets!
        private void Fire()
        {
            float playerDistance = Math.Abs(Vector2.Distance((Vector2)PlayerController.instance.gameObject.transform.position, this.gameObject.transform.position));

            int shouldReverse = -1;
            if (right) { shouldReverse *= -1; }

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
            /*
            if(chairSprite != null)
            {
                chairSprite.transform.localPosition = new Vector3(0 + UnityEngine.Random.Range(0,.3f), 0 + UnityEngine.Random.Range(0, .3f), 0);
            }
            else
            {
                chairSprite.transform.localPosition = Vector3.zero;
            }
            //*/
        }

    }
}
