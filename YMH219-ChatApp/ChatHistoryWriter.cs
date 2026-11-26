using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Ekrandaki mesaj geçmişini yerel bir metin dosyasına yazar.
    /// </summary>
    public static class ChatHistoryWriter
    {
        /// <summary>
        /// Mesaj satırlarını başlık bilgisiyle birlikte dosyaya yazar.
        /// Dosya yazılamazsa IOException veya UnauthorizedAccessException fırlatır.
        /// </summary>
        public static void Save(string path, string title, IReadOnlyList<string> lines)
        {
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("YMH219 ChatApp - " + title);
                writer.WriteLine("Kaydedilme: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                writer.WriteLine("Mesaj sayısı: " + lines.Count);
                writer.WriteLine(new string('-', 60));

                foreach (string line in lines)
                    writer.WriteLine(line);
            }
        }
    }
}
