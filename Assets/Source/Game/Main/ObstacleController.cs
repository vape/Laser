using System.Collections.Generic;
using UnityEngine;

namespace Laser.Game.Main
{
    // north is (0,0,1)
    public enum ObstacleOrientation
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

    public static class ObstacleOrientationExtensions
    {
        public static float ToRotationAngle(this ObstacleOrientation o)
        {
            return 45f * (int)o;
        }
    }

    public class ObstacleController : MonoBehaviour, IGameObjectClickHandler
    {
        public List<ObstacleOrientation> AvailableOrientations
        { get; set; } = new List<ObstacleOrientation>()
        {
            ObstacleOrientation.N,
            ObstacleOrientation.NE,
            ObstacleOrientation.E,
            ObstacleOrientation.SE,
            ObstacleOrientation.S,
            ObstacleOrientation.SW,
            ObstacleOrientation.W,
            ObstacleOrientation.NW
        };

        public ObstacleOrientation CurrentOrientation
        { get; set; } = ObstacleOrientation.N;

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
