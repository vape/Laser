using Laser.Game.Level;
using UnityEngine;

namespace Laser.Game.Main
{
    // north is (0,0,1)
    public enum EntityOrientation
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

    public static class EntityOrientationExtensions
    {
        public static float ToRotationAngle(this EntityOrientation o)
        {
            return 45f * (int)o;
        }
    }

    public enum EntityType
    {
        None,
        Emitter,
        Reflector,
        Absorber
    }

    public enum ReflectorType
    {
        None,
        HalfBlock
    }

    public enum EmitterType
    {
        None,
        Droid
    }

    public enum AbsorberType
    {
        None,
        Cubic
    }

    public class LevelEntityController : MonoBehaviour
    {
        public bool IsStationary
        { get; private set; }
        public EntityRulesBridge RulesBridge 
        { get; set; }

        public EntityType Type;
        public EntityOrientation Orientation;

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

            IsStationary = Mathf.Approximately(a, targetAngle);
        }

        public void ApplyOrientation(bool immediate = false)
        {
            targetAngle = Orientation.ToRotationAngle();

            if (immediate)
            {
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }
    }
}
