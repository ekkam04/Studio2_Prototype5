using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ekkam {
    public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RawImage image;
        private LineRenderer lineRenderer;
        private Canvas canvas;

        bool isDragStarted = false;

        void Start()
        {
            image = GetComponent<RawImage>();
            lineRenderer = GetComponent<LineRenderer>();
            canvas = GetComponentInParent<Canvas>();
        }

        void Update()
        {
            if (isDragStarted)
            {
                Vector2 movePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out movePosition);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, canvas.transform.TransformPoint(movePosition));
            }
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Needed for drag but not used
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragStarted = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragStarted = false;
        }
    }
}
