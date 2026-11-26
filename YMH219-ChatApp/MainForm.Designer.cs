namespace YMH219_ChatApp
{
    partial class MainForm
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
            lblTitle = new Label();
            lblInfo = new Label();
            btnOpenServer = new Button();
            btnOpenClient = new Button();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(18, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(241, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "YMH219 Chat Uygulaması";
            //
            // lblInfo
            //
            lblInfo.Location = new Point(22, 54);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(396, 82);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "TCP üzerinden çalışan istemci-sunucu sohbet uygulaması.\r\n\r\nÖnce bir sunucu penceresi açıp sunucuyu başlatın, sonra bir veya daha fazla istemci penceresiyle bağlanın. Farklı bilgisayarlarda denemek için uygulamayı her bilgisayarda çalıştırın.";
            //
            // btnOpenServer
            //
            btnOpenServer.Location = new Point(22, 146);
            btnOpenServer.Name = "btnOpenServer";
            btnOpenServer.Size = new Size(190, 44);
            btnOpenServer.TabIndex = 2;
            btnOpenServer.Text = "Sunucu Penceresi Aç";
            btnOpenServer.UseVisualStyleBackColor = true;
            btnOpenServer.Click += btnOpenServer_Click;
            //
            // btnOpenClient
            //
            btnOpenClient.Location = new Point(228, 146);
            btnOpenClient.Name = "btnOpenClient";
            btnOpenClient.Size = new Size(190, 44);
            btnOpenClient.TabIndex = 3;
            btnOpenClient.Text = "İstemci Penceresi Aç";
            btnOpenClient.UseVisualStyleBackColor = true;
            btnOpenClient.Click += btnOpenClient_Click;
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(440, 208);
            Controls.Add(btnOpenClient);
            Controls.Add(btnOpenServer);
            Controls.Add(lblInfo);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YMH219 ChatApp";
            FormClosing += MainForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblInfo;
        private Button btnOpenServer;
        private Button btnOpenClient;
    }
}
