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
using SITBusiness.WS_Link;
using SIT_classLib;

namespace SITBusiness.Views
{
    public partial class cwnd_Producer : ChildWindow
    {
        WServiceClient wc = new WServiceClient();
        
        public cwnd_Producer(wsProducerType P)
        {
            InitializeComponent();
            ContentControl_Producer.Content = P;
            Loaded += new RoutedEventHandler(cwnd_Producer_Loaded);
        }

        void cwnd_Producer_Loaded(object sender, RoutedEventArgs e)
        {
            
            wc.ws_updateProducerCompleted += new EventHandler<ws_updateProducerCompletedEventArgs>(wc_ws_updateProducerCompleted);

        }

        void wc_ws_updateProducerCompleted(object sender, ws_updateProducerCompletedEventArgs e)
        {

            if (e.Result == 1)
            {
                InfoWindow iw = new InfoWindow(Messages.m_Message, Messages.m_ProduserUpdated);
                iw.Show();
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }

            this.DialogResult = true;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            wsProducerType LocalProducer = ContentControl_Producer.Content as wsProducerType;
            wc.ws_updateProducerAsync(LocalProducer);
                    
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

