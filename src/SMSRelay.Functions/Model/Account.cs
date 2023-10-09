using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace SMSRelay.Functions.Model;

public record class Account
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonProperty("telegramUserId")]
    [JsonPropertyName("telegramUserId")]
    public required string TelegramUserId { get; init; }

    [JsonProperty("telegramChatId")]
    [JsonPropertyName("telegramChatId")]
    public required string TelegramChatId { get; init; }

    [JsonProperty("allowedPhoneNumbers")]
    [JsonPropertyName("allowedPhoneNumbers")]
    public required string[] AllowedPhoneNumbers { get; init; }
}