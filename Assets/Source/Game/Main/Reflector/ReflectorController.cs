using System.Collections.Generic;
using UnityEngine;

namespace Laser.Game.Main
{
    [RequireComponent(typeof(LevelEntityController))]
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
        private LevelEntityController EntityController
        {
            get
            {
                if (entityController == null)
                {
                    entityController = GetComponent<LevelEntityController>();
                }

                return entityController;
            }
        }

        public ReflectorType Type;

        private LevelEntityController entityController;

        public void NextOrientation()
        {
            if (!AvailableOrientations.Contains(EntityController.Orientation))
            {
                EntityController.Orientation = AvailableOrientations[0];
            }
            else
            {
                var index = AvailableOrientations.IndexOf(EntityController.Orientation);
                index = AvailableOrientations.Count - 1 == index ? 0 : index + 1;
                EntityController.Orientation = AvailableOrientations[index];
            }

            EntityController.ApplyOrientation();
        }

        public void HandleClick()
        {
            if (EntityController.RulesBridge?.IsLevelInteractive ?? true)
            {
                NextOrientation();
            }
        }
    }
}
