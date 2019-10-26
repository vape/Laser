using Laser.Game.Main;
using Laser.Game.Main.Grid;
using Laser.Shared;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Laser.Game.Level
{
    public partial class LevelController : MonoBehaviour
    {
        private GridElementController CreateGridElement(GridTile tile)
        {
            var elem = new GameObject($"GridElement", typeof(GridElementController)).GetComponent<GridElementController>();
            elem.Tile = tile;
            elem.transform.SetParent(Grid.transform, false);
            return elem;
        }

        private EmitterController SpawnEmitter(GameObject container, EmitterType type)
        {
            var prefab = ItemsMap.Emitters.FirstOrDefault((p) => p.Type == type).Prefab;
            var emitter = Instantiate(prefab, container.transform, false);
            return emitter;
        }

        private AbsorberController SpawnAbsorber(GameObject container, AbsorberType type)
        {
            var prefab = ItemsMap.Absorbers.FirstOrDefault((p) => p.Type == type).Prefab;
            var absorber = Instantiate(prefab, container.transform, false);
            return absorber;
        }

        private ReflectorController SpawnReflector(GameObject container, ReflectorType type)
        {
            var prefab = ItemsMap.Reflectors.FirstOrDefault((p) => p.Type == type).Prefab;
            var reflector = Instantiate(prefab, container.transform, false);
            return reflector;
        }

        public void Load(LevelData data)
        {
            Unload();

            for (int i = 0; i < data.Entities.Count; ++i)
            {
                var entity = data.Entities[i];
                if (!entity.IsTiled)
                {
                    throw new NotImplementedException();
                }

                var gridElem = CreateGridElement(entity.Tile);
                gridElem.name = $"GridElement_{entity.Type}_{i}";

                switch (entity.Type)
                {
                    case EntityType.Emitter:
                        SpawnEmitter(gridElem.gameObject, entity.EmitterType);
                        break;
                    case EntityType.Absorber:
                        SpawnAbsorber(gridElem.gameObject, entity.AbsorberType);
                        break;
                    case EntityType.Reflector:
                        SpawnReflector(gridElem.gameObject, entity.ReflectorType);
                        break;
                    default:
                        throw new Exception($"Unknown entity type {entity.Type}.");
                }
            }

            levelLoaded = true;
            Grid.IsDirty = true;
        }

        public void Load(string name)
        {
            var text = Resources.Load<TextAsset>($"Levels/{name}");
            var data = JsonConvert.DeserializeObject<LevelData>(text.text);
            
            Load(data);
        }

        public void Unload()
        {
            Grid.transform.DestroyChildrens(true);
            levelLoaded = false;
        }
    }
}
