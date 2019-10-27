using Laser.Game.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Laser.Game
{
    public enum GameState
    {
        Menu,
        Playing,
        EditorPlaying
    }

    public class GameManager : MonoBehaviour
    {
        public static bool EditorMode = false;

        public GameState GameState
        {
            get
            {
                return gameState;
            }
            set
            {
                gameState = value;
                L.Debug($"Game state is set to {gameState}.");
            }
        }

        public LevelSequence LevelSequence;
        public LevelController LevelController;

        private GameState gameState;

        private void Awake()
        {
            GameState = EditorMode ? GameState.EditorPlaying : GameState.Menu;
        }

        public void LoadNextLevel()
        {
            if (LevelSequence.Levels.Length == 0)
            {
                throw new Exception("Level sequence contains no levels!");
            }

            var level = App.Profile.Progression.CurrentLevel;
            if (LevelSequence.Levels.Length <= level)
            {
                level = LevelSequence.Levels.Length - 1;
            }

            LevelController.Load(LevelSequence.Levels[level]);
        }

        private void Update()
        {
            switch (GameState)
            {
                case GameState.Playing:
                    UpdatePlayingState();
                    break;
                case GameState.EditorPlaying:
                    UpdateEditorPlayingState();
                    break;
            }
        }

        private void UpdateEditorPlayingState()
        {
            
        }

        private void UpdatePlayingState()
        {

        }
    }
}
