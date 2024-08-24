using System.IO;

namespace CommandRunner
{
    /// <summary>
    /// Constants class
    /// </summary>
    public static class Consts
    {
        public const string AppVersion = "1.0.0";
        public const string AppName = "CommandRunner";
        public const string WindowTitleSeparator = " | ";
        // URLs
        public const string UpdateApiUrl = "http://api.adamsamec.cz/CommandRunner/Update.json";
        public const string ChangeLogUrl = "https://raw.githubusercontent.com/adamsamec/CommandRunner/main/ChangeLog/ChangeLog.{0}.md";

        // Paths and filenames
        public const string PagesFolder = "Pages";
        public static string HelpFileRelativePath = Path.Combine(PagesFolder, "Help.{0}.md");
        public static string localUserFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CommandRunner");
        public static string InstallerDownloadFolder = Path.Combine(localUserFolder, "installer");
        public const string ConfigDefaultFilename = "App.config.default.json";
        public const string ConfigFilename = "App.config.json";
        public static string InstallFolder
        {
            get
            {
                if (_installFolder == null)
                {
                    var assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    _installFolder = System.IO.Path.GetDirectoryName(assemblyPath);
                }
                if (_installFolder == null)
                {
                    throw new InstallPathUnknownException("Unable to determine CommandRunner install path");
                }
                return _installFolder;
            }
        }

        // Private fields
        private static string? _installFolder;
    }
}
