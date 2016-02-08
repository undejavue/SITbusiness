using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SIT_classLib;

namespace SITBusiness.Views
{
    public partial class cwnd_ResultToDBGrid : ChildWindow
    {
        public cwnd_ResultToDBGrid(string messageText, ObservableCollection<wsSimpleItem> slist)
        {
            InitializeComponent();
            txt_message.Text = messageText;
            dg_Result.ItemsSource = slist;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
