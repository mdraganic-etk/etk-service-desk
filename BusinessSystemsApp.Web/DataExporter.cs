using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace BusinessSystemsApp.Web
{
    public class DataExporter
    {
        public byte [] ExportData(string companyId, string productId)
        {
        SqlConnection _connection = new SqlConnection();
        SqlDataAdapter _dataAdapter = new SqlDataAdapter();
        SqlCommand _command = new SqlCommand();
        DataTable _dataTable = new DataTable();

        _connection = new SqlConnection();
        _dataAdapter = new SqlDataAdapter();
        _command = new SqlCommand();
        _dataTable = new DataTable();

        string dbServer = ConfigurationManager.AppSettings["DBServer"];
        string dbUser = ConfigurationManager.AppSettings["User"];
        string dbPassword = ConfigurationManager.AppSettings["Password"];

        //dbk is my database name that you can change it to your database name
        _connection.ConnectionString = "Data Source=" + dbServer + ";Initial Catalog=BusinessSystems;User ID=" + dbUser + ";Password=" + dbPassword;
        _connection.Open();

            return DumpTableToFile(_connection, companyId, productId);
       
        }

        private byte[] DumpTableToFile(SqlConnection connection, string companyId, string productId)
        {
            var myExport = new CsvExport();

            ///ASP.NET MVC action example
            //return File(myExport.ExportToBytes(), "text/csv", "results.csv");

            string sqlCommand = " SELECT csr.[Id] as Id " +
                      ",[CSRNumber] " +
                      ",[CallDate] " +
                      ",[RegisterDate] " +
                      ",[AnswerDate] " +
                      ",[FinishDate] " +
                      ",[Heading] " +
                      ",csr.[Description] " +
                      ",[Answer] " +
                      ",[TroubleReport] " +
                      ",comp.CompanyName " +
                      ",sit.SiteName " +
                      ",cal.FirstName + ' ' + cal.LastName as [Caller] " +
                      ",usr.FirstName + ' ' + usr.LastName as [User] " +
                      ",pri.PriorityName " +
                      ",product.ProductName " +
                      ",com.CommunicationChannelName " +
                      ",stat.StatusName " +
                      ",lastUsr.FirstName + ' ' + lastUsr.LastName as [Last User] " +
                      ",sev.SeverityName " +
                      ",freq.FrequencyName " +
                      ",[RiskAnalysisPerformed] " +
                      ",req.Name as RequesterType " +
                      ",iss.Name as IssueDomain" +
                      ",[Remedy] " +
                      ",[RemedyTime] " +
                  "FROM [BusinessSystems].[dbo].[Csr] as csr " +
                  "LEFT JOIN [BusinessSystems].[dbo].[User] usr ON (csr.UserId = usr.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[User] cal ON (csr.UserId = cal.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Company] comp ON (csr.CompanyId = comp.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Site] sit ON (csr.SiteId = sit.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Priority] pri ON (csr.PriorityId = pri.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Product] product ON (csr.ProductId = product.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[CommunicationChannel] com ON (csr.CommunicationId = com.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Csr_Status] stat ON (csr.StatusId = stat.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[User] lastUsr ON (csr.UserId = lastUsr.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Severity] sev ON (csr.SeverityId = sev.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[Frequency] freq ON (csr.SeverityId = freq.Id) " +
                  "LEFT JOIN [BusinessSystems].[dbo].[RequesterType] req ON (csr.SeverityId = req.Id)  " +
                  "LEFT JOIN [BusinessSystems].[dbo].[IssueDomain] iss ON (csr.SeverityId = iss.Id)" +
                  "WHERE 1=1 ";
            
            if (companyId != "0")
            {
                sqlCommand += " AND csr.CompanyId = " + companyId;
            }

            if (productId != "0")
            {
                sqlCommand += " AND csr.productId = " + productId;
            }


            using (var command = new SqlCommand(sqlCommand , connection))

            using (var reader = command.ExecuteReader())
            {
                string[] columnNames = GetColumnNames(reader).ToArray();
                int numFields = columnNames.Length;

                myExport.AddRow();
                myExport["Id"] = "Id";
                myExport["CSRNumber"] = "CSRNumber";
                myExport["RegisterDate"] = "RegisterDate";
                myExport["AnswerDate"] = "AnswerDate";
                myExport["Heading"] = "Heading";
                myExport["Description"] = "Description";
                myExport["TroubleReport"] = "TroubleReport";
                myExport["CompanyName"] = "CompanyName";
                myExport["SiteName"] = "SiteName";
                myExport["Caller"] = "Caller";
                myExport["PriorityName"] = "PriorityName";
                myExport["ProductName"] = "ProductName";
                myExport["CommunicationChannelName"] = "CommunicationChannelName";
                myExport["StatusName"] = "StatusName";
                myExport["Last User"] = "Last User";
                myExport["SeverityName"] = "SeverityName";
                myExport["FrequencyName"] = "FrequencyName";
                myExport["RiskAnalysisPerformed"] = "RiskAnalysisPerformed";
                myExport["RequesterType"] = "RequesterType";
                myExport["IssueDomain"] = "IssueDomain";
                myExport["Remedy"] = "Remedy";
                myExport["RemedyTime"] = "RemedyTime";


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        myExport.AddRow();
                        myExport["Id"] = reader["Id"];
                        myExport["CSRNumber"] = reader["CSRNumber"];
                        myExport["RegisterDate"] = reader["RegisterDate"];
                        myExport["AnswerDate"] = reader["AnswerDate"];
                        myExport["Heading"] = reader["Heading"];
                        myExport["Description"] = reader["Description"];
                        myExport["TroubleReport"] = reader["TroubleReport"];
                        myExport["CompanyName"] = reader["CompanyName"];
                        myExport["SiteName"] = reader["SiteName"];
                        myExport["Caller"] = reader["Caller"];
                        myExport["PriorityName"] = reader["PriorityName"];
                        myExport["ProductName"] = reader["ProductName"];
                        myExport["CommunicationChannelName"] = reader["CommunicationChannelName"];
                        myExport["StatusName"] = reader["StatusName"];
                        myExport["Last User"] = reader["Last User"];
                        myExport["SeverityName"] = reader["SeverityName"];
                        myExport["FrequencyName"] = reader["FrequencyName"];
                        myExport["RiskAnalysisPerformed"] = reader["RiskAnalysisPerformed"];
                        myExport["RequesterType"] = reader["RequesterType"];
                        myExport["IssueDomain"] = reader["IssueDomain"];
                        myExport["Remedy"] = reader["Remedy"];
                        myExport["RemedyTime"] = reader["RemedyTime"];
                    }
                }
            }

            return myExport.ExportToBytes();
    }

    private IEnumerable<string> GetColumnNames(IDataReader reader)
    {
        foreach (DataRow row in reader.GetSchemaTable().Rows)
        {
            yield return (string)row["ColumnName"];
        }
    }
    }
}