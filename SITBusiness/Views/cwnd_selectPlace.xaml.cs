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
using SITBusiness.WS_Link;
using SIT_classLib;

namespace SITBusiness.Views
{
    public partial class cwnd_selectPlace : ChildWindow
    {
        public wsSimpleItem selectedItem = new wsSimpleItem();
        public cwnd_selectPlace()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(cwnd_selectPlace_Loaded);
        }

        void cwnd_selectPlace_Loaded(object sender, RoutedEventArgs e)
        {
            WServiceClient wc = new WServiceClient();
            wc.ws_selectDBTreeCompleted += new EventHandler<ws_selectDBTreeCompletedEventArgs>(wc_ws_selectDBTreeCompleted);
            wc.ws_selectDBTreeAsync(3); // Places tree without baseItems
        }

        void  wc_ws_selectDBTreeCompleted(object sender, ws_selectDBTreeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DBTree DBTREE = new DBTree();
                DBTREE.DBDATA = e.Result.DBDATA;
                Node NNODE = new Node(DBTREE.GetAnyNodeinDB);

                tree_choosePlace.ItemsSource = NNODE.chldlist;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }  
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (tree_choosePlace.SelectedItem != null)
            {
                Node NN = tree_choosePlace.SelectedItem as Node;
                selectedItem.ID = NN.bi.ID;
                selectedItem.Description = NN.bi.Description;
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}

