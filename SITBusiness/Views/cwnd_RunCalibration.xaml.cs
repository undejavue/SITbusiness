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

namespace SITBusiness.Views
{
    public partial class cwnd_RunCalibration : ChildWindow
    {
        private wsCalibration item = new wsCalibration();
        private WServiceClient wc = new WServiceClient();

        private int ID;
        
        public cwnd_RunCalibration(int _id)
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(cwnd_RunCalibration_Loaded);

            ID = _id;

            List<wsSimpleItem> result_list = new List<wsSimpleItem>();

            result_list.Add(new wsSimpleItem { ID = 1, Description = "Годен" });
            result_list.Add(new wsSimpleItem { ID = 0, Description = "Не годен" });

            cbox_result.ItemsSource = result_list;
            cbox_result.SelectedIndex = 0;
            item.ID = ID;
            item.factDate = DateTime.Now;

            grid_Calibration.DataContext = item;
        }

        public void configure()
        {

        }

        void cwnd_RunCalibration_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_insertCalibrationCompleted += new EventHandler<ws_insertCalibrationCompletedEventArgs>(wc_ws_insertCalibrationCompleted);
            wc.ws_selectCalibrationTypesCompleted += new EventHandler<ws_selectCalibrationTypesCompletedEventArgs>(wc_ws_selectCalibrationTypesCompleted);

            wc.ws_selectCalibrationTypesAsync();
        }

        void wc_ws_selectCalibrationTypesCompleted(object sender, ws_selectCalibrationTypesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                cbox_type.ItemsSource = e.Result;
                cbox_type.SelectedIndex = 0;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_insertCalibrationCompleted(object sender, ws_insertCalibrationCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                this.DialogResult = true;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_INSERT, e.OpStatus.ToString());
                w.Show();
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            item = grid_Calibration.DataContext as wsCalibration;

            if ((int)(cbox_result.SelectedItem as wsSimpleItem).ID == 1)
                item.result = true;
            else
                item.result = false;

            item.typeID = (int)(cbox_type.SelectedItem as wsSimpleItem).ID;

            wc.ws_insertCalibrationAsync(item);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

