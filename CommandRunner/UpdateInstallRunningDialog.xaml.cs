using System.Windows;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for UpdateInstallRunningDialog.xaml
    /// </summary>
    public partial class UpdateInstallRunningDialog : Window
    {
        public UpdateInstallRunningDialog()
        {
            InitializeComponent();
        }

        private void UpdateInstallRunningDialog_Loaded(object sender, RoutedEventArgs e)
        {
            closeButton.Focus();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}