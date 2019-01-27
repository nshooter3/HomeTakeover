namespace HomeTakeover.Furniture
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HomeTakeover.Character;

    public class BlenderGun : Furniture
    {
        public ParticleSystem gunParticles;

        private SpriteRenderer sprite;


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
    }
}
