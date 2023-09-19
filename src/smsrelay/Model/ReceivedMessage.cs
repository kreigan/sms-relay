namespace SMSRelay.Model;

public record ReceivedMessage
    (
    Guid Id,
    int SimSlotIndex,
    DateTime ReceivedAt,
    string MessageBody,
    string Sender
    );
