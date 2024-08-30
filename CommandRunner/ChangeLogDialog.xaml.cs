using System.Windows;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for ChangeLogDialog.xaml
    /// </summary>
    public partial class ChangeLogDialog : Window
    {
        private string _pageContent;

        public ChangeLogDialog(string pageContent)
        {
            InitializeComponent();

            _pageContent = pageContent;
        }

        private void ChangeLogDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Page.SetupWebBrowser(webBrowser, _pageContent);
        }
    }
}