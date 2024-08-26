using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommandRunner
{
    /// <summary>
    /// ComboBox with Down arrow key behavior modification
    /// </summary>
    public class DownArrowDropdownComboBox : ComboBox
    {
        public DownArrowDropdownComboBox() { }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Down && !IsDropDownOpen)
            {
                IsDropDownOpen = true;
            }
        }
    }
}