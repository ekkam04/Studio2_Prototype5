using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ekkam {
    public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public bool isLeftWire = false;
        public Color wireColor;

        private RawImage image;
        public LineRenderer lineRenderer;
        private Canvas canvas;

        bool isDragStarted = false;
        public bool isSuccessful = false;

        GeneratorFixing generatorFixing;

        void Awake()
        {
            image = GetComponent<RawImage>();
            lineRenderer = GetComponent<LineRenderer>();
            canvas = GetComponentInParent<Canvas>();
            generatorFixing = FindObjectOfType<GeneratorFixing>();
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
            else
            {
                if (!isSuccessful)
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, transform.position);
                }
            }

            bool isHovered = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, canvas.worldCamera);

            if (isHovered)
            {
                generatorFixing.currentHoveredWire = this;
            }
        }

        public void SetColor(Color color)
        {
            print("Setting color to " + color.ToString());
            if (image != null) {
                image.color = color;
                wireColor = color;
            }
            if (lineRenderer != null) {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Needed for drag but not used
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isLeftWire) return;
            if (isSuccessful) return;
            isDragStarted = true;
            generatorFixing.currentDraggedWire = this;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (generatorFixing.currentHoveredWire != null)
            {
                if (generatorFixing.currentHoveredWire.wireColor == wireColor && !generatorFixing.currentHoveredWire.isLeftWire)
                {
                    isSuccessful = true;
                    generatorFixing.currentHoveredWire.isSuccessful = true;
                }
            }
            isDragStarted = false;
            generatorFixing.currentDraggedWire = null;
        }
    }
}
