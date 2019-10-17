using Newtonsoft.Json;
using System;
using System.IO;

namespace Laser
{
    public static partial class App
    {
        public static event Action ProfileLoaded;
        public static event Action ProfileUnloaded;

        public static Profile Profile
        { get; private set; }

        private static string currentProfileName;

        private static object opLock = new object();

        public static void UnloadProfile()
        {
            currentProfileName = null;
            Profile = null;
            ProfileUnloaded?.Invoke();
        }

        public static void LoadProfile(Profile profile, string name)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"Invalid profile name: {name}", nameof(name));
            }

            lock (opLock)
            {
                Profile = profile;
                currentProfileName = name;
            }

            ProfileLoaded?.Invoke();
        }

        public static void LoadProfile(string name)
        {
            var path = Path.Combine(GetProfilesDirectory(), name);
            var backupPath = $"{path}.backup";
            if (!File.Exists(path) && !File.Exists(backupPath))
            {
                throw new FileNotFoundException($"Couldn't find profile at {path}.", path);
            }

            Profile loadedProfile = null;
            try
            {
                loadedProfile = DeserializeProfile(path);
            }
            catch (Exception e0)
            {
                if (File.Exists(backupPath))
                {
                    L.Warn($"Trying to load backup after failed to load profile {name}");

                    try
                    {
                        if (File.Exists(backupPath))
                        {
                            loadedProfile = DeserializeProfile(backupPath);
                        }
                    }
                    catch (Exception e1)
                    {
                        throw new GameException(3, e1, "Failed to deserialize profile and its backup.");
                    }
                }
                else
                {
                    throw new GameException(2, e0, "Failed to deserialize profile.");
                }
            }


            lock (opLock)
            {
                Profile = loadedProfile;
                currentProfileName = name;
            }

            ProfileLoaded?.Invoke();
        }

        public static void SaveProfile()
        {
            lock (opLock)
            {
                if (Profile == null)
                {
                    throw new InvalidOperationException("Profile not set.");
                }

                if (currentProfileName == null)
                {
                    throw new InvalidOperationException("Profile name not set.");
                }

                Profile.SaveTime = GlobalTime.Current;

                var directory = GetProfilesDirectory();
                var path = Path.Combine(directory, currentProfileName);
                var newFilePath = $"{path}.new";
                var backupPath = $"{path}.backup";

                Directory.CreateDirectory(directory);
                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                try
                {
                    using (var stream = new FileStream(newFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (var writer = new StreamWriter(stream))
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        var settings = new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        };

                        JsonSerializer.Create(settings).Serialize(jsonWriter, Profile);
                    }
                }
                catch (Exception e)
                {
                    throw new GameException(0, e, "Failed to serialize profile.");
                }

                // TODO: Check if everything is alright with just saved profile

                if (File.Exists(path))
                {
                    try
                    {
                        if (File.Exists(backupPath))
                        {
                            File.Delete(backupPath);
                        }

                        File.Move(path, backupPath);
                    }
                    catch (Exception e)
                    {
                        throw new GameException(1, e, "Failed to create profile backup.");
                    }
                }

                File.Move(newFilePath, path);
            }
        }

        private static Profile DeserializeProfile(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                return JsonSerializer.Create(settings).Deserialize<Profile>(jsonReader);
            }
        }
    }
}
