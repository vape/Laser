using Laser.Game.Main.Grid;
using System.Linq;
using UnityEngine;

namespace Laser.Game.Level
{
    public enum LevelState
    {
        None,
        Playing,
        Won
    }

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

        public LevelState State
        {
            get
            {
                return state;
            }
            private set
            {
                state = value;
                rulesBridge.IsLevelInteractive = state == LevelState.Playing;
                L.Debug($"Level state is set to {state}");
            }
        }

#if UNITY_EDITOR
        public LevelEntity EntityToSpawn
        { get; set; }
#endif

        public GridController Grid;
        public LevelItemsMap ItemsMap;

        private bool levelLoaded;
        private LevelState state;
        private EntityRulesBridge rulesBridge = new EntityRulesBridge();

        private void Awake()
        { }

        private void Update()
        {
            RefreshState();
        }

        private void RefreshState()
        {
            if (State == LevelState.None)
            {
                if (IsLevelLoaded)
                {
                    State = LevelState.Playing;
                }

                return;
            }

            if (State == LevelState.Playing)
            {
                if (!IsLevelLoaded)
                {
                    State = LevelState.None;
                    return;
                }

                if (Emitters.Any((e) => e.LitAbsorber != null) && Entities.All((e) => e.IsStationary))
                {
                    State = LevelState.Won;
                }
            }
        }
    }
}
