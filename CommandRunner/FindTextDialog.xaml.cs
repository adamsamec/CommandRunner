using System.Windows;
using System.Windows.Controls;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for FindTextDialog.xaml
    /// </summary>
    public partial class FindTextDialog : Window
    {
        private Runner _runner;

        public FindTextDialog(Runner runner)
        {
            InitializeComponent();

            _runner = runner;
            _runner.AppFindTextDialog = this;
        }

        private void FindTextDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Set find text history from config
            var findTextsHistory = _runner.ConvertHistoryToList(_runner.AppHistory.findTexts);
            UpdateHistory(findTextsHistory);

            // Set ignore case checkbox state from settings
            ignoreCaseCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.findTextIgnoreCase);

            // Set initial focus
            findWhatComboBox.Focus();
        }

        private void ignoreCaseCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _runner.ChangeFindTextIgnoreCaseSetting(true);
        }

        private void ignoreCaseCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _runner.ChangeFindTextIgnoreCaseSetting(false);
        }

        private void findNextButton_Click(object sender, RoutedEventArgs e)
        {
            _runner.FindText(findWhatComboBox.Text, false);
            Close();
                    ((MainWindow) Owner).FocusOutput();
        }

                private void findPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _runner.FindText(findWhatComboBox.Text, true);
        }

        public void UpdateHistory(List<string> items)
        {
            findWhatComboBox.Items.Clear();
            if (items.Count() >= 1)
            {
                findWhatComboBox.Text = items[0];
            }
            foreach (string item in items)
            {
                findWhatComboBox.Items.Add(item);
            }
        }
    }
}