using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;
using SITBusiness.WS_Link;
using SITBusiness.Views;
using SITBusiness.Assets;
using SITBusiness;
using SIT_classLib;

namespace SITBusiness
{
    public partial class uc_DevWorkStates : UserControl
    {
        private int uc_PID;
        private string uc_descripton;
        public WServiceClient wc = new WServiceClient();

        private PagedCollectionView messageCollection;
        private PagedCollectionView sNdCollection;
        private int uc_PageCount = 25;

        public wsWorkState WSt;// = new wsWorkState();
        
        public uc_DevWorkStates()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(uc_DevWorkStates_Loaded);

        }

        public void uc_configure(int _id,string _description)
        {
            this.uc_PID = _id;
            this.uc_descripton = _description;

            WSt = new wsWorkState();          
            WSt.ID = _id;
            //WSt.setDefaults();
            wc.ws_selectDeviceWorkStatesAsync(uc_PID, WSt.startDate, WSt.finishDate);
        }

        void uc_DevWorkStates_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDeviceWorkStatesCompleted += new EventHandler<ws_selectDeviceWorkStatesCompletedEventArgs>(wc_ws_selectDeviceWorkStatesCompleted);
        }

        void wc_ws_selectDeviceWorkStatesCompleted(object sender, ws_selectDeviceWorkStatesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                WSt = e.Result;

                WSt.Description = this.uc_descripton;

                if (WSt.list_SCADAMessages.Count <= 0)
                {
                    dg_Root.Visibility = Visibility.Collapsed;
                    panel_NoInfo.Visibility = Visibility.Visible;
                }
                else
                {
                    dg_Root.Visibility = Visibility.Visible;
                    panel_NoInfo.Visibility = Visibility.Collapsed;
                }

                LayoutRoot.DataContext = WSt;

                if (box_ed.Items.Count >0)
                    box_ed.SelectedIndex = 0;
                
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }         
        }

        private void btn_applyPeriod_Click(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDeviceWorkStatesAsync(uc_PID, WSt.startDate, WSt.finishDate);
        }

        private void btn_resetPeriod_Click(object sender, RoutedEventArgs e)
        {
            WSt.setDefaults();
            wc.ws_selectDeviceWorkStatesAsync(uc_PID, WSt.startDate, WSt.finishDate);
        }

        private void box_ed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                wsSimpleItem item = (sender as ComboBox).SelectedItem as wsSimpleItem;
                WSt.setDurationFormat((int)item.ID);
                dg_control.ItemsSource = null;
                dg_control.ItemsSource = WSt.list_StatesDurations;
            }
        }

    }

}
