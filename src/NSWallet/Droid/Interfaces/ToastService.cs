using Android.Widget;
using NSWallet.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency (typeof(ToastService))]
namespace NSWallet.Droid
{
    public class ToastService : IMessage
    {
        public void ShowLongAlert(string message)
        {
            Toast.MakeText(Forms.Context, message, ToastLength.Long).Show();
        }

        public void ShowShortAlert(string message)
        {
            Toast.MakeText(Forms.Context, message, ToastLength.Short).Show();
        }
    }
}

