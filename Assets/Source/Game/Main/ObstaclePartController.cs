using UnityEngine;

namespace Laser.Game.Main
{
    public class ObstaclePartController : MonoBehaviour, IGameObjectClickHandler
    {
        public void HandleClick()
        {
            transform.parent.GetComponent<ObstacleController>().HandleClick();
        }
    }
}
