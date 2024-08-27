using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace CommandRunner
{
    /// <summary>
    /// Application configuration
    /// </summary>
    public class Config
    {
        private string _path;
        private ConfigJson _config;

        public Settings AppSettings
        {
            get { return _config.settings; }
        }
        public History AppHistory
        {
            get { return _config.history; }
        }

        private const string TrueString = "yes";
        private const string FalseString = "no";

        public Config()
        {
            Directory.CreateDirectory(Consts.localUserFolder);
            var defaultPath = Path.Combine(Consts.InstallFolder, Consts.ConfigDefaultFilename);
            _path = Path.Combine(Consts.localUserFolder, Consts.ConfigFilename);

            // Create the config if it not yet exists
            if (!File.Exists(_path))
            {
                File.Copy(defaultPath, _path);
            }

            // Load the config
            var configString = File.ReadAllText(_path, Encoding.UTF8);
            var config = JsonSerializer.Deserialize<ConfigJson>(configString);
            if (config == null)
            {
                throw new SerializationException("Unable to deserialize config file");
            }
            _config = config as ConfigJson;
            var settings = _config.settings;
            var history = _config.history;

            var defaultConfigString = File.ReadAllText(defaultPath, Encoding.UTF8);
            var defaultConfig = JsonSerializer.Deserialize<ConfigJson>(defaultConfigString);
            if (defaultConfig == null)
            {
                throw new SerializationException("Unable to deserialize default config file");
            }
            var defaultSettings = defaultConfig.settings;
            var defaultHistory = defaultConfig.history;

            // Set missing JSON properties to defaults
            Utils.SetYesOrNo(settings, defaultSettings, ["checkForUpdateOnLaunch", "ignoreCase", "playSuccessSound", "playErrorSound"]);
            Utils.SetStringNotNull(settings, defaultSettings, ["successRegex", "errorRegex"]);
            Utils.SetStringArrayNotNull(history, defaultHistory, ["commands", "workingDirs", "findTexts"]);
            Save();
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var configString = JsonSerializer.Serialize(_config, options);
            File.WriteAllText(_path, configString, Encoding.UTF8);
        }

        public static bool StringToBool(string? value)
        {
            return value == TrueString;
        }

        public static string BoolToString(bool value)
        {
            return value ? TrueString : FalseString;
        }
    }

    [Serializable]
    public class InstallPathUnknownException : Exception
    {
        public InstallPathUnknownException() { }

        public InstallPathUnknownException(string message)
            : base(message) { }

        public InstallPathUnknownException(string message, Exception inner)
            : base(message, inner) { }
    }
}

