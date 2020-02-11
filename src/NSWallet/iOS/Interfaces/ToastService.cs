using GlobalToast;
using NSWallet.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]
namespace NSWallet.iOS
{
	public class ToastService : IMessage
	{
		public void ShowLongAlert(string message)
		{
			Toast.MakeToast(message).SetDuration(ToastDuration.Long).Show();
		}

		public void ShowShortAlert(string message)
		{
			Toast.MakeToast(message).Show();
		}
	}
}
