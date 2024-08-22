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

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            _runner.Run(commandTextBox.Text);
        }

        public void AppendToOutput(String text)
        {
            outputTextBox.Text += text;
        }
    }
}