using SMSRelay.MobileApp.ViewModels;

namespace SMSRelay.MobileApp.Views;

public partial class SettingsView : ContentPage
{
    public SettingsView(SettingsViewModel vm)
	{
        BindingContext = vm;
		InitializeComponent();
    }
}