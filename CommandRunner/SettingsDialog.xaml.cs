﻿using System.Windows;
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

            // Set regex textboxes state according to checkboxes
            successRegexTextBox.IsEnabled = (bool) playSuccessSoundCheckBox.IsChecked;
            errorRegexTextBox.IsEnabled = (bool) playErrorSoundCheckBox.IsChecked;

            // Set initial focus
            checkForUpdateOnLaunchCheckBox.Focus();
        }

        private void checkForUpdateOnLaunchCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // _runner.ChangeCheckForUpdateOnLaunchSetting(true);
        }

        private void checkForUpdateOnLaunchCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // _runner.ChangeCheckForUpdateOnLaunchSetting(false);
        }

        private void checkForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void playSuccessSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            successRegexTextBox.IsEnabled = true;
        }

        private void playSuccessSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            successRegexTextBox.IsEnabled = false;
        }

        private void playErrorSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            errorRegexTextBox.IsEnabled = true;
        }

        private void playErrorSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            errorRegexTextBox.IsEnabled = false;
        }
    }
}