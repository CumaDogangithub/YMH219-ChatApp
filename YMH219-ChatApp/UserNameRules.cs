using System;
using System.Linq;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Kullanıcı adı kuralları. Hem istemci (bağlanmadan önce) hem sunucu (katılma isteğinde) kullanır.
    /// </summary>
    public static class UserNameRules
    {
        public const int MinLength = 2;
        public const int MaxLength = 20;

        /// <summary>İstemcide alıcı listesinin ilk satırı; kullanıcı adı olarak kullanılamaz.</summary>
        public const string EveryoneLabel = "Herkes";

        public const string Description =
            "Kullanıcı adı 2-20 karakter olmalı ve yalnızca harf, rakam, boşluk, '-' ve '_' içermelidir. " +
            "'Sunucu' ve 'Herkes' adları kullanılamaz.";

        public static bool IsValid(string name)
        {
            string trimmed = name.Trim();

            if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
                return false;

            if (IsReserved(trimmed))
                return false;

            return trimmed.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_');
        }

        private static bool IsReserved(string name)
        {
            return string.Equals(name, ChatServer.ServerName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, EveryoneLabel, StringComparison.OrdinalIgnoreCase);
        }
    }
}
