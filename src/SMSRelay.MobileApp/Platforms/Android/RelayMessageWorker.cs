using System.Text.Json;

using Android.Content;

using AndroidX.Work;

using SMSRelay.Core.Model;
using SMSRelay.MobileApp.Model;
using SMSRelay.MobileApp.Services.Relay;
using SMSRelay.MobileApp.Services.Settings;

namespace SMSRelay.MobileApp.Platforms.Android;

public class RelayMessageWorker : Worker
{
    public const string ReceivedMessageDataKey = "received_message";

    public const string RelayMessageDataKey = "relay_message";

    private readonly ISettingsService _settingsService;
    private readonly IRelayService _relayService;

    public RelayMessageWorker(Context context, WorkerParameters workerParams) : base(context, workerParams)
    {
        _settingsService = new SettingsService();
        _relayService = new RelayService(_settingsService);
    }

    public override Result DoWork()
    {
        if (!TryGetMessageFromInput(ReceivedMessageDataKey, out TextMessage? receivedMessage) || receivedMessage == null)
        {
            return Result.InvokeFailure();
        }

        if (!TryGetSimProperties(receivedMessage.SimSlotIndex, out string phoneNumber))
        {
            return Result.InvokeSuccess();
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return Result.InvokeFailure();
        }

        ReceivedMessage messageToRelay = new(
            Id: receivedMessage.Id,
            Sender: receivedMessage.Sender,
            RecipientPhoneNumber: phoneNumber,
            Body: receivedMessage.MessageBody,
            ReceivedAt: receivedMessage.ReceivedAt);

        return Relay(messageToRelay) ? Result.InvokeSuccess() : Result.InvokeFailure();
    }

    private bool TryGetMessageFromInput(string key, out TextMessage? receivedMessage)
    {
        receivedMessage = null;

        string? receivedMessageJson = InputData.GetString(key);
        if (receivedMessageJson == null)
            return false;

        try
        {
            receivedMessage = JsonSerializer.Deserialize<TextMessage>(receivedMessageJson)!;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private bool TryGetSimProperties(int simSlotIndex, out string phoneNumber)
    {
        phoneNumber = string.Empty;
        try
        {
            (phoneNumber, bool isActive) = _settingsService.GetSimProperties(simSlotIndex);
            return isActive;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }
    
    private bool Relay(ReceivedMessage message)
    {
        try
        {
            return _relayService.RelayAsync(message, new CancellationToken()).Result;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
