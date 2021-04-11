using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ini;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace WechatReport
{

    public partial class Form1 : Form
    {
        
        public string db_allpart = "9T7ctBhozbgBluKQi+6syvCLYAMa3RpkWYLd4OU+6deUEx7+MNw6af+ljdkXYKL4XxRM6JR5rIMb0ynec6MI+qN/m6Obzy37vCX3WJdurMKt8uNgUo/SSX+svzhn74RFDNRGT0LFZzx0sSkC3/Wd6w7ItH8A5JdeYecvQo71NAbI7VKVQeVQa6AnIx9xKaZxywy5Wu6AYRS4eHz1G9H29Hja9RZ/9jn7e/adGOK/QNHuflmI7j+gDmv7q5SFFGJxk5vTv1m3OtLUPxsy2krxSg9YmCYEQjR/Q4ZmH5p0kUvNF7+a9EC9qBIDRB+r1oHECa+FfXmLYEANCZo70x3bBg==";
        public string db_app = "Iie2BNMukSayc7OhZUabwlF1U7u7nwPR5Q8G4lijBXm+OeeRqVpF+LOWUCDJF4L/kuEqftutzzzu/k9ISYdQdjp4bm5yHILUE807Yujy4JMS+7CfNzx1tuw1ZWBfDJnQFqzAKdnmGeIz47a8ntDQOoXLR2woES9nezia9qL1102dL+usANxkagle7z+sN0WLicpibrx/Qjm+HrDkpWAYqg==";
        public string db_TH = "HDKAAJPB9HiVJZ0mt9EcjtDsspMpEH00f7Df6J5jJW1rb6v+NgIDZDkG07U/pISGJvcFneHXhGXM4m2ou29EuwjW9gcQ84rEJAoiWwsPzpjcJgrVaj3doy+DdEAQHiae2eQxf8OghQkUlNe3R/4OFvaqZOmVlqBwbopWzcOOKbn40YY6Jbs/2CFTf+uRnRfk3cd/MVAc+dSOBUIF23XEhrwdTdjnvPjF0gCM2hRcc6ofImTSxLlhTdtTcFv03+SeCXk6ZB/yvNgdwzyCebdo+DAFQVeLSDFea7p/D5alTJu8qV/O3UajXXCZFNNfH4RH1NQ8TvAsoFD4hIoSA5VRLA==";
        public string db_113 = "PABzeTi3WVLsQ5H6W+U6tWmKGK/It2J8PwpOY+crvppjM0/gB39CAJiJ4CKK6Eb/mGksN3St1UZdcygALQsMubsT64UjB5W+wEk1qj/sRQwqQYjd5B7zrrW/Q3LUQym30bcUZT7njCqhEwskOCY4ZdohY1780hUlr4DDYEUY3/BB6/cmFZyTYv8p46632HWIKHt4zqDzS32LQVD3vPtIudqEzJYX16/cjyfalzviJQugYnYr69auAbg/zEXafDrh02NMP/M4xn2ICRXbANV4tnVfEVS7h2+GP1daeVU6vmPUrt4HihPAG6HVyWPEaaCgEQu6783MfnK5g6srvncDWA==";
        //public string db_113 = "Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS =(PROTOCOL = TCP)(HOST =10.224.134.113)(PORT = 1527)))(CONNECT_DATA = (SERVICE_NAME = vnsfc)));User Id=SFIS1;Password=vnsfis2014#!";
        public bool issend = false;      
        public bool issend1 = false;      
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 30000;
            timer1.Start();
            //DataTable dt2 = ReportTextUtil.GenerateSharpoclockText(); ;
            //MessageBox.Show("" + dt2.Rows.Count);
        }

        [Obsolete]
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            string datetimenow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string Text = datetimenow.ToString();
            lb_time.Text = "Time :" + Text;
            DateTime dtime = DateTime.Now;
            int hour = dtime.Hour;
            int day = dtime.Day;

            //if (hour == 11 || hour ==13 )
            //{
            //    if (issend1 == false)
            //    {
            //        if(day > 28 && day < 31)
            //        {
            //            _ = ResultAsync();
            //            issend = true;
            //        }
            //    }
            //    issend1 = true;
            //}
            //else
            //{
            //    issend1 = false;
            //}


            SendAlertSMT();
            SendDBAPP();
            

            if (hour == 8)
            {
                if (issend == false)
                {
                    rundaily8H();
                }
                issend = true;
            }
            else
            {
                issend = false;
            }

            timer1.Enabled = true;
            timer1.Interval = 30000;
            timer1.Start();
        }

        //gui tin nhan 8h sang 1 ngay 1 lan
        [Obsolete]
        public void rundaily8H()
        {
            SendAlert_WECHAT_OVER48H();
            SendAlert_FEEDER_75D();
            SendAlert_STENCIL_4H();
            SendAlert_SOLDER_24H();
            SendAlert_TEM_HUMI();
            SendAlert_FEEDER_1TRIEU();
            SendAlert_TURN_INSP();
            SendAlert_NIC_MSD();
            SendAlert_MRB_GW();
            issend = true;
        }

        //canh bao allpart
        [Obsolete]
        public void SendAlertSMT() {
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    //string sql = "select * from MES4.R_SYSTEM_LOG where emp_no ='APP_AUTO' and (ACTION_TYPE='0' or ACTION_TYPE ='IPQC')";
                    string sql = "select * from MES4.R_SYSTEM_LOG where PRG_NAME='ALERT WECHAT' and (ACTION_TYPE='0' or ACTION_TYPE ='IPQC') and( ACTION_DESC like '%OK B3%' or ACTION_DESC like 'Model: F7%' or ACTION_DESC like 'Model: F5%' or ACTION_DESC like 'Model: F8%' or ACTION_DESC like 'Model: V5%' or ACTION_DESC like 'Model: V7%' or ACTION_DESC like 'Model: V8%') order by TIME desc ";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[3].ToString();
                                    //lb_message.Text = "Message :" + Message;
                                    txtMessage.Text = Message;
                                    string group ;   
                                    
                                    group = "NIC SMT ALERT";
                                    autosendsingleMessage("Alarm : " + Message, group);

                                    string query_update = ("update MES4.R_SYSTEM_LOG set ACTION_TYPE ='1' where ACTION_DESC= '" + dr[3].ToString() + "'");
                                    OracleCommand cmd1 = con.CreateCommand();
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.CommandText = query_update;
                                    cmd1.ExecuteNonQuery();                                   
                                }
                                lb_Count.Text = count.ToString();
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable." + ex.Message;
                }
            }
        }
        //canh bao qua 48h VW_NIC_WECHAT_OVER48H
        [Obsolete]
        public void SendAlert_WECHAT_OVER48H()
        {
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {                  
                    con.Open();
                    string sql = "SELECT * FROM VW_NIC_WECHAT_OVER48H ";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    //string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string time = dr[2].ToString();
                                    string Message = dr[0].ToString()+" ,"+dr[1].ToString()+" ,"+dr[2].ToString()+" ,"+dr[3].ToString();
                                    allMessage = allMessage+"\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if(allMessage != "")
                                {
                                    autosendsingleMessage("Over 48H :  DOC_NO , CUST_KP_NO ,RECEIVE_TIME ,TOTAL_TIME \n" + allMessage, group);
                                }                    
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable OVER48H" + ex.Message;
                }
            }
        }

        //tu dong gui solder 24h, VW_NIC_WECHAT_SOLDER_24
        [Obsolete]
        public void SendAlert_SOLDER_24H()
        {
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select STATION_NAME,IP,TR_SN,DIFF_TIME,WORK_TIME from VW_NIC_WECHAT_SOLDER_24";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString() + ", " + dr[4].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if (allMessage != "")
                                {
                                    autosendsingleMessage("SOLDER_24H  : STATION_NAME, IP, TR_SN, DIFF_TIME, WORK_TIME \n" + allMessage, group);
                                }           
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable SOLDER_24H" + ex.Message;
                }
            }
        }

        //VW_NIC_WECHAT_STENCIL_4H
        [Obsolete]
        public void SendAlert_STENCIL_4H()
        {
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select STATION_NAME,IP,STENCIL_SN,DIFF_TIME from VW_NIC_WECHAT_STENCIL_4H";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }

                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if (allMessage != "")
                                {
                                    autosendsingleMessage("STENCIL_4H  : STATION_NAME, IP, STENCIL_SN, DIFF_TIME \n" + allMessage, group);
                                }
                                
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable STENCIL_4H" + ex.Message;
                }
            }
        }

        //FEEDER 75H select FEEDER_NO,EDIT_TIME,LAST_STATION from VW_NIC_WECHAT_FEEDER_75;
        [Obsolete]
        public void SendAlert_FEEDER_75D()
        {
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select FEEDER_NO, EDIT_TIME, LAST_STATION from VW_NIC_WECHAT_FEEDER_75";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if (allMessage != "")
                                {
                                    autosendsingleMessage("FEEDER_75DAY  :  FEEDER_NO, EDIT_TIME, LAST_STATION \n" + allMessage, group);
                                }
                                
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable : FEEDER_75H " + ex.Message;
                }
            }
        }

        //Feeder 1trieu giay 
        [Obsolete]
        public void SendAlert_FEEDER_1TRIEU()
        {
            string a = Decrypt(db_allpart, "vietnamnumberone");
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select FEEDER_NO, FEEDER_SN, TOTAL_USE_TIMES, WORK_TIME from VW_NIC_WECHAT_FEEDER_1trieu";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if (allMessage != "")
                                {
                                    autosendsingleMessage("Feeder use times > 1.000.000 :  FEEDER_NO, FEEDER_SN, TOTAL_USE_TIMES, WORK_TIME \n" + allMessage, group);
                                }
                                
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable :  Feeder use times > 1.000.000 " + ex.Message;
                }
            }
        }

        //tu dong send TURN_INSP , quan che thoi gian tu tram nay den tram kia 
        [Obsolete]
        public void SendAlert_TURN_INSP()
        {         
            using (OracleConnection con = new OracleConnection(Decrypt(db_113, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select SERIAL_NUMBER, MODEL_NAME, MO_NUMBER, STATION, OVER_TIME from VW_NIC_WECHAT_SMT_TURN_INSP";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString() + ", " + dr[4].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if(allMessage != "")
                                {
                                    autosendsingleMessage("OSP : SERIAL_NUMBER, MODEL_NAME, MO_NUMBER, STATION, OVER_TIME \n" + allMessage, group);
                                }
                                
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable :  TURN_INSP" + ex.Message;
                }
            }
        }

        // tu dong gui  thoi gian quan che cac con lieu nhay cam
        [Obsolete]
        public void SendAlert_NIC_MSD()
        {
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select TR_SN ,CUST_KP_NO, LOCATION_FLAG, MSD_TYPE, USE_TIME, LEFT_TIME, LIMIT_TIME, START_TIME from VW_NIC_WECHAT_MSD";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString() + ", " + dr[4].ToString() + ", " + dr[5].ToString() + ", " + dr[6].ToString() + ", " + dr[7].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if (allMessage !="")
                                {
                                    autosendsingleMessage("MSD : TR_SN ,CUST_KP_NO, LOCATION_FLAG, MSD_TYPE, USE_TIME, LEFT_TIME, LIMIT_TIME, START_TIME \n" + allMessage, group);
                                }                               
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable :  TURN_INSP" + ex.Message;
                }
            }
        }
        [Obsolete]
        public void SendAlert_TEM_HUMI()
        {
            using (OracleConnection con = new OracleConnection(Decrypt(db_TH, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select BLOCK_NAME, LINE_NAME, TEMPERATURE, HUMIDITY, SAMPLETIME from VW_NIC_WECHAT_TEM_HUMI ";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString() + ", " + dr[4].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "NIC SMT ALERT";
                                if (allMessage != "")
                                {
                                    autosendsingleMessage("Temperature & Humidity : BLOCK_NAME, LINE_NAME, TEMPERATURE, HUMIDITY, SAMPLETIME \n" + allMessage, group);
                                }
                            }          
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable : FEEDER_75H " + ex.Message;
                }
            }
        }
        //lieu ton kho
        [Obsolete]
        public void SendAlert_MRB_GW()
        {        
            using (OracleConnection con = new OracleConnection(Decrypt(db_allpart, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "select  LOCATON, PLANT, CUST_KP_NO, STOCK_LOCATION, SAP_QTY, INSPECT_QTY, WORK_TIME from WECHAT_MRB";
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString() + ", " + dr[4].ToString() + ", " + dr[5].ToString() + ", " + dr[6].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                string group = "MRB ALERT";
                                if (allMessage != "")
                                {
                                    autosendsingleMessage("MRB :LOCATON, PLANT, CUST_KP_NO, STOCK_LOCATION, SAP_QTY, INSPECT_QTY, WORK_TIME \n" + allMessage, group);
                                }
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "The database is unavailable : MRB_GW" + ex.Message;
                }
            }
        }

        [Obsolete]
        public void SendQuery(string dbName,string query,string title,string group, int rows)
        {
            using (OracleConnection con = new OracleConnection(Decrypt(dbName, "vietnamnumberone")))
            {
                try
                {
                    con.Open();                  
                    using (OracleCommand cmd = new OracleCommand(query, con))
                    {
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                string allMessage = "";
                                while (dr.Read())
                                {
                                    count++;
                                    string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                    string Message="";
                                    for (int i = 0; i < rows; i++)
                                    {
                                        if (i == (rows - 1))
                                        {
                                            Message = Message + dr[i].ToString();
                                        }
                                        else
                                        {
                                            Message = Message + dr[i].ToString() + ", ";
                                        }     
                                    }
                                    //Message = dr[0].ToString() + " ," + dr[1].ToString() + ", " + dr[2].ToString() + ", " + dr[3].ToString() + ", " + dr[4].ToString() + ", " + dr[5].ToString() + ", " + dr[6].ToString() + ", " + dr[7].ToString();
                                    allMessage = allMessage + "\n" + Message + " . ";
                                }
                                lb_Count.Text = count.ToString();
                                txtMessage.Text = allMessage;
                                
                                if (allMessage != "")
                                {
                                    autosendsingleMessage(title+"\n" + allMessage, group);
                                }
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    //MessageBox.Show("The database is unavailable." + ex.Message);
                    txtMessage.Text = "Erro :"+ query +" ," + ex.Message;
                }
            }
        }
        public void SendDBAPP()
        {
            using (SqlConnection con = new SqlConnection(Decrypt(db_app, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "SELECT * FROM MessageContents where status =0";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count=0;
                                while (dr.Read())
                                {
                                    count++;
                                    string datetimenow2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); 
                                    string message = "Send to: " + dr[1].ToString() + " | Message :" + dr[2].ToString();
                                    //lb_message.Text = message;
                                    txtMessage.Text = message;

                                    autosendsingleMessage(dr[2].ToString(), dr[1].ToString());
                                    string query_update = ("update MessageContents set status ='1' where id= '" + dr[0].ToString() + "'");
                                    UpdateStatus(query_update);
                                }
                                lb_Count.Text = count.ToString();
                            }
                        }
                            
                    }
                    con.Close();
                    con.Dispose();

                }
                catch (OracleException ex) 
                {
                    //MessageBox.Show("Erro :" +ex.Message);
                    txtMessage.Text = "The database is unavailable." + ex.Message;
                }

            }
        }
        public void UpdateStatus(string query)
        {
            using (SqlConnection con = new SqlConnection(Decrypt(db_app, "vietnamnumberone")))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
        public string getIni()
        {
            string filePath2 = AppDomain.CurrentDomain.BaseDirectory + "\\group.ini"; //1
            string Group="";
            if (System.IO.File.Exists(filePath2))
            {
                IniFile ini = new IniFile(filePath2);
                 Group = ini.IniReadValue("group", "group_name");
                return Group;
            }
            return Group;
            //ini.IniWriteValue("send", "issend", newText);
        }
        public void writeIni(string save,string newText)
        {
            string filePath2 = AppDomain.CurrentDomain.BaseDirectory + "\\group.ini";
            if (System.IO.File.Exists(filePath2))
            {
                IniFile ini = new IniFile(filePath2);
                ini.IniWriteValue("log",save , newText);
            }
            
        }
        private void autosendMultiGroup(string messages, string[] group)
        {
            foreach (string gr in group)
            {
                autosendsingleMessage(messages, gr);
            }
        }
        private void autosendMultiMessage(string[] messages, string group)
        {
            foreach (string msg in messages)
            {
                autosendsingleMessage(msg, group);
            }
        }
        private void autosendsingleMessage(string message, string group)
        {
            WechatHelper.Send(group, message);
        }

        [Obsolete]
        private void Report_btn_Click(object sender, EventArgs e)
        {
            
            string datetimenow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string Text = datetimenow.ToString();
            writeIni("TimeStart", Text);
            lb_time.Text = "Time :" + Text;
            DateTime dtime = DateTime.Now;

            //SendQuery(db_allpart, "select * from WECHAT_MRB_GW", "MRB : PLANT, CUST_KP_NO, STOCK_LOCATION, SAP_QTY, INSPECT_QTY, WORK_TIME", "MRB ALERT",6);
            //Check_Location();
            SendDBAPP();
            SendAlertSMT();


            timer1.Enabled = true;
            timer1.Start();
        }
        public async System.Threading.Tasks.Task ResultAsync()
        {
            using (SqlConnection con = new SqlConnection(Decrypt(db_app, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    List<inforEmp> infor = new List<inforEmp>();
                    string sql = "select * from Employ";
                    string datetime = DateTime.Now.ToString("yyyy/MM/dd");
                    int count = 0;
                    int count1 = 0;
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    count++;
                                    List<Location> location;
                                    location = await PostDataAsync(dr[0].ToString().Trim(), "2021-01-01", datetime);
                                    if (location == null)
                                    {
                                        count1++;
                                        inforEmp inforemp = new inforEmp();
                                        inforemp.ID_Card = dr[0].ToString();
                                        inforemp.Name = dr[2].ToString();
                                        inforemp.Location = "null";
                                        inforemp.Warring = "No search location";
                                        infor.Add(inforemp);
                                    }
                                    else
                                    {
                                        foreach (var lc in location)
                                        {
                                            Boolean ResultCompare = CompareLocation(lc.civetno, lc.p_latitude, lc.p_longitude);
                                            if (ResultCompare == true)
                                            {
                                                count1++;
                                                inforEmp inforemp = new inforEmp();
                                                string locate = lc.p_latitude.ToString()+", "+lc.p_longitude.ToString();
                                                inforemp.ID_Card = lc.civetno;
                                                inforemp.Name = dr[2].ToString();
                                                inforemp.Location = locate;
                                                inforemp.Warring = "out range";
                                                infor.Add(inforemp);
                                                break;
                                            }
                                        }
                                    }
                                    lb_Count.Text = count.ToString();
                                }

                               lb_Count.Text = count1.ToString();
                               string allmessage = "";
                               foreach(var item in infor)
                                {
                                    allmessage += item.ID_Card + " , " + item.Name + " , " + item.Location + " , " + item.Warring + " . \n";
                                }
                               if(allmessage != "")
                                {
                                    txtMessage.Text = "Out of range : ID, Name, Location , Note \n" +allmessage;

                                    string group = "Alert detect location";
                                    //autosendsingleMessage("Out of range "+datetime+" : ID, Name, Location , Note \n" + allmessage, group);
                                }
                                else
                                {
                                    txtMessage.Text = "no conter";
                                }
                            }
                        }

                    }
                    con.Close();
                    con.Dispose();

                }
                catch (OracleException ex)
                {
                    //MessageBox.Show("Erro :" +ex.Message);
                    txtMessage.Text = "The database is unavailable." + ex.Message;
                }
            }
        }

        public Boolean CompareLocation(string IDCard, double lckd,double lcvd)
        {
            
            string all_message = "";
            double kd1 = 21.636764;
            double vd1 = 105.582947;
            double kd2 = 20.661204;
            double vd2 = 106.646345;

            if (lckd < kd2 || lckd > kd1 || lcvd < vd1 || lcvd > vd2)
            {
                
                all_message = all_message + IDCard + " : " + lckd.ToString() + ", " + lcvd.ToString() + " . \n";
                return true;
            }

            if (all_message != "")
            {
                //autosendsingleMessage("Out of range:  "+all_message, group);
                txtMessage.Text = all_message;
                
            }
            return false;
           
        }
        public void Check_Location()
        {
            using (SqlConnection con = new SqlConnection(Decrypt(db_app, "vietnamnumberone")))
            {
                try
                {
                    con.Open();
                    string sql = "SELECT * FROM Maplocation where Status ='0'";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {

                                List<string> err = new List<string>();
                                int count = 0;
                                string datetimenow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                string all_message = "";
                                while (dr.Read())
                                {                                 
                                    double kd1 = 21.636764;
                                    double vd1 = 105.582947;
                                    double kd2 = 20.661204;
                                    double vd2 = 106.646345;

                                    string IDCard = dr[1].ToString().Trim();
                                    string Loction = dr[3].ToString().Trim();
                                    string[] tokens = Loction.Split(',');
                                    string last = tokens[tokens.Length - 1];

                                    double lckd = Convert.ToDouble(tokens[0].Trim());
                                    double lcvd = Convert.ToDouble(tokens[1].Trim());
                                    double kinhdo;
                                    double vido;
                                    
                                    if(lckd<kd2 || lckd > kd1 || lcvd < vd1 || lcvd > vd2)
                                    {
                                        count++;
                                        kinhdo = lckd; 
                                        vido = lcvd;
                                        all_message = all_message + IDCard + " : " + kinhdo.ToString() + ", " + vido.ToString() +" . \n";
                                        
                                    }
                                    string query_update = ("update Maplocation set status ='1' where id= '" + dr[0].ToString() + "'");
                                    UpdateStatus(query_update);
                                }
                                string group = "Alert Location";
                                if (all_message != "")
                                {
                                    autosendsingleMessage("Out of range: " + datetimenow + " \n" + all_message, group);
                                    txtMessage.Text = all_message;
                                }
                            }
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (OracleException ex)
                {
                    txtMessage.Text = "The database is unavailable." + ex.Message;
                }

            }
        }
      
        public async System.Threading.Tasks.Task<List<Location>> PostDataAsync (string civetno,string start_date,string end_date)
        {
            string uri = "http://icivetapps.foxconn.com/SaaS/Attendance/000017/Interface/GetAttendanceResult";
            var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("key", "CNSBG_Training"),
                    new KeyValuePair<string, string>("start_date",start_date),
                    new KeyValuePair<string, string>("end_date", end_date),
                    new KeyValuePair<string, string>("civetno", civetno),
                    new KeyValuePair<string, string>("pageId", "1"),
                    new KeyValuePair<string, string>("pageSize", "10000"),
                    new KeyValuePair<string, string>("status", "-1")
                });

            var myHttpClient = new HttpClient();
            myHttpClient.Timeout = TimeSpan.FromSeconds(10);
            var response = await myHttpClient.PostAsync(uri.ToString(), formContent);
    
            var stringContent = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode && !string.IsNullOrEmpty(stringContent))
            {
                var locationData = Newtonsoft.Json.JsonConvert.DeserializeObject<LocationData>(stringContent);
                Console.WriteLine(stringContent);
                Console.WriteLine(locationData.message);
                return locationData.data;
            }
            else
            {
                return null;
            }
            

        }
        public void getLocation()
        {
            //string URL = "http://10.224.81.136:6868/api/values";
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(URL);
            //Location location;

            //client.DefaultRequestHeaders.Accept.Add(
            //new MediaTypeWithQualityHeaderValue("application/json"));

            //HttpResponseMessage response = client.GetAsync("?username=10.228.26.110").Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            //if (response.IsSuccessStatusCode)
            //{
            //    location = await response.Content.ReadAsAsync<Location>();
            //    string a = location.USERNAME;
            //}
            //else
            //{
            //    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            //}
            //client.Dispose();

        }
        private void stoppage_btn_Click(object sender, EventArgs e)
        {
            //Khối đầu tiên

            //timer1.Enabled = false;
            //timer1.Stop();
            //DateTime t1 = DateTime.Now;
            //List<string> textlist = ReportTextUtil.SITestFailureStoppageText();
            //int size = textlist.Count;
            //TimeSpan tspan = DateTime.Now - t1;

            //foreach (var text in textlist)
            //{
            //    if (!text.EndsWith("System send)\r\n"))
            //    {
            //        autosendsingleMessage(text, "Close");
            //    }
            //}

            //Đóng cửa sổ WeChat
            WechatHelper.CloseMainWin();

            //Giải phóng 
            timer1.Enabled = true;
            timer1.Start();

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                Status_notifyIcon.Visible = true;
                this.Hide();
            }
            else { Application.Exit(); }
        }
        private void Status_notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            this.Show();
            this.WindowState = FormWindowState.Normal;
            Status_notifyIcon.Visible = false;
            //}
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            WechatHelper.CloseMainWin();
            timer1.Enabled = false;
            timer1.Stop();
            Application.Exit();
            this.Close();
        }
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;
        //public static string Encrypt(string plainText, string passPhrase)
        //{
        //    var saltStringBytes = Generate256BitsOfRandomEntropy();
        //    var ivStringBytes = Generate256BitsOfRandomEntropy();
        //    var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        //    using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        //    {
        //        var keyBytes = password.GetBytes(Keysize / 8);
        //        using (var symmetricKey = new RijndaelManaged())
        //        {
        //            symmetricKey.BlockSize = 256;
        //            symmetricKey.Mode = CipherMode.CBC;
        //            symmetricKey.Padding = PaddingMode.PKCS7;
        //            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
        //            {
        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        //                    {
        //                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        //                        cryptoStream.FlushFinalBlock();
        //                        var cipherTextBytes = saltStringBytes;
        //                        cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
        //                        cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
        //                        memoryStream.Close();
        //                        cryptoStream.Close();
        //                        return Convert.ToBase64String(cipherTextBytes);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static string Decrypt(string cipherText, string passPhrase)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
        private void TestSession_Tick(object sender, EventArgs e)
        {
            string datetimenow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            autosendsingleMessage("Time send :" + datetimenow, "光明");
        }
    }
}





