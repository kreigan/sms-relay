using System.Text.Json.Serialization;

namespace SMSRelay.Azure.Model;

namespace SMSRelay.Functions.Model;

public record class Account
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonPropertyName("telegramUserId")]
    public required string TelegramUserId { get; init; }
    
    [JsonPropertyName("telegramChatId")]
    public required string TelegramChatId { get; init; }
    
    [JsonPropertyName("allowedPhoneNumbers")]
    public required string[] AllowedPhoneNumbers { get; init; }
}