using Laser.Game.Level;
using Laser.Game.Main;
using Laser.Game.Main.Grid;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        public static void Open()
        {
            GetWindow<LevelEditorWindow>(false, "Level Editor");
        }

        public LevelController LevelController
        {
            get
            {
                if (levelController == null)
                {
                    levelController = FindObjectOfType<LevelController>();
                }

                return levelController;
            }
        }

        private LevelController levelController;

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

        private string LastLoadedLevel
        {
            get
            {
                return SessionState.GetString("last_loaded_level", null);
            }
            set
            {
                SessionState.SetString("last_loaded_level", value);
            }
        }

        private string levelName;

        private void Awake()
        {
            levelName = LastLoadedLevel;

            if (!String.IsNullOrWhiteSpace(levelName) && !LevelController.IsLevelLoaded)
            {
                LoadLevel(levelName);
            }
        }

        private void OnGUI()
        {
            RenderMenu("Level", RenderLevelManagmentMenu);
        }

        private void RenderMenu(string name, Action render)
        {
            GUILayout.Label(name, EditorStyles.boldLabel);
            render?.Invoke();
        }

        private void RenderLevelManagmentMenu()
        {
            levelName = GUILayout.TextField(levelName);

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = LevelController.IsLevelLoaded;
            if (GUILayout.Button("Save"))
            {
                SaveLevel(levelName);
            }
            GUI.enabled = true;

            GUI.enabled = !String.IsNullOrWhiteSpace(levelName);
            if (GUILayout.Button("Load"))
            {
                LoadLevel(levelName);
                LastLoadedLevel = levelName;
            }
            GUI.enabled = true;

            GUI.enabled = LevelController.IsLevelLoaded;
            if (GUILayout.Button("Unload"))
            {
                UnloadLevel();
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void UnloadLevel()
        {
            LevelController.Unload();

            L.Info($"Successfully unloaded level!");
        }

        private void LoadLevel(string name)
        {
            var data = Read($"{name}.json");
            LevelController.Load(data);

            L.Info($"Successfully loaded level {name}!");
        }

        private void SaveLevel(string name)
        {
            var data = LevelController.Save();

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
