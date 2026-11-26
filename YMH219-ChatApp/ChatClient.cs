using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Sohbet istemcisi. TcpClient ile sunucuya bağlanır, kullanıcı adıyla katılır,
    /// mesaj gönderir ve gelen mesajları arka planda okuyup olay olarak bildirir.
    /// </summary>
    public class ChatClient
    {
        private const int TimeoutMilliseconds = 5000;

        private ChatConnection? connection;
        private bool disconnectRequested;

        public bool IsConnected { get; private set; }
        public string UserName { get; private set; } = "";

        /// <summary>Sunucudan her mesaj geldiğinde tetiklenir.</summary>
        public event Action<ChatMessage>? MessageReceived;

        /// <summary>
        /// Bağlantı kapandığında tetiklenir. Parametre, bağlantıyı kullanıcının kendisi
        /// kestiyse true, sunucu kapandıysa veya bağlantı koptuysa false olur.
        /// </summary>
        public event Action<bool>? Disconnected;

        /// <summary>
        /// Sunucuya bağlanır ve kullanıcı adıyla katılır.
        /// Sunucuya ulaşılamazsa SocketException veya TimeoutException,
        /// sunucu kullanıcı adını reddederse InvalidOperationException fırlatır.
        /// </summary>
        public async Task ConnectAsync(string host, int port, string userName)
        {
            if (IsConnected)
                throw new InvalidOperationException("Zaten bir sunucuya bağlısınız.");

            TcpClient tcpClient = new TcpClient();
            try
            {
                await WithTimeoutAsync(tcpClient.ConnectAsync(host, port),
                    "Sunucu " + TimeoutMilliseconds / 1000 + " saniye içinde yanıt vermedi.");

                ChatConnection newConnection = new ChatConnection(tcpClient);
                await newConnection.SendAsync(ChatMessage.Create(MessageType.Join, userName, ""));

                Task<ChatMessage> replyTask = WaitForJoinReplyAsync(newConnection);
                await WithTimeoutAsync(replyTask, "Sunucu katılma isteğine yanıt vermedi.");
                ChatMessage welcome = await replyTask;

                connection = newConnection;
                UserName = userName;
                disconnectRequested = false;
                IsConnected = true;

                MessageReceived?.Invoke(welcome);
                _ = ReceiveLoopAsync(newConnection);   // beklenmeden arka planda çalışır
            }
            catch
            {
                tcpClient.Close();
                throw;
            }
        }

        public Task SendChatAsync(string text)
        {
            return SendAsync(ChatMessage.Create(MessageType.Chat, UserName, text));
        }

        public Task SendPrivateAsync(string targetUserName, string text)
        {
            return SendAsync(new ChatMessage(MessageType.Private, UserName, targetUserName, text, DateTime.Now));
        }

        /// <summary>
        /// Bağlantıyı kapatır. Okuma döngüsü sona erer ve Disconnected olayı tetiklenir.
        /// </summary>
        public void Disconnect()
        {
            ChatConnection? active = connection;
            if (!IsConnected || active == null)
                return;

            disconnectRequested = true;
            active.Close();
        }

        private async Task SendAsync(ChatMessage message)
        {
            ChatConnection? active = connection;
            if (!IsConnected || active == null)
                throw new InvalidOperationException("Sunucuya bağlı değilsiniz.");

            await active.SendAsync(message);
        }

        /// <summary>
        /// Katılma isteğinin yanıtını bekler: Welcome gelirse mesajı döndürür,
        /// Error gelirse sunucunun açıklamasıyla InvalidOperationException fırlatır.
        /// </summary>
        private static async Task<ChatMessage> WaitForJoinReplyAsync(ChatConnection newConnection)
        {
            while (true)
            {
                ChatMessage? reply = await newConnection.ReceiveAsync();

                if (reply == null)
                    throw new IOException("Sunucu bağlantıyı kapattı.");

                if (reply.Type == MessageType.Welcome)
                    return reply;

                if (reply.Type == MessageType.Error)
                    throw new InvalidOperationException(reply.Text);
            }
        }

        /// <summary>Bağlantı açık kaldığı sürece sunucudan gelen mesajları okur.</summary>
        private async Task ReceiveLoopAsync(ChatConnection activeConnection)
        {
            try
            {
                while (true)
                {
                    ChatMessage? message = await activeConnection.ReceiveAsync();
                    if (message == null)
                        break;   // sunucu bağlantıyı kapattı

                    MessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException || ex is ObjectDisposedException)
            {
                // Bağlantı koptu veya Disconnect() ile kapatıldı.
            }
            finally
            {
                activeConnection.Close();

                bool byUser = disconnectRequested;
                connection = null;
                IsConnected = false;
                disconnectRequested = false;

                Disconnected?.Invoke(byUser);
            }
        }

        /// <summary>
        /// Görevi en fazla TimeoutMilliseconds kadar bekler; süre dolarsa TimeoutException fırlatır.
        /// </summary>
        private static async Task WithTimeoutAsync(Task task, string timeoutMessage)
        {
            Task finished = await Task.WhenAny(task, Task.Delay(TimeoutMilliseconds));
            if (finished != task)
                throw new TimeoutException(timeoutMessage);

            await task;   // görev hata ile bittiyse istisna burada yeniden fırlatılır
        }
    }
}
