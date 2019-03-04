using System.Collections.Generic;
using UnityEngine;

namespace BezierCanvas
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathView : MonoBehaviour
    {
        LineRenderer lineRenderer;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void UpdatePath(List<Vector3> points)
        {
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
    }
}
