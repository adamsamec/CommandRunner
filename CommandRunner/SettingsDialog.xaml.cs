using System.Windows;

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
            Title = CommandRunner.Resources.settingsDialogTitle + Consts.WindowTitleSeparator + Consts.AppName;

            // Set checkboxes state from settings
            checkForUpdateOnLaunchCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.checkForUpdateOnLaunch);
            playSuccessSoundCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.playSuccessSound);
            playErrorSoundCheckBox.IsChecked = Config.StringToBool(_runner.AppSettings.playErrorSound);

            // Set regex textboxes enabled state according to checkboxes and set their values from settings
            successRegexTextBox.IsEnabled = (bool)playSuccessSoundCheckBox.IsChecked;
            errorRegexTextBox.IsEnabled = (bool)playErrorSoundCheckBox.IsChecked;
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

        private async void checkForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var doUpdate = false;
            UpdateData? updateData = null;
            try
            {
                updateData = await _runner.AppUpdater.CheckForUpdate();
                if (updateData == null)
                {
                    var noUpdateAvailableDialog = new NoUpdateAvailableDialog();
                    noUpdateAvailableDialog.Owner = this;
                    noUpdateAvailableDialog.ShowDialog();
                }
                else
                {
                    var updateAvailableDialog = new UpdateAvailableDialog(_runner, updateData);
                    updateAvailableDialog.Owner = this;
                    doUpdate = updateAvailableDialog.ShowDialog() == true;
                }
            }
            catch (Exception)
            {
                var updateCheckFailedDialog = new UpdateCheckFailedDialog();
                updateCheckFailedDialog.Owner = this;
                updateCheckFailedDialog.ShowDialog();
            }
            if (doUpdate && updateData != null)
            {
                _runner.AppUpdateData = updateData;
                var downloadingUpdateDialog = new DownloadingUpdateDialog(_runner, updateData);
                downloadingUpdateDialog.Owner = this;
                downloadingUpdateDialog.DownloadUpdate();
                downloadingUpdateDialog.ShowDialog();
            }
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