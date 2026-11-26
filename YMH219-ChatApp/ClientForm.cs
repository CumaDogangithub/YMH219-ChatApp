using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;

namespace YMH219_ChatApp
{
    /// <summary>
    /// İstemci penceresi: kullanıcı adıyla sunucuya bağlanır, mesaj gönderir ve gelen mesajları gösterir.
    /// </summary>
    public partial class ClientForm : Form
    {
        private enum ConnectionState
        {
            Disconnected,
            Connecting,
            Connected
        }

        private static readonly Color ConnectedColor = Color.FromArgb(46, 125, 50);     // yeşil
        private static readonly Color ConnectingColor = Color.FromArgb(230, 126, 0);    // turuncu
        private static readonly Color DisconnectedColor = Color.FromArgb(97, 97, 97);   // gri

        private const string WindowTitle = "YMH219 ChatApp - İstemci";

        private readonly ChatClient client = new ChatClient();

        public ClientForm()
        {
            InitializeComponent();

            client.MessageReceived += Client_MessageReceived;
            client.Disconnected += Client_Disconnected;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            ResetUserList();
            SetConnectionState(ConnectionState.Disconnected);
            SetStatus("Sunucu adresini ve kullanıcı adını yazıp Bağlan düğmesine basın.");
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.MessageReceived -= Client_MessageReceived;
            client.Disconnected -= Client_Disconnected;
            client.Disconnect();
        }

        // ------------------------------------------------------------------
        // Bağlan ve Bağlantıyı Kes
        // ------------------------------------------------------------------

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string host = txtHost.Text.Trim();
            string userName = txtUserName.Text.Trim();
            int port = (int)nudPort.Value;

            if (host.Length == 0)
            {
                ShowWarning("Sunucu adresini yazın. Sunucu aynı bilgisayardaysa 127.0.0.1 kullanılır.");
                txtHost.Focus();
                return;
            }

            if (!UserNameRules.IsValid(userName))
            {
                ShowWarning(UserNameRules.Description);
                txtUserName.Focus();
                return;
            }

            SetConnectionState(ConnectionState.Connecting);
            SetStatus(host + ":" + port + " adresine bağlanılıyor...");

            try
            {
                await client.ConnectAsync(host, port, userName);
            }
            catch (InvalidOperationException ex)
            {
                // Sunucuya ulaşıldı ama katılma isteği reddedildi (örneğin ad kullanımda).
                ReportConnectFailure("Sunucu katılma isteğini reddetti.", ex.Message);
                return;
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException || ex is TimeoutException)
            {
                ReportConnectFailure("Sunucuya bağlanılamadı.", ex.Message + Environment.NewLine + Environment.NewLine
                    + "Sunucunun çalıştığını, adresin ve portun doğru olduğunu kontrol edin.");
                return;
            }

            if (IsDisposed)
            {
                client.Disconnect();   // bağlanırken pencere kapatılmış
                return;
            }

            SetConnectionState(ConnectionState.Connected);
            SetStatus("Bağlandı: " + host + ":" + port);
            txtMessage.Focus();
        }

        private void ReportConnectFailure(string title, string detail)
        {
            if (IsDisposed)
                return;

            SetConnectionState(ConnectionState.Disconnected);
            SetStatus(title);
            ShowError(title + Environment.NewLine + Environment.NewLine + detail);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Disconnect();   // arayüz, Disconnected olayı gelince güncellenir
        }

        // ------------------------------------------------------------------
        // Mesaj gönderme
        // ------------------------------------------------------------------

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string text = txtMessage.Text.Trim();
            if (text.Length == 0)
                return;

            string? target = GetSelectedTarget();

            try
            {
                if (target == null)
                    await client.SendChatAsync(text);
                else
                    await client.SendPrivateAsync(target, text);
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException || ex is InvalidOperationException)
            {
                if (IsDisposed)
                    return;

                SetStatus("Mesaj gönderilemedi.");
                ShowError("Mesaj gönderilemedi." + Environment.NewLine + Environment.NewLine + ex.Message);
                return;
            }

            if (IsDisposed)
                return;

            // Mesaj ekrana burada yazılmaz; sunucudan geri geldiğinde zamanıyla birlikte listeye eklenir.
            txtMessage.Clear();
            txtMessage.Focus();
        }

        /// <summary>Kullanıcı listesinde seçili alıcıyı döndürür; "Herkes" seçiliyse null.</summary>
        private string? GetSelectedTarget()
        {
            if (lstUsers.SelectedIndex <= 0)
                return null;

            return lstUsers.SelectedItem as string;
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? target = GetSelectedTarget();

            lblTarget.Text = target == null
                ? "Alıcı: Herkes"
                : "Alıcı: " + target + " (özel mesaj)";
        }

        // ------------------------------------------------------------------
        // İstemci olayları
        // Bu olaylar ağ işlemlerinin sürdüğü iş parçacığından gelebilir;
        // arayüz denetimlerine yalnızca arayüz iş parçacığından dokunulur.
        // ------------------------------------------------------------------

        private void Client_MessageReceived(ChatMessage message)
        {
            RunOnUiThread(() =>
            {
                if (message.Type == MessageType.Users)
                    UpdateUserList(message.Text);
                else
                    AddLine(message.ToDisplayString());
            });
        }

        private void Client_Disconnected(bool byUser)
        {
            RunOnUiThread(() =>
            {
                SetConnectionState(ConnectionState.Disconnected);
                ResetUserList();

                if (byUser)
                {
                    AddNotice("Bağlantı kesildi.");
                    SetStatus("Bağlantı kesildi.");
                    return;
                }

                // Bağlantı kullanıcı istemeden koptu: uygulama kapanmaz, kullanıcı bilgilendirilir.
                AddNotice("Sunucuyla bağlantı koptu.");
                SetStatus("Sunucuyla bağlantı koptu.");
                ShowWarning("Sunucuyla bağlantı koptu. Sunucu kapatılmış veya ağ bağlantısı kesilmiş olabilir.");
            });
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }

        // ------------------------------------------------------------------
        // Arayüz güncelleme
        // ------------------------------------------------------------------

        private void SetConnectionState(ConnectionState state)
        {
            bool disconnected = state == ConnectionState.Disconnected;
            bool connected = state == ConnectionState.Connected;

            txtHost.Enabled = disconnected;
            nudPort.Enabled = disconnected;
            txtUserName.Enabled = disconnected;
            btnConnect.Enabled = disconnected;
            btnDisconnect.Enabled = connected;
            txtMessage.Enabled = connected;
            btnSend.Enabled = connected;
            lstUsers.Enabled = connected;

            switch (state)
            {
                case ConnectionState.Connected:
                    lblState.Text = "BAĞLI: " + client.UserName;
                    lblState.BackColor = ConnectedColor;
                    Text = WindowTitle + " (" + client.UserName + ")";
                    break;
                case ConnectionState.Connecting:
                    lblState.Text = "BAĞLANIYOR...";
                    lblState.BackColor = ConnectingColor;
                    break;
                default:
                    lblState.Text = "BAĞLI DEĞİL";
                    lblState.BackColor = DisconnectedColor;
                    Text = WindowTitle;
                    break;
            }
        }

        /// <summary>
        /// Sunucudan gelen virgülle ayrılmış kullanıcı listesini gösterir.
        /// İlk satır "Herkes"tir; kullanıcının kendi adı listeye eklenmez.
        /// </summary>
        private void UpdateUserList(string userNames)
        {
            string[] names = userNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string? previousTarget = GetSelectedTarget();

            lstUsers.BeginUpdate();
            lstUsers.Items.Clear();
            lstUsers.Items.Add(UserNameRules.EveryoneLabel);

            foreach (string name in names)
            {
                if (!string.Equals(name, client.UserName, StringComparison.OrdinalIgnoreCase))
                    lstUsers.Items.Add(name);
            }

            lstUsers.EndUpdate();

            // Seçili alıcı hâlâ çevrimiçiyse seçim korunur, değilse "Herkes"e dönülür.
            int index = previousTarget == null ? 0 : lstUsers.Items.IndexOf(previousTarget);
            lstUsers.SelectedIndex = index < 0 ? 0 : index;

            grpUsers.Text = "Çevrimiçi (" + names.Length + ")";
        }

        private void ResetUserList()
        {
            lstUsers.Items.Clear();
            lstUsers.Items.Add(UserNameRules.EveryoneLabel);
            lstUsers.SelectedIndex = 0;
            grpUsers.Text = "Çevrimiçi (0)";
        }

        private void AddNotice(string text)
        {
            AddLine(ChatMessage.Create(MessageType.Notice, ChatServer.ServerName, text).ToDisplayString());
        }

        private void AddLine(string line)
        {
            lstMessages.Items.Add(line);
            lstMessages.TopIndex = lstMessages.Items.Count - 1;   // son mesaja kaydır
        }

        private void btnSaveHistory_Click(object sender, EventArgs e)
        {
            List<string> lines = lstMessages.Items.Cast<string>().ToList();
            if (lines.Count == 0)
            {
                ShowWarning("Kaydedilecek mesaj yok.");
                return;
            }

            saveFileDialog.FileName = "sohbet_gecmisi_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string title = "İstemci mesaj geçmişi (" + txtUserName.Text.Trim() + ")";
                ChatHistoryWriter.Save(saveFileDialog.FileName, title, lines);
                SetStatus(lines.Count + " satır kaydedildi: " + Path.GetFileName(saveFileDialog.FileName));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                SetStatus("Dosya kaydedilemedi.");
                ShowError("Dosya kaydedilemedi." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        private void SetStatus(string message)
        {
            lblStatus.Text = message;
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(this, message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
