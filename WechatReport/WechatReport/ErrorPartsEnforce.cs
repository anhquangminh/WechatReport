using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using System.Xml;
using System.Data;

namespace WechatReport
{
    class ErrorPartsEnforce
    {

        public static void StoreErrPartsMessagesIntoDB()
        {
            string message = RequestMessage();
            DataTable dt = new DataTable();
            List<string> errs = new List<string>();
            dt.Columns.Add("ID");
            dt.Columns.Add("Message");
            dt.Columns.Add("PushTime");
            dt.Columns.Add("pending");
            if (!message.Equals(""))
            {
                if (!message.Contains("#"))
                {
                    DataRow dr=dt.NewRow();
                    dr["Message"] = message;
                    dr["PushTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dr["pending"] = 1;
                    dt.Rows.Add(dr);
                }
                else
                {

                    var arr = message.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var m in arr)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Message"] = m;
                        dr["PushTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dr["pending"] = 1;
                        dt.Rows.Add(dr);
                    }
                }

            }
            if (dt.Rows.Count>0)
            {
                DBHelper.SqlBulkCopyByDatatable("[TELOGIN].[dbo].[SMTERRPARTSMEMESSAGE]", dt, errs);
            }
        }
        private static string RequestMessage()
        {
            /*= "http://10.221.68.202:10055/TESTSERVICE.asmx";//10.126.4.22*/
            string url ="";
            string methodname = "GetKPErrorINFO";
            string result = "";
            string err = "";
            result = RequestWebService(url, methodname, ref err);
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
        private static string RequestWebService(string url, string methodname, ref string err)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + methodname);
            request.Method = "GET";
            request.ContentType = "text/xml";
            //HttpWebRequest
            try
            {       
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

    }
}

