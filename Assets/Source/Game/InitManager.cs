using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Laser.Game
{
    public class InitManager : MonoBehaviour
    {
        public bool IsLoaded
        {
            get
            {
                return GameScene.isLoaded;
            }
        }

        [NonSerialized]
        public Scene GameScene;

        private void Awake()
        {
            GameScene = SceneManager.GetSceneByName("Game");
        }
    }
}
