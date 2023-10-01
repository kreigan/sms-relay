using Android.Content;

using AndroidX.Work;

using SMSRelay.Core.Model;
using SMSRelay.MobileApp.Model;
using SMSRelay.MobileApp.Services.Relay;
using SMSRelay.MobileApp.Services.Settings;

using System.Text.Json;

namespace SMSRelay.MobileApp.Platforms.Android;

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

    private static RelayedMessage Relay(ReceivedMessage message)
    {
        IRelayService relayService = new RelayService(new SettingsService());
        return relayService.Relay(message);
    }
}
