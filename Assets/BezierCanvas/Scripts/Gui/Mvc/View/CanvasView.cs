using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace BezierCanvas
{
    [RequireComponent(typeof(RectTransform))]
    public class CanvasView : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Button buttonCorner = default;
        [SerializeField] Button buttonSmooth = default;
        [SerializeField] Button buttonDelete = default;
        [SerializeField] Button buttonClear = default;

        Action<Vector2> onPointerDown;
        Action<Vector2> onDrag;
        Action<Vector2> onEndDrag;

        public void OnClickButtonSmooth(UnityAction callback)
        {
            buttonSmooth.onClick.AddListener(callback);
        }

        public void OnClickButtonCorner(UnityAction callback)
        {
            buttonCorner.onClick.AddListener(callback);
        }

        public void OnClickButtonClear(UnityAction callback)
        {
            buttonClear.onClick.AddListener(callback);
        }

        public void OnClickButtonDelete(UnityAction callback)
        {
            buttonDelete.onClick.AddListener(callback);
        }

        public void OnPointerDown(Action<Vector2> callback)
        {
            onPointerDown = callback;
        }

        public void OnDrag(Action<Vector2> callback)
        {
            onDrag = callback;
        }

        public void OnEndDrag(Action<Vector2> callback)
        {
            onEndDrag = callback;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData e)
        {
            onPointerDown?.Invoke(e.position);
        }

        void IDragHandler.OnDrag(PointerEventData e)
        {
            onDrag?.Invoke(e.position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData e)
        {
            onEndDrag?.Invoke(e.position);
        }
    }
}
