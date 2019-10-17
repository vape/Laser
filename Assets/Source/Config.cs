using Newtonsoft.Json;
using UnityEngine;

namespace Laser
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Config
    {
        public static Config Instnace
        {
            get
            {
                lock (loadingLock)
                {
                    if (instance == null)
                    {
                        instance = new Config();
                        instance.Load();
                    }

                    return instance;
                }
            }
        }

        private static Config instance;
        private static object loadingLock = new object();

        public static void Reload()
        {
            var config = new Config();
            config.Load();

            lock (loadingLock)
            {
                instance = config;
            }
        }

        private Config()
        { }

        private void Load()
        {
            var deserializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include
            };

            var configAsset = Resources.Load<TextAsset>("Config/config");
            if (configAsset?.text != null)
            {
                JsonConvert.PopulateObject(configAsset.text, this, deserializerSettings);
            }
        }
    }
}
