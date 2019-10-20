using Newtonsoft.Json;
using System.Collections.Generic;

namespace Laser.Game.Level
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LevelData
    {
        [JsonProperty("entities")]
        public List<LevelEntity> Entities = new List<LevelEntity>();
    }
}
