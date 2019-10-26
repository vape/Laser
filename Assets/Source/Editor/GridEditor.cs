using Laser.Game.Main;
using Laser.Game.Main.Grid;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(GridController))]
    public partial class GridEditor : UnityEditor.Editor
    {
        private static void DrawGridOutline(GridController target)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                for (int x = 0; x < target.Width; ++x)
                {
                    var startX = target.GridToWorld((Vector2.up * y * target.CellSize) + Vector2.right * (x * target.CellSize));
                    var endX = target.GridToWorld((Vector2.up * y * target.CellSize) + Vector2.right * ((x + 1) * target.CellSize));
                    Gizmos.DrawLine(startX, endX);

                    var startY = target.GridToWorld((Vector2.right * x * target.CellSize) + Vector2.up * (y * target.CellSize));
                    var endY = target.GridToWorld((Vector2.right * x * target.CellSize) + Vector2.up * ((y + 1) * target.CellSize));
                    Gizmos.DrawLine(startY, endY);
                }
            }

            var _startX = target.GridToWorld((Vector2.up * target.Height * target.CellSize));
            var _endX = target.GridToWorld((Vector2.up * target.Height * target.CellSize) + Vector2.right * (target.Width * target.CellSize));
            Gizmos.DrawLine(_startX, _endX);

            var _startY = target.GridToWorld((Vector2.right * target.Width * target.CellSize));
            var _endY = target.GridToWorld((Vector2.right * target.Width * target.CellSize) + Vector2.up * (target.Height * target.CellSize));
            Gizmos.DrawLine(_startY, _endY);
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawGizmos(GridController target, GizmoType gizmoType)
        {
            Gizmos.color = Color.black;
            DrawGridOutline(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Layout now"))
            {
                ((GridController)target).Layout();
            }
        }
    }
}
