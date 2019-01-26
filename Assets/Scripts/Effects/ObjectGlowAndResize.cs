namespace HomeTakeover.Effects
{
    using UnityEngine;
    using HomeTakeover.Character;
    using HomeTakeover.Furniture;

    public class ObjectGlowAndResize : MonoBehaviour
    {
        public float radius = 0.1f;
        public LayerMask furnitureGrabLayerMask;

        void Start()
        {
            
        }

        void Update()
        {
            GlowMouseOver();
        }


        private void GlowMouseOver()
        {
            Furniture furn = null;
            float closestDistance = float.MaxValue;
            Vector3 reticlePosition = PlayerController.instance.reticule.transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(reticlePosition, radius, furnitureGrabLayerMask);
            foreach (Collider2D col in cols)
            {
                if (Vector2.Distance(col.transform.position, reticlePosition) < closestDistance)
                {
                    // col.GetComponent<Furniture>().OnCursorOver();
                }
            }
        }
    }
}
