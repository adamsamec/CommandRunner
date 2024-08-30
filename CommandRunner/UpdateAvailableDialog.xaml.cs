using System.Windows;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for UpdateAvailableDialog.xaml
    /// </summary>
    public partial class UpdateAvailableDialog : Window
    {
        private Runner _runner;
        private UpdateData _updateData;

        public UpdateAvailableDialog(Runner runner, UpdateData updateData)
        {
            InitializeComponent();

            _runner = runner;
            _updateData = updateData;
        }

        private void UpdateAvailableDialog_Loaded(object sender, RoutedEventArgs e)
        {
            updateAvailableMessage.Text = String.Format(CommandRunner.Resources.updateAvailableMessage, _updateData.version, Consts.AppVersion);
            whatsNewButton.Focus();
        }

        private async void whatsNewButton_Click(object sender, RoutedEventArgs e)
        {
            string pageContent;
            try
            {
                pageContent = await Page.GetChangeLogPageContent();
            }
            catch (PageRetrieveFailedException)
            {
                pageContent = CommandRunner.Resources.changeLogRetrievalFailedMessage;
            }
            var changeLogWindow = new ChangeLogDialog(pageContent);
            changeLogWindow.Owner = this;
            changeLogWindow.ShowDialog();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void notNowButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}