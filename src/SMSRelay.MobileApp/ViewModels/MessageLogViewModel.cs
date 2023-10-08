using SMSRelay.Core.Model;

namespace SMSRelay.MobileApp.ViewModels;

public class MessageLogViewModel
{
    private readonly ICollection<ReceivedMessage> _messages;

    public MessageLogViewModel()
    {
        _messages = new List<ReceivedMessage>();
    }
}
