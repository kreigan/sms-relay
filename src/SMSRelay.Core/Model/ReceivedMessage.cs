namespace SMSRelay.Core.Model;

public record ReceivedMessage
    (
    Guid Id,
    string Sender,
    string RecipientPhoneNumber,
    string Body,
    DateTime ReceivedAt
    );