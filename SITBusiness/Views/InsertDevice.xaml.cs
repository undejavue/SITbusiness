using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using System.Windows.Navigation;

namespace SITBusiness.Views
{  
    public partial class InsertDevice : Page
    {

        WS_Link.BaseItem b = new WS_Link.BaseItem();
        WS_Link.WServiceClient wc = new WS_Link.WServiceClient();
        NaviTree tree = new NaviTree();
        WS_Link.PassportType LocalPassport = new WS_Link.PassportType();

        
        public InsertDevice()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(InsertDevice_Loaded);
        }

        void InsertDevice_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wc.WS_GetTreeCompleted += new EventHandler<WS_Link.WS_GetTreeCompletedEventArgs>(wc_WS_GetTreeCompleted);
                wc.WS_GetTreeAsync();
            }
            catch (Exception err)
            {
                ErrorWindow.CreateNew(err);
            }
        }

        void wc_WS_GetTreeCompleted(object sender, WS_Link.WS_GetTreeCompletedEventArgs e)
        {
            tree.set_ParentTree(e.Result);
            

            try
            {
                tree.refresh_Nodes(2, 0);
                box_Nodes_1.ItemsSource = tree.Node_1;
                if (box_Nodes_1.Items.Count > 1)
                    box_Nodes_1.SelectedIndex = 1;

                box_Nodes_2.ItemsSource = tree.Node_2;
                if (box_Nodes_2.Items.Count > 0)
                    box_Nodes_2.SelectedIndex = 0;

                box_Nodes_3.ItemsSource = tree.Node_3;
                if (box_Nodes_3.Items.Count > 0)
                    box_Nodes_3.SelectedIndex = 0;
            }
            catch (Exception err)
            {
                ErrorWindow.CreateNew(err);
            }
        }

        private void box_Nodes_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int n1=0;
                TreeItem selectedItem = box_Nodes_1.SelectedItem as TreeItem;
                if (selectedItem != null) n1 = selectedItem.ID;
                
                tree.refresh_Nodes(n1, 0);
                box_Nodes_2.ItemsSource = tree.Node_2;
                if (box_Nodes_2.Items.Count > 0)
                    box_Nodes_2.SelectedIndex = 0;

                box_Nodes_3.ItemsSource = tree.Node_3;
                if (box_Nodes_3.Items.Count > 0)
                    box_Nodes_3.SelectedIndex = 0;
                tree.Node_1_SelectedID = n1;
            }
            catch (Exception err)
            {
                ErrorWindow.CreateNew(err);
            }
        }

        private void box_Nodes_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int n1 = 0;
                int n2 = 0;

                TreeItem selectedItem1 = box_Nodes_1.SelectedItem as TreeItem;
                TreeItem selectedItem2 = box_Nodes_2.SelectedItem as TreeItem;
                if (selectedItem1 != null) n1 = selectedItem1.ID;
                if (selectedItem2 != null) n2 = selectedItem2.ID;
                
                tree.refresh_Nodes(n1,n2);
                box_Nodes_3.ItemsSource = tree.Node_3;
                if (box_Nodes_3.Items.Count > 0)
                    box_Nodes_3.SelectedIndex = 0;
                tree.Node_1_SelectedID = n1;
                tree.Node_2_SelectedID = n2;
            }
            catch (Exception err)
            {
                ErrorWindow.CreateNew(err);
            }
        }

        private void btn_INSERT_Click(object sender, RoutedEventArgs e)
        {
            //wc.WS_GetPassportCompleted += new EventHandler<WS_Link.WS_GetPassportCompletedEventArgs>(wc_WS_GetPassport);
            //wc.WS_GetPassportAsync(2);

            grid_Device_detail.DataContext = LocalPassport;
        }

        //private void wc_WS_GetPassport(object sender, WS_Link.WS_GetPassportCompletedEventArgs e)
        //{
        //    WS_Link.PassportType DevPassport = new WS_Link.PassportType();
        //    try
        //    {
        //        DevPassport = e.Result;
        //        //grid_Device_detail.DataContext = e.Result;
        //    }
        //    catch (Exception err)
        //    {
        //        ErrorWindow.CreateNew(err);
        //    }
        //}

        private void btn_VIEW_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text += LocalPassport.DevDescrRU;
        }

    }




}
