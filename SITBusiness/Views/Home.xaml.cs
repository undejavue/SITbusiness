using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;


namespace SITBusiness
{
    using System.Windows.Controls;
    using System.Windows.Navigation;


    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;

            NetworkChange.NetworkAddressChanged += NetworkChanged;
            CheckNetworkState();

            //Loaded += new RoutedEventHandler(Page_Loaded);
            
            
        }

        private void NetworkChanged(object sender, EventArgs e)
        {
            CheckNetworkState();
        }

        private void CheckNetworkState()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Сеть доступна
                panel_Simatek.Visibility = Visibility.Visible;
                img_Network_status.Visibility = System.Windows.Visibility.Collapsed;
                txt_Network_status.Visibility = System.Windows.Visibility.Collapsed;

                Deployment.Current.RuntimeVersion.ToString();

                WS_Link.WServiceClient wc = new WS_Link.WServiceClient();
                wc.ws_TestDBConnectionCompleted += new EventHandler<WS_Link.ws_TestDBConnectionCompletedEventArgs>(wc_ws_TestDBConnectionCompleted);
                wc.ws_TestDBConnectionAsync();
                wc.ws_TestAuthConnectionCompleted += new EventHandler<WS_Link.ws_TestAuthConnectionCompletedEventArgs>(wc_ws_TestAuthConnectionCompleted);
                wc.ws_TestAuthConnectionAsync();       
            }
            else
            {
                //Сеть недоступна
                panel_Simatek.Visibility = Visibility.Collapsed;
                img_Network_status.Visibility = System.Windows.Visibility.Visible;
                txt_Network_status.Visibility = System.Windows.Visibility.Visible;
                ErrorWindow.CreateNew("Нет соединения с сетью!");
            }
        }


        void wc_ws_TestAuthConnectionCompleted(object sender, WS_Link.ws_TestAuthConnectionCompletedEventArgs e)
        {
            SolidColorBrush brush1 = new SolidColorBrush(Colors.Gray);
            
            if (e.Result != 0)
            {
                txt_AuthResult.Text = e.OpStatus.ToString();
                brush1.Color = Colors.Green;
                txt_AuthResult.Foreground = brush1;
            }
            else
            {
                brush1.Color = Colors.Red;
                txt_AuthResult.Foreground = brush1;
                Views.InfoWindow iw = new Views.InfoWindow("Ошибка связи", e.OpStatus.ToString());
                iw.Show();
            }


        }

        void wc_ws_TestDBConnectionCompleted(object sender, WS_Link.ws_TestDBConnectionCompletedEventArgs e)
        {
            SolidColorBrush brush1 = new SolidColorBrush(Colors.Gray);

            if (e.Result != 0)
            {
                txt_result.Text = e.OpStatus.ToString();
                brush1.Color = Colors.Green;
                txt_AuthResult.Foreground = brush1;
            }
            else
            {
                brush1.Color = Colors.Red;
                txt_result.Foreground = brush1;
                Views.InfoWindow iw = new Views.InfoWindow("Ошибка связи", e.OpStatus.ToString());
                iw.Show();
            }
        }
        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //}
    }
}