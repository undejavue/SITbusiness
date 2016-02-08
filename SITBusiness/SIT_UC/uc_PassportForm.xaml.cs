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
using System.ComponentModel.DataAnnotations;
using SITBusiness.WS_Link;
using SITBusiness.Views;
using SITBusiness.Assets;
using SITBusiness;
using SIT_classLib;



namespace SITBusiness
{
    // mode = 0 for view current Passport 
    // mode = 1 for update current Passport
    // mode = 2 for insert new Passport

    public partial class uc_PassportForm : UserControl
    {
        public char ucMode = 'R';
        public int? ucPID = 0;

        public int ucResult = 0; // 1 - update complete, 2 - insert complete

        public wsPassportExtended Ex_Pass = new wsPassportExtended();
        public Node globalTreeModels = new Node();

        public InfoWindow resultChildWindow = new InfoWindow("", "");

        public string globalPath;

        public string reportPath = "http://localhost:8080/Report.aspx?PID=";

        public WServiceClient wc = new WServiceClient();
        //wsPassportType PassportClassic = new wsPassportType();

        ObservableCollection<wsSimpleItem> globalListModels = new ObservableCollection<wsSimpleItem>();
        ObservableCollection<wsSimpleItem> globalListSubModels = new ObservableCollection<wsSimpleItem>();
        ObservableCollection<wsSimpleItem> globalListMods = new ObservableCollection<wsSimpleItem>();

        SITcode ScoDe = new SITcode();
        
        public uc_PassportForm() 
        {
            InitializeComponent();

            grid_Passport_EX.DataContext = new wsPassportExtended();

            Loaded += new RoutedEventHandler(uc_PassportForm_Loaded);  
        }

        public void uc_Configure(char _mode = 'R', int _PID = 0)
        {
            ucMode = _mode;
            ucPID = _PID;
            //reportPath = "http://localhost:8080/Report.aspx?report=Report_PassportIU&PID=" + ucPID;

            reportPath = String.Format("{0}report=Report_PassportIU&PID={1}", SIT_Report.ReportPageModel.GetBaseAddress(), ucPID.ToString());

            txt_reportPath.Text = reportPath;
            switchUCmode(ucMode); 
        }

        void uc_PassportForm_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectPassport_EXCompleted += new EventHandler<ws_selectPassport_EXCompletedEventArgs>(wc_ws_selectPassport_EXCompleted);
            wc.ws_checkPassportProducerCompleted += new EventHandler<ws_checkPassportProducerCompletedEventArgs>(wc_ws_checkPassportProducerCompleted);
            wc.ws_updatePassportCompleted += new EventHandler<ws_updatePassportCompletedEventArgs>(wc_ws_updatePassportCompleted);
            wc.ws_insertPassportCompleted += new EventHandler<ws_insertPassportCompletedEventArgs>(wc_ws_insertPassportCompleted);
            
        }


/********************************************* HANDLERS *******************************************************************/ 

        // --- Check ProducerType data before insert/update Passport
        void wc_ws_checkPassportProducerCompleted(object sender, ws_checkPassportProducerCompletedEventArgs e)
        {
            int i = e.OpValue;
            string s = e.OpStatus;

            if (e.Result == 1)
            {
                switch (ucMode)
                {
                    case 'U': // mode_U
                        wc.ws_updatePassportAsync(Ex_Pass);
                        break;
                    case 'W': // mode_W
                        wc.ws_insertPassportAsync(Ex_Pass, (int)ucPID);
                        break;
                }
            }
            else if (e.OpValue == 50017)
            {
                InfoWindow w = new InfoWindow(Messages.m_Warning, e.OpStatus.ToString());
                w.Show();
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_DB, e.OpStatus.ToString());
                w.Show();
            }
        }

        // --- Update Passport
        void wc_ws_updatePassportCompleted(object sender, ws_updatePassportCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                ucResult = 1;
                ucPID = Ex_Pass.DevID;
                resultChildWindow.init(Messages.m_Message, Messages.m_PassportUpdated);
                resultChildWindow.Show();
                resultChildWindow.Closed += new EventHandler(resultChildWindow_Closed);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }
        }

        void resultChildWindow_Closed(object sender, EventArgs e)
        {
            switchUCmode('R');
        }

        // --- Insert Passport
        void wc_ws_insertPassportCompleted(object sender, ws_insertPassportCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                ucResult = 2;
                resultChildWindow.init(Messages.m_Message, Messages.m_PassportInserted);
                resultChildWindow.Show();
                //resultChildWindow.Closed +=new EventHandler(resultChildWindow_Closed);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_INSERT, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_selectPassport_EXCompleted(object sender, ws_selectPassport_EXCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Ex_Pass = e.Result;

                grid_Passport_EX.DataContext = Ex_Pass;

                DBTree DBTREEmodels = new DBTree();

                DBTREEmodels.DBDATA = e.Result.tbl_Models;

                Node treeModels = new Node(DBTREEmodels.GetAnyNodeinDB);

                globalTreeModels = treeModels;
                globalListModels = globalTreeModels.getListByLevel(1, null);
                cbox_ModelsList.ItemsSource = globalListModels;

                //cbox_ProducersList.ItemsSource = Ex_Pass.list_Producers;
                //auto_DevDescrRU.ItemsSource = Ex_Pass.helper_DevDescr;

                if (cbox_ModelsList.Items.Count > 0)
                {
                    cbox_ModelsList.SelectedIndex = ScoDe.SetBoxIndex_wsSimpleItem(cbox_ModelsList, Ex_Pass.DevModelID);
                }

                if (cbox_ProducersList.Items.Count > 0)
                {
                    cbox_ProducersList.SelectedIndex = ScoDe.SetBoxIndex_wsProducerType(cbox_ProducersList, Ex_Pass.ProducerID);
                }
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }            
        }


/********************************************* METODS *******************************************************************/
        // --- Configure UserControl Template
        void setUCtemplateByMode(char m)
        {
            switch (m)
            {
                case 'R':     // 0 = mode_R
                    ucModeSwitcher.IsReadOnly = true;
                    ucModeSwitcher.Text = Messages.m_modeR;
                    ucModeU_btn.Visibility = Visibility.Visible;
                    ucModeU_btn.IsEnabled = true;
                    check_newModel.IsChecked = true;
                    check_newProducer.IsChecked = true;
                    break;
                case 'U':     // 1 = mode_U
                    ucModeSwitcher.IsReadOnly = false;
                    ucModeSwitcher.Text = Messages.m_modeU;
                    ucModeU_btn.Visibility = Visibility.Collapsed;
                    check_oldModel.IsChecked = true;
                    check_oldProducer.IsChecked = true;
                    break;
                case 'W':     // 2 = mode_W
                    ucModeSwitcher.IsReadOnly = false;
                    ucModeSwitcher.Text = Messages.m_modeW;
                    ucModeU_btn.Visibility = Visibility.Collapsed;
                    check_oldModel.IsChecked = true;
                    check_oldProducer.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        // --- Switch mode of UserControl
        void switchUCmode(char mode)
        {
            setUCtemplateByMode(mode); // Set UserControl mode
            switch (mode)
            {
                case 'R':     // mode = 0 for view current Passport  mode_R
                    wc.ws_selectPassport_EXAsync(ucPID);
                    modeUW_panel.Visibility = Visibility.Collapsed;
                    break;
                case 'U':     // mode = 1 for update current Passport mode_U
                    wc.ws_selectPassport_EXAsync(ucPID);
                    modeUW_panel.Visibility = Visibility.Visible;
                    break;
                case 'W':     // mode = 2 for insert new Passport mode_W
                    wc.ws_selectPassport_EXAsync(null);
                    modeUW_panel.Visibility = Visibility.Visible;
                    break;
                default:
                    grid_Passport_EX.DataContext = new wsPassportExtended();
                    break;
            }
        }


        // --- Submit form
        private void btn_okUW_Click(object sender, RoutedEventArgs e)
        {
            bool t = validator.HasErrors;

            Ex_Pass = grid_Passport_EX.DataContext as wsPassportExtended;
            
            if (check_newProducer.IsChecked == true)
            {
                wsProducerType producer = Ex_Pass.extractProducerType();
                wc.ws_checkPassportProducerAsync(producer);
            }
            else
            {
                Ex_Pass.ProducerID = (cbox_ProducersList.SelectedItem as wsProducerType).ProducerID;
                //Ex_Pass.downgradeToDataOnly();
                switch (ucMode)
                {
                    case 'U': // mode_U
                        wc.ws_updatePassportAsync(Ex_Pass);
                        break;
                    case 'W': // mode_W
                        wc.ws_insertPassportAsync(Ex_Pass, (int)ucPID);
                        break;
                }
            }
        }

        // Show floating window with tree of Places for select Place
        private void btn_show_tree_Places_Click(object sender, RoutedEventArgs e)
        {
            cwnd_selectPlace w = new cwnd_selectPlace();
            w.Show();
            w.Closed += new EventHandler(w_selectPlace_Closed);
        }

        // ChildWindow closed handler
        void w_selectPlace_Closed(object sender, EventArgs e)
        {
            cwnd_selectPlace tmpwindow = sender as cwnd_selectPlace;
            if (tmpwindow.DialogResult == true)
            {
                Ex_Pass.DevPlaceID = tmpwindow.selectedItem.ID;
                Ex_Pass.DevPlacePath = tmpwindow.selectedItem.Description;
            }
        }


        private void cbox_ModelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wsSimpleItem si = cbox_ModelsList.SelectedItem as wsSimpleItem;
            
            if (si != null)
            {
                int id = (int)si.ID;
                globalListSubModels = globalTreeModels.getListByLevel(2, id);
                cbox_SubModelsList.ItemsSource = globalListSubModels;
                if (cbox_SubModelsList.Items.Count > 0)
                {
                    cbox_SubModelsList.SelectedIndex = ScoDe.SetBoxIndex_wsSimpleItem(cbox_SubModelsList, Ex_Pass.DevSubModelID);
                }
                Ex_Pass.DevModel = si.Description;
            }
        }

        private void cbox_SubModelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wsSimpleItem si = cbox_SubModelsList.SelectedItem as wsSimpleItem;

            if (si != null)
            {
                int id = (int)si.ID;
                globalListMods = globalTreeModels.getListByLevel(3, id);
                cbox_ModsList.ItemsSource = globalListMods;
                if (cbox_ModsList.Items.Count > 0)
                {
                    cbox_ModsList.SelectedIndex = ScoDe.SetBoxIndex_wsSimpleItem(cbox_ModsList, Ex_Pass.DevModificationID);
                }
                Ex_Pass.DevSubModel = si.Description;
            }
        }

        private void cbox_ModsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             wsSimpleItem si = cbox_ModsList.SelectedItem as wsSimpleItem;

             if (si != null)
             {
                 Ex_Pass.DevModification = si.Description;
             }
        }
        
        private void check_newProducer_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbox_ProducersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (check_oldProducer.IsChecked == true)
            {
                if (cbox_ProducersList.SelectedItem != null)
                {
                    Ex_Pass.insertProducerType(cbox_ProducersList.SelectedItem as wsProducerType);
                }
            }
        }

        private void ucModeU_btn_Click(object sender, RoutedEventArgs e)
        {
            ucMode = 'U';
            switchUCmode(ucMode);
            ucModeU_btn.IsEnabled = false;
        }

        private void btn_cancelUW_Click(object sender, RoutedEventArgs e)
        {
            if (ucMode == 1)
            {
                switchUCmode('R');
                ucModeU_btn.IsEnabled = true;
                check_newProducer.IsChecked = true;
                check_oldModel.IsChecked = true;
            }
        }



        private void grid_Passport_EX_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            //if (e.Action == ValidationErrorEventAction.Added)
            //{
            //    //change control's background to yellow
            //    (e.OriginalSource as Control).Background = new SolidColorBrush(Colors.Yellow);

            //    //Tooltip shows the error message
            //    ToolTipService.SetToolTip((e.OriginalSource as TextBox), e.Error.Exception.Message);

            //}

            //if (e.Action == ValidationErrorEventAction.Removed)
            //{
            //    //change control's background to white
            //    (e.OriginalSource as Control).Background = new SolidColorBrush(Colors.White);

            //    //Remove tooltip
            //    ToolTipService.SetToolTip((e.OriginalSource as TextBox), null);
            //}

            //btn_okUW.IsEnabled = !validator.HasErrors;
        }

        private void check_newModel_Checked(object sender, RoutedEventArgs e)
        {
            if (Ex_Pass.IsCalibrated == true)
            {
                if (ucMode > 0)
                txt_Calibration.Visibility = Visibility.Visible;
            }
        }

        private void check_oldModel_Checked(object sender, RoutedEventArgs e)
        {
            txt_Calibration.Visibility = Visibility.Collapsed;
        }

        private void btn_PassPrev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_PassNext_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
