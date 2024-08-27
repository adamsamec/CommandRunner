using System.Diagnostics;

namespace CommandRunner
{
    /// <summary>
    /// Main application logic
    /// </summary>
    public class Runner
    {
        private Config _config = new Config();
        private MainWindow _mainWindow;
        private Process _runningProcess;
        private bool _isRunning = false;

        public Settings AppSettings
        {
            get { return _config.AppSettings; }
        }
        public History AppHistory
        {
            get { return _config.AppHistory; }
        }
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public enum HistoryType
        {
            Commands,
            WorkingDirs,
            FindTexts
        }

        public Runner(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void RunCommand(string command, string workingDir)
        {

            // Ignore when already running or whitespace only command
            if (IsRunning || command.Trim() == "")
            {
                return;
            }
            _runningProcess = new Process();
            var startInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/C " + command + " 2>&1",
                WorkingDirectory = workingDir,
                Verb = "runas",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true
            };
            _runningProcess.StartInfo = startInfo;
            _runningProcess.Start();
            _isRunning = true;
            AddToHistory(command, AppHistory.commands, HistoryType.Commands);
            AddToHistory(workingDir, AppHistory.workingDirs, HistoryType.WorkingDirs);
            _mainWindow.Title = command + Consts.WindowTitleSeparator + Consts.AppName;

            // Run the command in a new thread
            var thread = new Thread(() =>
            {
                while (_runningProcess.StandardOutput.Peek() >= 0)
                {
                    var line = _runningProcess.StandardOutput.ReadLine() + "\r\n";
                    _mainWindow.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        _mainWindow.AppendToOutput(line);
                    }));
                }
                _isRunning = false;
                _mainWindow.Dispatcher.BeginInvoke((Action)(() =>
                {
                    _mainWindow.AllowRunning();
                    ResetMainWindowTitle();
                }));
            });
            thread.Start();
        }

        public void KillRunningProcess()
        {
            if (!IsRunning)
            {
                return;
            }
            _runningProcess.Kill(true);
            _isRunning = false;
            ResetMainWindowTitle();
        }

        private void ResetMainWindowTitle()
        {
            _mainWindow.Title = Consts.AppName;
        }

        private void AddToHistory(string item, string[] history, HistoryType type)
        {
            //  Ignore whitespace only items
            if (item.Trim() == "")
            {
                return;
            }
            var newHistoryList = ConvertHistoryToList(history);

            // Remove the item if already in history, then add item
            newHistoryList = newHistoryList.Where(existingItem => item != existingItem).ToList();
        newHistoryList.Insert(0, item);

            // Limit the number of stored items
            if (newHistoryList.Count() >= Consts.HistorySize + 1)
            {
                newHistoryList.RemoveAt(Consts.HistorySize);
            }

            // Update the new history
            var newHistory = newHistoryList.ToArray();
            switch (type)
            {
                case HistoryType.Commands:
                AppHistory.commands = newHistory;
                    break;
                case HistoryType.WorkingDirs:
                AppHistory.workingDirs = newHistory;
                    break;
            }
            _mainWindow.UpdateHistory(newHistoryList, type);

            SaveSettings();
        }

        public List<string> ConvertHistoryToList(string[] history)
        {
            var historyList = new List<string>(history);
            return historyList;
        }

        public void ChangeCheckForUpdateOnLaunchSetting(bool value)
        {
            AppSettings.checkForUpdateOnLaunch = Config.BoolToString(value);
            SaveSettings();
        }

        public void ChangePlaySuccessSoundSetting(bool value)
        {
            AppSettings.playSuccessSound = Config.BoolToString(value);
            SaveSettings();
        }

        public void ChangePlayErrorSoundSetting(bool value)
        {
            AppSettings.playErrorSound = Config.BoolToString(value);
            SaveSettings();
        }

        private void SaveSettings()
        {
            _config.Save();
        }
    }
}

