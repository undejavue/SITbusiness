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
using System.Windows.Navigation;
using SIT_classLib;
using SITBusiness.WS_Link;

namespace SITBusiness.Views
{
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();

            PID = 76;
            SetReportUrls();
            
        }

        #region SetReportUrls
        private void SetReportUrls()
        {
            // Set the Report URLs
            ReportViewerURL = String.Format("{0}PID={1}", GetBaseAddress(), PID.ToString());

            ReportPDFURL = String.Format("{0}PID={1}&ShowReportViewer=False", GetBaseAddress(), PID.ToString());

            //btn_RView.Triggers. .DataContext = ReportViewerURL;
            //btn_RView.
            btn_RPDF.DataContext = ReportPDFURL;
        }
        #endregion

       
        // Properties
        private int _PID;
        public int PID
        {
            get { return _PID; }
            set
            {
                _PID = value;
            }
        }


        #region ReportViewerURL
        private string _ReportViewerURL = "";
        public string ReportViewerURL
        {
            get { return _ReportViewerURL; }
            set
            {
                if (ReportViewerURL == value)
                {
                    return;
                }
                _ReportViewerURL = value;
            }
        }
        #endregion

        #region ReportPDFURL
        private string _ReportPDFURL = "";
        public string ReportPDFURL
        {
            get { return _ReportPDFURL; }
            set
            {
                if (ReportPDFURL == value)
                {
                    return;
                }
                _ReportPDFURL = value;
            }
        }
        #endregion

        // Events

        
        #region GetBaseAddress
        private static Uri GetBaseAddress()
        {
            // Get the web address of the .xap that launched this application  

            string strBaseWebAddress = Application.Current.Host.Source.AbsoluteUri;
            // Find the position of the ClientBin directory        
            int PositionOfClientBin = Application.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"/clientbin");
            // Strip off everything after the ClientBin directory         
            strBaseWebAddress = strBaseWebAddress.Substring(0, PositionOfClientBin);
            // Create a URI
            Uri UriWebService = new Uri(String.Format(@"{0}/Report.aspx?", strBaseWebAddress));
            // Return the base address          
            return UriWebService;
        }
        #endregion


        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
