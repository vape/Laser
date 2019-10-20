using UnityEngine;

namespace Laser.Game.Main
{
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
        public EntityType Type;
    }
}
