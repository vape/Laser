using UnityEditor;
using UnityEngine;

namespace Laser.Editor.Utilities
{
    public class AddScriptProcessor : UnityEditor.AssetModificationProcessor
    {
        public const string DefaultGameNamespace = "Laser";
        public const string DefaultEditorNamespace = "Laser.Editor";

        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");

            var index = path.LastIndexOf(".");
            if (index < 0)
            {
                return;
            }

            var extension = path.Substring(index);
            if (extension != ".cs")
            {
                return;
            }

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;

            var content = System.IO.File.ReadAllText(path);
            var lastPart = path.Substring(path.IndexOf("Assets"));
            var namespaceString = lastPart.Substring(0, lastPart.LastIndexOf('/'));
            namespaceString = namespaceString.Replace("Assets/Source/Editor", DefaultEditorNamespace).Replace("Assets/Source", DefaultGameNamespace).Replace('/', '.');
            content = content.Replace("#NAMESPACE#", namespaceString);
            System.IO.File.WriteAllText(path, content);

            AssetDatabase.Refresh();
        }
    }
}
