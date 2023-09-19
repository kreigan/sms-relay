using SMSRelay.ViewModels;

namespace SMSRelay.Views;

public partial class SettingsView : ContentPage
{
    public SettingsView(SettingsViewModel vm)
	{
        BindingContext = vm;
		InitializeComponent();
    }
}