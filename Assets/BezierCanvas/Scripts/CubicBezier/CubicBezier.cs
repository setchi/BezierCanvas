using System.Collections.Generic;
using UnityEngine;

namespace BezierCanvas {

    public class CubicBezier {

        public static List<Vector3> GetPoints(Path path, int segments) {
            segments = Mathf.Max (segments, 1);

            var points = new List<Vector3> ();
            float floatSegments = segments;

            for (int i = 1; i < path.Points.Count; i++) {
                var p0 = path.Points [i - 1].Anchore;
                var p1 = path.Points [i - 1].Handle2;
                var p2 = path.Points [i].Handle1;
                var p3 = path.Points [i].Anchore;

                for (int j = 0; j <= segments; j++) {
                    points.Add (GetPoint (p0, p1, p2, p3, j / floatSegments));
                }
            }
            return points;
        }

        public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            var oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }
    }
}
