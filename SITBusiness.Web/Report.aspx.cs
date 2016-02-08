using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using SITBusiness.Web;
using SIT_classLib;

namespace SITBusiness.Web
{
    public partial class Report : System.Web.UI.Page
    {
        private string reportName;
        private List<ReportDataSource> list_DS;

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((reportName = Request.QueryString["report"]) != null)
                {
                    switch (reportName)
                    {
                        case "Report_PassportIU":
                            if (Request.QueryString["PID"] != null)
                            {
                                list_DS = new List<ReportDataSource>();
                                list_DS.Add(new ReportDataSource("DS_Passport", LoadDS_PassportData()));
                                list_DS.Add(new ReportDataSource("DS_Service", LoadDS_ServiceData()));

                                ShowReport(reportName, list_DS);
                            }
                            break;
                        case "Report_Calibrations":
                            list_DS = new List<ReportDataSource>();
                            list_DS.Add(new ReportDataSource("DS_CNot", LoadDS_CNot()));
                            list_DS.Add(new ReportDataSource("DS_CMissed", LoadDS_CMissed()));
                            list_DS.Add(new ReportDataSource("DS_CNext", LoadDS_CNext()));
                            break;
                        default:
                            list_DS = new List<ReportDataSource>();
                            ShowReport(reportName, list_DS);
                            break;
                    }
                }
            }
        }

        #region RenderReport
        private void RenderReport()
        {
            string fileNameExtension = "PDF";
            // CheckBox to see if there is any data
            if (LoadDS_PassportData().Rows.Count > 0)
            {
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/ReportOne.rdlc");

                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", LoadDS_PassportData());
                localReport.DataSources.Add(reportDataSource);

                string reportType = "pdf";
                string mimeType = "application/pdf";
                string encoding;
                
                //The DeviceInfo settings should be changed based on the reportType
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                string deviceInfo =
                "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>8.5in</PageWidth>" +
                " <PageHeight>11in</PageHeight>" +
                " <MarginTop>0.5in</MarginTop>" +
                " <MarginLeft>1in</MarginLeft>" +
                " <MarginRight>1in</MarginRight>" +
                " <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                //Render the report
                renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

                //Clear the response stream and write the bytes to the outputstream
                //Set content-disposition to "attachment" so that user is prompted to take an action
                //on the file (open or save)
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=foo." + fileNameExtension);
                Response.BinaryWrite(renderedBytes);
                Response.End();
            }
            else
            {
                // There were no records returned
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=foo." + fileNameExtension);
                Response.Write("No Data");
                Response.End();
            }
        }
        #endregion



        //---- Отчет "Паспорт"

        private void ShowReport(string reportName, List<ReportDataSource> list_Datasources)
        {
            reportName = "~/Reports/" + reportName + ".rdlc";
            
            this.ReportViewer1.ProcessingMode = ProcessingMode.Local;
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath(reportName);

            foreach (ReportDataSource ds in list_Datasources)
            {
                ReportViewer1.LocalReport.DataSources.Add(ds);
            }
        }

        //---- for passport report
        private DataTable LoadDS_PassportData()
        {
            int PID = Convert.ToInt32(Request.QueryString["PID"]);

            DeviceDataManager DDM = new DeviceDataManager();

            var result = from Passport in DDM.selectDevicesPassports(null, null).AsQueryable()
                         where Passport.DevID == PID
                         select Passport;


            Utility objUtility = new Utility();
            DataTable dt = new DataTable();
            dt = objUtility.LINQToDataTable(result);


            return dt.DefaultView.Table;
        }

        //---- for passport report
        private DataTable LoadDS_ServiceData()
        {
            int PID = Convert.ToInt32(Request.QueryString["PID"]);

            DeviceDataManager DDM = new DeviceDataManager();

            var result = DDM.selectDeviceCalibrations(PID);

            Utility objUtility = new Utility();
            DataTable dt = new DataTable();
            dt = objUtility.LINQToDataTable(result);


            return dt.DefaultView.Table;
        }

        //---- for calibrations report
        private DataTable LoadDS_CNot()
        {
            DeviceDataManager DDM = new DeviceDataManager();

            var result = from item in DDM.selectNextCalibrations() where item.plannedDate == null select item;

            Utility objUtility = new Utility();
            DataTable dt = new DataTable();
            dt = objUtility.LINQToDataTable(result);

            return dt.DefaultView.Table;
        }

        //---- for calibrations report
        private DataTable LoadDS_CMissed()
        {
            DeviceDataManager DDM = new DeviceDataManager();

            var result = from item in DDM.selectNextCalibrations() where item.plannedDate <= DateTime.Now select item;

            Utility objUtility = new Utility();
            DataTable dt = new DataTable();
            dt = objUtility.LINQToDataTable(result);

            return dt.DefaultView.Table;
        }

        //---- for calibrations report
        private DataTable LoadDS_CNext()
        {
            DeviceDataManager DDM = new DeviceDataManager();

            var result = from item in DDM.selectNextCalibrations() where item.plannedDate > DateTime.Now select item;

            Utility objUtility = new Utility();
            DataTable dt = new DataTable();
            dt = objUtility.LINQToDataTable(result);

            return dt.DefaultView.Table;
        }

    }
}