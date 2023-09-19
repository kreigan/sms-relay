namespace SMSRelay.Model;

public record RelayedMessage
    (
    Guid Id,
    string Sender,
    string RecipientPhoneNumber,
    string Body,
    DateTime ReceivedAt
    );