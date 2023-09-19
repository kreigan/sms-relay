using SMSRelay.Model;

namespace SMSRelay.Services.Settings;

public class SettingsService : ISettingsService
{
    #region Constants

    public const string RemoteRelayReceiver_Uri = "remote_relay_receiver_uri";
    public const string RemoteRelayReceiver_ApiKey = "remote_relay_receiver_api_key";
    public const string RemoteRelayReceiver_ApiSecret = "remote_relay_receiver_api_secret";

    public const string Sim_1_Active = "sim_1_active";
    public const string Sim_1_PhoneNumber = "sim_1_phone_number";

    public const string Sim_2_Active = "sim_2_active";
    public const string Sim_2_PhoneNumber = "sim_2_phone_number";

    #endregion

    public string RemoteRelayReceiverUri
    {
        get => Preferences.Get(RemoteRelayReceiver_Uri, string.Empty);
        set => Preferences.Set(RemoteRelayReceiver_Uri, value);
    }

    public string RemoteRelayReceiverApiKey
    {
        get => Preferences.Get(RemoteRelayReceiver_ApiKey, string.Empty);
        set => Preferences.Set(RemoteRelayReceiver_ApiKey, value);
    }

    public string RemoteRelayReceiverApiSecret
    {
        get => Preferences.Get(RemoteRelayReceiver_ApiSecret, string.Empty);
        set => Preferences.Set(RemoteRelayReceiver_ApiSecret, value);
    }

    public bool Sim1Active
    {
        get => Preferences.Get(Sim_1_Active, false);
        set => Preferences.Set(Sim_1_Active, value);
    }

    public string Sim1PhoneNumber
    {
        get => Preferences.Get(Sim_1_PhoneNumber, string.Empty);
        set => Preferences.Set(Sim_1_PhoneNumber, value);
    }

    public bool Sim2Active
    {
        get => Preferences.Get(Sim_2_Active, false);
        set => Preferences.Set(Sim_2_Active, value);
    }

    public string Sim2PhoneNumber
    {
        get => Preferences.Get(Sim_2_PhoneNumber, string.Empty);
        set => Preferences.Set(Sim_2_PhoneNumber, value);
    }
}
