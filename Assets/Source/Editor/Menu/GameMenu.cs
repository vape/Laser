using System.IO;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor.Menu
{
    public static class GameMenu
    {
        [MenuItem("Game/Profile/Reset")]
        public static void ResetProfile()
        {
            Directory.Delete(App.GetProfilesDirectory(), true);
        }

        [MenuItem("Game/Profile/Reset", validate = true)]
        public static bool ResetProfileValidate()
        {
            return !Application.isPlaying;
        }
    }
}
