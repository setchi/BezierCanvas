using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform))]
public class AnchoreView : MonoBehaviour, IPointerDownHandler, IDragHandler {
    [SerializeField]
    Image image;

    RectTransform rectTransform;
    Action<Vector2> onDrag;
    Action onPointerDown;

    void Awake() {
        rectTransform = GetComponent<RectTransform> ();
    }

    public void SetColor(Color color) {
        image.color = color;
    }

    public void SetPosition(Vector3 position) {
        rectTransform.anchoredPosition3D = position;
    }

    public void OnPointerDown(Action callback) {
        onPointerDown = callback;
    }

    public void OnDrag(Action<Vector2> callback) {
        onDrag = callback;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData e) {
        e.Use ();
        if (onPointerDown == null) {
            return;
        }
        onPointerDown ();
    }

    void IDragHandler.OnDrag(PointerEventData e) {
        e.Use ();
        if (onDrag == null) {
            return;
        }
        onDrag (e.position);
    }
}
