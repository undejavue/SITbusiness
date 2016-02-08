using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Data;
using System.Windows.Printing;
using SIT_classLib;
using SITBusiness.WS_Link;

namespace SITBusiness.Views
{
    public partial class Devices : Page
    {
        WServiceClient wc = new WServiceClient();
        PagedCollectionView devicesCollection;
        //private string globalSearchCondition;

        wsSimpleItem globalFilterType = new wsSimpleItem(null, "Все");
        wsSimpleItem globalFilterPlace = new wsSimpleItem(null, "Все");

        public Devices()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Devices_Loaded);
        }

        void Devices_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_selectPassportsListCompleted += new EventHandler<ws_selectPassportsListCompletedEventArgs>(wc_ws_selectPassportsListCompleted);
            wc.ws_selectPassportsListAsync(null,null);

        }

        private void refreshDataGrid()
        {
            dg_Devices.ItemsSource = devicesCollection;
        }


        void wc_ws_selectPassportsListCompleted(object sender, ws_selectPassportsListCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                devicesCollection = new PagedCollectionView(e.Result);
              
                devicesCollection.PageSize = 20;

                dg_Devices.ItemsSource = devicesCollection;
                dp_Pager.Source = devicesCollection;
                //txt_ItemsCount.Text = "Всего устройств в базе: " + devicesCollection.Count.ToString();
                
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_SELECT, e.OpStatus.ToString());
                w.Show();
            }
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void dg_Devices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_Devices.SelectedItem != null)
            {

                //DataTemplate dt = Resources["modelColumn_Ex"] as DataTemplate;
               

            }
            
        }

        private void btn_selFilterType_Click(object sender, RoutedEventArgs e)
        {
            cwnd_treeTypes w = new cwnd_treeTypes();
            w.Show();
            w.Closed += new EventHandler(w_selFilterTypeClosed);
        }

        void w_selFilterTypeClosed(object sender, EventArgs e)
        {
            cwnd_treeTypes w = sender as cwnd_treeTypes;
            if (w.DialogResult == true)
            {
                globalFilterType = w.selectedItem;
            }
            else
            {
                globalFilterType.ID = null;
                globalFilterType.Description = "Все";
            }

            btn_selFilterType.Content = "Класс = \"" + globalFilterType.Description + "\"";
            wc.ws_selectPassportsListAsync(globalFilterType.ID,globalFilterPlace.ID);
        }

        private void btn_selFilterPlace_Click(object sender, RoutedEventArgs e)
        {
            cwnd_selectPlace w = new cwnd_selectPlace();
            w.Show();
            w.Closed += new EventHandler(w_selFilterPlaceClosed);
        }

        void w_selFilterPlaceClosed(object sender, EventArgs e)
        {
            cwnd_selectPlace w = sender as cwnd_selectPlace;
            if (w.DialogResult == true)
            {
                globalFilterPlace = w.selectedItem;
            }
            else
            {
                globalFilterPlace.ID = null;
                globalFilterPlace.Description = "Все";
            }

            btn_selFilterPlace.Content = "Место = \"" + globalFilterPlace.Description + "\"";
            wc.ws_selectPassportsListAsync(globalFilterType.ID, globalFilterPlace.ID);
        }

        private void btn_cancelFilters_Click(object sender, RoutedEventArgs e)
        {
            globalFilterPlace.ID = null;
            globalFilterPlace.Description = "Все";
            globalFilterType.ID = null;
            globalFilterType.Description = "Все";
            btn_selFilterType.Content = "Класс = \"" + globalFilterType.Description + "\"";
            btn_selFilterPlace.Content = "Место = \"" + globalFilterPlace.Description + "\"";
            wc.ws_selectPassportsListAsync(null,null);
        }

        private void btn_groupByType_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupBy = new PropertyGroupDescription("DevTypeName");

            devicesCollection.GroupDescriptions.Add(groupBy);
            //refreshDataGrid();
        }

        private void btn_groupByPlace_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupBy = new PropertyGroupDescription("DevPlaceName");

            devicesCollection.GroupDescriptions.Add(groupBy);
            //refreshDataGrid();
        }

        private void btn_cancelGroups_Click(object sender, RoutedEventArgs e)
        {
            devicesCollection.GroupDescriptions.Clear();
        }


        private void dg_Devices_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            if (dg_Devices.SelectedItem != null)
            {
                wsPassportExtended P = dg_Devices.SelectedItem as wsPassportExtended;

                cwnd_Passport w = new cwnd_Passport(P.DevID);
                w.Show();

            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var doc = new PrintDocument();

            doc.PrintPage += (s, ee) =>
            {
                ee.PageVisual = dg_Devices;
                ee.HasMorePages = true;
            };

            doc.Print("Отчет"); 
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap(this.dg_Devices, null);
        }

        private void btn_startSearch_Click(object sender, RoutedEventArgs e)
        {
            string s = txt_searchCondition.Text.ToString();

            SetTeamColors(dg_Devices, s);

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
                        Style borderStyle = Resources["IsSearchedBorder"] as Style;

                        brd.Style = borderStyle;

                        //brd.Background = new SolidColorBrush(Colors.Red);
                        //brd.BorderBrush = new SolidColorBrush(Colors.Gray);
                        //brd.BorderThickness = new Thickness(2);

                        //teamblk.Foreground = new SolidColorBrush(Colors.White);
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

        private void dg_Devices_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //wsPassportExtended P = e.Row.DataContext as wsPassportExtended;

            //SolidColorBrush brush1 = new SolidColorBrush(Colors.Blue);

            //dg_Devices.AlternatingRowBackground = null;

            //if (P.DevID == 11)
            //{
            //    e.Row.Background = brush1;
            //}

        }

        private void btn_cancelSearch_Click(object sender, RoutedEventArgs e)
        {
            dg_Devices.ItemsSource = null;
            

            dg_Devices.ItemsSource = devicesCollection;
        }

        private void btn_selFilterCalibrate_Click(object sender, RoutedEventArgs e)
        {
            devicesCollection.Filter = delegate(object filterObject)
            {
                wsPassportExtended Pass = (wsPassportExtended)filterObject;
                return (Pass.IsCalibrated == true);
            };
        }
  
    }



}
