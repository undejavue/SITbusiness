using System;
using System.Collections.Generic;
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
using SITBusiness.WS_Link;
using SITBusiness.Views;
using SITBusiness.Assets;
using SIT_classLib;

namespace SITBusiness
{
    public partial class uc_Models : UserControl
    {
        WServiceClient wc = new WServiceClient();
        PagedCollectionView collection;
        
        public uc_Models()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(uc_Models_Loaded);
        }

        void uc_Models_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectModelsCompleted += new EventHandler<ws_selectModelsCompletedEventArgs>(wc_ws_selectModelsCompleted);
            wc.ws_selectModelsAsync();

            wc.ws_ins_upd_PeriodCompleted += new EventHandler<ws_ins_upd_PeriodCompletedEventArgs>(wc_ws_ins_upd_PeriodCompleted);
        }

        void wc_ws_ins_upd_PeriodCompleted(object sender, ws_ins_upd_PeriodCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                wc.ws_selectModelsAsync();
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_selectModelsCompleted(object sender, ws_selectModelsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                collection = new PagedCollectionView(e.Result);

                dg_Models.ItemsSource = collection;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }
        }

        private void btn_group1_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupBy = new PropertyGroupDescription("ModelName");
            collection.GroupDescriptions.Add(groupBy);
        }

        private void btn_group2_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupBy = new PropertyGroupDescription("SubModelName");
            collection.GroupDescriptions.Add(groupBy);
        }

        private void btn_group3_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupBy = new PropertyGroupDescription("SubSubModelName");
            collection.GroupDescriptions.Add(groupBy);
        }

        private void btn_cancelGroups_Click(object sender, RoutedEventArgs e)
        {
            collection.GroupDescriptions.Clear();
        }

        private void btn_collapseGroups_Click(object sender, RoutedEventArgs e)
        {
            if (collection.GroupDescriptions.Count > 0)
            {
                foreach (CollectionViewGroup group in collection.Groups)
                {
                    dg_Models.CollapseRowGroup(group, true);
                }
            }
        }

        private void btn_expandGroups_Click(object sender, RoutedEventArgs e)
        {
            if (collection.GroupDescriptions.Count > 0)
            {
                foreach (CollectionViewGroup group in collection.Groups)
                {
                    dg_Models.ExpandRowGroup(group, true);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_period_Click(object sender, RoutedEventArgs e)
        {
            wsModelItem mitem = dg_Models.SelectedItem as wsModelItem;


            wc.ws_ins_upd_PeriodAsync(mitem.ModelID, mitem.CalibrationPeriod);

        }
    }
}
