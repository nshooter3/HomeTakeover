namespace HomeTakeover.Util
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class VerletConstraint : MonoBehaviour
    {
        public Transform t1, t2;
        public float distance, compensate1, compensate2, gravityCompensation;
        public bool usesGravity;
        public Vector3 passingForce;

        private Vector3 grav;
        private bool isPassed = false;

        void Start()
        {
            grav = new Vector3(0, -gravityCompensation, 0);
        }

        // Update is called once per frame
        void Update()
        {
            FixedDistanceConstraint();
            passingForce = Vector3.Lerp(passingForce, Vector3.zero, 1f);
            
        }

        public void FixedDistanceConstraint()
        {
            Vector3 delta = t1.transform.position - t2.transform.position;
            float length = delta.magnitude; 
            if (length > 0)
            {
                float diff = (length - distance) / length;
                if (compensate1 != 0)
                {
                    Vector3 gravity = usesGravity ? grav : Vector3.zero;
                    t1.transform.position -= (delta * compensate1 * diff - gravity - passingForce) * Time.deltaTime;
                }
                if (compensate2 != 0)
                {
                    Vector3 gravity = usesGravity ? grav : Vector3.zero;
                    t2.transform.position += (delta * compensate2 * diff + gravity + passingForce) * Time.deltaTime;
                }
            }
        }

        void PassingForce(Vector3 direction, float distance, Vector3 velocity)
        {
            if ( velocity.magnitude > 0.1f )
            {
                passingForce = new Vector3(-1, 0, 0) * direction.x * 25;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            PassingForce(direction.normalized, direction.magnitude, collision.attachedRigidbody.velocity);
        }

        void OnTriggerExit2D(Collider2D collision)
        {

        }
    }
}
