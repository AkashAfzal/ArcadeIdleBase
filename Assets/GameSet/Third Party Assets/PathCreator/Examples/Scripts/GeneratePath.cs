using UnityEngine;

namespace PathCreation.Examples {
    // Example of creating a path at runtime from a set of points.

    //[RequireComponent(typeof(PathCreator))]
    public class GeneratePath : MonoBehaviour {

        public bool        closedLoop = true;
        [Range(0,360)]
        public float       globalNormalsAngle;
        public Transform[] waypoints;

        void Start () {
            if (waypoints.Length > 0)
            {
                var pathCreator = FindObjectOfType<PathCreator>();
                // Create a new bezier path from the waypoints.
                BezierPath bezierPath = new BezierPath (waypoints, closedLoop, PathSpace.xyz);
                pathCreator.bezierPath                    = bezierPath;
                pathCreator.bezierPath.GlobalNormalsAngle = globalNormalsAngle;
            }
        }
    }
}