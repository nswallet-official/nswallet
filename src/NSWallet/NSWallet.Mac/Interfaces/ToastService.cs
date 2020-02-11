using NSWallet.iOS;
using NSWallet.Mac;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]
namespace NSWallet.Mac
{
	public class ToastService : IMessage
	{
		public void ShowLongAlert(string message)
		{
			//
		}

		public void ShowShortAlert(string message)
		{
			//
		}
	}
}
