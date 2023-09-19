using SMSRelay.Model;

namespace SMSRelay.ViewModels;

public class MessageLogViewModel
{
    private readonly ICollection<RelayedMessage> _messages;

    public MessageLogViewModel()
    {
        _messages = new List<RelayedMessage>();
    }
}
