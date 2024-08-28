using System.Diagnostics;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace CommandRunner
{
    /// <summary>
    /// Main application logic
    /// </summary>
    public class Runner
    {
        private Config _config = new Config();
        private MainWindow _mainWindow;
        private Process? _runningProcess;
        private bool _isRunning = false;
        private SoundPlayer? _successSoundPlayer;
        private SoundPlayer? _errorSoundPlayer;
        private string _findWhatText = "";

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
        public FindTextDialog? AppFindTextDialog;

        public enum HistoryType
        {
            Commands,
            WorkingDirs,
            FindTexts
        }
        private enum RegexMatchSound
        {
            Success,
            Error
        }

        public Runner(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            InitSoundPlayers();
        }

        private void InitSoundPlayers()
        {
            _successSoundPlayer = new SoundPlayer(Consts.SuccessSoundFilePath);
            _errorSoundPlayer = new SoundPlayer(Consts.ErrorSoundFilePath);
            _successSoundPlayer.Load();
            _errorSoundPlayer.Load();
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
            AddToHistoryConfig(command, HistoryType.Commands);
            AddToHistoryConfig(workingDir, HistoryType.WorkingDirs);
            _mainWindow.Title = command + Consts.WindowTitleSeparator + Consts.AppName;

            // Run the command in a new thread
            var thread = new Thread(() =>
            {
                while (_runningProcess.StandardOutput.Peek() >= 0)
                {
                    var line = _runningProcess.StandardOutput.ReadLine() + "\r\n";

                    // Match line to regexes and possibly play sound
                    MatchLineAndPlaySound(line, RegexMatchSound.Success);
                    MatchLineAndPlaySound(line, RegexMatchSound.Error);

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

        private void AddToHistoryConfig(string item, HistoryType type)
        {
            //  Ignore whitespace only items
            if (item.Trim() == "")
            {
                return;
            }
            string[]? history = null;
            switch (type)
            {
                case HistoryType.Commands:
                    history = AppHistory.commands;
                    break;
                case HistoryType.WorkingDirs:
                    history = AppHistory.workingDirs;
                    break;
                case HistoryType.FindTexts:
                    history = AppHistory.findTexts;
                    break;
                default:
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
                case HistoryType.FindTexts:
                    AppHistory.findTexts = newHistory;
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

        private void MatchLineAndPlaySound(string line, RegexMatchSound sound)
        {
            string? setting = null;
            string? pattern = null;
            SoundPlayer? player = null;
            switch (sound)
            {
                case RegexMatchSound.Success:
                    setting = AppSettings.playSuccessSound;
                    pattern = AppSettings.successRegex;
                    player = _successSoundPlayer;
                    break;
                case RegexMatchSound.Error:
                    setting = AppSettings.playErrorSound;
                    pattern = AppSettings.errorRegex;
                    player = _errorSoundPlayer;
                    break;
            }
            if (Config.StringToBool(setting) && pattern != null)
            {
                var regex = new Regex(pattern);
                var match = regex.Match(line);
                if (match.Success)
                {
                    player?.Play();
                    Thread.Sleep(500);
                }
            }
        }

        public void FindText(string? findWhatText, bool reverse)
        {
            if (findWhatText == null)
            {
                if (_findWhatText == "")
                {
                    return;
                }
                else
                {
                    findWhatText = _findWhatText;
                }
            }
            else
            {
                _findWhatText = findWhatText;
                AddToHistoryConfig(findWhatText, HistoryType.FindTexts);
                var newHistoryList = ConvertHistoryToList(AppHistory.findTexts);
                AppFindTextDialog?.UpdateHistory(newHistoryList);
            }
            var outputText = _mainWindow.GetOutput();
            var ignoreCase = Config.StringToBool(AppSettings.findTextIgnoreCase);
            if (ignoreCase == true)
            {
                findWhatText = findWhatText.ToLower();
                outputText = outputText.ToLower();
            }
            var caretIndex = _mainWindow.GetOutputCaretIndex();
            var foundIndex = -1;
            if (!reverse)
            {
                // Find the first text occurrance in the output text substring starting at the caret position + 1
                var findStartIndex = caretIndex + 1;
                if (findStartIndex > outputText.Length)
                {
                    return;
                }
                foundIndex = outputText.IndexOf(findWhatText, findStartIndex);
            }
            else
            {
                // Reverse find the last text occurrance in the output text substring starting from the caret position - 1 and ending at the beginning
                var findEndIndex = caretIndex - 1;
                if (findEndIndex <= 0)
                {
                    return;
                }
                foundIndex = outputText.IndexOf(findWhatText, findEndIndex);
            }
            if (foundIndex >= 0)
            {
                _mainWindow.SetOutputCaretIndex(foundIndex);
            }
            else
            {
                SystemSounds.Exclamation.Play();
            }
        }

        public void ChangeFindTextIgnoreCaseSetting(bool value)
        {
            AppSettings.findTextIgnoreCase = Config.BoolToString(value);
            SaveSettings();
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

        public void ChangeSuccessRegexSetting(string value)
        {
            AppSettings.successRegex = value;
            SaveSettings();
        }

        public void ChangeErrorRegexSetting(string value)
        {
            AppSettings.errorRegex = value;
            SaveSettings();
        }

        private void SaveSettings()
        {
            _config.Save();
        }
    }
}

