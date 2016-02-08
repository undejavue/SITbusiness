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
    public partial class cwnd_treeTypes : ChildWindow
    {
        WServiceClient wc = new WServiceClient();
        public wsSimpleItem selectedItem = new wsSimpleItem();
        public string storedString = null;

        public cwnd_treeTypes()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(cwnd_treeTypes_Loaded);
        }

        void cwnd_treeTypes_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDBTreeCompleted += new EventHandler<ws_selectDBTreeCompletedEventArgs>(wc_ws_selectDBTreeCompleted);
            wc.ws_selectDBTreeAsync(4);
        }

        void wc_ws_selectDBTreeCompleted(object sender, ws_selectDBTreeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DBTree DBTREE = new DBTree();
                DBTREE.DBDATA = e.Result.DBDATA;
                Node NNODE = new Node(DBTREE.GetAnyNodeinDB);
                tree_chooseType.ItemsSource = NNODE.chldlist;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }  
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (tree_chooseType.SelectedItem != null)
            {
                Node NN = tree_chooseType.SelectedItem as Node;
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

