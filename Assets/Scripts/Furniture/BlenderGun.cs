namespace HomeTakeover.Furniture
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HomeTakeover.Character;
    using Util.ObjectPooling;

    public class BlenderGun : Furniture
    {
        public ParticleSystem gunParticles;

        private SpriteRenderer sprite;

        public DamageDealer throwDamage;


        // Start is called before the first frame update
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            if (throwDamage == null)
            {
                throwDamage = GetComponentInChildren<DamageDealer>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isBroken)
            {
                sprite.transform.localScale = Vector3.one;
                breakTimer -= Time.deltaTime;
                float t = breakTimer / maxBreakTimer;
                sprite.color = Color.Lerp(new Color(1, 0, 0, 0), new Color(1, 0, 0, 1), t);
                if (breakTimer <= 0)
                {
                    Deallocate();
                }
            }
        }

        public override void OnUse()
        {
            gunParticles.Play();
            int attackId = PlayerController.instance.RequestAttackId();
            GameObject temp = Enemies.BulletPool.Instance.GetBullet(Enemies.BulletPool.BulletTypes.PlayerBullet);
            temp.transform.position = this.gameObject.transform.position;
            temp.GetComponent<Rigidbody2D>().velocity = PlayerController.instance.direction * 8f;
            temp.GetComponentInChildren<DamageDealer>().attackId = PlayerController.instance.RequestAttackId();
            TakeDurabilityDamage(1);
        }

        public override void OnThrowChild()
        {
            ToggleHurtBox(true);
            throwDamage.attackId = PlayerController.instance.RequestAttackId();
        }
    }
}
