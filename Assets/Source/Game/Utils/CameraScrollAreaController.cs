using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Laser.Game.Utils
{
    public class NonDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }

        private RectTransform rect;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }

    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class CameraScrollAreaController : NonDrawingGraphic, IPointerExitHandler, IPointerEnterHandler
    {
        public bool CanScroll
        { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CanScroll = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CanScroll = false;
        }
    }
}