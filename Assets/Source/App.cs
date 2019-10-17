using System.IO;
using UnityEngine;

namespace Laser
{
    public static partial class App
    {
        public static Environment Environment
        {
            get
            {
                return Environment.Debug;
            }
        }

        public static string GetAppDirectory()
        {
            return Application.persistentDataPath;
        }

        public static string GetProfilesDirectory()
        {
            return
                Path.Combine(
                    GetAppDirectory(),
                    "Profiles"
                );
        }
    }
}