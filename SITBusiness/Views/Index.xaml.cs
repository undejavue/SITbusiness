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
using System.Windows.Media.Imaging;
//using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Globalization;
using System.Windows.Data;
using SITBusiness.WS_Link;
using SIT_classLib;

namespace SITBusiness.Views
{

    public partial class Index : Page
    {

        WServiceClient wc = new WServiceClient();
        wsBaseItem globalBI = new wsBaseItem();   

        ObservableCollection<string> globalListDevDesrc = new ObservableCollection<string>();

        SelectedNode SN = new SelectedNode();

        uc_PassportForm ucDevPassport;
        uc_NodeOptions ucNodes;
        uc_DevWorkStates ucDevWorkStates;
        uc_Calibration ucDevCalibration;
        uc_Repair ucDevRepair;
        uc_CalibrationList ucDevCalibrationList;

        //ObservableCollection<wsCalibration> globalCalibrationList = new ObservableCollection<wsCalibration>();

        SITcode ScoDe = new SITcode();
        //int globalPlaceID = 0;

        ObservableCollection<string> helperTypes;

        PagedCollectionView ItemsByType;
        PagedCollectionView ItemsByPlace;
        int DevList_PageSize = 25;

        public Index()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Page_Loaded);
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDBTreeCompleted += new EventHandler<WS_Link.ws_selectDBTreeCompletedEventArgs>(wc_ws_selectDBTreeCompleted);
            wc.ws_insertNodeCompleted += new EventHandler<WS_Link.ws_insertNodeCompletedEventArgs>(wc_ws_insertNodeCompleted);
            wc.ws_deleteTreeItemCompleted += new EventHandler<WS_Link.ws_deleteTreeItemCompletedEventArgs>(wc_ws_deleteTreeItemCompleted);              
            
            //wc.ws_selectPlacesCompleted += new EventHandler<WS_Link.ws_selectPlacesCompletedEventArgs>(wc_ws_selectPlacesCompleted);

            wc.ws_selectTreeItemsListCompleted += new EventHandler<ws_selectTreeItemsListCompletedEventArgs>(wc_ws_selectTreeItemsListCompleted);
            wc.ws_selectDevListByPlaceCompleted += new EventHandler<ws_selectDevListByPlaceCompletedEventArgs>(wc_ws_selectDevListByPlaceCompleted);

            wc.ws_insertPlaceCompleted += new EventHandler<WS_Link.ws_insertPlaceCompletedEventArgs>(wc_ws_insertPlaceCompleted);
            wc.ws_deletePlaceCompleted += new EventHandler<WS_Link.ws_deletePlaceCompletedEventArgs>(wc_ws_deletePlaceCompleted);

            wc.ws_selectDBTreePlacesCompleted += new EventHandler<ws_selectDBTreePlacesCompletedEventArgs>(wc_ws_selectDBTreePlacesCompleted);
            wc.ws_moveItemsCompleted += new EventHandler<ws_moveItemsCompletedEventArgs>(wc_ws_moveItemsCompleted);

            wc.ws_helperTypesListCompleted += new EventHandler<ws_helperTypesListCompletedEventArgs>(wc_ws_helperTypesListCompleted);

            //wc.ws_selectNextCalibrationsCompleted += new EventHandler<ws_selectNextCalibrationsCompletedEventArgs>(wc_ws_selectNextCalibrationsCompleted);
            //wc.ws_selectNextCalibrationsAsync();

            wc.ws_selectDBTreeAsync(0);

            wc.ws_helperTypesListAsync();           
           
        }

/********************************************* ОБРАБОТЧИКИ   ***************************************************************/
        // Общее дерево элементов
        void wc_ws_selectDBTreeCompleted(object sender, WS_Link.ws_selectDBTreeCompletedEventArgs e)
        {
            if (e.Result != null) 
            {
                DBTree DBTREE = new DBTree();
                DBTREE.DBDATA = e.Result.DBDATA;

                Node NNODE = new Node(DBTREE.GetAnyNodeinDB);

                if (tabControl_Tree.SelectedIndex == 0)
                {
                    tree_Devices.ItemsSource = NNODE.chldlist;
                    

                    tree_Devices.UpdateLayout();
                    if (tree_Devices.Items.Count > 0)
                    {
                        TreeViewItem item = tree_Devices.ItemContainerGenerator.ContainerFromItem(tree_Devices.Items[0]) as TreeViewItem;
                        if (item != null)
                        {
                            item.IsSelected = true;
                        }
                    }
                }
                if (tabControl_Tree.SelectedIndex == 1)
                {
                    tree_Places.ItemsSource = NNODE.chldlist;

                    tree_Places.UpdateLayout();
                    if (tree_Places.Items.Count > 0)
                    {
                        TreeViewItem item = tree_Places.ItemContainerGenerator.ContainerFromItem(tree_Places.Items[0]) as TreeViewItem;
                        if (item != null)
                        {
                            item.IsSelected = true;
                        }
                    }
                }
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

//***************************************** DEVICES ***********************************************************************

        //---- Вставка ветви дерева
        void wc_ws_insertNodeCompleted(object sender, WS_Link.ws_insertNodeCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                wc.ws_selectDBTreeAsync(0);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_INSERT, e.OpStatus.ToString());
                w.Show();
            }
        }

        //---- Удаление элемента(ов)
        void wc_ws_deleteTreeItemCompleted(object sender, WS_Link.ws_deleteTreeItemCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                wc.ws_selectTreeItemsListAsync((int)(tree_Devices.SelectedItem as Node).bi.ID);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_DELETE, e.OpStatus.ToString());
                w.Show();
            }
        }

        //---- Перемещение элемента(ов)
        void wc_ws_moveItemsCompleted(object sender, ws_moveItemsCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                wc.ws_selectDBTreeAsync(0);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_INSERT, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_helperTypesListCompleted(object sender, ws_helperTypesListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                helperTypes = e.Result;
                auto_types.ItemsSource = helperTypes;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        //---- Список элементов по типу
        void wc_ws_selectTreeItemsListCompleted(object sender, ws_selectTreeItemsListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                ItemsByType = new PagedCollectionView(e.Result);
                ItemsByType.PageSize = DevList_PageSize;
                dp_Pager.Source = ItemsByType;
                dg_DevList.ItemsSource = ItemsByType;


                tabControl_Device.SelectedIndex = tabControl_Device.Items.IndexOf(tab_DevList);

                txt_NodeName.Text = e.Path;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        //---- Список элементов по месту
        void wc_ws_selectDevListByPlaceCompleted(object sender, ws_selectDevListByPlaceCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                ItemsByPlace = new PagedCollectionView(e.Result);
                ItemsByPlace.PageSize = DevList_PageSize;
                dp_Pager.Source = ItemsByPlace;
                dg_DevList.ItemsSource = ItemsByPlace;

                txt_NodeName.Text = e.Path;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

//***************************************** PLACES ***********************************************************************

        // Получение дерева Places
        void wc_ws_selectDBTreePlacesCompleted(object sender, ws_selectDBTreePlacesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DBTree DBTREE = new DBTree();
                DBTREE.DBDATA = e.Result.DBDATA;

                Node NNODE = new Node(DBTREE.GetAnyNodeinDB);
                tree_Places.ItemsSource = NNODE.chldlist;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        //Удалить Place
        void wc_ws_deletePlaceCompleted(object sender, WS_Link.ws_deletePlaceCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                wc.ws_selectDBTreeAsync(1);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_DELETE, e.OpStatus.ToString());
                w.Show();
            }
        }

        // Вставить Place
        void wc_ws_insertPlaceCompleted(object sender, WS_Link.ws_insertPlaceCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                wc.ws_selectDBTreeAsync(1);
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_INSERT, e.OpStatus.ToString());
                w.Show();
            }
        }

//**************** СОБЫТИЯ ЭЛЕМЕНТОВ СТРАНИЦЫ **********************************************************

        //---- Событие выбора вкладки TabControl_Tree
        private void tabControl_Tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (btn_show_insertForm != null)
            {
                switch ((sender as TabControl).SelectedIndex)
                {
                    case 0:
                        btn_show_insertForm.IsEnabled = true;
                        
                        break;
                    case 1:
                        btn_show_insertForm.IsEnabled = false;

                        break;
                    default:
                        btn_show_insertForm.IsEnabled = false;
                        break;
                }
            }

            wc.ws_selectDBTreeAsync((sender as TabControl).SelectedIndex); // 0 - tree_Devices, 1 - tree_Places
        }

        //---- Событие выбора ветки дерева Devices
        private void tree_Devices_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            show_DevicesForm();

            if ( ((sender as TreeView).SelectedItem as Node) != null)
            {
                SN.SetNode((sender as TreeView).SelectedItem as Node);

                reaction_OnTreeSelection(0, SN.SelNode);
            }
        }

        //---- События выбора ветки дерева Places
        private void tree_Places_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            show_DevicesForm();

            if (((sender as TreeView).SelectedItem as Node) != null)
            {
                SN.SetNode((sender as TreeView).SelectedItem as Node);

                reaction_OnTreeSelection(1, SN.SelNode);
            }
        }

        //---- События табов девайсов
        private void tabControl_Device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( (tabControl_Device != null) & (SN != null) )
            {
                switch (SN.IsType)
                {
                    case true:   // TYPE
                        if (tab_DevList.IsSelected)
                        {
                            show_DevList(tabControl_Tree.SelectedIndex,SN.id);
                        }

                        if (tab_DevServiceList.IsSelected)
                        {
                            show_DevServiceList();
                        }

                        if (tab_NodeOptions.IsSelected)
                        {
                            show_NodeOptions(SN.baseItem);
                        }

                        if (tab_DevPassport.IsSelected)
                        {
                            show_Passport('R', SN.id);
                        }

                        if (tab_DevService.IsSelected)
                        {
                            show_DevService(SN.id);
                        }

                        if (tab_DevStates.IsSelected)
                        {
                            show_DevWorkStates(SN.id,SN.name);
                        }
                        break;
                    case false: // DEVICE
                        if ((tab_DevList.IsSelected) & (SN.HasParents))
                        {
                            show_DevList(tabControl_Tree.SelectedIndex, SN.parentID);
                        }

                        if (tab_DevServiceList.IsSelected)
                        {
                            show_DevServiceList();
                        }

                        if ((tab_NodeOptions.IsSelected) & (SN.HasParents))
                        {
                           show_NodeOptions(SN.parentBaseItem);
                        }

                        if (tab_DevService.IsSelected)
                        {
                            show_DevService(SN.id);
                        }

                        if (tab_DevPassport.IsSelected)
                        {
                            show_Passport('R', SN.id);
                        }

                        if (tab_DevStates.IsSelected)
                        {
                            show_DevWorkStates(SN.id,SN.name);
                        }
                        break;
                }       
            }
        }

//************** ОПЕРАЦИИ СО СПИСКОМ ЭЛЕМЕНТОВ ***************************************************
        
        //---- Выбор строки в сетке элементов
        public void dg_DevList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SN.SetNode((sender as DataGrid).SelectedItem as wsBaseItem);
            tab_DevPassport.IsEnabled = true;

            //if (tab_DevPassport.IsSelected) show_Passport('R', SN.id);
        }

        //---- Кнопка "паспорт" в сетке элементов
        private void btn_devListShowPassport_Click(object sender, RoutedEventArgs e)
        {
            if (SN!= null)
            {
                tabControl_Device.SelectedIndex = tabControl_Device.Items.IndexOf(tab_DevPassport);
                show_Passport('R', SN.id); // Show passport form in Read mode
            }
        }

        //---- Кнопка "Поверка/ТО" в сетке элементов
        private void btn_devListShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SN != null)
            {
                tabControl_Device.SelectedIndex = tabControl_Device.Items.IndexOf(tab_DevService);
                show_DevService(SN.id);
            }
        }

        //---- Удаление элемента из листбокса
        private void btn_deleteItem_Click(object sender, RoutedEventArgs e)
        {
            //CaseSelNode();
            PagedCollectionView blist = dg_DevList.ItemsSource as PagedCollectionView;
            ObservableCollection<int> deleteList = new ObservableCollection<int>();

            foreach (wsBaseItem item in blist)
            {
                if (item.IsChecked)
                {
                    int i = (int)item.ID;
                    deleteList.Add(i);
                }
            }

            wc.ws_deleteTreeItemAsync(deleteList);
        }

        //---- Перемещение элемента в другой раздел
        private void btn_moveItem_Click(object sender, RoutedEventArgs e)
        {
            string xmlString = "<root>";
            PagedCollectionView blist = dg_DevList.ItemsSource as PagedCollectionView;
            ObservableCollection<int> deleteList = new ObservableCollection<int>();

            foreach (wsBaseItem item in blist)
            {
                if (item.IsChecked)
                {
                    int i = (int)item.ID;
                    xmlString += "<dev id=\" " + i + "\" />";
                }
            }
            xmlString += "</root>";


            cwnd_treeTypes w = new cwnd_treeTypes();
            w.storedString = xmlString;
            w.Show();
            w.Closed += new EventHandler(w_selectTypeClosed);

        }

        //---- Handle childwindow 
        private void w_selectTypeClosed(object sender, EventArgs e)
        {
            if ((sender as cwnd_treeTypes).DialogResult == true)
            {
                wsSimpleItem sitem = (sender as cwnd_treeTypes).selectedItem;
                string s = (sender as cwnd_treeTypes).storedString;

                wc.ws_moveItemsAsync((int)sitem.ID, s);
            }
        }

//*************** ОБРАБОТКА КНОПОК ******************************************

        //---- Button shows InsertDevice Form
        private void btn_show_insertForm_Click(object sender, RoutedEventArgs e)
        {
            
            if (tree_Devices.SelectedItem != null)
            {
                string s;
                int typeID;

                if (SN.IsType)
                {
                    s = SN.name;
                    typeID = SN.id;
                }
                else
                {
                    s = SN.parentName;
                    typeID = SN.parentID;
                }

                show_InsertForm();
                show_Passport('W', typeID);
                tab_InsertDevice.Header = "Добавить в раздел \"" + s + "\"";
            }
            else
            {
                InfoWindow iw = new InfoWindow(Messages.m_Warning, Messages.m_treeItemIsNotSelected);
                iw.Show();
            }
        }

        //---- Открыть "Добавление раздела дерева"
        private void btn_Node_2_Click(object sender, RoutedEventArgs e)
        {

            if (btn_Node_2.IsChecked == true) stp_add_Node_2.Visibility = System.Windows.Visibility.Visible;
            else stp_add_Node_2.Visibility = System.Windows.Visibility.Collapsed;
           
            if (SN.SelNode != null) 
            {
                box_IsRootItem.IsChecked = false;
            }
            else
            {
                box_IsRootItem.IsChecked = true;
            }
        }

        //---- Добавление раздела дерева
        private void btn_add_Node_2_Click(object sender, RoutedEventArgs e)
        {
            
            int? insID = null;
            if (box_IsRootItem.IsChecked == false) insID = SN.id;

            switch (tabControl_Tree.SelectedIndex)
            {
                case 0: // Вставка в дерево Devices
                    wc.ws_insertNodeAsync(auto_types.Text.ToString(), insID, (bool)box_IsCalibrated.IsChecked);
                    break;
                case 1: // Вставка в дерево Places
                    wc.ws_insertPlaceAsync(auto_types.Text.ToString(), insID);
                    break;
            }                       
        }

        //---- Добавление раздела дерева /отмена
        private void btn_сancel_Node_2_Click(object sender, RoutedEventArgs e)
        {
            stp_add_Node_2.Visibility = System.Windows.Visibility.Collapsed;
            btn_Node_2.IsChecked = false;
        }     
         
//************* МЕТОДЫ КЛАССА СТРАНИЦЫ *********************************************
        
        //---- Показать только форму "Devices"
        private void show_DevicesForm()
        {
            tabControl_InsertDevice.Visibility = Visibility.Collapsed;
            tabControl_Device.Visibility = Visibility.Visible;
        }

        //---- Показать только форму добавления устройства
        private void show_InsertForm()
        {
            tabControl_InsertDevice.Visibility = Visibility.Visible;
            tabControl_Device.Visibility = Visibility.Collapsed;
        }

        //---- Свернуть все формы
        private void collapse_Forms()
        {
            tabControl_InsertDevice.Visibility = Visibility.Collapsed;
            tabControl_Device.Visibility = Visibility.Collapsed;
        }

        //---- Показать паспорт в режиме просмотра
        private void show_Passport(char mode, int pID)
        {
            if (ucDevPassport == null)
                ucDevPassport = new uc_PassportForm();

            ucDevPassport.uc_Configure(mode, pID);

            switch (mode)
            {
                case 'W':
                    panel_DevPassportUR.Children.Clear();

                    if (!panel_DevPassportW.Children.Contains(ucDevPassport))
                    {
                        panel_DevPassportW.Children.Add(ucDevPassport);
                    }
                    break;
                default:
                    panel_DevPassportW.Children.Clear();

                    if (!panel_DevPassportUR.Children.Contains(ucDevPassport))
                    {
                        panel_DevPassportUR.Children.Add(ucDevPassport);
                    }
                    break;
            }

            ucDevPassport.btn_cancelUW.Click += new RoutedEventHandler(ucDevPassport_btn_cancelUW_Click);
            ucDevPassport.resultChildWindow.Closed += new EventHandler(ucDevPassport_resultChildWindow_Closed);
            ucDevPassport.btn_PassPrev.Click += new RoutedEventHandler(ucDevPassport_btn_PassPrev_Click);
            ucDevPassport.btn_PassNext.Click += new RoutedEventHandler(ucDevPassport_btn_PassNext_Click);
        }

        void ucDevPassport_btn_PassNext_Click(object sender, RoutedEventArgs e)
        {
            dg_DevList.SelectedIndex++;
        }

        void ucDevPassport_btn_PassPrev_Click(object sender, RoutedEventArgs e)
        {
            dg_DevList.SelectedIndex--;
        }

        //---- Обработка события закрытия окна "Паспорт добавлен/обновлен"
        void ucDevPassport_resultChildWindow_Closed(object sender, EventArgs e)
        {
            
        }

        //---- Обработка нажатия кнопки "Отмена" в форме паспорта
        void ucDevPassport_btn_cancelUW_Click(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDBTreeAsync(0);
        }

        private bool check_IsCalibration(Node N)
        {
            bool condition = false;

            if (N.bi.IsType == true)
            {
                condition = N.bi.IsCalibrated;
            }
            else if (N.bi.IsType == false)
            {
                condition = N.ParentNode.bi.IsCalibrated;
            }

            return condition;
        }

        //---- Загрузить блок "Обслуживание" - uс_DevService
        private void show_DevService(int pID)
        {
            if (check_IsCalibration(SN.SelNode))          // Устройство подлежит поверке
            {
                tab_Calibrations.IsEnabled = true;
                tabControl_Service.SelectedIndex = tabControl_Service.Items.IndexOf(tab_Calibrations);
                tab_Repairs.IsEnabled = false;

                if (ucDevCalibration == null)
                    ucDevCalibration = new uc_Calibration();

                ucDevCalibration.uc_Configure(pID);

                if (!panel_Calibrations.Children.Contains(ucDevCalibration))
                    panel_Calibrations.Children.Add(ucDevCalibration);              
            }
            else                                            // Устройство подлежит калибровке
            {
                tab_Repairs.IsEnabled = true;
                tabControl_Service.SelectedIndex = tabControl_Service.Items.IndexOf(tab_Repairs);
                tab_Calibrations.IsEnabled = false;

                if (ucDevRepair == null)
                    ucDevRepair = new uc_Repair();

                //ucDevRepair.uc_Configure(pID);

                if (!panel_Repairs.Children.Contains(ucDevRepair))
                    panel_Repairs.Children.Add(ucDevRepair);
            }

            txt_itemNameOnService.Text = SN.name;
        }

        //---- Показать состояния устройства
        private void show_DevWorkStates(int pid, string description)
        {
            if (ucDevWorkStates == null)
                ucDevWorkStates = new uc_DevWorkStates();

            ucDevWorkStates.uc_configure(pid, description);

            if (!panel_DevStates.Children.Contains(ucDevWorkStates))
                panel_DevStates.Children.Add(ucDevWorkStates);

        }

        //---- Загрузить блок "Настройки раздела/класса" - uc_NodeOptions
        private void show_NodeOptions(wsBaseItem BI)
        {
            if (ucNodes == null)
                ucNodes = new uc_NodeOptions();

            ucNodes.uc_configure(BI, tabControl_Tree.SelectedIndex);
            
            if (!panel_NodeOptions.Children.Contains(ucNodes))
                panel_NodeOptions.Children.Add(ucNodes);

            ucNodes.w.Closed += new EventHandler(ucNodes_w_Closed);
        }

        //---- Перехват закрытия окна подтверждения изменений/удаления раздела
        private void ucNodes_w_Closed(object sender, EventArgs e)
        {
            wc.ws_selectDBTreeAsync(0);
        }

        //---- Реакция на выбор элемента дерева устройств
        private void reaction_OnTreeSelection(int treeIndex, Node SelNode)
        {
            if (SelNode != null)
            {
                int id = (int)SelNode.bi.ID;
                string descr = SelNode.bi.Description;

                if (SelNode.bi.IsType == false)   // Если item есть device
                {
                    // Вкладки для девайсов
                    if (tab_DevPassport.IsSelected)
                    {
                        show_Passport('R', id);
                    }
                    else if (tab_DevService.IsSelected)
                    {
                        show_DevService(id);
                    }
                    else if (tab_DevStates.IsSelected)
                    {
                        show_DevWorkStates(id,descr);
                    }

                    // Вкладки для типов
                    else 
                    {
                        tabControl_Device.SelectedIndex = tabControl_Device.Items.IndexOf(tab_DevPassport);
                    }
                }
                else                        // Если item есть type
                {
                    // Вкладки для типов
                    if (tab_DevList.IsSelected)
                    {
                        if (treeIndex == 0)
                        {
                            wc.ws_selectTreeItemsListAsync(id);
                        }
                        else
                        {
                            wc.ws_selectDevListByPlaceAsync(id);
                        }
                    }
                    else if (tab_NodeOptions.IsSelected == true)
                    {
                        show_NodeOptions(SelNode.bi);
                    }

                    // Вкладки для девайсов
                    else
                    {
                        tabControl_Device.SelectedIndex = tabControl_Device.Items.IndexOf(tab_DevList);
                    }
                }
            }
        }

        //---- Вывод списка устройств по ID родительского раздела дерева
        private void show_DevList(int treeIndex, int id)
        {
            switch (treeIndex)
            {
                case 0:
                    wc.ws_selectTreeItemsListAsync(id);
                    break;
                case 1:
                    wc.ws_selectDevListByPlaceAsync(id);
                    break;
            }
        }

        //---- Вывод информации по обслуживанию для всех устройств/групп устройств
        private void show_DevServiceList()
        {
            if (ucDevCalibrationList == null)
                ucDevCalibrationList = new uc_CalibrationList();

            //ucDevCalibrationList.uc_configure();

            if (!panel_DevServiceList.Children.Contains(ucDevCalibrationList))
                panel_DevServiceList.Children.Add(ucDevCalibrationList);
        }

//**********************************************************************************

    } // Конец класса Index : Page

    public class SelectedNode
    {
        public Node SelNode;
        public bool IsType;
        public bool IsCalibrated;

        public bool HasParents;

        public wsBaseItem baseItem;

        public wsBaseItem parentBaseItem;

        public int id;
        public int parentID;

        public string name;
        public string parentName;

        public SelectedNode() 
        {
            HasParents = false;
        }

        public void SetNode(Node N)
        {
            if (N != null)
            {
                SelNode = N;

                if (N.bi != null)
                {
                    baseItem = N.bi;
                    id = (int)N.bi.ID;
                    name = N.bi.Description;
                    IsType = N.bi.IsType;
                    IsCalibrated = N.bi.IsCalibrated;
                }

                if (N.ParentNode.bi != null)
                {
                    HasParents = true;
                    if (N.ParentNode.bi != null)
                    {
                        parentBaseItem = N.ParentNode.bi;
                        parentID = (int)N.ParentNode.bi.ID;
                        parentName = N.ParentNode.bi.Description;
                    }
                }
                else
                {
                    HasParents = false;
                    parentBaseItem = N.bi;
                    parentID = (int)N.bi.ID;
                    parentName = N.bi.Description;
                }
            }
        }

        public void SetNode(wsBaseItem BI)
        {
                if (BI != null)
                {
                    baseItem = BI;
                    //parentID = id;
                    id = (int)BI.ID;
                    //parentName = name;
                    name = BI.Description;
                    IsType = BI.IsType;
                    IsCalibrated = BI.IsCalibrated;
                    //HasParents = false;
                }
        }
    }
}
