using UnityEngine;

namespace Laser.Game.Utils
{
    public class ClickRedirect : MonoBehaviour, IGameObjectClickHandler
    {
        public enum Direction
        {
            Up,
            Reference
        }

        public Direction Dir;
        public GameObject Reference;

        public void HandleClick()
        {
            switch (Dir)
            {
                case Direction.Up:
                    if (transform.parent.TryGetComponent<IGameObjectClickHandler>(out var parentClickHandler)) { parentClickHandler.HandleClick(); }
                    break;
                case Direction.Reference:
                    if (Reference.TryGetComponent<IGameObjectClickHandler>(out var refClickHandler)) { refClickHandler.HandleClick(); };
                    break;
            }
        }
    }
}
