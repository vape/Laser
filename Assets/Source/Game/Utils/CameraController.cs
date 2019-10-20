using System;
using UnityEngine;

namespace Laser.Game.Utils
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Plane Floor
        { get { return floor; } }

        public float MinCameraSize = 8;
        public float MaxCameraSize = 12;
        public Vector2 BoundsA;
        public Vector2 BoundsB;
        public float TouchZoomSpeed = 0.5f;
        [Range(0, 5)]
        public float Smoothness = 1f;

        private CameraScrollAreaController scrollArea;
        private new Camera camera;
        private Vector3 dragStartCameraPos;
        private Vector3 dragStartMousePos;
        private Plane floor = new Plane(Vector3.up, Vector3.zero);
        private bool dragging;
        private Vector3 target;
        private Vector3 velocity;
        private float targetSize;
        private float targetFov;
        private float sizeVelocity;
        private float fovVelocity;

        private CameraScrollAreaController GetScrollAreaController()
        {
            if (scrollArea == null)
            {
                scrollArea = FindObjectOfType<CameraScrollAreaController>();
            }

            return scrollArea;
        }

        private void Awake()
        {
            camera = GetComponent<Camera>();
            target = transform.position;
            targetSize = camera.orthographicSize;
            targetFov = camera.fieldOfView;
        }

        private void Update()
        {
            UpdateDrag();
            UpdateZoom();

            target = new Vector3(
                Mathf.Clamp(target.x, BoundsA.x, BoundsB.x),
                target.y,
                Mathf.Clamp(target.z, BoundsA.y, BoundsB.y)
            );

            transform.position = SmoothDamp(transform.position, target, ref velocity, 0.075f * Smoothness);
            camera.orthographicSize = Mathf.Clamp(Mathf.SmoothDamp(camera.orthographicSize, targetSize, ref sizeVelocity, 0.1f), MinCameraSize, MaxCameraSize);
        }

        private void FocusOnPoint(Vector3 point, bool immediately)
        {
            var dir = camera.transform.rotation * Vector3.back;
            var plane = new Plane(Vector3.down, camera.transform.position);
            if (plane.Raycast(new Ray(point, dir), out float distance))
            {
                var pos = point + dir.normalized * distance;
                target = pos;

                if (immediately)
                {
                    camera.transform.position = pos;
                }
            }
        }

        private void UpdateZoom()
        {
            var delta = 0f;

            if (Input.mouseScrollDelta.y != 0)
            {
                delta = Input.mouseScrollDelta.y;
            }

            if (delta != 0)
            {
                targetSize = Mathf.Clamp(targetSize - delta, MinCameraSize, MaxCameraSize);
            }
        }

        private void UpdateDrag()
        {
            if (Input.GetMouseButtonDown(0) && (GetScrollAreaController()?.CanScroll ?? true))
            {
                dragStartCameraPos = transform.position;
                dragStartMousePos = Input.mousePosition;
                dragging = true;
                return;
            }

            if (dragging && Input.GetMouseButton(0))
            {
                var p0 = RaycastFloor(camera.ScreenPointToRay(Input.mousePosition));
                var p1 = RaycastFloor(camera.ScreenPointToRay(dragStartMousePos));
                var delta = p0 - p1;
                target = dragStartCameraPos - new Vector3(delta.x, 0, delta.z);
            }

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }

        private Vector3 RaycastFloor(Ray ray)
        {
            var distance = 0f;
            if (floor.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 velocity, float time)
        {
            var x = Mathf.SmoothDamp(current.x, target.x, ref velocity.x, time);
            var y = Mathf.SmoothDamp(current.y, target.y, ref velocity.y, time);
            var z = Mathf.SmoothDamp(current.z, target.z, ref velocity.z, time);

            return new Vector3(x, y, z);
        }
    }
}
