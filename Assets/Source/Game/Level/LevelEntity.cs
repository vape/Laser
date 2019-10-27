using Laser.Game.Main;
using Laser.Game.Main.Grid;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Laser.Game.Level
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LevelEntity
    {
        [JsonProperty("tile")]
        public GridTile Tile;

        [DefaultValue(EntityOrientation.N)]
        [JsonProperty("orient", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public EntityOrientation Orientation;

        [JsonProperty("tiled")]
        public bool IsTiled;

        [JsonProperty("type")]
        public EntityType Type;

        [DefaultValue(ReflectorType.None)]
        [JsonProperty("r_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReflectorType ReflectorType;

        [DefaultValue(EmitterType.None)]
        [JsonProperty("e_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public EmitterType EmitterType;

        [DefaultValue(AbsorberType.None)]
        [JsonProperty("a_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AbsorberType AbsorberType;
    }
}
