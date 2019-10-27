using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laser.Game
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Progression
    {
        [JsonProperty]
        public int CurrentLevel;
    }
}
