namespace CommandRunner
{
    public class ConfigJson
    {
        public History history { get; set; }
        public Settings settings { get; set; }
    }

    public class History
    {
        public object[] commands { get; set; }
        public object[] workingDirs { get; set; }
        public object[] findTexts { get; set; }
    }

    public class Settings
    {
        public string checkForUpdateOnLaunch { get; set; }
        public string outputEnabled { get; set; }
        public string findBackward { get; set; }
        public string ignoreCase { get; set; }
        public string playSuccessSound { get; set; }
        public string successRegex { get; set; }
        public string playErrorSound { get; set; }
        public string errorRegex { get; set; }
    }
}
