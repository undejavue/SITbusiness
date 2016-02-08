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
using System.Windows.Navigation;


namespace SITBusiness.Views
{
    public partial class DevicesTree : Page
    {
        public DevicesTree()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Page_Loaded);

        }


        void Page_Loaded(object sender, RoutedEventArgs e)
        {

            WS_Link.WServiceClient wc = new WS_Link.WServiceClient();
            wc.WS_GetTreeCompleted += new EventHandler<WS_Link.WS_GetTreeCompletedEventArgs>(wc_WS_GetTreeCompleted);
            wc.WS_GetTreeAsync();

        }

        void wc_WS_GetTreeCompleted(object sender, WS_Link.WS_GetTreeCompletedEventArgs e)
        {
            WS_Link.Parent  FreshParentTree = new WS_Link.Parent();
            
            try
            {
                FreshParentTree = e.Result;
                treeView.ItemsSource = FreshParentTree.ListChild;
            }
            catch (Exception err)
            {
                HeaderText.Text += "Исключение в WS_GetTree: ";
                //HeaderText.Text +=  err.ToString();
                ErrorWindow.CreateNew(err);
                //InfoWindow iw = new InfoWindow("Ошибка в дереве!",);
            }
        }


        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                WS_Link.BaseItem bt = treeView.SelectedItem as WS_Link.BaseItem;
                WS_Link.WServiceClient wc = new WS_Link.WServiceClient();
                wc.WS_GetDevicesCompleted += new EventHandler<WS_Link.WS_GetDevicesCompletedEventArgs>(wc_WS_GetDevicesCompleted);

                //if (bt.lastchild == 1)
                //{
                    wc.WS_GetDevicesAsync(bt.ID);
                //}
            }
            catch (Exception err)
            {
                HeaderText.Text = "Исключение в WS_GetDevices: ";
                ErrorWindow.CreateNew(err);
            }
        }

        void wc_WS_GetDevicesCompleted(object sender, WS_Link.WS_GetDevicesCompletedEventArgs e)
        {
            
            list_devices.ItemsSource = e.Result;
            
        }


        private void btn_GetPassport_Click(object sender, RoutedEventArgs e)
        {
            int t = 1;
            //t = (int)((System.Windows.UIElement)sender).Tag;
            WS_Link.BaseItem b = list_devices.SelectedItem as WS_Link.BaseItem;
            
            if (b != null)
            {
                ChildWindow PWindow = new Passport(b.ID);
                PWindow.Show();
            }
        }


        private void list_devices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WS_Link.BaseItem b = list_devices.SelectedItem as WS_Link.BaseItem;        
            
        }
    }
}
