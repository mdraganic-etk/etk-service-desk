using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
//using System.Xml.Linq;

public static class DataGridExtensions
{
    public static void Export(this DataGrid dg, DataPager dp)
    {
        ExportDataGrid(dg, dp);
    }

    public static void ExportDataGrid(DataGrid dGrid, DataPager dPager)
    {
        SaveFileDialog objSFD = new SaveFileDialog() { DefaultExt = "csv", Filter = "CSV Files (*.csv)|*.csv|Excel XML (*.xml)|*.xml|All files (*.*)|*.*", FilterIndex = 1 };
        if (objSFD.ShowDialog() == true)
        {
            string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
            StringBuilder strBuilder = new StringBuilder();
            if (dPager.Source == null) return;
            List<string> lstFields = new List<string>();
            if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column || dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
            {
                foreach (DataGridColumn dgcol in dGrid.Columns)
                    lstFields.Add(FormatField(dgcol.Header.ToString(), strFormat));
                BuildStringOfRow(strBuilder, lstFields, strFormat);
            }

            //////////////////////// PAGE LOOP BEGIN  ////////////////////////////////////////
            for (int i = 0; i < dPager.PageCount; i++)
            {
                dPager.PageIndex = i;
                foreach (object data in dPager.Source)
                {
                    lstFields.Clear();
                    foreach (DataGridColumn col in dGrid.Columns)
                    {
                        string strValue = "";
                        Binding objBinding = null;
                        if (col is DataGridBoundColumn)
                            objBinding = (col as DataGridBoundColumn).Binding;
                        if (col is DataGridTemplateColumn)
                        {
                            //This is a template column... let us see the underlying dependency object
                            DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                            FrameworkElement oFE = (FrameworkElement)objDO;
                            FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                            if (oFI != null)
                            {
                                if (oFI.GetValue(null) != null)
                                {
                                    if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
                                        objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                }
                            }
                        }
                        if (objBinding != null)
                        {
                            if (objBinding.Path.Path != "")
                            {
                                PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                                if (pi != null) try
                                    {

                                        strValue = pi.GetValue(data, null).ToString();

                                    }
                                    catch (Exception ex)
                                    {
                                    }

                            }
                            if (objBinding.Converter != null)
                            {
                                if (!string.IsNullOrEmpty(strValue))
                                    strValue = objBinding.Converter.Convert(strValue, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                                else
                                    strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                            }
                        }
                        lstFields.Add(FormatField(strValue, strFormat));
                    }
                    BuildStringOfRow(strBuilder, lstFields, strFormat);
                }
            }
            //////////////////////// PAGE LOOP END  ////////////////////////////////////////

            StreamWriter sw = new StreamWriter(objSFD.OpenFile(), Encoding.UTF8);
            if (strFormat == "XML")
            {
                //Let us write the headers for the Excel XML
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                sw.WriteLine("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
                sw.WriteLine("<Author>Arasu Elango</Author>");
                sw.WriteLine("<Created>" + DateTime.Now.ToLocalTime().ToLongDateString() + "</Created>");
                sw.WriteLine("<LastSaved>" + DateTime.Now.ToLocalTime().ToLongDateString() + "</LastSaved>");
                sw.WriteLine("<Company>Atom8 IT Solutions (P) Ltd.,</Company>");
                sw.WriteLine("<Version>12.00</Version>");
                sw.WriteLine("</DocumentProperties>");
                sw.WriteLine("<Worksheet ss:Name=\"Silverlight Export\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                sw.WriteLine("<Table>");
            }
            sw.Write(strBuilder.ToString());
            if (strFormat == "XML")
            {
                sw.WriteLine("</Table>");
                sw.WriteLine("</Worksheet>");
                sw.WriteLine("</Workbook>");
            }
            sw.Close();
        }
    }
    private static void BuildStringOfRow(StringBuilder strBuilder, List<string> lstFields, string strFormat)
    {
        switch (strFormat)
        {
            case "XML":
                strBuilder.AppendLine("<Row>");
                strBuilder.AppendLine(String.Join("\r\n", lstFields.ToArray()));
                strBuilder.AppendLine("</Row>");
                break;
            case "CSV":
                strBuilder.AppendLine(String.Join(",", lstFields.ToArray()));
                break;
        }
    }
    private static string FormatField(string data, string format)
    {
        switch (format)
        {
            case "XML":
                return String.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", data);
            case "CSV":
                return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
        }
        return data;
    }
}