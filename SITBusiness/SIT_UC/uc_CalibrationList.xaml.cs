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
using SIT_classLib;
using SITBusiness.Views;
using SITBusiness.WS_Link;


namespace SITBusiness
{
    public partial class uc_CalibrationList : UserControl
    {
        private WServiceClient wc = new WServiceClient();
        private PagedCollectionView c_collection_next;
        private PagedCollectionView c_collection_missed; 
        private PagedCollectionView c_collection_not;

        public uc_CalibrationList()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(uc_CalibrationList_Loaded);

            txt_reportPath.Text = String.Format("{0}report=Report_Calibrations", SIT_Report.ReportPageModel.GetBaseAddress());
        }

        void uc_CalibrationList_Loaded(object sender, RoutedEventArgs e)
        {
            
            wc.ws_selectNextCalibrationsCompleted += new EventHandler<ws_selectNextCalibrationsCompletedEventArgs>(wc_ws_selectNextCalibrationsCompleted);
            wc.ws_selectNextCalibrationsAsync();
        }

        void wc_ws_selectNextCalibrationsCompleted(object sender, ws_selectNextCalibrationsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                c_collection_next = new PagedCollectionView(e.Result);

                IEnumerable<wsCalibration> next = from item in e.Result where item.plannedDate > DateTime.Now select item;
                IEnumerable<wsCalibration> missed = from item in e.Result where item.plannedDate <= DateTime.Now select item;
                IEnumerable<wsCalibration> not = from item in e.Result where item.plannedDate == null select item;

                c_collection_next = new PagedCollectionView(next);
                c_collection_missed = new PagedCollectionView(missed);
                c_collection_not = new PagedCollectionView(not);

                dg_NextWeek.ItemsSource = c_collection_next;
                dg_Missed.ItemsSource = c_collection_missed;
                dg_Not.ItemsSource = c_collection_not;

                txt_nextCaption.Text = "Период поверки устройств истекает в ближайшую неделю (список на дату: "+ DateTime.Now.Date + ")";
                txt_missedCaption.Text = "Поверка просрочена (список на дату: " + DateTime.Now.Date + ")";
                txt_notCaption.Text = "Отсутствуют сведения  поверке (список на дату: " + DateTime.Now.Date + ")"; 
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }   
        }

        public void uc_configure(ObservableCollection<wsCalibration> c_list)
        {
            
        }


        private void btn_Calibrate1_Click(object sender, RoutedEventArgs e)
        {
            wsCalibration item = dg_NextWeek.SelectedItem as wsCalibration;

            //Parent p = (sender as Button).Parent;

            cwnd_RunCalibration w = new cwnd_RunCalibration((int)item.ID);
            w.Show();
            w.Closed += new EventHandler(w_Closed);
        }

        private void btn_Calibrate2_Click(object sender, RoutedEventArgs e)
        {
            wsCalibration item = dg_Missed.SelectedItem as wsCalibration;

            cwnd_RunCalibration w = new cwnd_RunCalibration((int)item.ID);
            w.Show();
            w.Closed += new EventHandler(w_Closed);
        }


        private void btn_Calibrate3_Click(object sender, RoutedEventArgs e)
        {
            wsCalibration item = dg_Not.SelectedItem as wsCalibration;

            cwnd_RunCalibration w = new cwnd_RunCalibration((int)item.ID);
            w.Show();
            w.Closed += new EventHandler(w_Closed);
        }

        void w_Closed(object sender, EventArgs e)
        {
            if ((sender as cwnd_RunCalibration).DialogResult == true)
            {
                wc.ws_selectNextCalibrationsAsync();               
            }
        }

        private void dg_CalibrationList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //wsCalibration item = e.Row.DataContext as wsCalibration;

            //SolidColorBrush brush1 = new SolidColorBrush(Colors.Red);
            //SolidColorBrush brush2 = new SolidColorBrush(Colors.White);

            //dg_CalibrationList.AlternatingRowBackground = null;

            //if ((item.plannedDate != null) & (item.plannedDate <= DateTime.Now) )
            //{
            //    e.Row.Background = brush1;
            //    e.Row.Foreground = brush2;
            //}
        }
    }
}
