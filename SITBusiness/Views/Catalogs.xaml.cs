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
using System.Windows.Navigation;
using SITBusiness.WS_Link;
using SIT_classLib;
using SITBusiness;

namespace SITBusiness.Views
{
    public partial class Catalogs : Page
    {
        WServiceClient wc = new WServiceClient();
        wsSimpleItem simpleItem = new wsSimpleItem();
        wsProducerType LocalProducer = new wsProducerType();
        wsProducerType producerType = new wsProducerType();

        uc_ProducerType ucProducer = new uc_ProducerType();

        uc_Models ucModels = new uc_Models();

        public Catalogs()
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
            wc.ws_selectProducersListCompleted += new EventHandler<ws_selectProducersListCompletedEventArgs>(wc_ws_selectProducersListCompleted);
            
            wc.ws_checkProducerCompleted += new EventHandler<ws_checkProducerCompletedEventArgs>(wc_ws_checkProducerCompleted);
            wc.ws_deleteProducerCompleted += new EventHandler<ws_deleteProducerCompletedEventArgs>(wc_ws_deleteProducerCompleted);
           
            wc.ws_selectModelsListCompleted += new EventHandler<ws_selectModelsListCompletedEventArgs>(wc_ws_selectModelsListCompleted);

            wc.ws_selectDBTreeCompleted += new EventHandler<WS_Link.ws_selectDBTreeCompletedEventArgs>(wc_ws_selectDBTreeCompleted);

            ucProducer.btn_cancelUW.Click += new RoutedEventHandler(btn_cancelUW_Click);

            ucProducer.resultChildWindow.Closed += new EventHandler(resultChildWindow_Closed);
        }

        void resultChildWindow_Closed(object sender, EventArgs e)
        {
            if (ucProducer.ucResult == 2)
            {
                ShowStartForms();
                wc.ws_selectProducersListAsync();
                panel_Element.Children.Clear();
            }
        }

        void btn_cancelUW_Click(object sender, RoutedEventArgs e)
        {
            if (ucProducer.ucResult == 2)
            {
                ShowStartForms();
                panel_Element.Children.Clear();
            }
        }

        void wc_ws_selectDBTreeCompleted(object sender, WS_Link.ws_selectDBTreeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DBTree DBTREE = new DBTree();
                DBTREE.DBDATA = e.Result.DBDATA;

                Node NNODE = new Node(DBTREE.GetAnyNodeinDB);

                tree_Models.ItemsSource = NNODE.chldlist;

                tree_Models.UpdateLayout();
                if (tree_Models.Items.Count > 0)
                {
                    TreeViewItem item = tree_Models.ItemContainerGenerator.ContainerFromItem(tree_Models.Items[0]) as TreeViewItem;
                    if (item != null)
                    {
                        item.IsSelected = true;
                    }
                }
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_checkProducerCompleted(object sender, WS_Link.ws_checkProducerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                int r = e.OpValue;
                ObservableCollection<wsSimpleItem> slist = e.Result;
                cwnd_ResultToDBGrid cw = new cwnd_ResultToDBGrid(e.OpStatus,slist);
                cw.Show();
                cw.Closed += new EventHandler(cw_Closed);
 
            }
            else if (e.Result == null)
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_DB, e.OpStatus.ToString());
                w.Show();
            }
        }

        void cw_Closed(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                wc.ws_deleteProducerAsync((int)producerType.ProducerID);
            }
        }

        void wc_ws_deleteProducerCompleted(object sender, WS_Link.ws_deleteProducerCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                wc.ws_selectProducersListAsync();
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_DELETE, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_selectProducersListCompleted(object sender, WS_Link.ws_selectProducersListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DataTemplate dt = Resources["listProducerItemTemplate"] as DataTemplate;
                list_Catalogs.ItemTemplate = dt;
                list_Catalogs.ItemsSource = e.Result;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_selectModelsListCompleted(object sender, WS_Link.ws_selectModelsListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DataTemplate dt = Resources["listSimpleItemTemplate"] as DataTemplate;
                list_Catalogs.ItemTemplate = dt;
                list_Catalogs.ItemsSource = e.Result;
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

//------------------------------------------------------------------------------------------------------------

        private void list_Dictionarys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowStartForms();
            tabControl_Catalogs.SelectedIndex = 0;

            if ((sender as ListBox).SelectedItem != null)
            {

                list_Catalogs.ItemsSource = null;
                tab_Element.IsEnabled = false;

                switch ((sender as ListBox).SelectedIndex)
                {
                    case 0: // Производители
                        list_Catalogs.Visibility = Visibility.Visible;
                        panel_Catalogs.Visibility = Visibility.Collapsed;

                        b_tree_Models.Visibility = Visibility.Collapsed;
                        wc.ws_selectProducersListAsync();
                        btn_insertForm_show.IsEnabled = true;

                        break;
                    case 1: // Модели

                        //b_tree_Models.Visibility = Visibility.Visible;
                        //wc.ws_selectDBTreeAsync(2);
                        //btn_insertForm_show.IsEnabled = false;

                        list_Catalogs.Visibility = Visibility.Collapsed;

                        panel_Catalogs.Visibility = Visibility.Visible;

                        panel_Catalogs.Children.Clear();
                        panel_Catalogs.Children.Add(ucModels);

                        break;
                }
            }
        }

        private void list_Catalogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((list_Dictionarys.SelectedItem != null)  & ((sender as ListBox).SelectedItem != null))
            {
                tab_Element.IsEnabled = true;
            }
        }

        private void tabControl_Catalogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedIndex==1)
            {
                switch (list_Dictionarys.SelectedIndex)
                {
                    case 0: // Производители
                        if (list_Catalogs.SelectedItem != null)
                        {
                            ucProducer.ucConfigure(0, (list_Catalogs.SelectedItem as wsProducerType).ProducerID);
                            panel_Element.Children.Clear();
                            panel_Element.Children.Add(ucProducer);                          
                        }
                        break;
                    case 1: // Модели
                        if (list_Catalogs.SelectedItem != null)
                        {
                            DataTemplate dt = App.Current.Resources["catalogsModel"] as DataTemplate;
                            //ContentContro_ElementRead.ContentTemplate = dt;
                            panel_Element.Children.Clear();
                            
                            //ContentContro_ElementRead.Content = modelItem;
                        }
                        break;
                }
            }         
        }


        private void btn_deleteProducer_Click(object sender, RoutedEventArgs e)
        {
            producerType = list_Catalogs.SelectedItem as wsProducerType;
            if (producerType != null)
            {
                wc.ws_checkProducerAsync((int)producerType.ProducerID);
            }
            else
            {
                InfoWindow iw = new InfoWindow(Messages.m_Message,"Не выбран элемент для удаления");
                iw.Show();
            }
        }

        private void btn_insertForm_show_Click(object sender, RoutedEventArgs e)
        {


            switch (list_Dictionarys.SelectedIndex)
            {
                case 0: // Производители
                        ucProducer.ucConfigure(2, null);
                        panel_Element.Children.Clear();
                        panel_ElementWrite.Children.Clear();
                        panel_ElementWrite.Children.Add(ucProducer);

                        tabControl_Catalogs.Visibility = Visibility.Collapsed;
                        tabControl_insert.Visibility = Visibility.Visible;
                    break;
                case 1: // Модели

                        tabControl_Catalogs.Visibility = Visibility.Collapsed;
                        tabControl_insert.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ShowStartForms()
        {
            tabControl_Catalogs.Visibility = Visibility.Visible;
            tabControl_insert.Visibility = Visibility.Collapsed;
            panel_Element.Children.Clear();
            panel_ElementWrite.Children.Clear();
        }

        private void btn_OK_insertModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_CANCEL_insertModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_deleteModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tree_Models_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tree_Models.SelectedItem != null)
            {
               wc.ws_selectModelsListAsync((tree_Models.SelectedItem as Node).bi.ID);
            }
        }

        private void btn_show_tree_Models_Click(object sender, RoutedEventArgs e)
        {
            wc.ws_selectDBTreeAsync(2);
        }

    }

    
}
