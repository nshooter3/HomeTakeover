namespace HomeTakeover.Effects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CurtainLineRenderer : MonoBehaviour
    {
        public Transform[] links;
        private LineRenderer line;
        private List<Vector3> points;

        // Start is called before the first frame update
        void Start()
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 0;
            points = new List<Vector3>();
        }

        // Update is called once per frame
        void Update()
        {
            points.Clear();
            foreach (Transform t in links)
            {
                points.Add(t.position);
            }
            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
        }
    }
}
