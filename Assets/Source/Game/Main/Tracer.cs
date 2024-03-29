﻿#if UNITY_EDITOR
// #define DEBUG_DRAW
#endif

using Laser.Game.Main.Reflector;
using System.Collections.Generic;
using UnityEngine;

namespace Laser.Game.Main
{
    public class TraceHitPoint
    {
        public Vector3 Position;
        public Vector3 ReflectedDirection;
        public Transform Transform;
    }

    public class Trace
    {
        public List<TraceHitPoint> Points;
        public bool Closed;
    }

    public class Tracer
    {
        public int MaxRays = 100;

        public Trace Trace(Vector3 origin, Vector3 direction)
        {
            var points = new List<TraceHitPoint>();
            points.Add(new TraceHitPoint() {
                Position = origin,
                ReflectedDirection = direction
            });

            var r = 1;
            var p = TraceInternal(origin, direction);
            while (p != null && r < MaxRays)
            {
#if DEBUG_DRAW
                var __prev = points[points.Count - 1];
                Debug.DrawLine(__prev.Position, p.Position);
#endif

                points.Add(p);

                if (p?.Transform?.GetComponent<ReflectiveSurfaceController>() == null)
                {
                    break;
                }

                if (p?.Transform?.GetComponent<AbsorberController>() != null)
                {
                    break;
                }

                p = TraceInternal(p.Position, p.ReflectedDirection);
                r++;
            }

            var trace = new Trace()
            {
                Points = points,
                Closed = p != null
            };

#if DEBUG_DRAW
            Debug.DrawRay(points[points.Count - 1].Position, points[points.Count - 1].ReflectedDirection);
#endif

            return trace;
        }

        private static TraceHitPoint TraceInternal(Vector3 origin, Vector3 direction)
        {
            var ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out var hit, 1000))
            {
                var dirn = direction.normalized;
                var reflection = -(2 * Vector3.Dot(hit.normal, dirn) * hit.normal - dirn);

                return new TraceHitPoint()
                {
                    Position = hit.point,
                    ReflectedDirection = reflection,
                    Transform = hit.transform
                };
            }

            return null;
        }
    }
}
