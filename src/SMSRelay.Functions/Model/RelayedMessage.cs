using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace SMSRelay.Functions.Model;

public record class RelayedMessage
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonProperty("textMessageId")]
    [JsonPropertyName("textMessageId")]
    public required Guid TextMessageId { get; init; }

    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public required string Sender { get; init; }

    [JsonProperty("receiverPhoneNumber")]
    [JsonPropertyName("receiverPhoneNumber")]
    public required string Receiver { get; init; }

    [JsonProperty("body")]
    [JsonPropertyName("body")]
    public required string Body { get; init; }

    [JsonProperty("textMessageReceivedAt")]
    [JsonPropertyName("textMessageReceivedAt")]
    public required DateTime TextMessageReceivedAt { get; init; }

    [JsonProperty("receivedForRelayAt")]
    [JsonPropertyName("receivedForRelayAt")]
    public required DateTime ReceivedAt { get; init; }

    [JsonProperty("relayStatus")]
    [JsonPropertyName("relayStatus")]
    public required RelayStatus Status { get; init; }

    [JsonProperty("relayStatusMessage")]
    [JsonPropertyName("relayStatusMessage")]
    public string? StatusMessage { get; set; }

    [JsonProperty("relayedAt")]
    [JsonPropertyName("relayedAt")]
    public DateTime? RelayedAt { get; set; }

    [JsonProperty("lastUpdatedAt")]
    [JsonPropertyName("lastUpdatedAt")]
    public DateTime? LastUpdatedAt { get; set; }
}
