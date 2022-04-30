using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace BusinessSystemsApp.Web
{
    public partial class Report : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            int Tip = 0;
            
            int _csrId = 0;

            if(HttpContext.Current.Request.Params["Id"] != String.Empty)
                _csrId = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);

            String _text = HttpContext.Current.Request.Params["Text"];
            String _companyList = (HttpContext.Current.Request.Params["CompanyList"]);
            int _companyId = Convert.ToInt32(HttpContext.Current.Request.Params["CompanyId"]);
            int _productId = Convert.ToInt32(HttpContext.Current.Request.Params["ProductId"]);
            
            DateTime _reportedFrom = new DateTime(Convert.ToInt32(HttpContext.Current.Request.Params["ReportedFrom"].Split('.')[2]), Convert.ToInt32(HttpContext.Current.Request.Params["ReportedFrom"].Split('.')[1]), Convert.ToInt32(HttpContext.Current.Request.Params["ReportedFrom"].Split('.')[0])); //Convert.ToDateTime(HttpContext.Current.Request.Params["ReportedFrom"]);
            DateTime _reportedTo = new DateTime(Convert.ToInt32(HttpContext.Current.Request.Params["ReportedTo"].Split('.')[2]), Convert.ToInt32(HttpContext.Current.Request.Params["ReportedTo"].Split('.')[1]), Convert.ToInt32(HttpContext.Current.Request.Params["ReportedTo"].Split('.')[0])); //Convert.ToDateTime(HttpContext.Current.Request.Params["ReportedTo"]);
            DateTime _finishedFrom = new DateTime(Convert.ToInt32(HttpContext.Current.Request.Params["FinishedFrom"].Split('.')[2]), Convert.ToInt32(HttpContext.Current.Request.Params["FinishedFrom"].Split('.')[1]), Convert.ToInt32(HttpContext.Current.Request.Params["FinishedFrom"].Split('.')[0])); //Convert.ToDateTime(HttpContext.Current.Request.Params["FinishedFrom"]);
            DateTime _finishedTo = new DateTime(Convert.ToInt32(HttpContext.Current.Request.Params["FinishedTo"].Split('.')[2]), Convert.ToInt32(HttpContext.Current.Request.Params["FinishedTo"].Split('.')[1]), Convert.ToInt32(HttpContext.Current.Request.Params["FinishedTo"].Split('.')[0])); //Convert.ToDateTime(HttpContext.Current.Request.Params["FinishedTo"]);
            int _userId = Convert.ToInt32(HttpContext.Current.Request.Params["UserId"]);
            bool _onlyMine = Convert.ToBoolean(HttpContext.Current.Request.Params["Mine"]);
            String _statusList = HttpContext.Current.Request.Params["StatusList"];

            Tip = Convert.ToInt32(HttpContext.Current.Request.Params["Type"]);

            ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            String reportPath = Server.MapPath("Reports/CSRReport.rpt"); 

           

            if (Tip == 1)
            {
                ConnectionInfo ciReportConnection = new ConnectionInfo();
                reportDocument.Load(reportPath);

                //reportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), ConfigurationManager.AppSettings["DBServer"].ToString(), "BusinessSystems");
                

                reportDocument = ConnectionInfo(reportDocument, ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), ConfigurationManager.AppSettings["DBServer"].ToString(), "BusinessSystems");
              
                reportDocument.SetParameterValue("@CsrId", _csrId);
                reportDocument.SetParameterValue("@Text", _text);
                reportDocument.SetParameterValue("@CompanyList", _companyList);
                reportDocument.SetParameterValue("@CompanyId", _companyId);
                reportDocument.SetParameterValue("@ProductId", _productId);
                reportDocument.SetParameterValue("@StatusList", _statusList);
                reportDocument.SetParameterValue("@CSRReportedFrom", _reportedFrom);
                reportDocument.SetParameterValue("@CSRReportedTo", _reportedTo);
                reportDocument.SetParameterValue("@CSRFinishedFrom", _finishedFrom);
                reportDocument.SetParameterValue("@CSRFinishedTo", _finishedTo);
                if(_onlyMine)
                    reportDocument.SetParameterValue("@UserId", _userId);
                else
                    reportDocument.SetParameterValue("@UserId", 0);

                //reportDocument.Load(reportPath);

                CrystalReportViewer1.ReportSource = reportDocument;

            }
            else if (Tip == 2)
            {
                
                reportPath = Server.MapPath("Reports/CSRReport_Customer.rpt");

                reportDocument.Load(reportPath);

                //reportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), ConfigurationManager.AppSettings["DBServer"].ToString(), "BusinessSystems");
                reportDocument = ConnectionInfo(reportDocument, ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), ConfigurationManager.AppSettings["DBServer"].ToString(), "BusinessSystems");

                reportDocument.SetParameterValue("@CsrId", _csrId);
                reportDocument.SetParameterValue("@Text", _text);
                reportDocument.SetParameterValue("@CompanyList", _companyList);
                reportDocument.SetParameterValue("@CompanyId", _companyId);
                reportDocument.SetParameterValue("@ProductId", _productId);
                reportDocument.SetParameterValue("@StatusList", _statusList);
                reportDocument.SetParameterValue("@CSRReportedFrom", _reportedFrom);
                reportDocument.SetParameterValue("@CSRReportedTo", _reportedTo);
                reportDocument.SetParameterValue("@CSRFinishedFrom", _finishedFrom);
                reportDocument.SetParameterValue("@CSRFinishedTo", _finishedTo);
                if (_onlyMine)
                    reportDocument.SetParameterValue("@UserId", _userId);
                else
                    reportDocument.SetParameterValue("@UserId", 0);
                
                

                CrystalReportViewer1.ReportSource = reportDocument;
            }
        }


        private ReportDocument ConnectionInfo(ReportDocument rpt, string user, string PW, string DBserver, string DBbaseName)
        {
            ReportDocument crSubreportDocument;
            //string[] strConnection = ConfigurationManager.ConnectionStrings[("AppConn")].ConnectionString.Split(new char[] { ';' });

            Database oCRDb = rpt.Database;

            Tables oCRTables = oCRDb.Tables;

            CrystalDecisions.CrystalReports.Engine.Table oCRTable = default(CrystalDecisions.CrystalReports.Engine.Table);

            TableLogOnInfo oCRTableLogonInfo = default(CrystalDecisions.Shared.TableLogOnInfo);

            ConnectionInfo oCRConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();

            oCRConnectionInfo.ServerName = DBserver;
            oCRConnectionInfo.DatabaseName = DBbaseName;
            oCRConnectionInfo.Password = PW;
            oCRConnectionInfo.UserID = user;

            for (int i = 0; i < oCRTables.Count; i++)
            {
                oCRTable = oCRTables[i];
                oCRTableLogonInfo = oCRTable.LogOnInfo;
                oCRTableLogonInfo.ConnectionInfo = oCRConnectionInfo;
                oCRTable.ApplyLogOnInfo(oCRTableLogonInfo);
                if (oCRTable.TestConnectivity())
                    //' If there is a "." in the location then remove the
                    // ' beginning of the fully qualified location.
                    //' Example "dbo.northwind.customers" would become
                    //' "customers".
                    oCRTable.Location = oCRTable.Location.Substring(oCRTable.Location.LastIndexOf(".") + 1);


            }

            for (int i = 0; i < rpt.Subreports.Count; i++)
            {

                {
                    crSubreportDocument = rpt.OpenSubreport(rpt.Subreports[i].Name);
                    oCRDb = crSubreportDocument.Database;
                    oCRTables = oCRDb.Tables;
                    foreach (CrystalDecisions.CrystalReports.Engine.Table aTable in oCRTables)
                    {
                        oCRTableLogonInfo = aTable.LogOnInfo;
                        oCRTableLogonInfo.ConnectionInfo = oCRConnectionInfo;
                        aTable.ApplyLogOnInfo(oCRTableLogonInfo);
                        if (aTable.TestConnectivity())
                            //' If there is a "." in the location then remove the
                            // ' beginning of the fully qualified location.
                            //' Example "dbo.northwind.customers" would become
                            //' "customers".
                            aTable.Location = aTable.Location.Substring(aTable.Location.LastIndexOf(".") + 1);

                    }
                }
            }
            //  }

            rpt.Refresh();
            return rpt;
        }
    }
}