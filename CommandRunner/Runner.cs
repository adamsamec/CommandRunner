using System.Diagnostics;

namespace CommandRunner
{
    /// <summary>
    /// Main application logic
    /// </summary>
    public class Runner
    {
        private MainWindow _mainWindow;
        private Process _runningProcess;
        private bool _isRunning = false;

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

            var thread = new Thread(() =>
            {
                while (_runningProcess.StandardOutput.Peek() >= 0)
                {
                    var line = _runningProcess.StandardOutput.ReadLine() + "\r\n";
                    _mainWindow.Dispatcher.BeginInvoke((Action)(() => {
                        _mainWindow.AppendToOutput(line);
                    }));
                }
                _isRunning = false;
                    _mainWindow.Dispatcher.BeginInvoke((Action)(() => {
                _mainWindow.AllowRunning();
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
        }
    }
}

