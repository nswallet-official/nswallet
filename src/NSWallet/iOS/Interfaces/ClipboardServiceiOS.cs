using NSWallet.Interfaces;
using NSWallet.iOS.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClipboardServiceiOS))]
namespace NSWallet.iOS.Interfaces
{
	public class ClipboardServiceiOS : IClipboardService
	{
		public void CopyToClipboard(string text)
		{
			var clipboard = UIPasteboard.General;
			clipboard.String = text;
		}

		public void CleanClipboard()
		{
			var clipboard = UIPasteboard.General;
			clipboard.String = "";
		}
	}
}