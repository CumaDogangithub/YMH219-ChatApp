using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Sunucu penceresi: sunucuyu başlatır ve durdurur, bağlı istemcileri ve mesajları gösterir.
    /// </summary>
    public partial class ServerForm : Form
    {
        private static readonly Color RunningColor = Color.FromArgb(46, 125, 50);   // yeşil
        private static readonly Color StoppedColor = Color.FromArgb(97, 97, 97);    // gri

        private readonly ChatServer server = new ChatServer();

        public ServerForm()
        {
            InitializeComponent();

            server.MessageLogged += Server_MessageLogged;
            server.ClientListChanged += Server_ClientListChanged;
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            lblAddresses.Text = "Bu bilgisayarın ağ adresi: " + GetLocalAddresses()
                + "   (aynı bilgisayardaki istemciler için 127.0.0.1)";

            UpdateServerUi();
            SetStatus("Hazır. Portu seçip sunucuyu başlatın.");
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.MessageLogged -= Server_MessageLogged;
            server.ClientListChanged -= Server_ClientListChanged;
            server.Stop();
        }

        // ------------------------------------------------------------------
        // Düğmeler
        // ------------------------------------------------------------------

        private void btnStart_Click(object sender, EventArgs e)
        {
            int port = (int)nudPort.Value;

            try
            {
                server.Start(port);
            }
            catch (SocketException ex)
            {
                SetStatus("Sunucu başlatılamadı.");
                ShowError("Sunucu " + port + " numaralı portta başlatılamadı. "
                    + "Port başka bir uygulama tarafından kullanılıyor olabilir."
                    + Environment.NewLine + Environment.NewLine + ex.Message);
                return;
            }

            AddNotice("Sunucu başlatıldı (port " + port + ").");
            UpdateServerUi();
            SetStatus("Sunucu çalışıyor. İstemciler bağlanabilir.");
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;

            await server.StopAsync();   // önce istemcilere kapanış bildirimi gönderilir

            if (IsDisposed)
                return;

            AddNotice("Sunucu durduruldu.");
            UpdateServerUi();
            SetStatus("Sunucu durduruldu.");
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string text = txtMessage.Text.Trim();
            if (text.Length == 0)
                return;

            await server.SendServerMessageAsync(text);

            if (IsDisposed)
                return;

            txtMessage.Clear();
            txtMessage.Focus();
        }

        private void btnSaveHistory_Click(object sender, EventArgs e)
        {
            List<string> lines = lstMessages.Items.Cast<string>().ToList();
            if (lines.Count == 0)
            {
                ShowWarning("Kaydedilecek mesaj yok.");
                return;
            }

            saveFileDialog.FileName = "sohbet_gecmisi_sunucu_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                ChatHistoryWriter.Save(saveFileDialog.FileName, "Sunucu mesaj geçmişi", lines);
                SetStatus(lines.Count + " satır kaydedildi: " + Path.GetFileName(saveFileDialog.FileName));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                SetStatus("Dosya kaydedilemedi.");
                ShowError("Dosya kaydedilemedi." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        // ------------------------------------------------------------------
        // Sunucu olayları
        // Bu olaylar ağ işlemlerinin sürdüğü iş parçacığından gelebilir;
        // arayüz denetimlerine yalnızca arayüz iş parçacığından dokunulur.
        // ------------------------------------------------------------------

        private void Server_MessageLogged(ChatMessage message)
        {
            RunOnUiThread(() => AddLine(message.ToDisplayString()));
        }

        private void Server_ClientListChanged()
        {
            RunOnUiThread(RefreshClientList);
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

        /// <summary>Bağlı istemcilerin kullanıcı adını, adresini ve bağlanma saatini listeler.</summary>
        private void RefreshClientList()
        {
            List<ClientSession> clients = server.GetClients();

            lvClients.BeginUpdate();
            lvClients.Items.Clear();

            foreach (ClientSession client in clients)
            {
                ListViewItem item = new ListViewItem(client.UserName);
                item.SubItems.Add(client.RemoteAddress);
                item.SubItems.Add(client.ConnectedAt.ToString("HH:mm:ss"));
                lvClients.Items.Add(item);
            }

            lvClients.EndUpdate();
            grpClients.Text = "Bağlı İstemciler (" + clients.Count + ")";
        }

        private void UpdateServerUi()
        {
            bool running = server.IsRunning;

            nudPort.Enabled = !running;
            btnStart.Enabled = !running;
            btnStop.Enabled = running;
            txtMessage.Enabled = running;
            btnSend.Enabled = running;

            lblState.Text = running ? "SUNUCU ÇALIŞIYOR (port " + server.Port + ")" : "SUNUCU KAPALI";
            lblState.BackColor = running ? RunningColor : StoppedColor;
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

        /// <summary>Diğer bilgisayarların bağlanırken kullanacağı IPv4 adreslerini bulur.</summary>
        private static string GetLocalAddresses()
        {
            try
            {
                List<string> addresses = Dns.GetHostAddresses(Dns.GetHostName())
                    .Where(a => a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a))
                    .Select(a => a.ToString())
                    .ToList();

                return addresses.Count > 0 ? string.Join(", ", addresses) : "bulunamadı";
            }
            catch (SocketException)
            {
                return "bulunamadı";
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
