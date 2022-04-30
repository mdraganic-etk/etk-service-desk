using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessSystemsApp.Web
{
    public partial class Reporting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Clear();

                ClientScript.RegisterStartupScript(this.GetType(), "Korisnik", "alert('" + System.Web.HttpContext.Current.User.ToString() + "');", true);

                Response.ContentType = "text/csv;charset=utf-8;";
                //Response.ContentType = "application/msword";
                //you also can write this, if the client install pdf plug-in then the pdf file will be displayed online
                //Response.ContentType = "application/pdf";
                DataExporter de = new DataExporter();

                byte[] export = de.ExportData(Request["companyId"], Request["productId"]);

                Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode("csrs.csv"));
                //   
                Response.AddHeader("Content-Length", export.Length.ToString());
                //Response.OutputStream.Write(buffer, 0, (int)buffer.Length);

                Response.BinaryWrite(export);
                Response.Flush();
                Response.Close();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Zabranjen pristup", "alert('Nemate prava za pristup ovom resursu!');", true);
            }
        }
    }
}