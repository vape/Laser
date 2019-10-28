using Newtonsoft.Json;

namespace Laser.Core
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Progression
    {
        [JsonProperty]
        public int CurrentLevel;
    }
}
