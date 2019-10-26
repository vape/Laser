using Laser.Game.Main.Grid;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(GridElementController))]
    public class GridElementEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawGizmos(GridElementController target, GizmoType gizmoType)
        {
            // Only for convenient grid tile selection

            var grid = target.GetComponentInParent<GridController>();
            if (grid == null)
            {
                return;
            }

            var gridPos = grid.WorldToGrid(target.transform.position);
            var rect = grid.GetTileRect(grid.GetGridTile(gridPos));
            var a = grid.GridToWorld(new Vector2(rect.xMin, rect.yMin));
            var b = grid.GridToWorld(new Vector2(rect.xMax, rect.yMax));
            var p = grid.GridToWorld(rect.center) + new Vector3(0, 0, 0);
            var s = new Vector3(b.x - a.x, 0, b.z - a.z);

            Gizmos.color = new Color(0, 0, 0, 0);
            Gizmos.DrawCube(p, s);
        }

        private void OnEnable()
        {
            Tools.hidden = true;
        }

        private void OnDisable()
        {
            Tools.hidden = false;
        }

        public GridTile targetTile;
        public bool isSuitableTile = true;

        private void OnSceneGUI()
        {
            MovingHandle();
        }

        private void MovingHandle()
        {
            var elem = target as GridElementController;
            var grid = elem.GetComponentInParent<GridController>();
            if (grid == null)
            {
                return;
            }

            var id = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.type)
            {
                case EventType.Layout:
                case EventType.Repaint:
                    var gridPos = grid.WorldToGrid(elem.transform.position);
                    if (GUIUtility.hotControl == id)
                    {
                        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        gridPos = grid.RaycastGrid(ray) ?? gridPos;
                    }

                    var tile = grid.GetGridTile(gridPos);
                    var tileRect = grid.GetWorldSpacedTileRect(tile);
                    var rectColor = isSuitableTile ? new Color(0, 1, 0, 0.1f) : new Color(1, 0, 0, 0.3f);
                    var outlineColor = isSuitableTile ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.7f);
                    var handleColor = outlineColor;

                    Handles.DrawSolidRectangleWithOutline(tileRect, rectColor, outlineColor);
                    Handles.color = handleColor;
                    Handles.DrawSolidDisc(grid.GridToWorld(gridPos), Vector3.up, 3);
                    Handles.CircleHandleCap(id, grid.GridToWorld(gridPos), Quaternion.Euler(90, 0, 0), 3, Event.current.type);
                    break;
                case EventType.MouseDown:
                    if (HandleUtility.nearestControl == id && Event.current.button == 0)
                    {
                        GUIUtility.hotControl = id;
                        RefreshTargetTile(grid, elem);
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        RefreshTargetTile(grid, elem, grid.WorldToGrid(elem.transform.position));
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        RefreshTargetTile(grid, elem);

                        if (isSuitableTile)
                        {
                            elem.X = targetTile.X;
                            elem.Y = targetTile.Y;
                            grid.Layout();
                        }

                        Event.current.Use();
                    }
                    break;
            }
        }

        private void RefreshTargetTile(GridController grid, GridElementController elem, Vector2? gridPosition = null)
        {
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            gridPosition = gridPosition ?? grid.RaycastGrid(ray);

            targetTile = gridPosition == null ? new GridTile(0, 0) : grid.GetGridTile(gridPosition.Value);
            isSuitableTile = grid.CanPlaceIn(elem, targetTile);
        }
    }
}
