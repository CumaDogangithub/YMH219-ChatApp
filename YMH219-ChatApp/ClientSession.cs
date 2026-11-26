using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Sunucuya bağlı tek bir istemcinin sunucu tarafındaki kaydı:
    /// bağlantısı, kullanıcı adı, adresi ve bağlanma zamanı.
    /// </summary>
    public class ClientSession
    {
        private readonly ChatConnection connection;

        /// <summary>Katılma isteği kabul edilince atanır.</summary>
        public string UserName { get; set; } = "";

        public string RemoteAddress
        {
            get { return connection.RemoteAddress; }
        }

        public DateTime ConnectedAt { get; }

        public ClientSession(TcpClient tcpClient)
        {
            connection = new ChatConnection(tcpClient);
            ConnectedAt = DateTime.Now;
        }

        public Task<ChatMessage?> ReceiveAsync()
        {
            return connection.ReceiveAsync();
        }

        public Task SendAsync(ChatMessage message)
        {
            return connection.SendAsync(message);
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
