using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CommandRunner
{
    /// <summary>
    /// Main application logic
    /// </summary>
    public class Runner
    {
        private MainWindow _mainWindow;

        public Runner(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void Run(String command)
        {
            var process = new Process();
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
            process.StartInfo = startInfo;
            process.Start();

            var thread = new Thread(() =>
            {
                while (process.StandardOutput.Peek() >= 0)
                {
                    var line = process.StandardOutput.ReadLine() + "\r\n";
                    _mainWindow.Dispatcher.BeginInvoke((Action)(() => {
                        _mainWindow.AppendToOutput(line);
                    }));
                }
            });
            thread.Start();
        }
    }
}

