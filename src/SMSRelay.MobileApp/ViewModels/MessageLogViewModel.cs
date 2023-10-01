using SMSRelay.Core.Model;

namespace SMSRelay.MobileApp.ViewModels;

public class MessageLogViewModel
{
    private readonly ICollection<RelayedMessage> _messages;

    public MessageLogViewModel()
    {
        _messages = new List<RelayedMessage>();
    }
}
