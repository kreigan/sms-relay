using SMSRelay.Core.Model;

namespace SMSRelay.MobileApp.Services.Relay;

public interface IRelayService
{
    Task<bool> RelayAsync(ReceivedMessage message, CancellationToken cancellationToken);
}
