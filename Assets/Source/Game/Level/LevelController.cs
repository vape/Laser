using Laser.Game.Main.Grid;
using UnityEngine;

namespace Laser.Game.Level
{
    [RequireComponent(typeof(GridController))]
    public partial class LevelController : MonoBehaviour
    {
        public bool IsLevelLoaded
        {
            get
            {
                return levelLoaded;
            }
        }

        public GridController Grid;
        public LevelItemsMap ItemsMap;

        private bool levelLoaded;

        private void Awake()
        { }
    }
}
