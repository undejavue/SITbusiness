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
using SIT_classLib;
using SITBusiness.WS_Link;

namespace SITBusiness
{
    public partial class uc_Repair : UserControl
    {
        public uc_Repair()
        {
            InitializeComponent();

            List<wsSimpleItem> slist = new List<wsSimpleItem>();

            slist.Add(new wsSimpleItem(0, "Сведения о ремонте пример"));
            slist.Add(new wsSimpleItem(1, "Сведения о ТО пример"));
            slist.Add(new wsSimpleItem(2, "Сведения о ремонте пример"));
            slist.Add(new wsSimpleItem(3, "Сведения о ТО пример"));
            slist.Add(new wsSimpleItem(4, "Сведения о ремонте пример"));
            slist.Add(new wsSimpleItem(5, "Сведения о ТО пример"));

            dg_Items.ItemsSource = slist;
        }

        private void btn_Insert_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
