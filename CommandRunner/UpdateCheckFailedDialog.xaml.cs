using System.Windows;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for UpdateCheckFailedDialog.xaml
    /// </summary>
    public partial class UpdateCheckFailedDialog : Window
    {
        public UpdateCheckFailedDialog()
        {
            InitializeComponent();
        }

        private void UpdateCheckFailedDialog_Loaded(object sender, RoutedEventArgs e)
        {
            closeButton.Focus();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}