using AppKit;
using NSWallet.Interfaces;
using NSWallet.Mac;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClipboardServiceMac))]
namespace NSWallet.Mac
{
	public class ClipboardServiceMac : IClipboardService
	{
		public void CopyToClipboard(string text)
		{
			var pasteboard = NSPasteboard.GeneralPasteboard;
			pasteboard.ClearContents();
			pasteboard.SetStringForType(text, NSPasteboard.NSPasteboardTypeString);
		}

		public void CleanClipboard()
		{
			var pasteboard = NSPasteboard.GeneralPasteboard;
			pasteboard.ClearContents();
		}
	}
}