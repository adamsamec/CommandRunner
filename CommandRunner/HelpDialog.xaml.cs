using System.Windows;

namespace CommandRunner
{
    /// <summary>
    /// Interaction logic for HelpDialog.xaml
    /// </summary>
    public partial class HelpDialog : Window
    {
        public HelpDialog()
        {
            InitializeComponent();
        }

        private void HelpDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Title = CommandRunner.Resources.helpDialogTitle + Consts.WindowTitleSeparator + Consts.AppName;

            var pageContent = "<h1>CommandRunner " + Consts.AppVersion + @"</h1>
" + Page.GetHelpPageContent();
            Page.SetupWebBrowser(webBrowser, pageContent);
        }
    }
}