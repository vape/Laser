using Laser.Game.Level;
using Laser.Game.Main;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(LevelController))]
    public class LevelEditor : UnityEditor.Editor
    {
        private static string GetLevelsDirectory()
        {
            return Path.Combine(Application.dataPath, "Resources", "Levels");
        }

        private static void Write(string name, LevelData data)
        {
            var dir = GetLevelsDirectory();
            var path = Path.Combine(dir, name);

            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            if (File.Exists(path)) { File.Delete(path); }

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                JsonSerializer.Create(settings).Serialize(jsonWriter, data);
            }
        }

        private static LevelData Read(string name)
        {
            var dir = GetLevelsDirectory();
            var path = Path.Combine(dir, name);

            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                return JsonSerializer.Create(settings).Deserialize<LevelData>(jsonReader);
            }
        }

        private string currentName;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Base", EditorStyles.boldLabel);

            base.OnInspectorGUI();

            GUILayout.Label("Editor", EditorStyles.boldLabel);

            currentName = GUILayout.TextField(currentName);

            if (GUILayout.Button("Save")) { SaveLevel(currentName); }
            if (GUILayout.Button("Load")) { LoadLevel(currentName); }
            if (GUILayout.Button("Unload")) { LoadLevel(currentName); }
        }

        private void UnloadLevel()
        {
            var controller = ((LevelController)target);
            controller.Unload();

            L.Info($"Successfully unloaded level!");
        }

        private void LoadLevel(string name)
        {
            var controller = ((LevelController)target);
            var data = Read($"{name}.json");
            controller.Load(data);

            L.Info($"Successfully loaded level {name}!");
        }

        private void SaveLevel(string name)
        {
            var controller = ((LevelController)target);
            var data = controller.Save();

            Validate(data);
            Write($"{name}.json", data);

            L.Info($"Successfully saved level \"{name}\"!");
        }

        private bool Validate(LevelData data)
        {
            if (!data.Entities.Any((e) => e.Type == EntityType.Emitter))
            {
                throw new Exception("Level has no emitter.");
            }

            if (!data.Entities.Any((e) => e.Type == EntityType.Absorber))
            {
                throw new Exception("Level has no absorber.");
            }

            return true;
        }
    }
}
