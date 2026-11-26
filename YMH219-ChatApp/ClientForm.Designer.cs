namespace YMH219_ChatApp
{
    partial class ClientForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grpConnection = new GroupBox();
            lblState = new Label();
            btnDisconnect = new Button();
            btnConnect = new Button();
            txtUserName = new TextBox();
            lblUserName = new Label();
            nudPort = new NumericUpDown();
            lblPort = new Label();
            txtHost = new TextBox();
            lblHost = new Label();
            lstMessages = new ListBox();
            grpUsers = new GroupBox();
            lblTarget = new Label();
            lstUsers = new ListBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnSaveHistory = new Button();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            saveFileDialog = new SaveFileDialog();
            grpConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
            grpUsers.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // grpConnection
            //
            grpConnection.Controls.Add(lblState);
            grpConnection.Controls.Add(btnDisconnect);
            grpConnection.Controls.Add(btnConnect);
            grpConnection.Controls.Add(txtUserName);
            grpConnection.Controls.Add(lblUserName);
            grpConnection.Controls.Add(nudPort);
            grpConnection.Controls.Add(lblPort);
            grpConnection.Controls.Add(txtHost);
            grpConnection.Controls.Add(lblHost);
            grpConnection.Location = new Point(12, 8);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new Size(736, 92);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "Bağlantı";
            //
            // lblState
            //
            lblState.BackColor = Color.Gray;
            lblState.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblState.ForeColor = Color.White;
            lblState.Location = new Point(262, 54);
            lblState.Name = "lblState";
            lblState.Size = new Size(462, 28);
            lblState.TabIndex = 8;
            lblState.Text = "BAĞLI DEĞİL";
            lblState.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnDisconnect
            //
            btnDisconnect.Location = new Point(128, 54);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(124, 28);
            btnDisconnect.TabIndex = 7;
            btnDisconnect.Text = "Bağlantıyı Kes";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            //
            // btnConnect
            //
            btnConnect.Location = new Point(12, 54);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(110, 28);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Bağlan";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            //
            // txtUserName
            //
            txtUserName.Location = new Point(554, 23);
            txtUserName.MaxLength = 20;
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(170, 23);
            txtUserName.TabIndex = 5;
            //
            // lblUserName
            //
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(466, 26);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(77, 15);
            lblUserName.TabIndex = 4;
            lblUserName.Text = "Kullanıcı adı:";
            //
            // nudPort
            //
            nudPort.Location = new Point(366, 23);
            nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudPort.Name = "nudPort";
            nudPort.Size = new Size(76, 23);
            nudPort.TabIndex = 3;
            nudPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(326, 26);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(32, 15);
            lblPort.TabIndex = 2;
            lblPort.Text = "Port:";
            //
            // txtHost
            //
            txtHost.Location = new Point(110, 23);
            txtHost.MaxLength = 100;
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(190, 23);
            txtHost.TabIndex = 1;
            txtHost.Text = "127.0.0.1";
            //
            // lblHost
            //
            lblHost.AutoSize = true;
            lblHost.Location = new Point(12, 26);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(86, 15);
            lblHost.TabIndex = 0;
            lblHost.Text = "Sunucu adresi:";
            //
            // lstMessages
            //
            lstMessages.FormattingEnabled = true;
            lstMessages.HorizontalScrollbar = true;
            lstMessages.IntegralHeight = false;
            lstMessages.ItemHeight = 15;
            lstMessages.Location = new Point(12, 110);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(556, 336);
            lstMessages.TabIndex = 1;
            //
            // grpUsers
            //
            grpUsers.Controls.Add(lblTarget);
            grpUsers.Controls.Add(lstUsers);
            grpUsers.Location = new Point(580, 104);
            grpUsers.Name = "grpUsers";
            grpUsers.Size = new Size(168, 342);
            grpUsers.TabIndex = 2;
            grpUsers.TabStop = false;
            grpUsers.Text = "Çevrimiçi (0)";
            //
            // lblTarget
            //
            lblTarget.Location = new Point(10, 298);
            lblTarget.Name = "lblTarget";
            lblTarget.Size = new Size(148, 36);
            lblTarget.TabIndex = 1;
            lblTarget.Text = "Alıcı: Herkes";
            //
            // lstUsers
            //
            lstUsers.FormattingEnabled = true;
            lstUsers.IntegralHeight = false;
            lstUsers.ItemHeight = 15;
            lstUsers.Location = new Point(10, 22);
            lstUsers.Name = "lstUsers";
            lstUsers.Size = new Size(148, 270);
            lstUsers.TabIndex = 0;
            lstUsers.SelectedIndexChanged += lstUsers_SelectedIndexChanged;
            //
            // txtMessage
            //
            txtMessage.Location = new Point(12, 457);
            txtMessage.MaxLength = 500;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(422, 23);
            txtMessage.TabIndex = 3;
            //
            // btnSend
            //
            btnSend.Location = new Point(440, 454);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(128, 28);
            btnSend.TabIndex = 4;
            btnSend.Text = "Mesaj Gönder";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            //
            // btnSaveHistory
            //
            btnSaveHistory.Location = new Point(580, 454);
            btnSaveHistory.Name = "btnSaveHistory";
            btnSaveHistory.Size = new Size(168, 28);
            btnSaveHistory.TabIndex = 5;
            btnSaveHistory.Text = "Geçmişi Kaydet (.txt)";
            btnSaveHistory.UseVisualStyleBackColor = true;
            btnSaveHistory.Click += btnSaveHistory_Click;
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 492);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(760, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 6;
            statusStrip.Text = "statusStrip";
            //
            // lblStatus
            //
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(34, 17);
            lblStatus.Text = "Hazır";
            //
            // saveFileDialog
            //
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Metin dosyası (*.txt)|*.txt";
            saveFileDialog.Title = "Mesaj Geçmişini Kaydet";
            //
            // ClientForm
            //
            AcceptButton = btnSend;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 514);
            Controls.Add(statusStrip);
            Controls.Add(btnSaveHistory);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(grpUsers);
            Controls.Add(lstMessages);
            Controls.Add(grpConnection);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ClientForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YMH219 ChatApp - İstemci";
            FormClosing += ClientForm_FormClosing;
            Load += ClientForm_Load;
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
            grpUsers.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpConnection;
        private Label lblState;
        private Button btnDisconnect;
        private Button btnConnect;
        private TextBox txtUserName;
        private Label lblUserName;
        private NumericUpDown nudPort;
        private Label lblPort;
        private TextBox txtHost;
        private Label lblHost;
        private ListBox lstMessages;
        private GroupBox grpUsers;
        private Label lblTarget;
        private ListBox lstUsers;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnSaveHistory;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private SaveFileDialog saveFileDialog;
    }
}
