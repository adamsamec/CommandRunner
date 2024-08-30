using System.Windows;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for UpdateDownloadFailedDialog.xaml
    /// </summary>
    public partial class UpdateDownloadFailedDialog : Window
    {
        public UpdateDownloadFailedDialog()
        {
            InitializeComponent();
        }

        private void UpdateDownloadFailedDialog_Loaded(object sender, RoutedEventArgs e)
        {
            closeButton.Focus();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}