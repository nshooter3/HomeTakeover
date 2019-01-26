namespace HomeTakeover.Camera
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FollowPlayer : MonoBehaviour
    {
        public Transform target;
        public Vector2 offset;

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, transform.position.z);
        }
    }
}
