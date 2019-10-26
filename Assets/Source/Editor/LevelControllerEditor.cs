using Laser.Editor.Utilities;
using Laser.Game.Level;
using Laser.Game.Main.Grid;
using UnityEditor;
using UnityEngine;

namespace Laser.Editor
{
    [CustomEditor(typeof(LevelController))]
    public class LevelControllerEditor : UnityEditor.Editor
    {
        private GridElementController entityToSpawnGridElementContainer;
        private int placeControlId;

        private void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

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

        private void OnSceneGUI()
        {
            RenderEntityPreview();
        }

        private void RenderEntityPreview()
        {
            var level = ((LevelController)target);
            placeControlId = EditorGUIUtility.GetControlID(FocusType.Keyboard);

            if (level.EntityToSpawn != null && entityToSpawnGridElementContainer == null)
            {
                var grid = level.GetComponent<GridController>();
                var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                var tile = grid.RaycastTile(ray);

                if (tile == null)
                {
                    tile = new GridTile(0, 0);
                }

                entityToSpawnGridElementContainer = level.CreateGridElement(tile.Value);
                entityToSpawnGridElementContainer.tag = "EditorOnly";
                entityToSpawnGridElementContainer.name = $"Temporal_{entityToSpawnGridElementContainer.name}";

                level.InstantiateEntity(entityToSpawnGridElementContainer, level.EntityToSpawn);
                EditorUtilities.ApplyMaterialRecursively(entityToSpawnGridElementContainer.gameObject, (Material)EditorGUIUtility.Load("Materials/temp_grid_item.mat"));

                GUIUtility.hotControl = placeControlId;
            }

            if (entityToSpawnGridElementContainer != null && GUIUtility.hotControl == placeControlId)
            {
                var grid = level.GetComponent<GridController>();
                var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                var tile = grid.RaycastTile(ray);
                var isSuitableTile = tile == null ? false : grid.CanPlaceIn(entityToSpawnGridElementContainer, tile.Value);

                if (Event.current.type == EventType.MouseMove)
                {
                    if (tile != null && isSuitableTile)
                    {
                        entityToSpawnGridElementContainer.Tile = tile.Value;
                        level.EntityToSpawn.Tile = tile.Value;
                        grid.Layout();
                    }

                    Event.current.Use();
                }
                else if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
                {
                    if (tile != null)
                    {
                        var rectColor = isSuitableTile ? new Color(0, 1, 0, 0.1f) : new Color(1, 0, 0, 0.3f);
                        var outlineColor = isSuitableTile ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.7f);
                        var rect = grid.GetWorldSpacedTileRect(tile.Value);
                        Handles.DrawSolidRectangleWithOutline(rect, rectColor, outlineColor);
                    }
                }
                else if (Event.current.type == EventType.KeyDown)
                {
                    if (Event.current.keyCode == KeyCode.Delete || Event.current.keyCode == KeyCode.Escape)
                    {
                        ResetEntityToSpawn();
                        Event.current.Use();
                    }
                }
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && level.EntityToSpawn != null)
            {
                level.SpawnEntity(level.EntityToSpawn);
                ResetEntityToSpawn();
                Event.current.Use();
            }
        }

        private void SelectionChanged()
        {
            if (target == null)
            {
                return;
            }

            var _target = target as LevelController;
            if (Selection.activeGameObject != _target.gameObject && _target.EntityToSpawn != null)
            {
                ResetEntityToSpawn();
            }
        }

        private void ResetEntityToSpawn()
        {
            if (placeControlId == GUIUtility.hotControl)
            {
                GUIUtility.hotControl = 0;
            }

            var levelController = ((LevelController)target);
            levelController.EntityToSpawn = null;

            if (entityToSpawnGridElementContainer != null)
            {
                DestroyImmediate(entityToSpawnGridElementContainer.gameObject);
                entityToSpawnGridElementContainer = null;
            }
        }
    }
}
