using System.Linq;
using UnityEngine;

namespace Laser.Game.Main
{
    public class EmitterController : MonoBehaviour
    {
        public AbsorberController LitAbsorber
        { get { return litAbsorber; } }

        public EmitterType Type;
        public LineRenderer RayLine;

        private Tracer tracer = new Tracer();
        private Trace currentTrace;
        private AbsorberController litAbsorber;
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
            litAbsorber = null;
            currentTrace = tracer.Trace(RayLine.transform.position, transform.rotation * Vector3.forward);

            for (int i = 0; i < currentTrace.Points.Count; ++i)
            {
                var absorber = currentTrace.Points[i].Transform?.GetComponent<AbsorberController>();
                if (absorber != null)
                {
                    litAbsorber = absorber;
                    break;
                }
            }
        }

        private void RefreshLineRenderer()
        {
            if (currentTrace == null)
            {
                RayLine.SetPositions(new Vector3[0]);
                return;
            }

            var _positions = currentTrace.Points.Select((t) => RayLine.transform.InverseTransformPoint(t.Position));
            if (!currentTrace.Closed)
            {
                var p = RayLine.transform.InverseTransformPoint(currentTrace.Points[currentTrace.Points.Count - 1].Position);
                var d = RayLine.transform.InverseTransformDirection(currentTrace.Points[currentTrace.Points.Count - 1].ReflectedDirection);
                _positions = _positions.Concat(new Vector3[1] { p + (d * 100) });
            }
            var positions = _positions.ToArray();
            var color = litAbsorber == null ? Color.red : Color.green;

            RayLine.positionCount = positions.Length;
            RayLine.SetPositions(positions);
            RayLine.colorGradient = new Gradient() { colorKeys = new GradientColorKey[1] { new GradientColorKey(color, 0) } };
        }
    }
}
