#if UNITY_EDITOR

using Laser.Game.Main;
using Laser.Game.Main.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Laser.Game.Level
{
    public partial class LevelController : MonoBehaviour
    {
        public LevelData Save()
        {
            var entities = new List<LevelEntity>();
            for (int i = 0; i < Grid.transform.childCount; ++i)
            {
                var child = Grid.transform.GetChild(i);
                if (child.TryGetComponent<GridElementController>(out var gridElement))
                {
                    if (gridElement.transform.childCount == 0)
                    {
                        continue;
                    }

                    if (gridElement.tag == "EditorOnly")
                    {
                        continue;
                    }

                    var mainEntity = gridElement.transform.GetChild(0);
                    if (mainEntity.gameObject.TryGetComponent<LevelEntityController>(out var entity))
                    {
                        var serializableEntity = new LevelEntity();
                        serializableEntity.IsTiled = true;
                        serializableEntity.Type = entity.Type;
                        serializableEntity.Tile = gridElement.Tile;

                        if (entity.Type == EntityType.Reflector)
                        {
                            if (entity.TryGetComponent<ReflectorController>(out var reflector))
                            {
                                serializableEntity.ReflectorType = reflector.Type;
                            }
                            else
                            {
                                throw new Exception("Entity has a reflector type, but reflector controller isn't attached to it.");
                            }
                        }

                        if (entity.Type == EntityType.Absorber)
                        {
                            if (entity.TryGetComponent<AbsorberController>(out var absorber))
                            {
                                serializableEntity.AbsorberType = absorber.Type;
                            }
                            else
                            {
                                throw new Exception("Entity has a absorber type, but absorber controller isn't attached to it.");
                            }
                        }

                        if (entity.Type == EntityType.Emitter)
                        {
                            if (entity.TryGetComponent<EmitterController>(out var emitter))
                            {
                                serializableEntity.EmitterType = emitter.Type;
                            }
                            else
                            {
                                throw new Exception("Entity has a emitter type, but emitter controller isn't attached to it.");
                            }
                        }

                        entities.Add(serializableEntity);
                    }
                }
            }

            return new LevelData()
            {
                Entities = entities
            };
        }
    }
}

#endif