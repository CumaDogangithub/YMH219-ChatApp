using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Tek bir TCP bağlantısı üzerinden mesaj gönderip alır. Hem sunucu hem istemci tarafında kullanılır.
    /// Mesajlar UTF-8 ile, her biri bir satır olacak şekilde StreamReader/StreamWriter ile aktarılır.
    /// </summary>
    public class ChatConnection
    {
        private readonly TcpClient tcpClient;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        /// <summary>Aynı anda iki mesajın satırları birbirine karışmasın diye yazma işlemleri sıraya alınır.</summary>
        private readonly SemaphoreSlim writeLock = new SemaphoreSlim(1, 1);

        /// <summary>Karşı tarafın adresi, örneğin "192.168.1.5:53124".</summary>
        public string RemoteAddress { get; }

        public ChatConnection(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            tcpClient.NoDelay = true;   // kısa mesajlar bekletilmeden gönderilsin

            NetworkStream stream = tcpClient.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, new UTF8Encoding(false));
            writer.AutoFlush = true;

            RemoteAddress = tcpClient.Client.RemoteEndPoint?.ToString() ?? "bilinmiyor";
        }

        /// <summary>
        /// Bir sonraki mesajı bekler. Karşı taraf bağlantıyı kapattıysa null döner.
        /// Bağlantı koparsa IOException fırlatır.
        /// </summary>
        public async Task<ChatMessage?> ReceiveAsync()
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                    return null;

                ChatMessage? message = ChatMessage.Parse(line);
                if (message != null)
                    return message;

                // Biçimi bozuk satırlar atlanır, bir sonraki satır beklenir.
            }
        }

        public async Task SendAsync(ChatMessage message)
        {
            await writeLock.WaitAsync();
            try
            {
                await writer.WriteLineAsync(message.ToLine());
            }
            finally
            {
                writeLock.Release();
            }
        }

        public void Close()
        {
            tcpClient.Close();
        }
    }
}
