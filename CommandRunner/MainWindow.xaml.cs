using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Runner _runner;

        public MainWindow()
        {
            InitializeComponent();

            _runner = new Runner(this);

            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set history from config
            var commandsHistory = _runner.ConvertHistoryToList(_runner.AppHistory.commands);
            var workingDirsHistory = _runner.ConvertHistoryToList(_runner.AppHistory.workingDirs);
            UpdateHistory(commandsHistory, Runner.HistoryType.Commands);
            UpdateHistory(workingDirsHistory, Runner.HistoryType.WorkingDirs);

            killButton.IsEnabled = false;

            // Set initial focus
            commandComboBox.Focus();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            if (isControlDown && e.Key == Key.F)
            {
                var findTextDialog = new FindTextDialog(_runner);
                findTextDialog.Owner = this;
                findTextDialog.ShowDialog();
            }
            else if (e.Key == Key.F3)
            {
                if (isShiftDown)
                {
                    _runner.FindText(null, true);
                } else
                {
                    _runner.FindText(null, false);
                }
            }
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            RunCommand();
        }

        private void killButton_Click(object sender, RoutedEventArgs e)
        {
            KillRunningProcess();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsDialog = new SettingsDialog(_runner);
            settingsDialog.Owner = this;
            settingsDialog.ShowDialog();
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            var helpDialog = new HelpDialog();
            helpDialog.Owner = this;
            helpDialog.ShowDialog();
        }

        public void AppendToOutput(string text)
        {
            outputTextBox.Text += text;
        }

        public void RunCommand()
        {
            _runner.RunCommand(commandComboBox.Text, workingDirComboBox.Text);
            if (_runner.IsRunning)
            {
                DisallowRunningAndFocusOutput();
            }
        }

        public void KillRunningProcess()
        {
            _runner.KillRunningProcess();
            if (!_runner.IsRunning)
            {
                AllowRunning();
            }
        }

        public void AllowRunning()
        {
            runButton.IsEnabled = true;
            if (killButton.IsFocused)
            {
                runButton.Focus();
            }
            killButton.IsEnabled = false;
        }

        public void DisallowRunningAndFocusOutput()
        {
            runButton.IsEnabled = false;
            killButton.IsEnabled = true;
            outputTextBox.Focus();
        }

        public void FocusOutput()
        {
            outputTextBox.Focus();
        }

        public void UpdateHistory(List<string> items, Runner.HistoryType type)
        {
            ComboBox? comboBox = null;
            switch (type)
            {
                case Runner.HistoryType.Commands:
                    comboBox = commandComboBox;
                    break;
                case Runner.HistoryType.WorkingDirs:
                    comboBox = workingDirComboBox;
                    break;
            }
            if (comboBox != null)
            {
                comboBox.Items.Clear();
                if (items.Count() >= 1)
                {
                    comboBox.Text = items[0];
                }
                foreach (string item in items)
                {
                    comboBox.Items.Add(item);
                }
            }
        }

        public string GetOutput()
        {
            return outputTextBox.Text;
        }

        public int GetOutputCaretIndex()
        {
            return outputTextBox.CaretIndex;
        }

        public void SetOutputCaretIndex(int index)
        {
            outputTextBox.CaretIndex = index;
        }
    }
}