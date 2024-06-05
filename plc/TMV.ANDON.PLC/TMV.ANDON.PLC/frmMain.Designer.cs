namespace TMV.ANDON.PLC
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
            this.txtSend_Data_LGW = new System.Windows.Forms.TextBox();
            this.NotifyIconPLC = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btHide = new System.Windows.Forms.ToolStripMenuItem();
            this.btShow = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSockBin_LGW = new System.Windows.Forms.TextBox();
            this.lbReceiveDataPLC = new System.Windows.Forms.Label();
            this.lbSendDataPLC = new System.Windows.Forms.Label();
            this.grpUP = new System.Windows.Forms.GroupBox();
            this.txtPLCManual = new System.Windows.Forms.TextBox();
            this.grpUPManual = new System.Windows.Forms.GroupBox();
            this.chkPUP_W2 = new System.Windows.Forms.CheckBox();
            this.chkLUP_W3 = new System.Windows.Forms.CheckBox();
            this.chkPIK_W1 = new System.Windows.Forms.CheckBox();
            this.chkPIK_W2 = new System.Windows.Forms.CheckBox();
            this.chkPIK_W3 = new System.Windows.Forms.CheckBox();
            this.chkPUP_W1 = new System.Windows.Forms.CheckBox();
            this.chkLUP_W1 = new System.Windows.Forms.CheckBox();
            this.chkLUP_W2 = new System.Windows.Forms.CheckBox();
            this.chkPUP_W3 = new System.Windows.Forms.CheckBox();
            this.chkLDS_FN_A2 = new System.Windows.Forms.CheckBox();
            this.chkLDS_ST_A2 = new System.Windows.Forms.CheckBox();
            this.grpPLCManual = new System.Windows.Forms.GroupBox();
            this.grpLGAManual = new System.Windows.Forms.GroupBox();
            this.chkLDS_LS_A2 = new System.Windows.Forms.CheckBox();
            this.chkLDS_PS_A2 = new System.Windows.Forms.CheckBox();
            this.chkLDS_NT_A2 = new System.Windows.Forms.CheckBox();
            this.chkLDS_LS_A1 = new System.Windows.Forms.CheckBox();
            this.chkLDS_PS_A1 = new System.Windows.Forms.CheckBox();
            this.chkLDS_NT_A1 = new System.Windows.Forms.CheckBox();
            this.grpA1_C_KURU6 = new System.Windows.Forms.GroupBox();
            this.chkBP2_FN_A2 = new System.Windows.Forms.CheckBox();
            this.chkBP2_ST_A2 = new System.Windows.Forms.CheckBox();
            this.chkLDS_ST_A1 = new System.Windows.Forms.CheckBox();
            this.chkLDS_FN_A1 = new System.Windows.Forms.CheckBox();
            this.tmrPLCManual = new System.Windows.Forms.Timer(this.components);
            this.chkManualActive = new System.Windows.Forms.CheckBox();
            this.lblConfig_LGA = new System.Windows.Forms.Label();
            this.tmrLGW = new System.Windows.Forms.Timer(this.components);
            this.lblTiming = new System.Windows.Forms.Label();
            this.tmrNow = new System.Windows.Forms.Timer(this.components);
            this.grpLGA = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSockBin_LGA_A2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSend_Data_LGA_A2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSockBin_LGA_A1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSend_Data_LGA_A1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSockBin_LGA = new System.Windows.Forms.TextBox();
            this.lbPLCReceiveLGA = new System.Windows.Forms.Label();
            this.txtSend_Data_LGA = new System.Windows.Forms.TextBox();
            this.lbPLCSendLGA = new System.Windows.Forms.Label();
            this.tmrLGA = new System.Windows.Forms.Timer(this.components);
            this.lblConfig_LGW = new System.Windows.Forms.Label();
            this.tmrLGA_A1 = new System.Windows.Forms.Timer(this.components);
            this.tmrLGA_A2 = new System.Windows.Forms.Timer(this.components);
            this.grpLWA = new System.Windows.Forms.GroupBox();
            this.txtSockBin_LWA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSend_Data_LWA = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tmrLWA = new System.Windows.Forms.Timer(this.components);
            this.lblConfig_LWA = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            this.grpUP.SuspendLayout();
            this.grpUPManual.SuspendLayout();
            this.grpPLCManual.SuspendLayout();
            this.grpLGAManual.SuspendLayout();
            this.grpA1_C_KURU6.SuspendLayout();
            this.grpLGA.SuspendLayout();
            this.grpLWA.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSend_Data_LGW
            // 
            this.txtSend_Data_LGW.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSend_Data_LGW.Location = new System.Drawing.Point(123, 25);
            this.txtSend_Data_LGW.Name = "txtSend_Data_LGW";
            this.txtSend_Data_LGW.ReadOnly = true;
            this.txtSend_Data_LGW.Size = new System.Drawing.Size(300, 20);
            this.txtSend_Data_LGW.TabIndex = 7;
            // 
            // NotifyIconPLC
            // 
            this.NotifyIconPLC.ContextMenuStrip = this.contextMenu;
            this.NotifyIconPLC.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIconPLC.Icon")));
            this.NotifyIconPLC.Text = "TMV.ANDON.PLC";
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
            // txtSockBin_LGW
            // 
            this.txtSockBin_LGW.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSockBin_LGW.Location = new System.Drawing.Point(123, 48);
            this.txtSockBin_LGW.Name = "txtSockBin_LGW";
            this.txtSockBin_LGW.ReadOnly = true;
            this.txtSockBin_LGW.Size = new System.Drawing.Size(300, 20);
            this.txtSockBin_LGW.TabIndex = 8;
            this.txtSockBin_LGW.Tag = "UP-UP";
            // 
            // lbReceiveDataPLC
            // 
            this.lbReceiveDataPLC.AutoSize = true;
            this.lbReceiveDataPLC.Location = new System.Drawing.Point(3, 51);
            this.lbReceiveDataPLC.Name = "lbReceiveDataPLC";
            this.lbReceiveDataPLC.Size = new System.Drawing.Size(114, 13);
            this.lbReceiveDataPLC.TabIndex = 10;
            this.lbReceiveDataPLC.Text = "Data receive from PLC";
            // 
            // lbSendDataPLC
            // 
            this.lbSendDataPLC.AutoSize = true;
            this.lbSendDataPLC.Location = new System.Drawing.Point(26, 28);
            this.lbSendDataPLC.Name = "lbSendDataPLC";
            this.lbSendDataPLC.Size = new System.Drawing.Size(91, 13);
            this.lbSendDataPLC.TabIndex = 9;
            this.lbSendDataPLC.Text = "Data send to PLC";
            // 
            // grpUP
            // 
            this.grpUP.Controls.Add(this.txtSockBin_LGW);
            this.grpUP.Controls.Add(this.lbReceiveDataPLC);
            this.grpUP.Controls.Add(this.txtSend_Data_LGW);
            this.grpUP.Controls.Add(this.lbSendDataPLC);
            this.grpUP.ForeColor = System.Drawing.Color.ForestGreen;
            this.grpUP.Location = new System.Drawing.Point(5, 74);
            this.grpUP.Name = "grpUP";
            this.grpUP.Size = new System.Drawing.Size(440, 74);
            this.grpUP.TabIndex = 23;
            this.grpUP.TabStop = false;
            this.grpUP.Text = "PLC-LGW";
            // 
            // txtPLCManual
            // 
            this.txtPLCManual.Location = new System.Drawing.Point(8, 19);
            this.txtPLCManual.Name = "txtPLCManual";
            this.txtPLCManual.ReadOnly = true;
            this.txtPLCManual.Size = new System.Drawing.Size(402, 20);
            this.txtPLCManual.TabIndex = 29;
            // 
            // grpUPManual
            // 
            this.grpUPManual.Controls.Add(this.chkPUP_W2);
            this.grpUPManual.Controls.Add(this.chkLUP_W3);
            this.grpUPManual.Controls.Add(this.chkPIK_W1);
            this.grpUPManual.Controls.Add(this.chkPIK_W2);
            this.grpUPManual.Controls.Add(this.chkPIK_W3);
            this.grpUPManual.Controls.Add(this.chkPUP_W1);
            this.grpUPManual.Controls.Add(this.chkLUP_W1);
            this.grpUPManual.Controls.Add(this.chkLUP_W2);
            this.grpUPManual.Controls.Add(this.chkPUP_W3);
            this.grpUPManual.ForeColor = System.Drawing.Color.ForestGreen;
            this.grpUPManual.Location = new System.Drawing.Point(8, 45);
            this.grpUPManual.Name = "grpUPManual";
            this.grpUPManual.Size = new System.Drawing.Size(402, 91);
            this.grpUPManual.TabIndex = 71;
            this.grpUPManual.TabStop = false;
            this.grpUPManual.Text = "LGW";
            // 
            // chkPUP_W2
            // 
            this.chkPUP_W2.AutoSize = true;
            this.chkPUP_W2.Location = new System.Drawing.Point(177, 42);
            this.chkPUP_W2.Name = "chkPUP_W2";
            this.chkPUP_W2.Size = new System.Drawing.Size(68, 17);
            this.chkPUP_W2.TabIndex = 78;
            this.chkPUP_W2.Tag = "000000000000000000011000000000010000010000000000";
            this.chkPUP_W2.Text = "PUP W2";
            this.chkPUP_W2.UseVisualStyleBackColor = true;
            this.chkPUP_W2.CheckedChanged += new System.EventHandler(this.chkUP_STARTWK_CheckedChanged);
            // 
            // chkLUP_W3
            // 
            this.chkLUP_W3.AutoSize = true;
            this.chkLUP_W3.Location = new System.Drawing.Point(304, 19);
            this.chkLUP_W3.Name = "chkLUP_W3";
            this.chkLUP_W3.Size = new System.Drawing.Size(67, 17);
            this.chkLUP_W3.TabIndex = 77;
            this.chkLUP_W3.Tag = "000000000000000000011000010000000000010000000000";
            this.chkLUP_W3.Text = "LUP W3";
            this.chkLUP_W3.UseVisualStyleBackColor = true;
            this.chkLUP_W3.CheckedChanged += new System.EventHandler(this.chkUP_STOPWK_CheckedChanged);
            // 
            // chkPIK_W1
            // 
            this.chkPIK_W1.AutoSize = true;
            this.chkPIK_W1.Location = new System.Drawing.Point(42, 65);
            this.chkPIK_W1.Name = "chkPIK_W1";
            this.chkPIK_W1.Size = new System.Drawing.Size(63, 17);
            this.chkPIK_W1.TabIndex = 76;
            this.chkPIK_W1.Tag = "000000000000000000011000000010000000010000000000";
            this.chkPIK_W1.Text = "PIK W1";
            this.chkPIK_W1.UseVisualStyleBackColor = true;
            this.chkPIK_W1.CheckedChanged += new System.EventHandler(this.chkUP_START2_CheckedChanged);
            // 
            // chkPIK_W2
            // 
            this.chkPIK_W2.AutoSize = true;
            this.chkPIK_W2.Location = new System.Drawing.Point(177, 65);
            this.chkPIK_W2.Name = "chkPIK_W2";
            this.chkPIK_W2.Size = new System.Drawing.Size(63, 17);
            this.chkPIK_W2.TabIndex = 75;
            this.chkPIK_W2.Tag = "000000000000000000011000000001000000010000000000";
            this.chkPIK_W2.Text = "PIK W2";
            this.chkPIK_W2.UseVisualStyleBackColor = true;
            this.chkPIK_W2.CheckedChanged += new System.EventHandler(this.chkUP_FINISH2_CheckedChanged);
            // 
            // chkPIK_W3
            // 
            this.chkPIK_W3.AutoSize = true;
            this.chkPIK_W3.Location = new System.Drawing.Point(304, 65);
            this.chkPIK_W3.Name = "chkPIK_W3";
            this.chkPIK_W3.Size = new System.Drawing.Size(63, 17);
            this.chkPIK_W3.TabIndex = 73;
            this.chkPIK_W3.Tag = "000000000000000000011000000000100000010000000000";
            this.chkPIK_W3.Text = "PIK W3";
            this.chkPIK_W3.UseVisualStyleBackColor = true;
            this.chkPIK_W3.CheckedChanged += new System.EventHandler(this.chkUP_CALLLEADER_CheckedChanged);
            // 
            // chkPUP_W1
            // 
            this.chkPUP_W1.AutoSize = true;
            this.chkPUP_W1.Location = new System.Drawing.Point(42, 42);
            this.chkPUP_W1.Name = "chkPUP_W1";
            this.chkPUP_W1.Size = new System.Drawing.Size(68, 17);
            this.chkPUP_W1.TabIndex = 72;
            this.chkPUP_W1.Tag = "000000000000000000011000100000000000010000000000";
            this.chkPUP_W1.Text = "PUP W1";
            this.chkPUP_W1.UseVisualStyleBackColor = true;
            this.chkPUP_W1.CheckedChanged += new System.EventHandler(this.chkUP_UNCALLLEADER_CheckedChanged);
            // 
            // chkLUP_W1
            // 
            this.chkLUP_W1.AutoSize = true;
            this.chkLUP_W1.Location = new System.Drawing.Point(42, 19);
            this.chkLUP_W1.Name = "chkLUP_W1";
            this.chkLUP_W1.Size = new System.Drawing.Size(67, 17);
            this.chkLUP_W1.TabIndex = 71;
            this.chkLUP_W1.Tag = "000000000000000000011001000000000000010000000000";
            this.chkLUP_W1.Text = "LUP W1";
            this.chkLUP_W1.UseVisualStyleBackColor = true;
            this.chkLUP_W1.CheckedChanged += new System.EventHandler(this.chkUP_START1_CheckedChanged);
            // 
            // chkLUP_W2
            // 
            this.chkLUP_W2.AutoSize = true;
            this.chkLUP_W2.Location = new System.Drawing.Point(177, 19);
            this.chkLUP_W2.Name = "chkLUP_W2";
            this.chkLUP_W2.Size = new System.Drawing.Size(67, 17);
            this.chkLUP_W2.TabIndex = 69;
            this.chkLUP_W2.Tag = "000000000000000000011000000001000000010000000000";
            this.chkLUP_W2.Text = "LUP W2";
            this.chkLUP_W2.UseVisualStyleBackColor = true;
            this.chkLUP_W2.CheckedChanged += new System.EventHandler(this.chkUP_FINISH1_CheckedChanged);
            // 
            // chkPUP_W3
            // 
            this.chkPUP_W3.AutoSize = true;
            this.chkPUP_W3.Location = new System.Drawing.Point(304, 42);
            this.chkPUP_W3.Name = "chkPUP_W3";
            this.chkPUP_W3.Size = new System.Drawing.Size(68, 17);
            this.chkPUP_W3.TabIndex = 68;
            this.chkPUP_W3.Tag = "000000000000000000011000000000010000010000000000";
            this.chkPUP_W3.Text = "PUP W3";
            this.chkPUP_W3.UseVisualStyleBackColor = true;
            this.chkPUP_W3.CheckedChanged += new System.EventHandler(this.chkUP_UNDO_CheckedChanged);
            // 
            // chkLDS_FN_A2
            // 
            this.chkLDS_FN_A2.AutoSize = true;
            this.chkLDS_FN_A2.Location = new System.Drawing.Point(263, 19);
            this.chkLDS_FN_A2.Name = "chkLDS_FN_A2";
            this.chkLDS_FN_A2.Size = new System.Drawing.Size(56, 17);
            this.chkLDS_FN_A2.TabIndex = 74;
            this.chkLDS_FN_A2.Tag = "000000000000000100000000000000000000000000000000";
            this.chkLDS_FN_A2.Text = "FN A2";
            this.chkLDS_FN_A2.UseVisualStyleBackColor = true;
            this.chkLDS_FN_A2.CheckedChanged += new System.EventHandler(this.chkUP_UNPAUSE_CheckedChanged);
            // 
            // chkLDS_ST_A2
            // 
            this.chkLDS_ST_A2.AutoSize = true;
            this.chkLDS_ST_A2.Location = new System.Drawing.Point(178, 19);
            this.chkLDS_ST_A2.Name = "chkLDS_ST_A2";
            this.chkLDS_ST_A2.Size = new System.Drawing.Size(56, 17);
            this.chkLDS_ST_A2.TabIndex = 67;
            this.chkLDS_ST_A2.Tag = "000000100000000000000000000000000000000000000000";
            this.chkLDS_ST_A2.Text = "ST A2";
            this.chkLDS_ST_A2.UseVisualStyleBackColor = true;
            this.chkLDS_ST_A2.CheckedChanged += new System.EventHandler(this.chkUP_PAUSE_CheckedChanged);
            // 
            // grpPLCManual
            // 
            this.grpPLCManual.Controls.Add(this.grpLGAManual);
            this.grpPLCManual.Controls.Add(this.txtPLCManual);
            this.grpPLCManual.Controls.Add(this.grpUPManual);
            this.grpPLCManual.Location = new System.Drawing.Point(460, 74);
            this.grpPLCManual.Name = "grpPLCManual";
            this.grpPLCManual.Size = new System.Drawing.Size(422, 366);
            this.grpPLCManual.TabIndex = 74;
            this.grpPLCManual.TabStop = false;
            this.grpPLCManual.Text = "PLC Manual";
            // 
            // grpLGAManual
            // 
            this.grpLGAManual.Controls.Add(this.chkLDS_LS_A2);
            this.grpLGAManual.Controls.Add(this.chkLDS_PS_A2);
            this.grpLGAManual.Controls.Add(this.chkLDS_NT_A2);
            this.grpLGAManual.Controls.Add(this.chkLDS_LS_A1);
            this.grpLGAManual.Controls.Add(this.chkLDS_PS_A1);
            this.grpLGAManual.Controls.Add(this.chkLDS_NT_A1);
            this.grpLGAManual.Controls.Add(this.grpA1_C_KURU6);
            this.grpLGAManual.ForeColor = System.Drawing.Color.ForestGreen;
            this.grpLGAManual.Location = new System.Drawing.Point(6, 174);
            this.grpLGAManual.Name = "grpLGAManual";
            this.grpLGAManual.Size = new System.Drawing.Size(402, 133);
            this.grpLGAManual.TabIndex = 79;
            this.grpLGAManual.TabStop = false;
            this.grpLGAManual.Text = "LGA";
            // 
            // chkLDS_LS_A2
            // 
            this.chkLDS_LS_A2.AutoSize = true;
            this.chkLDS_LS_A2.Location = new System.Drawing.Point(177, 108);
            this.chkLDS_LS_A2.Name = "chkLDS_LS_A2";
            this.chkLDS_LS_A2.Size = new System.Drawing.Size(55, 17);
            this.chkLDS_LS_A2.TabIndex = 85;
            this.chkLDS_LS_A2.Tag = "000010000000000000000000000000000000000000000000";
            this.chkLDS_LS_A2.Text = "LS A2";
            this.chkLDS_LS_A2.UseVisualStyleBackColor = true;
            this.chkLDS_LS_A2.CheckedChanged += new System.EventHandler(this.chkLDS_LS_A2_CheckedChanged);
            // 
            // chkLDS_PS_A2
            // 
            this.chkLDS_PS_A2.AutoSize = true;
            this.chkLDS_PS_A2.Location = new System.Drawing.Point(96, 108);
            this.chkLDS_PS_A2.Name = "chkLDS_PS_A2";
            this.chkLDS_PS_A2.Size = new System.Drawing.Size(56, 17);
            this.chkLDS_PS_A2.TabIndex = 84;
            this.chkLDS_PS_A2.Tag = "001000000000000000000000000000000000000000000000";
            this.chkLDS_PS_A2.Text = "PS A2";
            this.chkLDS_PS_A2.UseVisualStyleBackColor = true;
            this.chkLDS_PS_A2.CheckedChanged += new System.EventHandler(this.chkLDS_PS_A2_CheckedChanged);
            // 
            // chkLDS_NT_A2
            // 
            this.chkLDS_NT_A2.AutoSize = true;
            this.chkLDS_NT_A2.Location = new System.Drawing.Point(10, 108);
            this.chkLDS_NT_A2.Name = "chkLDS_NT_A2";
            this.chkLDS_NT_A2.Size = new System.Drawing.Size(57, 17);
            this.chkLDS_NT_A2.TabIndex = 83;
            this.chkLDS_NT_A2.Tag = "000100000000000001010111000000000000000000000000";
            this.chkLDS_NT_A2.Text = "NT A2";
            this.chkLDS_NT_A2.UseVisualStyleBackColor = true;
            this.chkLDS_NT_A2.CheckedChanged += new System.EventHandler(this.chkLDS_NT_A2_CheckedChanged);
            // 
            // chkLDS_LS_A1
            // 
            this.chkLDS_LS_A1.AutoSize = true;
            this.chkLDS_LS_A1.Location = new System.Drawing.Point(177, 85);
            this.chkLDS_LS_A1.Name = "chkLDS_LS_A1";
            this.chkLDS_LS_A1.Size = new System.Drawing.Size(55, 17);
            this.chkLDS_LS_A1.TabIndex = 82;
            this.chkLDS_LS_A1.Tag = "000010000000000000000000000000000000000000000000";
            this.chkLDS_LS_A1.Text = "LS A1";
            this.chkLDS_LS_A1.UseVisualStyleBackColor = true;
            this.chkLDS_LS_A1.CheckedChanged += new System.EventHandler(this.chkLDS_LS_A1_CheckedChanged);
            // 
            // chkLDS_PS_A1
            // 
            this.chkLDS_PS_A1.AutoSize = true;
            this.chkLDS_PS_A1.Location = new System.Drawing.Point(96, 85);
            this.chkLDS_PS_A1.Name = "chkLDS_PS_A1";
            this.chkLDS_PS_A1.Size = new System.Drawing.Size(56, 17);
            this.chkLDS_PS_A1.TabIndex = 81;
            this.chkLDS_PS_A1.Tag = "001000000000000000000000000000000000000000000000";
            this.chkLDS_PS_A1.Text = "PS A1";
            this.chkLDS_PS_A1.UseVisualStyleBackColor = true;
            this.chkLDS_PS_A1.CheckedChanged += new System.EventHandler(this.chkLDS_PS_A1_CheckedChanged);
            // 
            // chkLDS_NT_A1
            // 
            this.chkLDS_NT_A1.AutoSize = true;
            this.chkLDS_NT_A1.Location = new System.Drawing.Point(10, 85);
            this.chkLDS_NT_A1.Name = "chkLDS_NT_A1";
            this.chkLDS_NT_A1.Size = new System.Drawing.Size(57, 17);
            this.chkLDS_NT_A1.TabIndex = 80;
            this.chkLDS_NT_A1.Tag = "000100000000000000000000000000000000000000000000";
            this.chkLDS_NT_A1.Text = "NT A1";
            this.chkLDS_NT_A1.UseVisualStyleBackColor = true;
            this.chkLDS_NT_A1.CheckedChanged += new System.EventHandler(this.chkLDS_NT_A1_CheckedChanged);
            // 
            // grpA1_C_KURU6
            // 
            this.grpA1_C_KURU6.Controls.Add(this.chkBP2_FN_A2);
            this.grpA1_C_KURU6.Controls.Add(this.chkBP2_ST_A2);
            this.grpA1_C_KURU6.Controls.Add(this.chkLDS_ST_A1);
            this.grpA1_C_KURU6.Controls.Add(this.chkLDS_FN_A1);
            this.grpA1_C_KURU6.Controls.Add(this.chkLDS_FN_A2);
            this.grpA1_C_KURU6.Controls.Add(this.chkLDS_ST_A2);
            this.grpA1_C_KURU6.Location = new System.Drawing.Point(10, 19);
            this.grpA1_C_KURU6.Name = "grpA1_C_KURU6";
            this.grpA1_C_KURU6.Size = new System.Drawing.Size(386, 60);
            this.grpA1_C_KURU6.TabIndex = 79;
            this.grpA1_C_KURU6.TabStop = false;
            this.grpA1_C_KURU6.Text = "LDS-BP2";
            // 
            // chkBP2_FN_A2
            // 
            this.chkBP2_FN_A2.AutoSize = true;
            this.chkBP2_FN_A2.Location = new System.Drawing.Point(263, 37);
            this.chkBP2_FN_A2.Name = "chkBP2_FN_A2";
            this.chkBP2_FN_A2.Size = new System.Drawing.Size(97, 17);
            this.chkBP2_FN_A2.TabIndex = 80;
            this.chkBP2_FN_A2.Tag = "000000000000000001000000000000000000000000000000";
            this.chkBP2_FN_A2.Text = "FN BP2 UP A2";
            this.chkBP2_FN_A2.UseVisualStyleBackColor = true;
            this.chkBP2_FN_A2.CheckedChanged += new System.EventHandler(this.chkBP2_FN_A2_CheckedChanged);
            // 
            // chkBP2_ST_A2
            // 
            this.chkBP2_ST_A2.AutoSize = true;
            this.chkBP2_ST_A2.Location = new System.Drawing.Point(178, 37);
            this.chkBP2_ST_A2.Name = "chkBP2_ST_A2";
            this.chkBP2_ST_A2.Size = new System.Drawing.Size(97, 17);
            this.chkBP2_ST_A2.TabIndex = 79;
            this.chkBP2_ST_A2.Tag = "000000000000000010000000000000000000000000000000";
            this.chkBP2_ST_A2.Text = "ST BP2 UP A2";
            this.chkBP2_ST_A2.UseVisualStyleBackColor = true;
            this.chkBP2_ST_A2.CheckedChanged += new System.EventHandler(this.chkBP2_ST_A2_CheckedChanged);
            // 
            // chkLDS_ST_A1
            // 
            this.chkLDS_ST_A1.AutoSize = true;
            this.chkLDS_ST_A1.Location = new System.Drawing.Point(12, 19);
            this.chkLDS_ST_A1.Name = "chkLDS_ST_A1";
            this.chkLDS_ST_A1.Size = new System.Drawing.Size(56, 17);
            this.chkLDS_ST_A1.TabIndex = 78;
            this.chkLDS_ST_A1.Tag = "000000001000000000000000000000000000000000000000";
            this.chkLDS_ST_A1.Text = "ST A1";
            this.chkLDS_ST_A1.UseVisualStyleBackColor = true;
            this.chkLDS_ST_A1.CheckedChanged += new System.EventHandler(this.chkStartA1_C_KURU6_CheckedChanged);
            // 
            // chkLDS_FN_A1
            // 
            this.chkLDS_FN_A1.AutoSize = true;
            this.chkLDS_FN_A1.Location = new System.Drawing.Point(97, 19);
            this.chkLDS_FN_A1.Name = "chkLDS_FN_A1";
            this.chkLDS_FN_A1.Size = new System.Drawing.Size(56, 17);
            this.chkLDS_FN_A1.TabIndex = 77;
            this.chkLDS_FN_A1.Tag = "000000010000000000000000000000000000000000000000";
            this.chkLDS_FN_A1.Text = "FN A1";
            this.chkLDS_FN_A1.UseVisualStyleBackColor = true;
            this.chkLDS_FN_A1.CheckedChanged += new System.EventHandler(this.chkFinishA1_C_KURU6_CheckedChanged);
            // 
            // tmrPLCManual
            // 
            this.tmrPLCManual.Interval = 500;
            this.tmrPLCManual.Tick += new System.EventHandler(this.tmrManual_Tick);
            // 
            // chkManualActive
            // 
            this.chkManualActive.AutoSize = true;
            this.chkManualActive.ForeColor = System.Drawing.Color.Red;
            this.chkManualActive.Location = new System.Drawing.Point(369, 51);
            this.chkManualActive.Name = "chkManualActive";
            this.chkManualActive.Size = new System.Drawing.Size(76, 17);
            this.chkManualActive.TabIndex = 75;
            this.chkManualActive.Text = "Manual >>";
            this.chkManualActive.UseVisualStyleBackColor = true;
            this.chkManualActive.CheckedChanged += new System.EventHandler(this.chkManualActive_CheckedChanged);
            // 
            // lblConfig_LGA
            // 
            this.lblConfig_LGA.ForeColor = System.Drawing.Color.Red;
            this.lblConfig_LGA.Location = new System.Drawing.Point(241, 26);
            this.lblConfig_LGA.Name = "lblConfig_LGA";
            this.lblConfig_LGA.Size = new System.Drawing.Size(170, 16);
            this.lblConfig_LGA.TabIndex = 76;
            this.lblConfig_LGA.Text = "DB Info LGA";
            this.lblConfig_LGA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmrLGW
            // 
            this.tmrLGW.Interval = 500;
            this.tmrLGW.Tick += new System.EventHandler(this.tmrLGW_Tick);
            // 
            // lblTiming
            // 
            this.lblTiming.AutoSize = true;
            this.lblTiming.Location = new System.Drawing.Point(12, 6);
            this.lblTiming.Name = "lblTiming";
            this.lblTiming.Size = new System.Drawing.Size(48, 13);
            this.lblTiming.TabIndex = 78;
            this.lblTiming.Text = "lblTiming";
            // 
            // tmrNow
            // 
            this.tmrNow.Interval = 1000;
            this.tmrNow.Tick += new System.EventHandler(this.tmrNow_Tick);
            // 
            // grpLGA
            // 
            this.grpLGA.Controls.Add(this.label6);
            this.grpLGA.Controls.Add(this.label5);
            this.grpLGA.Controls.Add(this.txtSockBin_LGA_A2);
            this.grpLGA.Controls.Add(this.label3);
            this.grpLGA.Controls.Add(this.txtSend_Data_LGA_A2);
            this.grpLGA.Controls.Add(this.label4);
            this.grpLGA.Controls.Add(this.txtSockBin_LGA_A1);
            this.grpLGA.Controls.Add(this.label1);
            this.grpLGA.Controls.Add(this.txtSend_Data_LGA_A1);
            this.grpLGA.Controls.Add(this.label2);
            this.grpLGA.Controls.Add(this.txtSockBin_LGA);
            this.grpLGA.Controls.Add(this.lbPLCReceiveLGA);
            this.grpLGA.Controls.Add(this.txtSend_Data_LGA);
            this.grpLGA.Controls.Add(this.lbPLCSendLGA);
            this.grpLGA.ForeColor = System.Drawing.Color.ForestGreen;
            this.grpLGA.Location = new System.Drawing.Point(5, 245);
            this.grpLGA.Name = "grpLGA";
            this.grpLGA.Size = new System.Drawing.Size(440, 195);
            this.grpLGA.TabIndex = 24;
            this.grpLGA.TabStop = false;
            this.grpLGA.Text = "PLC-LGA";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(7, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "A2-Trim";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(7, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "A1-Trim";
            // 
            // txtSockBin_LGA_A2
            // 
            this.txtSockBin_LGA_A2.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSockBin_LGA_A2.Location = new System.Drawing.Point(123, 161);
            this.txtSockBin_LGA_A2.Name = "txtSockBin_LGA_A2";
            this.txtSockBin_LGA_A2.ReadOnly = true;
            this.txtSockBin_LGA_A2.Size = new System.Drawing.Size(300, 20);
            this.txtSockBin_LGA_A2.TabIndex = 16;
            this.txtSockBin_LGA_A2.Tag = "LGA-LGA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Data receive from PLC";
            // 
            // txtSend_Data_LGA_A2
            // 
            this.txtSend_Data_LGA_A2.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSend_Data_LGA_A2.Location = new System.Drawing.Point(123, 138);
            this.txtSend_Data_LGA_A2.Name = "txtSend_Data_LGA_A2";
            this.txtSend_Data_LGA_A2.ReadOnly = true;
            this.txtSend_Data_LGA_A2.Size = new System.Drawing.Size(300, 20);
            this.txtSend_Data_LGA_A2.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Data send to PLC";
            // 
            // txtSockBin_LGA_A1
            // 
            this.txtSockBin_LGA_A1.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSockBin_LGA_A1.Location = new System.Drawing.Point(123, 99);
            this.txtSockBin_LGA_A1.Name = "txtSockBin_LGA_A1";
            this.txtSockBin_LGA_A1.ReadOnly = true;
            this.txtSockBin_LGA_A1.Size = new System.Drawing.Size(300, 20);
            this.txtSockBin_LGA_A1.TabIndex = 12;
            this.txtSockBin_LGA_A1.Tag = "LGA-LGA";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Data receive from PLC";
            // 
            // txtSend_Data_LGA_A1
            // 
            this.txtSend_Data_LGA_A1.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSend_Data_LGA_A1.Location = new System.Drawing.Point(123, 76);
            this.txtSend_Data_LGA_A1.Name = "txtSend_Data_LGA_A1";
            this.txtSend_Data_LGA_A1.ReadOnly = true;
            this.txtSend_Data_LGA_A1.Size = new System.Drawing.Size(300, 20);
            this.txtSend_Data_LGA_A1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Data send to PLC";
            // 
            // txtSockBin_LGA
            // 
            this.txtSockBin_LGA.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSockBin_LGA.Location = new System.Drawing.Point(123, 36);
            this.txtSockBin_LGA.Name = "txtSockBin_LGA";
            this.txtSockBin_LGA.ReadOnly = true;
            this.txtSockBin_LGA.Size = new System.Drawing.Size(300, 20);
            this.txtSockBin_LGA.TabIndex = 8;
            this.txtSockBin_LGA.Tag = "LGA-LGA";
            // 
            // lbPLCReceiveLGA
            // 
            this.lbPLCReceiveLGA.AutoSize = true;
            this.lbPLCReceiveLGA.Location = new System.Drawing.Point(3, 39);
            this.lbPLCReceiveLGA.Name = "lbPLCReceiveLGA";
            this.lbPLCReceiveLGA.Size = new System.Drawing.Size(114, 13);
            this.lbPLCReceiveLGA.TabIndex = 10;
            this.lbPLCReceiveLGA.Text = "Data receive from PLC";
            // 
            // txtSend_Data_LGA
            // 
            this.txtSend_Data_LGA.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSend_Data_LGA.Location = new System.Drawing.Point(123, 13);
            this.txtSend_Data_LGA.Name = "txtSend_Data_LGA";
            this.txtSend_Data_LGA.ReadOnly = true;
            this.txtSend_Data_LGA.Size = new System.Drawing.Size(300, 20);
            this.txtSend_Data_LGA.TabIndex = 7;
            // 
            // lbPLCSendLGA
            // 
            this.lbPLCSendLGA.AutoSize = true;
            this.lbPLCSendLGA.Location = new System.Drawing.Point(26, 16);
            this.lbPLCSendLGA.Name = "lbPLCSendLGA";
            this.lbPLCSendLGA.Size = new System.Drawing.Size(91, 13);
            this.lbPLCSendLGA.TabIndex = 9;
            this.lbPLCSendLGA.Text = "Data send to PLC";
            // 
            // tmrLGA
            // 
            this.tmrLGA.Interval = 500;
            this.tmrLGA.Tick += new System.EventHandler(this.tmrLGA_Tick);
            // 
            // lblConfig_LGW
            // 
            this.lblConfig_LGW.ForeColor = System.Drawing.Color.Red;
            this.lblConfig_LGW.Location = new System.Drawing.Point(11, 26);
            this.lblConfig_LGW.Name = "lblConfig_LGW";
            this.lblConfig_LGW.Size = new System.Drawing.Size(170, 16);
            this.lblConfig_LGW.TabIndex = 76;
            this.lblConfig_LGW.Text = "DB Info LGW";
            this.lblConfig_LGW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmrLGA_A1
            // 
            this.tmrLGA_A1.Interval = 500;
            this.tmrLGA_A1.Tick += new System.EventHandler(this.tmrLGA_A1_Tick);
            // 
            // tmrLGA_A2
            // 
            this.tmrLGA_A2.Interval = 500;
            this.tmrLGA_A2.Tick += new System.EventHandler(this.tmrLGA_A2_Tick);
            // 
            // grpLWA
            // 
            this.grpLWA.Controls.Add(this.txtSockBin_LWA);
            this.grpLWA.Controls.Add(this.label7);
            this.grpLWA.Controls.Add(this.txtSend_Data_LWA);
            this.grpLWA.Controls.Add(this.label8);
            this.grpLWA.ForeColor = System.Drawing.Color.ForestGreen;
            this.grpLWA.Location = new System.Drawing.Point(5, 161);
            this.grpLWA.Name = "grpLWA";
            this.grpLWA.Size = new System.Drawing.Size(440, 74);
            this.grpLWA.TabIndex = 79;
            this.grpLWA.TabStop = false;
            this.grpLWA.Text = "PLC-LWA";
            // 
            // txtSockBin_LWA
            // 
            this.txtSockBin_LWA.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSockBin_LWA.Location = new System.Drawing.Point(123, 48);
            this.txtSockBin_LWA.Name = "txtSockBin_LWA";
            this.txtSockBin_LWA.ReadOnly = true;
            this.txtSockBin_LWA.Size = new System.Drawing.Size(300, 20);
            this.txtSockBin_LWA.TabIndex = 8;
            this.txtSockBin_LWA.Tag = "UP-UP";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Data receive from PLC";
            // 
            // txtSend_Data_LWA
            // 
            this.txtSend_Data_LWA.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtSend_Data_LWA.Location = new System.Drawing.Point(123, 25);
            this.txtSend_Data_LWA.Name = "txtSend_Data_LWA";
            this.txtSend_Data_LWA.ReadOnly = true;
            this.txtSend_Data_LWA.Size = new System.Drawing.Size(300, 20);
            this.txtSend_Data_LWA.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Data send to PLC";
            // 
            // tmrLWA
            // 
            this.tmrLWA.Interval = 500;
            this.tmrLWA.Tick += new System.EventHandler(this.tmrLWA_Tick);
            // 
            // lblConfig_LWA
            // 
            this.lblConfig_LWA.ForeColor = System.Drawing.Color.Red;
            this.lblConfig_LWA.Location = new System.Drawing.Point(11, 47);
            this.lblConfig_LWA.Name = "lblConfig_LWA";
            this.lblConfig_LWA.Size = new System.Drawing.Size(170, 16);
            this.lblConfig_LWA.TabIndex = 80;
            this.lblConfig_LWA.Text = "DB Info LWA";
            this.lblConfig_LWA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 456);
            this.Controls.Add(this.lblConfig_LWA);
            this.Controls.Add(this.grpLWA);
            this.Controls.Add(this.grpLGA);
            this.Controls.Add(this.chkManualActive);
            this.Controls.Add(this.lblConfig_LGW);
            this.Controls.Add(this.lblConfig_LGA);
            this.Controls.Add(this.grpPLCManual);
            this.Controls.Add(this.grpUP);
            this.Controls.Add(this.lblTiming);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TMV.ANDON.PLC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenu.ResumeLayout(false);
            this.grpUP.ResumeLayout(false);
            this.grpUP.PerformLayout();
            this.grpUPManual.ResumeLayout(false);
            this.grpUPManual.PerformLayout();
            this.grpPLCManual.ResumeLayout(false);
            this.grpPLCManual.PerformLayout();
            this.grpLGAManual.ResumeLayout(false);
            this.grpLGAManual.PerformLayout();
            this.grpA1_C_KURU6.ResumeLayout(false);
            this.grpA1_C_KURU6.PerformLayout();
            this.grpLGA.ResumeLayout(false);
            this.grpLGA.PerformLayout();
            this.grpLWA.ResumeLayout(false);
            this.grpLWA.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSend_Data_LGW;
        internal System.Windows.Forms.NotifyIcon NotifyIconPLC;
        internal System.Windows.Forms.ContextMenuStrip contextMenu;
        internal System.Windows.Forms.ToolStripMenuItem btHide;
        internal System.Windows.Forms.ToolStripMenuItem btShow;
        internal System.Windows.Forms.ToolStripMenuItem bt_Exit;
        private System.Windows.Forms.TextBox txtSockBin_LGW;
        private System.Windows.Forms.Label lbReceiveDataPLC;
        private System.Windows.Forms.Label lbSendDataPLC;
        private System.Windows.Forms.GroupBox grpUP;
        private System.Windows.Forms.TextBox txtPLCManual;
        private System.Windows.Forms.GroupBox grpUPManual;
        private System.Windows.Forms.CheckBox chkLUP_W2;
        private System.Windows.Forms.CheckBox chkPUP_W3;
        private System.Windows.Forms.CheckBox chkLDS_ST_A2;
        private System.Windows.Forms.GroupBox grpPLCManual;
        private System.Windows.Forms.Timer tmrPLCManual;
        private System.Windows.Forms.CheckBox chkManualActive;
        private System.Windows.Forms.Label lblConfig_LGA;
        private System.Windows.Forms.CheckBox chkLUP_W1;
        private System.Windows.Forms.CheckBox chkPIK_W3;
        private System.Windows.Forms.CheckBox chkPUP_W1;
        private System.Windows.Forms.Timer tmrLGW;
        private System.Windows.Forms.Label lblTiming;
        private System.Windows.Forms.Timer tmrNow;
        private System.Windows.Forms.CheckBox chkLDS_FN_A2;
        private System.Windows.Forms.CheckBox chkPIK_W1;
        private System.Windows.Forms.CheckBox chkPIK_W2;
        private System.Windows.Forms.GroupBox grpLGA;
        private System.Windows.Forms.TextBox txtSockBin_LGA;
        private System.Windows.Forms.Label lbPLCReceiveLGA;
        private System.Windows.Forms.TextBox txtSend_Data_LGA;
        private System.Windows.Forms.Label lbPLCSendLGA;
        private System.Windows.Forms.CheckBox chkPUP_W2;
        private System.Windows.Forms.CheckBox chkLUP_W3;
        private System.Windows.Forms.Timer tmrLGA;
        private System.Windows.Forms.GroupBox grpLGAManual;
        private System.Windows.Forms.CheckBox chkLDS_ST_A1;
        private System.Windows.Forms.CheckBox chkLDS_FN_A1;
        private System.Windows.Forms.GroupBox grpA1_C_KURU6;
        private System.Windows.Forms.Label lblConfig_LGW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSockBin_LGA_A2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSend_Data_LGA_A2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSockBin_LGA_A1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSend_Data_LGA_A1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer tmrLGA_A1;
        private System.Windows.Forms.Timer tmrLGA_A2;
        private System.Windows.Forms.CheckBox chkLDS_PS_A1;
        private System.Windows.Forms.CheckBox chkLDS_NT_A1;
        private System.Windows.Forms.CheckBox chkLDS_LS_A1;
        private System.Windows.Forms.CheckBox chkLDS_LS_A2;
        private System.Windows.Forms.CheckBox chkLDS_PS_A2;
        private System.Windows.Forms.CheckBox chkLDS_NT_A2;
        private System.Windows.Forms.CheckBox chkBP2_FN_A2;
        private System.Windows.Forms.CheckBox chkBP2_ST_A2;
        private System.Windows.Forms.GroupBox grpLWA;
        private System.Windows.Forms.TextBox txtSockBin_LWA;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSend_Data_LWA;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer tmrLWA;
        private System.Windows.Forms.Label lblConfig_LWA;
    }
}

