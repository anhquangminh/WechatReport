namespace WechatReport
{
    partial class Form1
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Report_btn = new System.Windows.Forms.Button();
            this.Status_notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lb_time = new System.Windows.Forms.Label();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.TestSession = new System.Windows.Forms.Timer(this.components);
            this.grbox = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.grbMessage = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_Count = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grbox.SuspendLayout();
            this.grbMessage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Report_btn
            // 
            this.Report_btn.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Report_btn.Location = new System.Drawing.Point(12, 21);
            this.Report_btn.Name = "Report_btn";
            this.Report_btn.Size = new System.Drawing.Size(108, 55);
            this.Report_btn.TabIndex = 0;
            this.Report_btn.Text = "Start";
            this.Report_btn.UseVisualStyleBackColor = true;
            this.Report_btn.Click += new System.EventHandler(this.Report_btn_Click);
            // 
            // Status_notifyIcon
            // 
            this.Status_notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("Status_notifyIcon.Icon")));
            this.Status_notifyIcon.Text = "Auto Send Wechat";
            this.Status_notifyIcon.Visible = true;
            this.Status_notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Status_notifyIcon_MouseDoubleClick);
            // 
            // lb_time
            // 
            this.lb_time.AutoSize = true;
            this.lb_time.Location = new System.Drawing.Point(4, 21);
            this.lb_time.Name = "lb_time";
            this.lb_time.Size = new System.Drawing.Size(61, 13);
            this.lb_time.TabIndex = 1;
            this.lb_time.Text = "Time Start :";
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(217, 22);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(109, 54);
            this.btn_Stop.TabIndex = 3;
            this.btn_Stop.Text = "Stop";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(154, 38);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(30, 25);
            this.hScrollBar1.TabIndex = 4;
            // 
            // TestSession
            // 
            this.TestSession.Enabled = true;
            this.TestSession.Interval = 3600000;
            this.TestSession.Tick += new System.EventHandler(this.TestSession_Tick);
            // 
            // grbox
            // 
            this.grbox.Controls.Add(this.hScrollBar1);
            this.grbox.Controls.Add(this.btn_Stop);
            this.grbox.Controls.Add(this.Report_btn);
            this.grbox.Location = new System.Drawing.Point(12, 66);
            this.grbox.Name = "grbox";
            this.grbox.Size = new System.Drawing.Size(341, 96);
            this.grbox.TabIndex = 6;
            this.grbox.TabStop = false;
            this.grbox.Text = "Action";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(6, 19);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(329, 112);
            this.txtMessage.TabIndex = 7;
            // 
            // grbMessage
            // 
            this.grbMessage.Controls.Add(this.txtMessage);
            this.grbMessage.Location = new System.Drawing.Point(12, 168);
            this.grbMessage.Name = "grbMessage";
            this.grbMessage.Size = new System.Drawing.Size(341, 142);
            this.grbMessage.TabIndex = 8;
            this.grbMessage.TabStop = false;
            this.grbMessage.Text = "Message Content";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_Count);
            this.groupBox1.Controls.Add(this.lb_time);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 53);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // lb_Count
            // 
            this.lb_Count.AutoSize = true;
            this.lb_Count.Location = new System.Drawing.Point(289, 19);
            this.lb_Count.Name = "lb_Count";
            this.lb_Count.Size = new System.Drawing.Size(41, 13);
            this.lb_Count.TabIndex = 2;
            this.lb_Count.Text = "Count :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 326);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbMessage);
            this.Controls.Add(this.grbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Auto send message to wechat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grbox.ResumeLayout(false);
            this.grbMessage.ResumeLayout(false);
            this.grbMessage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button Report_btn;
        private System.Windows.Forms.NotifyIcon Status_notifyIcon;
        private System.Windows.Forms.Label lb_time;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Timer TestSession;
        private System.Windows.Forms.GroupBox grbox;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox grbMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_Count;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

