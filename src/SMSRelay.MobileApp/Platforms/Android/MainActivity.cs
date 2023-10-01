using Android;
using Android.App;
using Android.Content.PM;

namespace SMSRelay.MobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private static readonly string[] _permissions = new[] { Manifest.Permission.ReadSms, Manifest.Permission.ReceiveSms };


    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults); 
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}