using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SITBusiness.Views;
using SITBusiness.Assets;
using SIT_classLib;

namespace SITBusiness
{

    // mode = 0 for view 
    // mode = 1 for update 
    // mode = 2 for insert 

    public partial class uc_ProducerType : UserControl
    {
        public int ucMode = 0;
        public int? ucPID = 0;

        public int ucResult = 0; // 1 - update complete, 2 - insert complete

        public InfoWindow resultChildWindow = new InfoWindow("", "");

        WServiceClient wc = new WServiceClient();
        SITcode ScoDe = new SITcode();

        wsProducerType globalProducer = new wsProducerType();
        
        
        public uc_ProducerType()
        {
            InitializeComponent();
            grid_ProducerType.DataContext = new wsProducerType();
            
        }

        public void ucConfigure(int _mode, int? _PID)
        {
            ucMode = _mode;
            ucPID = _PID;
            switchUCmode(ucMode);
            Loaded += new RoutedEventHandler(uc_ProducerType_Loaded);
        }


        void uc_ProducerType_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectProducersDataCompleted += new EventHandler<ws_selectProducersDataCompletedEventArgs>(wc_ws_selectProducersDataCompleted);
            wc.ws_updateProducerCompleted += new EventHandler<ws_updateProducerCompletedEventArgs>(wc_ws_updateProducerCompleted);
            wc.ws_insertProducerCompleted += new EventHandler<ws_insertProducerCompletedEventArgs>(wc_ws_insertProducerCompleted);
        }

        void wc_ws_insertProducerCompleted(object sender, ws_insertProducerCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                ucResult = 2;
                ucPID = globalProducer.ProducerID;
                resultChildWindow.init(Messages.m_Message, Messages.m_ProducerInserted);
                resultChildWindow.Show();
                resultChildWindow.Closed += new EventHandler(resultChildWindowW_Closed);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }

        }

        void wc_ws_updateProducerCompleted(object sender, ws_updateProducerCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                ucResult = 1;
                ucPID = globalProducer.ProducerID;
                resultChildWindow.init(Messages.m_Message, Messages.m_ProducerUpdated);
                resultChildWindow.Show();
                resultChildWindow.Closed += new EventHandler(resultChildWindowU_Closed);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }
        }

        void resultChildWindowU_Closed(object sender, EventArgs e)
        {
            switchUCmode(0);
        }

        void resultChildWindowW_Closed(object sender, EventArgs e)
        {
            
        }

       
        void wc_ws_selectProducersDataCompleted(object sender, ws_selectProducersDataCompletedEventArgs e)
        {
            if (e.Result != null)
            {

                globalProducer = e.Result;
                grid_ProducerType.DataContext = globalProducer;

            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        /********************************************* METODS *******************************************************************/
        // --- Configure UserControl Template
        void setUCtemplateByMode(int m)
        {
            switch (m)
            {
                case 0:     // 0 = mode_R
                    ucModeSwitcher.IsReadOnly = true;
                    ucModeSwitcher.Text = Messages.m_modeR;
                    ucModeU_btn.Visibility = Visibility.Visible;
                    ucModeU_btn.IsEnabled = true;
                    break;
                case 1:     // 1 = mode_U
                    ucModeSwitcher.IsReadOnly = false;
                    ucModeSwitcher.Text = Messages.m_modeU;
                    ucModeU_btn.Visibility = Visibility.Collapsed;
                    break;
                case 2:     // 2 = mode_W
                    ucModeSwitcher.IsReadOnly = false;
                    ucModeSwitcher.Text = Messages.m_modeW;
                    ucModeU_btn.Visibility = Visibility.Collapsed;
                    break;
                default:

                    break;
            }
        }

        // --- Switch mode of UserControl
        void switchUCmode(int mode)
        {
            setUCtemplateByMode(mode); // Set UserControl mode

            switch (mode)
            {
                case 0:     // mode = 0 for view 
                    wc.ws_selectProducersDataAsync((int)ucPID);
                    modeUW_panel.Visibility = Visibility.Collapsed;
                    break;
                case 1:     // mode = 1 for update 
                    wc.ws_selectProducersDataAsync((int)ucPID);
                    modeUW_panel.Visibility = Visibility.Visible;
                    break;
                case 2:     // mode = 2 for insert 

                    grid_ProducerType.DataContext = new wsProducerType();
                    modeUW_panel.Visibility = Visibility.Visible;
                    break;
                default:
                    grid_ProducerType.DataContext = new wsProducerType();
                    break;
            }
        }

        // --- Submit form
        private void btn_okUW_Click(object sender, RoutedEventArgs e)
        {
            switch (ucMode)
            {
                case 1: // mode_U
                    wc.ws_updateProducerAsync(globalProducer);
                    break;
                case 2: // mode_W
                    globalProducer = grid_ProducerType.DataContext as wsProducerType;
                    wc.ws_insertProducerAsync(globalProducer);
                    break;
            }
           
        }

        private void ucModeU_btn_Click(object sender, RoutedEventArgs e)
        {
            ucMode = 1;
            switchUCmode(ucMode);
            ucModeU_btn.IsEnabled = false;
        }

        private void btn_cancelUW_Click(object sender, RoutedEventArgs e)
        {
            if (ucMode == 1)
            {
                switchUCmode(0);
                ucModeU_btn.IsEnabled = true;

                ucResult = 0;
            }
            else if (ucMode == 2)
            {
                ucResult = 2;
            }
        }

        private void check_newProducer_Checked(object sender, RoutedEventArgs e)
        {

        }


    }
}
