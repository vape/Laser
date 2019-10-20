using Laser.Game.Main;
using Laser.Game.Main.Grid;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(GridController))]
    public partial class GridEditor : UnityEditor.Editor
    {
        private static Vector3? tileRectPosition;
        private static Vector3? tileRectSize;

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

            if ((gizmoType & GizmoType.Selected) != 0)
            {
                if (tileRectPosition != null)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.75f);
                    Gizmos.DrawCube(tileRectPosition.Value, tileRectSize.Value);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Layout now"))
            {
                ((GridController)target).Layout();
            }
        }

        private void OnSceneGUI()
        {
            if (Event.current.type == EventType.MouseMove)
            {
                var _target = ((GridController)target);

                var cameraRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                var gridPosition = _target.RaycastGrid(cameraRay);

                if (gridPosition != null)
                {
                    var rect = _target.GetTileRect(_target.GetGridTile(gridPosition.Value));
                    var a = _target.GridToWorld(new Vector2(rect.xMin, rect.yMin));
                    var b = _target.GridToWorld(new Vector2(rect.xMax, rect.yMax));

                    tileRectPosition = _target.GridToWorld(rect.center) + new Vector3(0, 1, 0);
                    tileRectSize = new Vector3(b.x - a.x, 1, b.z - a.z);
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                // repaint gizmos
                SceneView.RepaintAll();
            }
        }
    }
}
