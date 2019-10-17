using Newtonsoft.Json;
using System.IO;

namespace Laser
{
    public static partial class App
    {
        public static Settings Settings
        { get; private set; } = new Settings();

        public static void ResetSettings()
        {
            Settings = new Settings();
        }

        public static void SaveSettings()
        {
            var path = Path.Combine(GetAppDirectory(), "settings");

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                JsonSerializer.Create(settings).Serialize(jsonWriter, Settings);
            }
        }

        public static void LoadSettings()
        {
            var path = Path.Combine(GetAppDirectory(), "settings");
            if (!File.Exists(path))
            {
                L.Debug("Settings file not found, falling back to default.");
                ResetSettings();
                return;
            }

            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                Settings = JsonSerializer.Create(settings).Deserialize<Settings>(jsonReader);
            }
        }
    }
}
