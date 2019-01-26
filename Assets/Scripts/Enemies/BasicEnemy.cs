namespace HomeTakeover.Enemies
{
    using UnityEngine;

    public class BasicEnemy : Enemy
    {
        public Rigidbody2D rgbd2d;
        public float flipTime;
        public float speed;

        private float timer;
        private bool right;

        protected override void Init()
        {
            timer = 0;
            right = false;
        }

        protected override void Run()
        {
            if((timer += Time.deltaTime) > flipTime)
            {
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
