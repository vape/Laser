using System.Collections.Generic;
using UnityEngine;

namespace Laser.Game.Main
{
    public class ReflectorController : MonoBehaviour, IGameObjectClickHandler
    {
        public List<EntityOrientation> AvailableOrientations
        { get; set; } = new List<EntityOrientation>()
        {
            EntityOrientation.N,
            EntityOrientation.NE,
            EntityOrientation.E,
            EntityOrientation.SE,
            EntityOrientation.S,
            EntityOrientation.SW,
            EntityOrientation.W,
            EntityOrientation.NW
        };

        public ReflectorType Type;

        public void NextOrientation()
        {
            var entity = GetComponent<LevelEntityController>();
            if (!AvailableOrientations.Contains(entity.Orientation))
            {
                entity.Orientation = AvailableOrientations[0];
            }
            else
            {
                var index = AvailableOrientations.IndexOf(entity.Orientation);
                index = AvailableOrientations.Count - 1 == index ? 0 : index + 1;
                entity.Orientation = AvailableOrientations[index];
            }

            entity.ApplyOrientation();
        }

        public void HandleClick()
        {
            NextOrientation();
        }
    }
}
