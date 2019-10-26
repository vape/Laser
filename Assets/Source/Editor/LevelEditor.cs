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
    public class LevelControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open Editor"))
            {
                LevelEditorWindow.Open();
            }

            var _target = ((LevelController)target);
            GUILayout.Label(_target.IsLevelLoaded ? "Level is loaded" : "Level isn't loaded");
        }
    }
}
