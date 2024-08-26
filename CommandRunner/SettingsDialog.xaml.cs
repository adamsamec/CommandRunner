using System.Windows;
using System.Windows.Controls;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        private Runner _runner;

        public SettingsDialog(Runner runner)
        {
            InitializeComponent();

            _runner = runner;
        }

        private void SettingsDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Set checkboxes state from settings
            checkForUpdateOnLaunchCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.checkForUpdateOnLaunch);

            // Set initial focus
            checkForUpdateOnLaunchCheckBox.Focus();
        }

        private void checkForUpdateOnLaunchCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // _runner.ChangeCheckUpdateOnFirstShowSetting(true);
        }

        private void checkForUpdateOnLaunchCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // _runner.ChangeCheckUpdateOnFirstShowSetting(false);
        }

        private void checkForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void playSuccessSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void playSuccessSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void playErrorSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void playErrorSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
        }
    }
}