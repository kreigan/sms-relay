using SMSRelay.MobileApp.Model;

namespace SMSRelay.MobileApp.Services.Settings;

public interface ISettingsService
{
    string RemoteRelayReceiverUri { get; set; }

    string RemoteRelayReceiverApiKey { get; set; }

    string RemoteRelayReceiverApiSecret { get; set; }


    bool Sim1Active { get; set; }

    string Sim1PhoneNumber { get; set; }


    bool Sim2Active { get; set; }

    string Sim2PhoneNumber { get; set; }
}