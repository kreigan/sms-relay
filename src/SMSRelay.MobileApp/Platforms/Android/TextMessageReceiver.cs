using Android.App;
using Android.Content;
using Android.Provider;

using AndroidX.Work;

using SMSRelay.MobileApp.Model;

using System.Text.Json;

using SmsMessage = Android.Telephony.SmsMessage;

namespace SMSRelay.MobileApp.Platforms.Android;

[BroadcastReceiver(Permission = "android.permission.BROADCAST_SMS", Exported = true)]
[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]
public class TextMessageReceiver : BroadcastReceiver
{
    private const string SlotIndexExtraName = "android.telephony.extra.SLOT_INDEX";

    public override void OnReceive(Context? context, Intent? intent)
    {
        var registrationTime = DateTime.UtcNow;
    
        if (context == null || intent?.Action != Telephony.Sms.Intents.SmsReceivedAction)
        {
            return;
        }

        SmsMessage[]? messageParts = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

        if (messageParts == null || messageParts.Length == 0)
        {
            return;
        }

        ReceivedMessage receivedMessage = new
        (
            Id: Guid.NewGuid(),
            SimSlotIndex: intent.GetIntExtra(SlotIndexExtraName, 0),
            ReceivedAt: registrationTime,
            MessageBody: messageParts.FirstOrDefault(x => x.DisplayOriginatingAddress != null)?.DisplayOriginatingAddress ?? string.Empty,
            Sender: string.Concat(messageParts.Select(x => x.MessageBody))
        );

        Data relayData = new Data.Builder()
            .PutString(RelayMessageWorker.ReceivedMessageDataKey, JsonSerializer.Serialize(receivedMessage))
            .Build();

        WorkRequest relayRequest = OneTimeWorkRequest.Builder.From<RelayMessageWorker>()
            .SetExpedited(OutOfQuotaPolicy.RunAsNonExpeditedWorkRequest!)
            .SetInputData(relayData)
            .Build();

        WorkManager.GetInstance(context).Enqueue(relayRequest);
    }
}
