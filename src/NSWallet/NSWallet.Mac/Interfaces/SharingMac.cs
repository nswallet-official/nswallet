using System;
using System.IO;
using Foundation;
using NSWallet.Interfaces;
using NSWallet.Mac.Interfaces;
//using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SharingMac))]
namespace NSWallet.Mac.Interfaces
{
	public class SharingMac : IShare
	{
		public void Share(string message)
		{
			//// Set data to share
			//var activityController = new UIActivityViewController(new NSObject[] {
			//	NSObject.FromObject(message)
			//}, null);
			//// Get controller to handle share process
			//var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			//while (topController.PresentedViewController != null) {
			//	topController = topController.PresentedViewController;
			//}
			//// Show share options
			//topController.PresentViewController(activityController, true, null);
		}

		public void ShareFile(string fileName, string extraText, string mimeType, string popupText, Action action)
		{
			//var ii = NSUrl.FromFilename(fileName);
			//var item = ii.Copy();
			//var message = NSObject.FromObject(extraText);
			//var activityItems = new[] { item, message };
			//var activityController = new UIActivityViewController(activityItems, null);
			//var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			//while (topController.PresentedViewController != null) {
			//	topController = topController.PresentedViewController;
			//}

			//topController.PresentViewController(activityController, true, () => { });
		}
	}
}