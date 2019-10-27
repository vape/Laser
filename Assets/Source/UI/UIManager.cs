using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Laser.UI
{
    public enum ScreenType
    {
        None,
        MainMenu,
        Win
    }

    [Serializable]
    public class ScreenTypePair
    {
        [SerializeField]
        public ScreenType Type;
        [SerializeField]
        public ScreenController Prefab;
    }

    public class UIManager : MonoBehaviour
    {
        public ScreenType CurrentScreen
        { get; private set; }

        [SerializeField]
        public List<ScreenTypePair> ScreensPrefabs;
        [SerializeField]
        public GameObject ScreenContainer;

        private ScreenController currentScreenController;

        public ScreenController CurrentScreenController
        {
            get
            {
                return currentScreenController;
            }
        }

        public void OpenScreen(ScreenType type, params object[] param)
        {
            if (CurrentScreen != ScreenType.None)
            {
                currentScreenController.Close();
            }

            var screen = ScreensPrefabs.FirstOrDefault((p) => p.Type == type);
            if (screen == null)
            {
                throw new Exception("Unknown screen type.");
            }

            currentScreenController = Instantiate(screen.Prefab, ScreenContainer.transform);
            currentScreenController.Closing += OnScreenClosing;
            currentScreenController.Init(param);

            CurrentScreen = type;
        }

        private void OnScreenClosing()
        {
            CurrentScreen = ScreenType.None;
        }
    }
}
