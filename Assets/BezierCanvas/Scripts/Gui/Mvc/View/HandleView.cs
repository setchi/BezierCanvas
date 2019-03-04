using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace BezierCanvas
{
    [RequireComponent(typeof(RectTransform))]
    public class HandleView : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        RectTransform rectTransform;
        Action<Vector2> onDrag;
        Action onPointerDown;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetPosition(Vector3 position)
        {
            rectTransform.anchoredPosition3D = position;
        }

        public void OnDrag(Action<Vector2> callback)
        {
            onDrag = callback;
        }

        public void OnPointerDown(Action callback)
        {
            onPointerDown = callback;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData e)
        {
            e.Use();
            onPointerDown?.Invoke();
        }

        void IDragHandler.OnDrag(PointerEventData e)
        {
            e.Use();
            onDrag?.Invoke(e.position);
        }
    }
}
