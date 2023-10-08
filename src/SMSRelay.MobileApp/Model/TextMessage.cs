namespace SMSRelay.MobileApp.Model;

public record TextMessage
    (
    Guid Id,
    int SimSlotIndex,
    DateTime ReceivedAt,
    string MessageBody,
    string Sender
    );
