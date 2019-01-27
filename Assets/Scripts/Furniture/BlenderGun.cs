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
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnUse()
        {
            gunParticles.Play();
            int attackId = PlayerController.instance.RequestAttackId();
        }

        public override void OnThrowChild()
        {
            ToggleHurtBox(true);
            throwDamage.attackId = PlayerController.instance.RequestAttackId();
        }
    }
}
