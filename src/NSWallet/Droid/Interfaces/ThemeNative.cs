using Android.App;
using Android.OS;
using Android.Views;
using NSWallet.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ThemeNative))]
namespace NSWallet.Droid
{
    public class ThemeNative : IThemeNative
    {
        public void SetColors(Color color) {
            var activity = (Activity)Forms.Context;
            if ((activity.Window != null) && (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop))
            {
                var statusColor = color.ToAndroid();
                activity.Window.SetStatusBarColor(statusColor);
                activity.Window.SetNavigationBarColor(statusColor);
            }
        }

    }
}
