namespace RPA.EXRATE.TOOL
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.NotifyIconPLC = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btHide = new System.Windows.Forms.ToolStripMenuItem();
            this.btShow = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.tmrNow = new System.Windows.Forms.Timer(this.components);
            this.lblTiming = new System.Windows.Forms.Label();
            this.lblConfig_LGW = new System.Windows.Forms.Label();
            this.lblConfig = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.btnOpenLogFile = new System.Windows.Forms.Button();
            this.btnOpenPDF = new System.Windows.Forms.Button();
            this.btnOpenExcel = new System.Windows.Forms.Button();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIconPLC
            // 
            this.NotifyIconPLC.ContextMenuStrip = this.contextMenu;
            this.NotifyIconPLC.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIconPLC.Icon")));
            this.NotifyIconPLC.Text = "RPA.EXRATE.TOOL";
            this.NotifyIconPLC.Visible = true;
            this.NotifyIconPLC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btHide,
            this.btShow,
            this.bt_Exit});
            this.contextMenu.Name = "btShow";
            this.contextMenu.Size = new System.Drawing.Size(105, 70);
            // 
            // btHide
            // 
            this.btHide.Name = "btHide";
            this.btHide.Size = new System.Drawing.Size(104, 22);
            this.btHide.Text = "Ẩn";
            this.btHide.Click += new System.EventHandler(this.btHide_Click);
            // 
            // btShow
            // 
            this.btShow.Name = "btShow";
            this.btShow.Size = new System.Drawing.Size(104, 22);
            this.btShow.Text = "Hiện";
            this.btShow.Click += new System.EventHandler(this.btShow_Click);
            // 
            // bt_Exit
            // 
            this.bt_Exit.Name = "bt_Exit";
            this.bt_Exit.Size = new System.Drawing.Size(104, 22);
            this.bt_Exit.Text = "Thoát";
            this.bt_Exit.Click += new System.EventHandler(this.bt_Exit_Click);
            // 
            // tmrMain
            // 
            this.tmrMain.Enabled = true;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // tmrNow
            // 
            this.tmrNow.Interval = 1000;
            this.tmrNow.Tick += new System.EventHandler(this.tmrNow_Tick);
            // 
            // lblTiming
            // 
            this.lblTiming.AutoSize = true;
            this.lblTiming.Location = new System.Drawing.Point(12, 9);
            this.lblTiming.Name = "lblTiming";
            this.lblTiming.Size = new System.Drawing.Size(48, 13);
            this.lblTiming.TabIndex = 79;
            this.lblTiming.Text = "lblTiming";
            // 
            // lblConfig_LGW
            // 
            this.lblConfig_LGW.ForeColor = System.Drawing.Color.Red;
            this.lblConfig_LGW.Location = new System.Drawing.Point(12, 36);
            this.lblConfig_LGW.Name = "lblConfig_LGW";
            this.lblConfig_LGW.Size = new System.Drawing.Size(170, 16);
            this.lblConfig_LGW.TabIndex = 80;
            this.lblConfig_LGW.Text = "DB Info";
            this.lblConfig_LGW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConfig
            // 
            this.lblConfig.AutoSize = true;
            this.lblConfig.Location = new System.Drawing.Point(74, 38);
            this.lblConfig.Name = "lblConfig";
            this.lblConfig.Size = new System.Drawing.Size(47, 13);
            this.lblConfig.TabIndex = 81;
            this.lblConfig.Text = "lblConfig";
            // 
            // lblState
            // 
            this.lblState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(12, 110);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(30, 13);
            this.lblState.TabIndex = 82;
            this.lblState.Text = "state";
            this.lblState.TextChanged += new System.EventHandler(this.lblState_TextChanged);
            // 
            // btnOpenLogFile
            // 
            this.btnOpenLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenLogFile.Location = new System.Drawing.Point(12, 76);
            this.btnOpenLogFile.Name = "btnOpenLogFile";
            this.btnOpenLogFile.Size = new System.Drawing.Size(74, 22);
            this.btnOpenLogFile.TabIndex = 99;
            this.btnOpenLogFile.Text = "Open Log";
            this.btnOpenLogFile.UseVisualStyleBackColor = true;
            this.btnOpenLogFile.Click += new System.EventHandler(this.btnOpenLogFile_Click);
            // 
            // btnOpenPDF
            // 
            this.btnOpenPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenPDF.Location = new System.Drawing.Point(92, 75);
            this.btnOpenPDF.Name = "btnOpenPDF";
            this.btnOpenPDF.Size = new System.Drawing.Size(110, 23);
            this.btnOpenPDF.TabIndex = 100;
            this.btnOpenPDF.Text = "Open PDF Folder";
            this.btnOpenPDF.UseVisualStyleBackColor = true;
            this.btnOpenPDF.Click += new System.EventHandler(this.btnOpenAttm_Click);
            // 
            // btnOpenExcel
            // 
            this.btnOpenExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenExcel.Location = new System.Drawing.Point(208, 76);
            this.btnOpenExcel.Name = "btnOpenExcel";
            this.btnOpenExcel.Size = new System.Drawing.Size(137, 23);
            this.btnOpenExcel.TabIndex = 101;
            this.btnOpenExcel.Text = "Open Excel Folder";
            this.btnOpenExcel.UseVisualStyleBackColor = true;
            this.btnOpenExcel.Click += new System.EventHandler(this.btnOpenExcel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 132);
            this.Controls.Add(this.btnOpenExcel);
            this.Controls.Add(this.btnOpenPDF);
            this.Controls.Add(this.btnOpenLogFile);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblConfig);
            this.Controls.Add(this.lblConfig_LGW);
            this.Controls.Add(this.lblTiming);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RPA.EXRATE.TOOL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.NotifyIcon NotifyIconPLC;
        internal System.Windows.Forms.ContextMenuStrip contextMenu;
        internal System.Windows.Forms.ToolStripMenuItem btHide;
        internal System.Windows.Forms.ToolStripMenuItem btShow;
        internal System.Windows.Forms.ToolStripMenuItem bt_Exit;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.Timer tmrNow;
        private System.Windows.Forms.Label lblTiming;
        private System.Windows.Forms.Label lblConfig_LGW;
        private System.Windows.Forms.Label lblConfig;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnOpenLogFile;
        private System.Windows.Forms.Button btnOpenPDF;
        private System.Windows.Forms.Button btnOpenExcel;
    }
}

