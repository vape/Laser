using Laser.Game.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(CameraController))]
    public class CameraEditor : UnityEditor.Editor
    {
        public bool ShowCoverageProjection;

        [DrawGizmo(GizmoType.Selected)]
        private static void DrawGizmos(CameraController target, GizmoType gizmoType)
        {
            var y = 0.1f;
            var a = target.BoundsA;
            var b = target.BoundsB;

            var l0 = (new Vector3(a.x, y, a.y), new Vector3(a.x, y, b.y));
            var l0c = b.x < a.x ? Color.red : Color.green;
            var l1 = (new Vector3(a.x, y, b.y), new Vector3(b.x, y, b.y));
            var l1c = b.y < a.y ? Color.red : Color.green;
            var l2 = (new Vector3(b.x, y, b.y), new Vector3(b.x, y, a.y));
            var l2c = b.x < a.x ? Color.red : Color.green;
            var l3 = (new Vector3(b.x, y, a.y), new Vector3(a.x, y, a.y));
            var l3c = b.y < a.y ? Color.red : Color.green;

            Gizmos.color = l0c;
            Gizmos.DrawLine(l0.Item1, l0.Item2);
            Gizmos.color = l1c;
            Gizmos.DrawLine(l1.Item1, l1.Item2);
            Gizmos.color = l2c;
            Gizmos.DrawLine(l2.Item1, l2.Item2);
            Gizmos.color = l3c;
            Gizmos.DrawLine(l3.Item1, l3.Item2);
        }

        public static void Header(string value)
        {
            GUILayout.Label(value, EditorStyles.boldLabel);
        }

        public override void OnInspectorGUI()
        {
            var controller = target as CameraController;
            var camera = controller.GetComponent<Camera>();

            Header("Main");

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(controller.Smoothness)));

            Header("Coverage");

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(controller.MinCameraSize)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(controller.MaxCameraSize)));

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(controller.BoundsA)), new GUIContent("Bounds Top Left"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(controller.BoundsB)), new GUIContent("Bounds Bottom Right"));
            ShowCoverageProjection = GUILayout.Toggle(ShowCoverageProjection, "Show Coverage Map");

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var controller = target as CameraController;
            var cam = controller.GetComponent<Camera>();

            if (ShowCoverageProjection)
            {
                DrawCoverageProjection(cam, controller.Floor, controller.BoundsA, controller.BoundsB, controller.MaxCameraSize, new Color(1, 0, 0));
                DrawCoverageProjection(cam, controller.Floor, controller.BoundsA, controller.BoundsB, controller.MinCameraSize, new Color(0, 1, 0));
            }

            var aBoundDragId = GUIUtility.GetControlID(FocusType.Passive);
            var bBoundDragId = GUIUtility.GetControlID(FocusType.Passive);

            var newPositionA = Handles.FreeMoveHandle(new Vector3(controller.BoundsA.x, 0, controller.BoundsA.y), Quaternion.identity, 3, Vector3.zero, Handles.CubeHandleCap);
            controller.BoundsA = new Vector2(newPositionA.x, newPositionA.z);

            var newPositionB = Handles.FreeMoveHandle(new Vector3(controller.BoundsB.x, 0, controller.BoundsB.y), Quaternion.identity, 3, Vector3.zero, Handles.CubeHandleCap);
            controller.BoundsB = new Vector2(newPositionB.x, newPositionB.z);
        }

        private void DrawCoverageProjection(Camera cam, Plane floor, Vector2 p0, Vector2 p1, float orthographicSize, Color color)
        {
            var f = cam.fieldOfView;
            var s = cam.orthographicSize;
            var p = cam.transform.position;
            cam.orthographicSize = orthographicSize;

            var positions = new Vector3[4]
            {
                new Vector3(p0.x, cam.transform.position.y, p0.y),
                new Vector3(p1.x, cam.transform.position.y, p0.y),
                new Vector3(p1.x, cam.transform.position.y, p1.y),
                new Vector3(p0.x, cam.transform.position.y, p1.y)
            };
            var projectionOrigins = new Vector3[4 * 4];
            var projectionDirections = new Vector3[4 * 4];
            for (int i = 0; i < 4; ++i)
            {
                cam.transform.position = positions[i];

                if (cam.orthographic)
                {
                    projectionOrigins[(i * 4) + 0] = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
                    projectionOrigins[(i * 4) + 1] = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
                    projectionOrigins[(i * 4) + 2] = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
                    projectionOrigins[(i * 4) + 3] = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));

                    projectionDirections[(i * 4) + 0] = cam.transform.rotation * Vector3.forward;
                    projectionDirections[(i * 4) + 1] = cam.transform.rotation * Vector3.forward;
                    projectionDirections[(i * 4) + 2] = cam.transform.rotation * Vector3.forward;
                    projectionDirections[(i * 4) + 3] = cam.transform.rotation * Vector3.forward;
                }
                else
                {
                    projectionOrigins[(i * 4) + 0] = cam.transform.position;
                    projectionOrigins[(i * 4) + 1] = cam.transform.position;
                    projectionOrigins[(i * 4) + 2] = cam.transform.position;
                    projectionOrigins[(i * 4) + 3] = cam.transform.position;

                    var frustumCorners = new Vector3[4];
                    cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

                    projectionDirections[(i * 4) + 0] = cam.transform.TransformDirection(frustumCorners[0]);
                    projectionDirections[(i * 4) + 1] = cam.transform.TransformDirection(frustumCorners[1]);
                    projectionDirections[(i * 4) + 2] = cam.transform.TransformDirection(frustumCorners[2]);
                    projectionDirections[(i * 4) + 3] = cam.transform.TransformDirection(frustumCorners[3]);
                }
            }

            cam.fieldOfView = f;
            cam.orthographicSize = s;
            cam.transform.position = p;
            
            var projectedPoints = new Vector3[projectionOrigins.Length];
            for (int i = 0; i < projectionOrigins.Length; ++i)
            {
                var ray = new Ray(projectionOrigins[i], projectionDirections[i]);
                if (floor.Raycast(ray, out float distance))
                {
                    // FIXME: Dirty hack because current convex hull finding alg. sometimes fails on points whose positions is equal on one of the axises.
                    projectedPoints[i] = ray.GetPoint(distance) + new Vector3(UnityEngine.Random.Range(0.01f, 0.04f), 0, UnityEngine.Random.Range(0.01f, 0.04f));
                }
            }

            var hull = FindConvexHull(projectedPoints);
            Handles.color = new Color(color.r, color.g, color.b, 0.5f * color.a);
            Handles.DrawAAConvexPolygon(hull);
            Handles.color = new Color(color.r, color.g, color.b, color.a);
            Handles.DrawAAPolyLine(hull.Concat(new Vector3[1] { hull[0] }).ToArray());
        }

        public Vector3[] FindConvexHull(Vector3[] points)
        {
            void Swap(ref Vector3 v1, ref Vector3 v2)
            {
                Vector3 tmp = v1;
                v1 = v2;
                v2 = tmp;
            }

            float Theta(Vector3 p1, Vector3 p2)
            {
                float dx = p2.x - p1.x;
                float dy = p2.z - p1.z;
                float ax = Mathf.Abs(dx);
                float ay = Mathf.Abs(dy);
                float t = (ax + ay == 0) ? 0 : dy / (ax + ay);

                if (dx < 0)
                {
                    t = 2 - t;
                }
                else if (dy < 0)
                {
                    t = 4 + t;
                }

                return t * 90.0f;
            }
            
            var hull = new Vector3[points.Length + 1];
            var min = 0;
            var hullPoints = 0;
            var v = 0.0f;

            for (int i = 0; i < points.Length; i++)
            {
                hull[i] = points[i];

                if (hull[i].z < hull[min].z)
                {
                    min = i;
                }
            }

            hull[points.Length] = hull[min];
            Swap(ref hull[0], ref hull[min]);

            while (min != points.Length)
            {
                float minAngle = 360.0f;

                for (int i = hullPoints + 1; i < points.Length + 1; i++)
                {
                    float angle = Theta(hull[hullPoints], hull[i]);

                    if (angle > v && angle < minAngle)
                    {
                        minAngle = angle;
                        min = i;
                    }
                }

                v = minAngle;
                hullPoints++;
                if ((360 - v) < 1e-6)
                {
                    hull[hullPoints] = hull[min];
                    break;
                }

                Swap(ref hull[hullPoints], ref hull[min]);
            }

            Array.Resize(ref hull, hullPoints);
            return hull;
        }
    }
}
