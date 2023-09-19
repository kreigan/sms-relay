using Android.Content;

using AndroidX.Work;

using SMSRelay.Model;
using SMSRelay.Services.Settings;

using System.Text.Json;

namespace SMSRelay.Platforms.Android;

public class RelayMessageWorker : Worker
{
    public const string ReceivedMessageDataKey = "received_message";
    
    public const string RelayMessageDataKey = "relay_message";

    public RelayMessageWorker(Context context, WorkerParameters workerParams) : base(context, workerParams)
    {
    }

    public override Result DoWork()
    {
        string? receivedMessageJson = InputData.GetString(ReceivedMessageDataKey);
        if (receivedMessageJson == null)
        {
            return Result.InvokeFailure();
        }

        ReceivedMessage receivedMessage;
        try
        {
            receivedMessage = JsonSerializer.Deserialize<ReceivedMessage>(receivedMessageJson)!;
        }
        catch (Exception)
        {
            return Result.InvokeFailure();
        }

        RelayedMessage relayedMessage;
        try
        {
            relayedMessage = Relay(receivedMessage);
        }
        catch (Exception)
        {
            return Result.InvokeRetry();
        }

        return Result.InvokeSuccess(
                new Data.Builder()
                    .PutString(RelayMessageDataKey, JsonSerializer.Serialize(relayedMessage))
                    .Build()
            );
    }

    private RelayedMessage Relay(ReceivedMessage message)
    {
        SettingsService settings = new();
        string url = settings.RemoteRelayReceiverUri;
        return new(message.Id, message.Sender, "", message.MessageBody, message.ReceivedAt);
    }
}
