using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
namespace WechatReport
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool IscreatingNew;
            Mutex instance = new Mutex(true, "SingleStart", out IscreatingNew);
            if (IscreatingNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                instance.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Program is open");
                Application.Exit();
            }
        }
    }
}
