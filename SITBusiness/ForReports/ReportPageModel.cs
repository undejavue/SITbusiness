using System;



//using Microsoft.VisualBasic;
using System.Windows.Browser;
using System.Windows;
using SIT_classLib;
using SITBusiness.WS_Link;

namespace SIT_Report
{
    public class ReportPageModel
    {
        public ReportPageModel()
        {

        }

        private void SetReportUrls()
        {
            //ReportViewerURL = String.Format("{0}PID={1}",GetBaseAddress(),PID.ToString());
            //ReportPDFURL = String.Format("{0}PID={1}&ShowReportViewer=False", GetBaseAddress(), PID.ToString());
        } 


        public static Uri GetBaseAddress()
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
    }
}