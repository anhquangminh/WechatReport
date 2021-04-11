using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;//sql
using System.Data.Common;
using System.Data.Odbc; //oracle
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Collections;
using System.Xml;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WechatReport
{
    class DBHelper
    {
        public static string strConn_SH = "Data Source=10.117.46.41; Persist Security Info=True;User ID=sa;Password=sa;Initial Catalog=TELOGIN;";//10.117.46.41
        public static string strConn_GW = "Data Source=10.224.130.50; Persist Security Info=True;User ID=sa;Password=sa;Initial Catalog=TELOGIN;";
        public static string strConn_GZ = "Data Source=10.221.68.202,1435; Persist Security Info=True;User ID=sa;Password=sa;Initial Catalog=NSN;";

        public static string strConn_Oracle_SH = ConfigurationManager.AppSettings["DbHelperConnStrBTSSFC"];
        public static string strConn_Oracle_GW = ConfigurationManager.AppSettings["DbHelperConnStrBTSSFCVN"];

        public static string strConn_Oracle_GZ = ConfigurationManager.AppSettings["DbHelperConnStrBTSSFCGZ"];
        public static string err = "";


        public static void SqlBulkCopyByDatatable(string TableName, DataTable dt, List<string> errorlist)
        {
            SqlConnection Conn = new SqlConnection(strConn_SH);
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(strConn_SH, SqlBulkCopyOptions.UseInternalTransaction);
            try
            {
                sqlbulkcopy.DestinationTableName = TableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                }
                sqlbulkcopy.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                errorlist.Add(ex.Message);
            }
        }


        public static void execsql(string strSql) 
        {
            //MessageBox.Show("strSql：" + strSql, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SqlConnection Conn = new SqlConnection(strConn_SH);
            SqlCommand Comm = new SqlCommand(strSql, Conn);
            Comm.CommandTimeout = 300;
            Conn.Open();
            //Open connect
            Comm.ExecuteNonQuery();

            Conn.Close();
        }

        private static DataTable GetTable(string strsql, string type)
        {
            DataTable dt = new DataTable();
            JArray RowsData = new JArray();
            try
            {
                string jsonstr = RequestJsonstr(strsql, type);
                RowsData = JArray.Parse(jsonstr);
            }
            catch (Exception err)
            {
               var message= err.Message;
            }

            return JArrayToDataTable(RowsData);
        
        }
        private static DataTable JArrayToDataTable(JArray jarr)
        {
            if (jarr.Count > 0)
            {
                //StringBuilder columns = new StringBuilder();
                DataTable table = new DataTable();
                JObject objcolumns = jarr[0] as JObject; 
                foreach(JToken jkon in objcolumns.AsJEnumerable())
                {
                    string name = ((JProperty)jkon).Name;
                    //columns.Append(name + ",");
                    table.Columns.Add(name);
                
                }     
                for (int i = 0; i < jarr.Count;i++ )
                {
                    DataRow row = table.NewRow();
                    JObject obj = jarr[i] as JObject;
                    foreach(JToken jkon in obj.AsJEnumerable())
                    {
                        string name = ((JProperty)jkon).Name;
                        string value = ((JProperty)jkon).Value.ToString();
                        row[name] = value;
                    
                    }
                    table.Rows.Add(row);
                }
                return table;
            }
            else
            {
                return new DataTable();
            }
            
        }
        private static string RequestJsonstr(string strsql, string type)
        {
            //string url = "http://10.116.100.155:8085/RetestSource.asmx";//10.126.4.22
            string url = "";
            string methodname = "Source";
            //string err = "";
            Hashtable hs = new Hashtable();
            hs.Add("strsql", strsql);
            hs.Add("type", type);
            string result = "";
            result = RequestWebService(url, methodname, hs, ref err);
            if (!err.Equals(""))
            {
                result = "err:" + err;     
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                result = doc.InnerText;              
            }
            return result;
        }
        private static string RequestWebService(string url, string methodname, Hashtable Paras, ref string err)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + methodname);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";           
            try
            {            
                byte[] data = Encoding.UTF8.GetBytes(ParasToString(Paras));
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(data, 0, data.Length);
                reqstream.Close(); 
                Stream respstream = null;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                respstream = response.GetResponseStream();
                result = new StreamReader(respstream, Encoding.UTF8).ReadToEnd();
                response.Close();
                request = null;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return result;
        }
        private static string ParasToString(Hashtable Paras)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Paras.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");

                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Paras[k].ToString()));
            }

            return sb.ToString();
        }
        //doc data table
        public static DataTable ReadTable(String strSql, string strConn) 
        {
            DataTable dt = new DataTable();           
            if (strConn.Contains("10.221.68.202"))
            {
                string starttime
                =strSql
                .Split(new string[]{"[StopTime]>='"},StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(new string[]{"'"},StringSplitOptions.RemoveEmptyEntries)[0];
                starttime = DateTime.Parse(starttime).AddDays(2).ToString("yyyy-MM-dd HH:mm:ss");
                 string stoptime=strSql
                .Split(new string[]{"[StopTime]<='"},StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(new string[]{"'"},StringSplitOptions.RemoveEmptyEntries)[0];
                dt=GetTable(strSql, "SQL");
                return dt;
            }


            SqlConnection Conn = new SqlConnection(strConn);
            try
            {
                Conn.Open();

                SqlDataAdapter Cmd = new SqlDataAdapter(strSql, Conn);
                Cmd.SelectCommand.CommandTimeout = 600;
                Cmd.Fill(dt);

            } catch(Exception err)
            {
                var error = err.Message;
            }
            Conn.Close();

            return dt;
         }

        [Obsolete]
        public static DataTable Oracle_ReadTable(String strOracle, string strConn_Oracle)
        {
            DataTable dt1 = new DataTable();
            if (strConn_Oracle.Contains("NSNSFC_GZ"))
            {
                dt1 = GetTable(strOracle, "ORACLE");
                return dt1;
            }

            OracleConnection Conn = new OracleConnection(strConn_Oracle);
            try
            {
               Conn.Open();
               OracleDataAdapter Cmd = new OracleDataAdapter(strOracle, Conn);
               Cmd.SelectCommand.CommandTimeout = 600;
               Cmd.Fill(dt1);
            }
            catch (Exception err)
            {
                var error = err.Message;
            }
            Conn.Close();
            return dt1;
        }

    }
}
