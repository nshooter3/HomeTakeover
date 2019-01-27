namespace HomeTakeover.Furniture
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HomeTakeover.Character;

    public class Box : Furniture
    {
        private float cooldown = 0, maxCooldown = 0.15f;

        public float attackReach;
        Vector3 attackPos;
        Vector3 initScale;
        Vector3 initPos;
        SpriteRenderer sprite;

        public DamageDealer punchDamage;

        public ParticleSystem punchParticles;

        private void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            attackPos = new Vector3(0, -1 * attackReach, 0);
            initScale = sprite.transform.localScale;
            initPos = sprite.transform.localPosition;
        }

        public override void OnUse()
        {
            if (cooldown == 0)
            {
                cooldown = maxCooldown;
                PlayerController.instance.MeleeWeaponPunch(maxCooldown);
                punchParticles.Play();
                ToggleHurtBox(true);
                punchDamage.attackId = PlayerController.instance.RequestAttackId();
                //print("ATTACK ID: " + punchDamage.attackId);
            }
        }

        private void Update()
        {
            if (cooldown > 0)
            {
                cooldown = Mathf.Max(0, cooldown - Time.deltaTime);
                float t = cooldown / maxCooldown;
                sprite.transform.localPosition = Vector2.Lerp(initPos, initPos + attackPos, t);
                sprite.transform.localScale = Vector3.Lerp(initScale, initScale * 1.5f, t);
                sprite.color = Color.Lerp(Color.white, Color.red, t);
                if (cooldown == 0)
                {
                    ToggleHurtBox(false);
                }
            }
        }
    }
}
