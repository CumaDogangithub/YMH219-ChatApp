using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Sohbet sunucusu. TcpListener ile bağlantıları kabul eder, her istemciyi ayrı bir
    /// asenkron döngüde dinler ve gelen mesajları ilgili istemcilere iletir.
    /// </summary>
    public class ChatServer
    {
        public const string ServerName = "Sunucu";
        public const int MaxTextLength = 500;

        private readonly object syncRoot = new object();
        private readonly List<ClientSession> sessions = new List<ClientSession>();
        private TcpListener? listener;

        public bool IsRunning { get; private set; }
        public int Port { get; private set; }

        /// <summary>Sunucu ekranında gösterilecek her mesaj için tetiklenir.</summary>
        public event Action<ChatMessage>? MessageLogged;

        /// <summary>Bir istemci katıldığında veya ayrıldığında tetiklenir.</summary>
        public event Action? ClientListChanged;

        // ------------------------------------------------------------------
        // Başlatma ve durdurma
        // ------------------------------------------------------------------

        /// <summary>
        /// Sunucuyu verilen portta başlatır. Port kullanımdaysa SocketException fırlatır.
        /// </summary>
        public void Start(int port)
        {
            if (IsRunning)
                return;

            TcpListener newListener = new TcpListener(IPAddress.Any, port);
            newListener.Start();

            listener = newListener;
            Port = port;
            IsRunning = true;

            _ = AcceptLoopAsync(newListener);   // beklenmeden arka planda çalışır
        }

        /// <summary>İstemcilere kapanış bildirimi gönderir, sonra sunucuyu durdurur.</summary>
        public async Task StopAsync()
        {
            if (!IsRunning)
                return;

            await BroadcastAsync(ChatMessage.Create(MessageType.Notice, ServerName, "Sunucu kapatılıyor."));
            Stop();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            listener?.Stop();
            listener = null;

            List<ClientSession> toClose;
            lock (syncRoot)
            {
                toClose = new List<ClientSession>(sessions);
                sessions.Clear();
            }

            foreach (ClientSession session in toClose)
                session.Close();   // istemciler bağlantının kapandığını görür

            ClientListChanged?.Invoke();
        }

        /// <summary>Bağlı istemcilerin o anki listesinin kopyasını döndürür.</summary>
        public List<ClientSession> GetClients()
        {
            lock (syncRoot)
            {
                return new List<ClientSession>(sessions);
            }
        }

        /// <summary>Sunucunun kendi adına herkese mesaj gönderir.</summary>
        public Task SendServerMessageAsync(string text)
        {
            return BroadcastAsync(ChatMessage.Create(MessageType.Chat, ServerName, Shorten(text)));
        }

        // ------------------------------------------------------------------
        // Bağlantı kabul etme ve istemci döngüsü
        // ------------------------------------------------------------------

        private async Task AcceptLoopAsync(TcpListener activeListener)
        {
            while (IsRunning)
            {
                TcpClient tcpClient;
                try
                {
                    tcpClient = await activeListener.AcceptTcpClientAsync();
                }
                catch (Exception ex) when (ex is SocketException || ex is ObjectDisposedException || ex is InvalidOperationException)
                {
                    break;   // Stop() dinleyiciyi kapatınca bekleyen kabul işlemi hata verir
                }

                _ = HandleClientAsync(new ClientSession(tcpClient));
            }
        }

        /// <summary>
        /// Bir istemciyle ilgili tüm işlemler: katılma, mesajları okuma ve ayrılma.
        /// </summary>
        private async Task HandleClientAsync(ClientSession session)
        {
            bool joined = false;
            try
            {
                joined = await TryJoinAsync(session);
                if (!joined)
                    return;

                while (true)
                {
                    ChatMessage? message = await session.ReceiveAsync();
                    if (message == null)
                        break;   // istemci bağlantıyı kapattı

                    await ProcessClientMessageAsync(session, message);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException || ex is ObjectDisposedException)
            {
                // Bağlantı beklenmedik şekilde koptu; istemci aşağıda listeden çıkarılır.
            }
            finally
            {
                session.Close();
                if (joined)
                    await RemoveSessionAsync(session);
            }
        }

        /// <summary>
        /// İstemcinin ilk mesajı katılma isteği olmalıdır. Ad geçerli ve boştaysa istemci listeye eklenir.
        /// </summary>
        private async Task<bool> TryJoinAsync(ClientSession session)
        {
            ChatMessage? request = await session.ReceiveAsync();
            if (request == null || request.Type != MessageType.Join)
                return false;

            string name = request.Sender.Trim();
            string? error = null;

            lock (syncRoot)
            {
                if (!UserNameRules.IsValid(name))
                    error = UserNameRules.Description;
                else if (sessions.Any(s => string.Equals(s.UserName, name, StringComparison.OrdinalIgnoreCase)))
                    error = "'" + name + "' kullanıcı adı şu anda kullanılıyor. Başka bir ad seçin.";
                else
                {
                    session.UserName = name;
                    sessions.Add(session);
                }
            }

            if (error != null)
            {
                await session.SendAsync(ChatMessage.Create(MessageType.Error, ServerName, error));
                return false;
            }

            await session.SendAsync(ChatMessage.Create(MessageType.Welcome, ServerName, "Sohbete hoş geldin, " + name + "."));
            ClientListChanged?.Invoke();
            await BroadcastAsync(ChatMessage.Create(MessageType.Notice, ServerName, name + " sohbete katıldı."));
            await BroadcastUserListAsync();
            return true;
        }

        private async Task ProcessClientMessageAsync(ClientSession sender, ChatMessage message)
        {
            string text = Shorten(message.Text.Trim());
            if (text.Length == 0)
                return;

            // Gönderen adı ve zaman istemcinin yazdığından değil, sunucunun kaydından alınır.
            if (message.Type == MessageType.Chat)
                await BroadcastAsync(new ChatMessage(MessageType.Chat, sender.UserName, "", text, DateTime.Now));
            else if (message.Type == MessageType.Private)
                await SendPrivateAsync(sender, message.Target, text);
        }

        private async Task RemoveSessionAsync(ClientSession session)
        {
            bool removed;
            lock (syncRoot)
            {
                removed = sessions.Remove(session);
            }

            // Sunucu durdurulurken liste zaten boşaltılmıştır; o durumda bildirim gönderilmez.
            if (!removed || !IsRunning)
                return;

            ClientListChanged?.Invoke();
            await BroadcastAsync(ChatMessage.Create(MessageType.Notice, ServerName, session.UserName + " sohbetten ayrıldı."));
            await BroadcastUserListAsync();
        }

        // ------------------------------------------------------------------
        // Mesaj gönderme
        // ------------------------------------------------------------------

        /// <summary>Mesajı sunucu ekranına yazar ve bağlı tüm istemcilere gönderir.</summary>
        private async Task BroadcastAsync(ChatMessage message)
        {
            MessageLogged?.Invoke(message);
            await Task.WhenAll(GetClients().Select(session => SendSafeAsync(session, message)));
        }

        /// <summary>
        /// Özel mesajı yalnızca alıcıya ve gönderene iletir. Sunucu ekranına mesajın içeriği yazılmaz.
        /// </summary>
        private async Task SendPrivateAsync(ClientSession sender, string targetName, string text)
        {
            ClientSession? target = GetClients().FirstOrDefault(
                s => string.Equals(s.UserName, targetName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (target == null)
            {
                await SendSafeAsync(sender, ChatMessage.Create(MessageType.Notice, ServerName,
                    "Özel mesaj gönderilemedi: '" + targetName + "' çevrimiçi değil."));
                return;
            }

            ChatMessage message = new ChatMessage(MessageType.Private, sender.UserName, target.UserName, text, DateTime.Now);

            await SendSafeAsync(target, message);
            if (target != sender)
                await SendSafeAsync(sender, message);

            MessageLogged?.Invoke(ChatMessage.Create(MessageType.Notice, ServerName,
                sender.UserName + ", " + target.UserName + " kullanıcısına özel mesaj gönderdi."));
        }

        private Task BroadcastUserListAsync()
        {
            List<ClientSession> clients = GetClients();
            string names = string.Join(",", clients.Select(s => s.UserName));
            ChatMessage message = ChatMessage.Create(MessageType.Users, ServerName, names);

            return Task.WhenAll(clients.Select(session => SendSafeAsync(session, message)));
        }

        /// <summary>
        /// Tek bir istemciye mesaj gönderir. İstemcinin bağlantısı kopmuşsa hata yutulur;
        /// o istemci kendi okuma döngüsünde listeden çıkarılır.
        /// </summary>
        private static async Task SendSafeAsync(ClientSession session, ChatMessage message)
        {
            try
            {
                await session.SendAsync(message);
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException || ex is ObjectDisposedException)
            {
            }
        }

        private static string Shorten(string text)
        {
            return text.Length <= MaxTextLength ? text : text.Substring(0, MaxTextLength);
        }
    }
}
