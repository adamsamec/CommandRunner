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
            playSuccessSoundCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.playSuccessSound);
            playErrorSoundCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.playErrorSound);

            // Set regex textboxes enabled state according to checkboxes and set their values from settings
            successRegexTextBox.IsEnabled = (bool) playSuccessSoundCheckBox.IsChecked;
            errorRegexTextBox.IsEnabled = (bool) playErrorSoundCheckBox.IsChecked;
            successRegexTextBox.Text = _runner.AppSettings.successRegex;
            errorRegexTextBox.Text = _runner.AppSettings.errorRegex;

            // Set initial focus
            checkForUpdateOnLaunchCheckBox.Focus();
        }

        private void checkForUpdateOnLaunchCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _runner.ChangeCheckForUpdateOnLaunchSetting(true);
        }

        private void checkForUpdateOnLaunchCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _runner.ChangeCheckForUpdateOnLaunchSetting(false);
        }

        private void checkForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void playSuccessSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _runner.ChangePlaySuccessSoundSetting(true);
            successRegexTextBox.IsEnabled = true;
        }

        private void playSuccessSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _runner.ChangePlaySuccessSoundSetting(false);
            successRegexTextBox.IsEnabled = false;
        }

        private void successRegexTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            _runner.ChangeSuccessRegexSetting(successRegexTextBox.Text);
        }

        private void playErrorSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _runner.ChangePlayErrorSoundSetting(true);
            errorRegexTextBox.IsEnabled = true;
        }

        private void playErrorSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _runner.ChangePlayErrorSoundSetting(false);
            errorRegexTextBox.IsEnabled = false;
        }

        private void errorRegexTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            _runner.ChangeErrorRegexSetting(errorRegexTextBox.Text);
        }
    }
}