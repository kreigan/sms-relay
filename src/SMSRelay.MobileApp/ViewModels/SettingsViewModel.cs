using System.ComponentModel;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;

using SMSRelay.MobileApp.Services.Settings;

using Font = Microsoft.Maui.Font;

namespace SMSRelay.MobileApp.ViewModels;

public class SettingsViewModel : ObservableValidator
{
    private const string smsPermissionWarningMessage = "Allow SMS access";

    private readonly ISettingsService _settingsService;

    private string _remoteRelayReceiverUri;
    private string _remoteRelayReceiverApiKey;
    private string _remoteRelayReceiverApiSecret;

    private bool _sim1Active;
    private string _sim1PhoneNumber;

    private bool _sim2Active;
    private string _sim2PhoneNumber;

    #region Properties

    public string RemoteRelayReceiverUri
    {
        get => _remoteRelayReceiverUri;
        set
        {
            SetProperty(ref _remoteRelayReceiverUri, value);
            _settingsService.RemoteRelayReceiverUri = value;
        }
    }

    public string RemoteRelayReceiverApiKey
    {
        get => _remoteRelayReceiverApiKey;
        set
        {
            SetProperty(ref _remoteRelayReceiverApiKey, value);
            _settingsService.RemoteRelayReceiverApiKey = value;
        }
    }

    public string RemoteRelayReceiverApiSecret
    {
        get => _remoteRelayReceiverApiSecret;
        set
        {
            SetProperty(ref _remoteRelayReceiverApiSecret, value);
            _settingsService.RemoteRelayReceiverApiSecret = value;
        }
    }

    public bool Sim1Active
    {
        get => _sim1Active;
        set => SetProperty(ref _sim1Active, value);
    }

    public string Sim1PhoneNumber
    {
        get => _sim1PhoneNumber;
        set
        {
            SetProperty(ref _sim1PhoneNumber, value);
            _settingsService.Sim1PhoneNumber = value;
        }
    }

    public bool Sim2Active
    {
        get => _sim2Active;
        set => SetProperty(ref _sim2Active, value);
    }

    public string Sim2PhoneNumber
    {
        get => _sim2PhoneNumber;
        set
        {
            SetProperty(ref _sim2PhoneNumber, value);
            _settingsService.Sim2PhoneNumber = value;
        }
    }

    #endregion

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
    }

    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(Sim1Active):
                await ToggleSim1Active();
                break;
            case nameof(Sim2Active):
                await ToggleSim2Active();
                break;
            default:
                break;
        }
    }

    private async Task ToggleSim1Active()
    {
        if (_sim1Active)
        {
            _settingsService.Sim1Active = _sim1Active = await CheckOrRequestPermissionAsync();
            if (!_sim1Active)
                await ShowSmsPermissionWarning();
        }
        else
        {
            _settingsService.Sim1Active = false;
        }
    }

    private async Task ToggleSim2Active()
    {
        if (_sim2Active)
        {
            _settingsService.Sim2Active = _sim2Active = await CheckOrRequestPermissionAsync();
            if (!_sim2Active)
                await ShowSmsPermissionWarning();
        }
        else
        {
            _settingsService.Sim2Active = _sim2Active;
        }
    }

    private static async Task<bool> CheckOrRequestPermissionAsync()
    {
        return await Permissions.CheckStatusAsync<Permissions.Sms>() == PermissionStatus.Granted
            || await Permissions.RequestAsync<Permissions.Sms>() == PermissionStatus.Granted;
    }

    private static async Task ShowSmsPermissionWarning()
    {
        SnackbarOptions snackbarOptions = new()
        {
            BackgroundColor = Colors.DarkRed,
            TextColor = Colors.AntiqueWhite,
            Font = Font.SystemFontOfSize(14),
            ActionButtonTextColor = Colors.AntiqueWhite,
            CornerRadius = new CornerRadius(5)
        };

        string actionButtonText = "Dismiss";
        TimeSpan duration = TimeSpan.FromSeconds(3);

        ISnackbar snackbar = Snackbar.Make(smsPermissionWarningMessage, action: null, actionButtonText, duration, snackbarOptions);

        await snackbar.Show(new CancellationTokenSource().Token);
    }
}
