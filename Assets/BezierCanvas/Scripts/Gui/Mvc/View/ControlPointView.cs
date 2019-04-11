using UnityEngine;
using System;

namespace BezierCanvas
{
    [RequireComponent(typeof(LineRenderer))]
    public class ControlPointView : MonoBehaviour
    {
        [SerializeField] HandleView handle1 = default;
        [SerializeField] HandleView handle2 = default;
        [SerializeField] AnchoreView anchore = default;

        [HideInInspector] public Guid Id;

        LineRenderer lineRenderer;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 3;
        }

        public void SetSelected(bool selected)
        {
            anchore.SetColor(selected ? Color.blue : Color.white);
        }

        public void UpdateControlPoint(ControlPoint point)
        {
            anchore.SetPosition(point.Anchore);
            handle1.SetPosition(point.Handle1);
            handle2.SetPosition(point.Handle2);

            lineRenderer.SetPosition(0, point.Handle1);
            lineRenderer.SetPosition(1, point.Anchore);
            lineRenderer.SetPosition(2, point.Handle2);
        }

        public void OnPointerDown(Action<Guid> callback)
        {
            anchore.OnPointerDown(() => callback(Id));
            handle1.OnPointerDown(() => callback(Id));
            handle2.OnPointerDown(() => callback(Id));
        }

        public void OnDragAnchore(Action<Guid, Vector2> callback)
        {
            anchore.OnDrag(position => callback(Id, position));
        }

        public void OnDragHandle1(Action<Guid, Vector2> callback)
        {
            handle1.OnDrag(position => callback(Id, position));
        }

        public void OnDragHandle2(Action<Guid, Vector2> callback)
        {
            handle2.OnDrag(position => callback(Id, position));
        }
    }
}
