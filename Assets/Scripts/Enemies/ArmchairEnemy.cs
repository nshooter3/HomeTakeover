namespace HomeTakeover.Enemies
{
    using UnityEngine;
    using HomeTakeover.Util;
    using HomeTakeover.Character;
    using System;

    public class ArmchairEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;


        public enum ChairState { roaming, charging, attacking }

        public ChairState state = ChairState.roaming;
        //Attack Distance 
        public float chargeTime;
        public float runTime;
        private float initialChargeTime;

        //Attack Distance 
        public float enemyAttackRange = 2f;

        //Player is in Range
        public bool inRange = false;

        public Vector3 chargingRotation = new Vector3(0, 0, 90);

        public GameObject armchairSprite;

        public ParticleSystem glowParticles;
        public ParticleSystem dustParticles;
        public ParticleSystem absorbParticles;

        public Animator AC;
        public Animator AC2;

        //If enemy should be using extra powerful horizontal shot.
        public bool powerShot = false;

        private float timer;
        [SerializeField]
        private bool right;


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
                    Attack();
                    break;

                default:

                    break;
            }
        }


        public void Charge()
        {
            if (!absorbParticles.isPlaying) { absorbParticles.Play(); }
            AC.SetFloat("Vel", 0f);
            AC2.SetFloat("Vel", 0f);
            int shouldReverse = 1;
            if (right) { shouldReverse *= -1; }
            if (!facing) { Flip(); }
            if (timer <= chargeTime)
            {
                timer += Time.deltaTime;
                float midStep = .5f;

                float timerPercent = timer / (chargeTime - midStep);
                float colorLerp = Mathf.Lerp(0, 255, timerPercent);
                armchairSprite.GetComponent<SpriteRenderer>().color = new Color(255, 255 - colorLerp, 255 - colorLerp);
            }
            else
            {
                timer = 0;
                state = ChairState.attacking;
            }
        }

        /*
        Checks a circle collider for player Tag to confimr if player is in Range 
        */
        protected override void Attack()
        {
            absorbParticles.Stop();
            if (!glowParticles.isPlaying) { glowParticles.Play(); }
            if (!dustParticles.isPlaying) { dustParticles.Play(); }
            if (timer < runTime)
            {
                rgbd2d.velocity = new Vector2(speed * 5, rgbd2d.velocity.y);
                AC.SetFloat("Vel", 1.0f);
                AC2.SetFloat("Vel", 1.0f);
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                state = ChairState.roaming;
                resetToRoaming();
            }

        }

        public void resetToRoaming()
        {
            glowParticles.Stop();
            dustParticles.Stop();
            absorbParticles.Stop();
            armchairSprite.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            armchairSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
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
                AC2.SetFloat("Vel", 1.0f);
                rgbd2d.velocity = new Vector2(speed, rgbd2d.velocity.y);
            }
            else
            {
                Flip();
            }


            if (inRange)
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

            glowParticles.Emit(1);
        }

        public void IsFacing()
        {
            if (right && PlayerController.instance.gameObject.transform.position.x >= this.transform.position.x)
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

    }
}
