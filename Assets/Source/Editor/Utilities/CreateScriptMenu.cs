using System.IO;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor.Utilities
{
    public static class CreateMenu
    {
        [MenuItem("Assets/Create/Script/C# Editor Script", priority = 80)]
        public static void CreateEditorScript()
        {
            CreateTextAsset("editor_script_template", "EditorScript.cs");
        }

        [MenuItem("Assets/Create/Script/C# Game Script", priority = 81)]
        public static void CreateGameScript()
        {
            CreateTextAsset("game_script_template", "GameScript.cs");
        }

        [MenuItem("Assets/Create/Script/C# Game Script (serializable)", priority = 81)]
        public static void CreateSerializableGameScript()
        {
            CreateTextAsset("serializable_game_script_template", "GameScript.cs");
        }

        [MenuItem("Assets/Create/Script/C# Behaviour Script", priority = 81)]
        public static void CreateBehaviourScript()
        {
            CreateTextAsset("behaviour_script_template", "BehaviourScript.cs");
        }

        public static void CreateTextAsset(string templateName, string defaultName)
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(GetTemplatePath(templateName), defaultName);
            AssetDatabase.Refresh();
        }

        private static string GetTemplatePath(string templateName)
        {
            return Path.Combine(Application.dataPath, "Editor Default Resources", "Script Templates", templateName + ".txt");
        }
    }
}
