using Laser.Game.Main;
using Laser.Game.Main.Grid;
using System;
using System.Collections.Generic;
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
            Undo.undoRedoPerformed += UndoCallback;
        }

        private void OnDisable()
        {
            Tools.hidden = false;
            Undo.undoRedoPerformed -= UndoCallback;
        }

        private void UndoCallback()
        {
            if (target == null)
            {
                return;
            }
            var elem = target as GridElementController;
            var entity = elem.GetComponentInChildren<LevelEntityController>();
            if (entity != null)
            {
                entity.ApplyOrientation(true);
            }
        }

        public GridTile targetTile;
        public bool isSuitableTile = true;
        private Vector2 dragStartPosition;
        private float currentRotation;

        private void OnSceneGUI()
        {
            var elem = target as GridElementController;
            var entity = elem.GetComponentInChildren<LevelEntityController>();
            if (entity != null)
            {
                LevelEntityEditor.RotationHandle(entity, ref dragStartPosition, ref currentRotation, new Color(0, 1, 0), new Color(1, 1, 0), 7);
            }

            MovingHandle(elem);
        }

        private void MovingHandle(GridElementController elem)
        {
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

                    var isMoving = GUIUtility.hotControl == id;
                    var tile = grid.GetGridTile(gridPos);
                    var tileRect = grid.GetWorldSpacedTileRect(tile);
                    var rectColor = isSuitableTile ? new Color(0, 1, 0, 0.1f) : new Color(1, 0, 0, 0.3f);
                    var outlineColor = isSuitableTile ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.7f);
                    var innerRectColor = isSuitableTile ? (isMoving ? new Color(1, 1, 0, 0.1f) : rectColor) : new Color(1, 0, 0, 0.3f);
                    var innerRectOutlineColor = isSuitableTile ? (isMoving ? new Color(1, 1, 0, 0.5f) : outlineColor) : new Color(1, 0, 0, 0.7f);
                    var handlePos = grid.GridToWorld(gridPos);
                    var size = 4;
                    var innerRectSize = size * 0.5f;

                    Handles.DrawSolidRectangleWithOutline(tileRect, rectColor, outlineColor);

                    Handles.color = new Color(1, 1, 1, 1);
                    Handles.color = innerRectColor;
                    Handles.DrawSolidDisc(grid.GridToWorld(gridPos), Vector3.up, innerRectSize);
                    Handles.color = innerRectOutlineColor;
                    Handles.CircleHandleCap(id, grid.GridToWorld(gridPos), Quaternion.Euler(90, 0, 0), innerRectSize, Event.current.type);
                    // Handles.DrawSolidRectangleWithOutline(CreateTileRect(handlePos, innerRectSize), innerRectColor, innerRectOutlineColor);
                    Handles.color = innerRectOutlineColor;
                    Handles.ArrowHandleCap(id, handlePos + Vector3.forward * innerRectSize, Quaternion.identity, size, Event.current.type);
                    Handles.ArrowHandleCap(id, handlePos + Vector3.right * innerRectSize, Quaternion.Euler(0, 90, 0), size, Event.current.type);
                    Handles.ArrowHandleCap(id, handlePos + Vector3.back * innerRectSize, Quaternion.Euler(0, 180, 0), size, Event.current.type);
                    Handles.ArrowHandleCap(id, handlePos + Vector3.left * innerRectSize, Quaternion.Euler(0, 270, 0), size, Event.current.type);
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

        private Vector3[] CreateTileRect(Vector3 center, float size)
        {
            var v0 = new Vector3(center.x - size, 0, center.z - size);
            var v1 = new Vector3(center.x + size, 0, center.z - size);
            var v2 = new Vector3(center.x + size, 0, center.z + size);
            var v3 = new Vector3(center.x - size, 0, center.z + size);

            return new Vector3[4] { v0, v1, v2, v3 };
        }
    }
}
