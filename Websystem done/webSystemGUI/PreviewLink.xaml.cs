using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for PreviewLink.xaml
    /// </summary>
    public partial class PreviewLink : Window
    {
        public PreviewLink(String linkCode)
        {
            InitializeComponent();

            previewLinkScrollBox.Content = linkCode;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void copyTextBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(previewLinkScrollBox.Content.ToString());
        }
    }
}
