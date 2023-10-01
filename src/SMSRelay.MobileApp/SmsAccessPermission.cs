using Android;

namespace SMSRelay.MobileApp.MobileApp;

internal class SmsAccessPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions => 
        new[]
        {
            (Manifest.Permission.ReadSms, true),
            (Manifest.Permission.ReceiveSms, true)
        };
}
