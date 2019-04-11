using System.Collections.Generic;
using UnityEngine;
using System;

namespace BezierCanvas
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] RectTransform curveContainer = default;
        [SerializeField] CanvasView canvasView = default;
        [SerializeField] ControlPointView controlPointView = default;
        [SerializeField] PathView pathView = default;
        [SerializeField] PathModel model = default;

        ControlPointView currentEditingControlPoint;

        readonly Dictionary<Guid, ControlPointView> controlPointViews =
            new Dictionary<Guid, ControlPointView>();

        void Start()
        {
            canvasView.OnPointerDown(position => AddControlPoint(position));

            canvasView.OnDrag(position =>
            {
                if (!IsSelected())
                {
                    return;
                }

                MoveHandle2Symmetry(currentEditingControlPoint.Id, position);
            });

            canvasView.OnEndDrag(position =>
            {
                if (!IsSelected())
                {
                    return;
                }

                MoveHandle2Symmetry(currentEditingControlPoint.Id, position);
            });

            canvasView.OnClickButtonCorner(() =>
            {
                if (!IsSelected())
                {
                    return;
                }

                ToCorner(currentEditingControlPoint.Id);
            });

            canvasView.OnClickButtonSmooth(() =>
            {
                if (!IsSelected())
                {
                    return;
                }

                ToSmooth(currentEditingControlPoint.Id);
            });

            canvasView.OnClickButtonDelete(() =>
            {
                if (!IsSelected())
                {
                    return;
                }

                DeleteControlPoint(currentEditingControlPoint.Id);
            });

            canvasView.OnClickButtonClear(Clear);
        }

        void Select(Guid id)
        {
            var point = model.GetControlPoint(id);
            if (point == null)
            {
                return;
            }

            foreach (var view in controlPointViews.Values)
            {
                view.SetSelected(view.Id == id);
            }

            currentEditingControlPoint = controlPointViews[id];
        }

        bool IsSelected()
        {
            return currentEditingControlPoint != null;
        }

        void AddControlPoint(Vector3 anchore)
        {
            var id = Guid.NewGuid();
            var view = InstantiateControlPointView(id);
            controlPointViews.Add(id, view);

            var controlPoint = CreateControlPoint(anchore);
            model.AddControlPoint(id, controlPoint);
            view.UpdateControlPoint(controlPoint);

            Select(id);
        }

        void DeleteControlPoint(Guid id)
        {
            if (controlPointViews.ContainsKey(id))
            {
                Destroy(controlPointViews[id].gameObject);
                controlPointViews.Remove(id);
            }

            model.DeleteControlPoint(id);
        }

        void Clear()
        {
            foreach (var view in controlPointViews.Values)
            {
                model.DeleteControlPoint(view.Id);
                Destroy(view.gameObject);
            }

            controlPointViews.Clear();
        }

        void ToSmooth(Guid id)
        {
            model.ToSmooth(id);
            UpdateControlPoint(id);
        }

        void ToCorner(Guid id)
        {
            model.ToCorner(id);
            UpdateControlPoint(id);
        }

        void MoveHandle1(Guid id, Vector2 handle)
        {
            model.MoveHandle1(id, handle);
            UpdateControlPoint(id);
        }

        void MoveHandle2(Guid id, Vector2 handle)
        {
            model.MoveHandle2(id, handle);
            UpdateControlPoint(id);
        }

        void MoveHandle1Symmetry(Guid id, Vector2 handle)
        {
            model.MoveHandle1Symmetry(id, handle);
            UpdateControlPoint(id);
        }

        void MoveHandle2Symmetry(Guid id, Vector2 handle)
        {
            model.MoveHandle2Symmetry(id, handle);
            UpdateControlPoint(id);
        }

        void MoveControlPoint(Guid id, Vector2 anchore)
        {
            model.MoveControlPoint(id, anchore);
            UpdateControlPoint(id);
        }

        void UpdateControlPoint(Guid id)
        {
            var point = model.GetControlPoint(id);
            controlPointViews[id].UpdateControlPoint(point);
        }

        void UpdatePath(Path path)
        {
            const int Segments = 20;
            var points = CubicBezier.GetPoints(path, Segments);
            pathView.UpdatePath(points);
        }

        void LateUpdate()
        {
            if (model.Dirty)
            {
                UpdatePath(model.GetPath());
                model.ClearDirty();
            }
        }

        ControlPoint CreateControlPoint(Vector3 position)
        {
            return new ControlPoint
            {
                Anchore = position,
                Handle1 = position,
                Handle2 = position
            };
        }

        ControlPointView InstantiateControlPointView(Guid id)
        {
            var viewObject = Instantiate(controlPointView.gameObject, curveContainer);
            var view = viewObject.GetComponent<ControlPointView>();
            view.Id = id;
            view.OnPointerDown(Select);
            view.OnDragHandle1(MoveHandle1);
            view.OnDragHandle2(MoveHandle2);
            view.OnDragAnchore(MoveControlPoint);
            return view;
        }
    }
}
