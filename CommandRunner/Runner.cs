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

        public void RunCommand(string command)
        {
            if (IsRunning)
            {
                return;
            }
            _runningProcess = new Process();
            var startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                Verb = "runas",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            _runningProcess.StartInfo = startInfo;
            _runningProcess.Start();
            _isRunning = true;
            AddToHistory(command, AppHistory.commands, HistoryType.Commands);
            _mainWindow.Title = command + Consts.WindowTitleSeparator + Consts.AppName;

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
            var historyList = ConvertHistoryToList(history);

            // Remove the item if already in history, then add item
            historyList = historyList.Where(existingItem => item != existingItem).ToList();
            historyList.Insert(0, item);

            // Limit the number of stored items
            if (historyList.Count() >= Consts.HistorySize + 1)
            {
                historyList.RemoveAt(Consts.HistorySize);
            }

            // Update the new history
            var newHistory = historyList.ToArray();
            switch (type)
            {
                case HistoryType.Commands:
                AppHistory.commands = newHistory;
                    break;
            }
        _mainWindow.UpdateHistory(historyList, type);

            SaveSettings();
        }

        public List<string> ConvertHistoryToList(string[] history)
        {
            var historyList = new List<string>(history);
            return historyList;
        }
        private void SaveSettings()
        {
            _config.Save();
        }
    }
}

