using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SMSRelay.Services.Settings;

namespace SMSRelay.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    private string _remoteRelayReceiverUri;
    private string _remoteRelayReceiverApiKey;
    private string _remoteRelayReceiverApiSecret;

    private bool _sim1Active;
    private string _sim1PhoneNumber;

    private bool _sim2Active;
    private string _sim2PhoneNumber;

    private bool _smsPermissionsGranted;

    public ICommand ToggleSimForwardCommand { get; }

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        _remoteRelayReceiverUri = _settingsService.RemoteRelayReceiverUri;
        _remoteRelayReceiverApiKey = _settingsService.RemoteRelayReceiverApiKey;
        _remoteRelayReceiverApiSecret = _settingsService.RemoteRelayReceiverApiSecret;

        _sim1Active = _settingsService.Sim1Active;
        _sim1PhoneNumber = _settingsService.Sim1PhoneNumber;

        _sim2Active = _settingsService.Sim2Active;
        _sim2PhoneNumber = _settingsService.Sim2PhoneNumber;

        ToggleSimForwardCommand = new AsyncRelayCommand<int>(ToggleSimForward);
    }

    private async Task ToggleSimForward(int simSlotId)
    {

    }
}
