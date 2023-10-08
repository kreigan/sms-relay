using System.Text.Json.Serialization;

namespace SMSRelay.Azure.Model;

public record class RelayedMessage
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonPropertyName("textMessageId")]
    public required Guid TextMessageId { get; init; }

    [JsonPropertyName("sender")]
    public required string Sender { get; init; }

    [JsonPropertyName("receiverPhoneNumber")]
    public required string Receiver { get; init; }

    [JsonPropertyName("body")]
    public required string Body { get; init; }

    [JsonPropertyName("textMessageReceivedAt")]
    public required DateTime TextMessageReceivedAt { get; init; }

    [JsonPropertyName("receivedForRelayAt")]
    public required DateTime ReceivedAt { get; init; }

    [JsonPropertyName("relayStatus")]
    public required RelayStatus Status { get; init; }

    [JsonPropertyName("relayStatusMessage")]
    public string? StatusMessage { get; set; }
    
    [JsonPropertyName("relayedAt")]
    public DateTime? RelayedAt { get; set; }

    [JsonPropertyName("lastUpdatedAt")]
    public DateTime? LastUpdatedAt { get; set; }
}
