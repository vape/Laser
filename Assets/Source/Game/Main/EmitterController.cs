using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laser.Game.Main
{
    public class EmitterController : MonoBehaviour
    {
        public LineRenderer RayLine;

        private Tracer tracer = new Tracer();
        private List<TraceHitPoint> currentTrace = new List<TraceHitPoint>();
        private AbsorberController touchedAbsorber;
        private int skippedFrames;

        private void FixedUpdate()
        {
            if (skippedFrames < 1)
            {
                skippedFrames++;
                return;
            }

            RefreshTrace();
            RefreshLineRenderer();
            skippedFrames = 0;
        }

        private void RefreshTrace()
        {
            touchedAbsorber = null;
            currentTrace = tracer.Trace(RayLine.transform.position, transform.rotation * Vector3.forward);

            for (int i = 0; i < currentTrace.Count; ++i)
            {
                var absorber = currentTrace[i].Transform?.GetComponent<AbsorberController>();
                if (absorber != null)
                {
                    touchedAbsorber = absorber;
                    break;
                }
            }
        }

        private void RefreshLineRenderer()
        {
            if (currentTrace?.Count == 0)
            {
                RayLine.SetPositions(new Vector3[0]);
                return;
            }

            var positions = currentTrace.Select((t) => RayLine.transform.InverseTransformPoint(t.Position)).ToArray();
            var color = touchedAbsorber == null ? Color.red : Color.green;

            RayLine.positionCount = positions.Length;
            RayLine.SetPositions(positions);
            RayLine.colorGradient = new Gradient() { colorKeys = new GradientColorKey[1] { new GradientColorKey(color, 0) } };
        }
    }
}
