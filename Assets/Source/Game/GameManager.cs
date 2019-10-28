using Laser.Game.Level;
using Laser.Game.UI;
using System;
using UnityEngine;

namespace Laser.Game
{
    public enum GameState
    {
        Menu,
        Playing,
        EditorPlaying,
        Won
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
                var prev = gameState;
                gameState = value;
                L.Debug($"Game state is set to {gameState}.");
                InitState(prev);
            }
        }

        public LevelSequence LevelSequence;
        public LevelController LevelController;
        public UIManager UIManager;

        private GameState gameState;

        private void Awake()
        {
            if (EditorMode)
            {
                GameState = GameState.EditorPlaying;
            }
            else
            {
                OpenMainMenuScreen();
            }
        }

        private void Update()
        {
            UpdateState();
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
            GameState = GameState.Playing;
        }

        public void OpenMainMenuScreen()
        {
            if (GameState == GameState.EditorPlaying)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }

            if (LevelController.IsLevelLoaded)
            {
                LevelController.Unload();
            }

            UIManager.OpenScreen(ScreenType.MainMenu);
            GameState = GameState.Menu;
        }

        private void InitState(GameState previous)
        { }

        private void UpdateState()
        {
            switch (GameState)
            {
                case GameState.Playing:
                    UpdatePlayingState();
                    break;
            }
        }

        private void UpdatePlayingState()
        {
            if (LevelController.State == LevelState.Won)
            {
                HandleWin();
            }
        }

        private void HandleWin()
        {
            App.Profile.Progression.CurrentLevel++;
            App.SaveProfile();
            GameState = GameState.Won;
            UIManager.OpenScreen(ScreenType.Win);
        }
    }
}
