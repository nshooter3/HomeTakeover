namespace HomeTakeover.Character
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Reticule : MonoBehaviour
    {
        public float mouseFollowTolerance;
        private Vector3 mousePosition;

        public Transform playerCenter;
        /// <summary>
        /// Determines length of tether between player and arm reticule
        /// </summary>
        public float maxDistanceFromPlayerCenter;

        private SpriteRenderer sprite;

        // Use this for initialization
        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (Vector2.Distance(mousePosition, transform.position) > mouseFollowTolerance)
            {
                transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
            }
            //If the reticule is further away than maxDistanceFromPlayerCenter, cap its distance from the center of the player
            float reticuleDistanceFromPlayer = Vector2.Distance(playerCenter.transform.position, mousePosition);
            if (reticuleDistanceFromPlayer > maxDistanceFromPlayerCenter)
            {
                Vector2 direction = new Vector2((mousePosition - playerCenter.transform.position).x, (mousePosition - playerCenter.transform.position).y).normalized;
                transform.position = new Vector3(playerCenter.transform.position.x + direction.x* maxDistanceFromPlayerCenter, 
                    playerCenter.transform.position.y + direction.y* maxDistanceFromPlayerCenter, transform.position.z);
            }
            sprite.color = Color.Lerp(Color.white, new Color(1, 0.5f, 0.5f, 1), Mathf.Min(reticuleDistanceFromPlayer, maxDistanceFromPlayerCenter)/ maxDistanceFromPlayerCenter);
        }
    }
}
