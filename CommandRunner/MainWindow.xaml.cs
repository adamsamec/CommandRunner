using System.Windows;
using System.Windows.Shapes;

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
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            killButton.IsEnabled = false;
            commandComboBox.Focus();
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            RunCommand();
        }

        private void killButton_Click(object sender, RoutedEventArgs e)
        {
            KillRunningProcess();
        }

        public void AppendToOutput(string text)
        {
            outputTextBox.Text += text;
        }

        public void RunCommand()
        {
            _runner.RunCommand(commandComboBox.Text);
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
    }
}