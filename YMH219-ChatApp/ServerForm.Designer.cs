namespace YMH219_ChatApp
{
    partial class ServerForm
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
            grpServer = new GroupBox();
            lblAddresses = new Label();
            lblState = new Label();
            btnStop = new Button();
            btnStart = new Button();
            nudPort = new NumericUpDown();
            lblPort = new Label();
            lstMessages = new ListBox();
            grpClients = new GroupBox();
            lvClients = new ListView();
            colUser = new ColumnHeader();
            colAddress = new ColumnHeader();
            colTime = new ColumnHeader();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnSaveHistory = new Button();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            saveFileDialog = new SaveFileDialog();
            grpServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
            grpClients.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // grpServer
            //
            grpServer.Controls.Add(lblAddresses);
            grpServer.Controls.Add(lblState);
            grpServer.Controls.Add(btnStop);
            grpServer.Controls.Add(btnStart);
            grpServer.Controls.Add(nudPort);
            grpServer.Controls.Add(lblPort);
            grpServer.Location = new Point(12, 8);
            grpServer.Name = "grpServer";
            grpServer.Size = new Size(796, 88);
            grpServer.TabIndex = 0;
            grpServer.TabStop = false;
            grpServer.Text = "Sunucu";
            //
            // lblAddresses
            //
            lblAddresses.Location = new Point(12, 60);
            lblAddresses.Name = "lblAddresses";
            lblAddresses.Size = new Size(772, 18);
            lblAddresses.TabIndex = 5;
            lblAddresses.Text = "Bu bilgisayarın ağ adresi:";
            //
            // lblState
            //
            lblState.BackColor = Color.Gray;
            lblState.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblState.ForeColor = Color.White;
            lblState.Location = new Point(412, 22);
            lblState.Name = "lblState";
            lblState.Size = new Size(372, 28);
            lblState.TabIndex = 4;
            lblState.Text = "SUNUCU KAPALI";
            lblState.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnStop
            //
            btnStop.Location = new Point(272, 22);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(130, 28);
            btnStop.TabIndex = 3;
            btnStop.Text = "Sunucuyu Durdur";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            //
            // btnStart
            //
            btnStart.Location = new Point(136, 22);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(130, 28);
            btnStart.TabIndex = 2;
            btnStart.Text = "Sunucuyu Başlat";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            //
            // nudPort
            //
            nudPort.Location = new Point(52, 25);
            nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudPort.Name = "nudPort";
            nudPort.Size = new Size(72, 23);
            nudPort.TabIndex = 1;
            nudPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(12, 28);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(32, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "Port:";
            //
            // lstMessages
            //
            lstMessages.FormattingEnabled = true;
            lstMessages.HorizontalScrollbar = true;
            lstMessages.IntegralHeight = false;
            lstMessages.ItemHeight = 15;
            lstMessages.Location = new Point(12, 106);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(500, 360);
            lstMessages.TabIndex = 1;
            //
            // grpClients
            //
            grpClients.Controls.Add(lvClients);
            grpClients.Location = new Point(524, 100);
            grpClients.Name = "grpClients";
            grpClients.Size = new Size(284, 366);
            grpClients.TabIndex = 2;
            grpClients.TabStop = false;
            grpClients.Text = "Bağlı İstemciler (0)";
            //
            // lvClients
            //
            lvClients.Columns.AddRange(new ColumnHeader[] { colUser, colAddress, colTime });
            lvClients.FullRowSelect = true;
            lvClients.Location = new Point(10, 22);
            lvClients.Name = "lvClients";
            lvClients.Size = new Size(264, 334);
            lvClients.TabIndex = 0;
            lvClients.UseCompatibleStateImageBehavior = false;
            lvClients.View = View.Details;
            //
            // colUser
            //
            colUser.Text = "Kullanıcı";
            colUser.Width = 84;
            //
            // colAddress
            //
            colAddress.Text = "Adres";
            colAddress.Width = 112;
            //
            // colTime
            //
            colTime.Text = "Bağlanma";
            colTime.Width = 64;
            //
            // txtMessage
            //
            txtMessage.Location = new Point(12, 477);
            txtMessage.MaxLength = 500;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(366, 23);
            txtMessage.TabIndex = 3;
            //
            // btnSend
            //
            btnSend.Location = new Point(384, 474);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(128, 28);
            btnSend.TabIndex = 4;
            btnSend.Text = "Herkese Gönder";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            //
            // btnSaveHistory
            //
            btnSaveHistory.Location = new Point(524, 474);
            btnSaveHistory.Name = "btnSaveHistory";
            btnSaveHistory.Size = new Size(284, 28);
            btnSaveHistory.TabIndex = 5;
            btnSaveHistory.Text = "Mesaj Geçmişini Kaydet (.txt)";
            btnSaveHistory.UseVisualStyleBackColor = true;
            btnSaveHistory.Click += btnSaveHistory_Click;
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 512);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(820, 22);
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
            // ServerForm
            //
            AcceptButton = btnSend;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 534);
            Controls.Add(statusStrip);
            Controls.Add(btnSaveHistory);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(grpClients);
            Controls.Add(lstMessages);
            Controls.Add(grpServer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ServerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YMH219 ChatApp - Sunucu";
            FormClosing += ServerForm_FormClosing;
            Load += ServerForm_Load;
            grpServer.ResumeLayout(false);
            grpServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
            grpClients.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpServer;
        private Label lblAddresses;
        private Label lblState;
        private Button btnStop;
        private Button btnStart;
        private NumericUpDown nudPort;
        private Label lblPort;
        private ListBox lstMessages;
        private GroupBox grpClients;
        private ListView lvClients;
        private ColumnHeader colUser;
        private ColumnHeader colAddress;
        private ColumnHeader colTime;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnSaveHistory;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private SaveFileDialog saveFileDialog;
    }
}
