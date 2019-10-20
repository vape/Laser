using UnityEngine;

namespace Laser.Game.Main
{
    public enum EntityType
    {
        Emitter,
        Reflector,
        Absorber
    }

    public enum ReflectorType
    {
        HalfBlock
    }

    public class LevelEntityController : MonoBehaviour
    {
        public EntityType Type;
    }
}
