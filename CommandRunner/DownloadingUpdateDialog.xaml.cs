using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for DownloadingUpdateDialog.xaml
    /// </summary>
    public partial class DownloadingUpdateDialog : Window
    {
        private Runner _runner;
        private UpdateData? _updateData;
        private UpdateDownloadInProgressDialog? _updateDownloadInProgressDialog;

        public DownloadingUpdateDialog(Runner runner, UpdateData? updateData)
        {
            InitializeComponent();

            _runner = runner;
            _updateData = updateData;

            _runner.AppUpdater.DownloadingDialog = this;
            KeyDown += DownloadingUpdateDialog_KeyDown;
        }

        public void DownloadUpdate()
        {
            Updater.DownloadProgressHandler downloadProgressHandler = (progress) =>
            {
                if (_runner.AppUpdater.DownloadingDialog != null)
                {
                _runner.AppUpdater.DownloadingDialog.updateDownloadProgressBar.Value = progress;
                }
            };
            Updater.DownloadCompleteHandler downloadCompleteHandler = () =>
            {
                if (_updateDownloadInProgressDialog != null && _updateDownloadInProgressDialog.IsVisible)
                {
                    _updateDownloadInProgressDialog.DialogResult = false;
                }
                ShowLaunchUpdateInstallerDialog();
                if (_runner.AppUpdater.DownloadingDialog != null) { 
                _runner.AppUpdater.DownloadingDialog.DialogResult = true;
                }
            };
            Updater.InstallerRunningHandler installerRunningHandler = () =>
            {
                var updateInstallRunningDialog = new UpdateInstallRunningDialog();
                updateInstallRunningDialog.Owner = this;
                updateInstallRunningDialog.ShowDialog();
            };
            Updater.DownloadErrorHandler downloadErrorHandler = () =>
            {
                var updateDownloadFailedDialog = new UpdateDownloadFailedDialog();
                if (IsVisible)
                {
                    updateDownloadFailedDialog.Owner = this;
                }
                updateDownloadFailedDialog.ShowDialog();
            };
            if (_updateData != null)
            {
            var downloadTask = _runner.AppUpdater.DownloadAsync(_updateData, downloadProgressHandler, downloadCompleteHandler, installerRunningHandler, downloadErrorHandler);
            }
        }

        public void ShowLaunchUpdateInstallerDialog()
        {
            if (_runner.AppUpdater.State != Updater.UpdateState.Downloaded)
            {
                return;
            }
            var launchUpdateInstallerDialog = new LaunchUpdateInstallerDialog();
            _runner.AppUpdater.LaunchInstallerDialog = launchUpdateInstallerDialog;
            if (IsVisible)
            {
                launchUpdateInstallerDialog.Owner = this;
            }
            var doLaunchInstaller = launchUpdateInstallerDialog.ShowDialog() == true;
            _runner.AppUpdater.State = Updater.UpdateState.Initial;
            if (doLaunchInstaller)
            {
                _runner.AppUpdater.LaunchInstaller();
            }
        }

        private void DownloadingUpdateDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (_updateData != null) { 
            downloadingUpdateMessage.Text = String.Format(CommandRunner.Resources.downloadingUpdateMessage, _updateData.version, Consts.AppVersion);
            }
            updateDownloadProgressBar.Focus();
        }

        private void DownloadingUpdateDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || (e.Key == Key.System && e.SystemKey == Key.F4))
            {
                if (CancelUpdateDownloadAndClose())
                {
                    DialogResult = true;
                }
            }
        }

        private bool CancelUpdateDownloadAndClose()
        {
            if (_runner.AppUpdater.State != Updater.UpdateState.Downloading)
            {
                return true;
            }
            _updateDownloadInProgressDialog = new UpdateDownloadInProgressDialog();
            _updateDownloadInProgressDialog.Owner = this;
            var doCancelAndClose = _updateDownloadInProgressDialog.ShowDialog() == true;
            if (doCancelAndClose)
            {
                _runner.AppUpdater.CancelDownload();
            }
            return doCancelAndClose;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if (CancelUpdateDownloadAndClose())
            {
                DialogResult = true;
            }
        }

    }
}