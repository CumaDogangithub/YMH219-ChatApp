namespace YMH219_ChatApp
{
    /// <summary>
    /// Sunucu ile istemci arasında gönderilen mesajın türü.
    /// </summary>
    public enum MessageType
    {
        /// <summary>İstemciden sunucuya: kullanıcı adıyla sohbete katılma isteği.</summary>
        Join,

        /// <summary>Sunucudan istemciye: katılma isteği kabul edildi.</summary>
        Welcome,

        /// <summary>Sunucudan istemciye: istek reddedildi (örneğin kullanıcı adı kullanımda).</summary>
        Error,

        /// <summary>Herkese açık sohbet mesajı.</summary>
        Chat,

        /// <summary>Yalnızca bir kullanıcıya gönderilen özel mesaj.</summary>
        Private,

        /// <summary>Sunucu bildirimi (katıldı, ayrıldı, sunucu kapatılıyor).</summary>
        Notice,

        /// <summary>Sunucudan istemciye: çevrimiçi kullanıcıların virgülle ayrılmış listesi.</summary>
        Users
    }
}
