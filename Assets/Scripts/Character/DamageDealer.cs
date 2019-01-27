namespace HomeTakeover.Character
{
    using UnityEngine;

    public class DamageDealer : MonoBehaviour
    {
        /// <summary>
        /// Damage dealt to enemies
        /// </summary>
        public int damage;

        /// <summary>
        /// 0 stuns nothing, 1 stuns small enemies, 2 stuns big enemies
        /// </summary>
        public int stun;

        /// <summary>
        /// custom id to ensure that enemies don't take same attack twice
        /// </summary>
        public int attackId;

        /// <summary>
        /// Set to true when an enemy takes damage from this. Used to notify the furniture to take durability damage
        /// </summary>
        public bool hitEnemy = false;
    }
}
