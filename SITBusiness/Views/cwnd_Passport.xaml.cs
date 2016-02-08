using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SITBusiness.Views
{
    public partial class cwnd_Passport : ChildWindow
    {
        public cwnd_Passport(int PID)
        {
            InitializeComponent();
            uc_PassportForm ucR = new uc_PassportForm();
            ucR.uc_Configure('R', PID);
            RootPanel.Children.Add(ucR);
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

