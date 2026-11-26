using System;
using System.Globalization;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Ağ üzerinden gönderilen tek bir mesaj. Her mesaj tek satırlık metne çevrilerek gönderilir:
    /// Tür [TAB] Zaman [TAB] Gönderen [TAB] Alıcı [TAB] Metin
    /// </summary>
    public class ChatMessage
    {
        private const char Separator = '\t';
        private const int FieldCount = 5;

        public MessageType Type { get; }
        public DateTime Timestamp { get; }
        public string Sender { get; }

        /// <summary>Özel mesajda alıcının adı; diğer türlerde boştur.</summary>
        public string Target { get; }

        public string Text { get; }

        public ChatMessage(MessageType type, string sender, string target, string text, DateTime timestamp)
        {
            Type = type;
            Sender = sender;
            Target = target;
            Text = text;
            Timestamp = timestamp;
        }

        /// <summary>Alıcısı olmayan, şu anki zamanla damgalanmış bir mesaj oluşturur.</summary>
        public static ChatMessage Create(MessageType type, string sender, string text)
        {
            return new ChatMessage(type, sender, "", text, DateTime.Now);
        }

        /// <summary>Mesajı ağ üzerinden gönderilecek tek satırlık metne çevirir.</summary>
        public string ToLine()
        {
            return string.Join(Separator.ToString(),
                Type.ToString(),
                Timestamp.ToString("o", CultureInfo.InvariantCulture),
                Sender,
                Target,
                RemoveLineBreaks(Text));
        }

        /// <summary>
        /// Ağdan okunan satırı mesaja çevirir. Satır beklenen biçimde değilse null döner.
        /// </summary>
        public static ChatMessage? Parse(string line)
        {
            // Metin son alan olduğu için içinde TAB karakteri bulunsa bile bölünmez.
            string[] parts = line.Split(new[] { Separator }, FieldCount);
            if (parts.Length != FieldCount)
                return null;

            if (!Enum.TryParse(parts[0], out MessageType type) || !Enum.IsDefined(typeof(MessageType), type))
                return null;

            if (!DateTime.TryParse(parts[1], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime timestamp))
                return null;

            return new ChatMessage(type, parts[2], parts[3], parts[4], timestamp);
        }

        /// <summary>
        /// Mesajın ekranda ve geçmiş dosyasında gösterilen hali. Örnek: "[14:05:12] Ali: Merhaba"
        /// </summary>
        public string ToDisplayString()
        {
            string time = Timestamp.ToLocalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture);

            switch (Type)
            {
                case MessageType.Chat:
                    return string.Format("[{0}] {1}: {2}", time, Sender, Text);
                case MessageType.Private:
                    return string.Format("[{0}] [Özel] {1} -> {2}: {3}", time, Sender, Target, Text);
                default:
                    return string.Format("[{0}] *** {1}", time, Text);
            }
        }

        /// <summary>Her mesaj tek satır olarak gönderildiği için satır sonları boşluğa çevrilir.</summary>
        private static string RemoveLineBreaks(string text)
        {
            return text.Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ');
        }
    }
}
