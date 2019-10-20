using System.Collections.Generic;
using UnityEngine;

namespace Laser.Game.Main
{
    // north is (0,0,1)
    public enum ReflectorOrientation
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    public static class ReflectorOrientationExtensions
    {
        public static float ToRotationAngle(this ReflectorOrientation o)
        {
            return 45f * (int)o;
        }
    }

    public class ReflectorController : MonoBehaviour, IGameObjectClickHandler
    {
        public List<ReflectorOrientation> AvailableOrientations
        { get; set; } = new List<ReflectorOrientation>()
        {
            ReflectorOrientation.N,
            ReflectorOrientation.NE,
            ReflectorOrientation.E,
            ReflectorOrientation.SE,
            ReflectorOrientation.S,
            ReflectorOrientation.SW,
            ReflectorOrientation.W,
            ReflectorOrientation.NW
        };

        public ReflectorOrientation CurrentOrientation
        { get; set; } = ReflectorOrientation.N;

        public ReflectorType Type;

        private float targetAngle;
        private float speedTowardsTargetAngle;

        private void Start()
        {
            ApplyOrientation();
        }

        private void Update()
        {
            var a = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref speedTowardsTargetAngle, 0.1f);
            transform.rotation = Quaternion.Euler(0, a, 0);
        }

        public void HandleClick()
        {
            NextOrientation();
        }

        public void NextOrientation()
        {
            if (!AvailableOrientations.Contains(CurrentOrientation))
            {
                CurrentOrientation = AvailableOrientations[0];
            }
            else
            {
                var index = AvailableOrientations.IndexOf(CurrentOrientation);
                index = AvailableOrientations.Count - 1 == index ? 0 : index + 1;
                CurrentOrientation = AvailableOrientations[index];
            }

            ApplyOrientation();
        }

        private void ApplyOrientation()
        {
            targetAngle = CurrentOrientation.ToRotationAngle();
        }
    }
}
