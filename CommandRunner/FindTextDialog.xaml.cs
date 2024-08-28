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
        }

        private void FindTextDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Set checkboxes state from settings
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
        }

                private void findPreviousButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}