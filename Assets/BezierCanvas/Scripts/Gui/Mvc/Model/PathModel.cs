using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BezierCanvas
{
    public class PathModel : MonoBehaviour
    {
        readonly Dictionary<Guid, ControlPoint> controlPoints =
            new Dictionary<Guid, ControlPoint>();

        public bool Dirty { get; private set; }

        int index;

        public void AddControlPoint(Guid id, ControlPoint controlPoint)
        {
            Dirty = true;
            controlPoint.Index = index;
            controlPoints.Add(id, controlPoint);
            index++;
        }

        public ControlPoint GetControlPoint(Guid id)
        {
            return controlPoints.ContainsKey(id) ? controlPoints[id] : default(ControlPoint);
        }

        public void MoveControlPoint(Guid id, Vector2 anchore)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            Vector3 newAnchore = anchore;
            var direction = newAnchore - point.Anchore;
            point.Handle1 += direction;
            point.Handle2 += direction;
            point.Anchore = anchore;
        }

        public void ToSmooth(Guid id)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            point.Handle1 = point.Anchore + Vector3.up * 100;
            point.Handle2 = point.Anchore + Vector3.down * 100;
        }

        public void ToCorner(Guid id)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            point.Handle1 = point.Anchore;
            point.Handle2 = point.Anchore;
        }

        public void MoveHandle1(Guid id, Vector2 handle)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            point.Handle1 = handle;

            var direction = (point.Anchore - point.Handle1).normalized;
            var length = (point.Anchore - point.Handle2).magnitude;
            point.Handle2 = point.Anchore + direction * length;
        }

        public void MoveHandle2(Guid id, Vector2 handle)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            point.Handle2 = handle;

            var direction = (point.Anchore - point.Handle2).normalized;
            var length = (point.Anchore - point.Handle1).magnitude;
            point.Handle1 = point.Anchore + direction * length;
        }

        public void MoveHandle1Symmetry(Guid id, Vector2 handle)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            point.Handle1 = handle;
            point.Handle2 = Vector3.LerpUnclamped(handle, point.Anchore, 2f);
        }

        public void MoveHandle2Symmetry(Guid id, Vector2 handle)
        {
            Dirty = true;
            var point = GetControlPoint(id);
            point.Handle2 = handle;
            point.Handle1 = Vector3.LerpUnclamped(handle, point.Anchore, 2f);
        }

        public void DeleteControlPoint(Guid id)
        {
            Dirty = true;
            if (controlPoints.ContainsKey(id))
            {
                controlPoints.Remove(id);
            }
        }

        public void ClearDirty()
        {
            Dirty = false;
        }

        public Path GetPath()
        {
            return new Path
            {
                Points = controlPoints.Values.OrderBy(x => x.Index).ToList()
            };
        }
    }
}
