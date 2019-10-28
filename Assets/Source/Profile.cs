using Laser.Core;
using Newtonsoft.Json;
using System;

namespace Laser
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Profile
    {
        public static Profile Create()
        {
            return new Profile(true);
        }

        [JsonProperty]
        public DateTime CreationTime
        { get; private set; } = DateTime.UtcNow;

        [JsonProperty]
        public DateTime SaveTime
        { get; set; }

        [JsonProperty]
        public Progression Progression
        { get; set; }

        public Profile()
            : this(false)
        { }

        private Profile(bool initAsNew)
        {
            if (initAsNew)
            {
                Progression = new Progression();
            }
        }
    }
}
