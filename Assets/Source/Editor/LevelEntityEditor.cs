using Laser.Game.Main;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(LevelEntityController))]
    public class LevelEntityEditor : UnityEditor.Editor
    {
        private Vector2 dragStartPosition;
        private List<(EntityOrientation orientation, float angle)> orientationAngles;
        private float currentRotation;

        private void OnEnable()
        {
            orientationAngles = new List<(EntityOrientation, float)>();
            foreach (EntityOrientation value in Enum.GetValues(typeof(EntityOrientation)))
            {
                orientationAngles.Add((value, value.ToRotationAngle()));
            }

            Undo.undoRedoPerformed += UndoCallback;
        }

        private void UndoCallback()
        {
            if (target == null)
            {
                return;
            }

            var entity = (LevelEntityController)target;
            entity.ApplyOrientation(true);
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
        }

        private void OnSceneGUI()
        {
            RotationHandle();
        }

        private void RotationHandle()
        {
            var entity = (LevelEntityController)target;
            var id = EditorGUIUtility.GetControlID(UnityEngine.FocusType.Passive);
            

            switch (Event.current.type)
            {
                case EventType.Repaint:
                case EventType.Layout:
                    var size = HandleUtility.GetHandleSize(entity.transform.position);
                    var radius = 1.5f * size;
                    var isRotating = GUIUtility.hotControl == id;
                    var fillColor = isRotating ? new Color(1, 1, 0, 0.1f) : new Color(0.6f, 1, 0.2f, 0.05f);
                    var outlineColor = isRotating ? new Color(1, 1, 0, 0.7f) : new Color(0.6f, 1, 0.2f, 1f);
                    var nearestOrientation = NearestOrientation(entity.transform.rotation.eulerAngles.y);

                    Handles.color = outlineColor;
                    Handles.CircleHandleCap(id, entity.transform.position, entity.transform.rotation * Quaternion.Euler(90, 0, 0), radius, Event.current.type);

                    var currentRotationArcFromPoint = GetCircumferencePoint(entity.Orientation.ToRotationAngle(), size);
                    var currentRotationArcToPoint = GetCircumferencePoint(entity.Orientation.ToRotationAngle() + currentRotation, radius);

                    Handles.color = fillColor;
                    Handles.DrawSolidArc(entity.transform.position, Vector3.up, currentRotationArcFromPoint, currentRotation, radius);
                    
                    if (isRotating)
                    {
                        Handles.color = outlineColor;
                        Handles.DrawLine(entity.transform.position, entity.transform.position + currentRotationArcFromPoint);
                        Handles.DrawLine(entity.transform.position, entity.transform.position + currentRotationArcToPoint);
                    }

                    for (int i = 0; i < orientationAngles.Count; ++i)
                    {
                        var orientationSnapPoint = GetCircumferencePoint(orientationAngles[i].angle, radius);
                        Handles.color = outlineColor;
                        Handles.CubeHandleCap(id, entity.transform.position + orientationSnapPoint, Quaternion.identity, 0.04f * size, Event.current.type);

                        if (orientationAngles[i].orientation == nearestOrientation)
                        {
                            var targetRotationArcToPoint = GetCircumferencePoint(nearestOrientation.ToRotationAngle(), radius);
                            Handles.DrawDottedLine(entity.transform.position, entity.transform.position + targetRotationArcToPoint, radius / 5);
                        }
                    }
                    
                    break;
                case EventType.MouseDown:
                    if (HandleUtility.nearestControl == id && Event.current.button == 0)
                    {
                        dragStartPosition = Event.current.mousePosition;
                        Undo.RegisterCompleteObjectUndo(entity, "Rotating");
                        GUIUtility.hotControl = id;
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseLeaveWindow:
                case EventType.MouseUp:
                    if (Event.current.type == EventType.MouseLeaveWindow || GUIUtility.hotControl == id)
                    {
                        entity.Orientation = NearestOrientation(entity.transform.rotation.eulerAngles.y);
                        entity.ApplyOrientation(true);
                        currentRotation = 0;
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id && Event.current.button == 0)
                    {
                        var deltax = (Event.current.mousePosition - dragStartPosition).x;
                        var deltay = (Event.current.mousePosition - dragStartPosition).y;
                        
                        currentRotation = -deltax / 2;
                        var angle = entity.Orientation.ToRotationAngle() + currentRotation;
                        
                        if (angle < 0)
                        {
                            angle = 360 - (Mathf.Abs(angle) % 360);
                        }
                        else
                        {
                            angle = angle % 360;
                        }

                        var snapAngle = NearestOrientation(angle).ToRotationAngle();
                        var rotation = Quaternion.Euler(0, snapAngle, 0);
                        entity.transform.rotation = rotation;
                        Event.current.Use();
                    }
                    break;
            }
        }

        private EntityOrientation NearestOrientation(float angle)
        {
            var minDist = float.MaxValue;
            var o = EntityOrientation.N;

            for (int i = 0; i < orientationAngles.Count; ++i)
            {
                var dist = Mathf.Abs(angle - orientationAngles[i].angle);
                if (dist < minDist)
                {
                    minDist = dist;
                    o = orientationAngles[i].orientation;
                }
            }

            return o;
        }

        private Vector3 GetCircumferencePoint(float angle, float radius)
        {
            angle = Mathf.Deg2Rad * angle;
            var point = new Vector3((Vector3.forward.x * Mathf.Cos(-angle) - Vector3.forward.z * Mathf.Sin(-angle)), 0, (Vector3.forward.z * Mathf.Cos(-angle) + Vector3.forward.x * Mathf.Sin(-angle)));
            return point.normalized * radius;
        }
    }
}
