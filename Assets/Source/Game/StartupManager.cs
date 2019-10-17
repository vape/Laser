using System.IO;
using UnityEngine;

namespace Laser.Game
{
    public class StartupManager : MonoBehaviour
    {
        private const string ProfileName = "profile";

        private static void Initialize()
        {
            Application.targetFrameRate = 60;

            if (App.Profile == null)
            {
                try
                {
                    App.LoadProfile(ProfileName);
                }
                catch (FileNotFoundException)
                {
                    var profile = Profile.Create();
                    App.LoadProfile(profile, ProfileName);
                    App.SaveProfile();
                }
            }

            if (App.Settings == null)
            {
                App.LoadSettings();
            }

            Config.Reload();
        }

        private void Awake()
        {
            Initialize();
        }
    }
}
