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
            AddToCommandHistory(command);
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

        private void AddToCommandHistory(string command)
        {
            var history = ConvertHistoryToList(AppHistory.commands);

            // Remove the item if already in history, then add item
            history = history.Where(item => item != command).ToList();
            history.Insert(0, command);

            // Limit the number of stored items
            if (history.Count() >= Consts.HistorySize + 1)
            {
                history.RemoveAt(Consts.HistorySize);
            }

            // Update the new history
                AppHistory.commands = history.ToArray();
        _mainWindow.UpdateCommandsHistory(history);

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

