using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SIT_classLib;
using SITBusiness.WS_Link;
using SITBusiness.Views;

namespace SITBusiness
{
    public partial class uc_Calibration : UserControl
    {
        public int PID;
        public string uc_DevDescrRU;
        WServiceClient wc = new WServiceClient();

        public uc_Calibration()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(uc_Calibration_Loaded);
        }

        public void uc_Configure(int _PID)
        {
            PID = _PID;
            wc.ws_selectDeviceCalibrationsAsync(PID);

        }

        void uc_Calibration_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDeviceCalibrationsCompleted += new EventHandler<ws_selectDeviceCalibrationsCompletedEventArgs>(wc_ws_selectDeviceCalibrationsCompleted);
        }

        void wc_ws_selectDeviceCalibrationsCompleted(object sender, ws_selectDeviceCalibrationsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.Count > 0)
                {
                    dg_Calibration.Visibility = Visibility.Visible;
                    no_Calibration.Visibility = Visibility.Collapsed;

                    PagedCollectionView collection = new PagedCollectionView(e.Result);

                    dg_Calibration.ItemsSource = collection;

                }
                else
                {
                    dg_Calibration.Visibility = Visibility.Collapsed;
                    no_Calibration.Visibility = Visibility.Visible;
                }
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        private void btn_Calibrate_Click(object sender, RoutedEventArgs e)
        {
            cwnd_RunCalibration w = new cwnd_RunCalibration(PID);
            w.Show();
            w.Closed += new EventHandler(w_Closed);
        }

        void w_Closed(object sender, EventArgs e)
        {
            if ((sender as cwnd_RunCalibration).DialogResult == true)
            {
                uc_Configure(PID);
            }
        }

        private void dg_Calibration_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //wsCalibration item = e.Row.DataContext as wsCalibration;

            //dg_Calibration.AlternatingRowBackground = null;

            //if (item.resultName.Contains("не"))
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Red);
            //    e.Row.Foreground = new SolidColorBrush(Colors.White);
            //}
            //else
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Green);
            //    e.Row.Foreground = new SolidColorBrush(Colors.White);
            //}
        }



        private void SetTeamColors(DependencyObject element, string searchText)
        {
            Border brd = element as Border;
            if (brd != null)
            {
                if (brd.Name == "teamBorder")
                {
                    TextBlock teamblk = VisualTreeHelper.GetChild(brd, 0) as TextBlock;

                    if (teamblk.Text.Contains(searchText))
                    {
                        Style borderStyle = Resources["ThisIsSearchedBorder"] as Style;

                        brd.Style = borderStyle;
                        teamblk.Foreground = new SolidColorBrush(Colors.White);
                    }
                }
            }


            int children = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < children; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i);
                SetTeamColors(child, searchText);
            }

        }

        private void dg_Calibration_LayoutUpdated(object sender, EventArgs e)
        {
            SetTeamColors(dg_Calibration, "не годен");
        }
    }

}
