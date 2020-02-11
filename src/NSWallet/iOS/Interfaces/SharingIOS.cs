using System;
using System.IO;
using Foundation;
using NSWallet.Interfaces;
using NSWallet.iOS.Interfaces;
using QuickLook;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SharingIOS))]
namespace NSWallet.iOS.Interfaces
{
	public class SharingIOS : IShare
	{
		public void Share(string message)
		{
			// Set data to share
			var activityController = new UIActivityViewController(new NSObject[] {
				NSObject.FromObject(message)
			}, null);
			// Get controller to handle share process
			var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			while (topController.PresentedViewController != null) {
				topController = topController.PresentedViewController;
			}
			// Show share options
			topController.PresentViewController(activityController, true, null);
		}

		public void ShareFile(string fileName, string extraText, string mimeType, string popupText, Action action)
		{
			//var ii = NSUrl.FromFilename(fileName);
			//var item = ii.Copy();
			//var message = NSObject.FromObject(extraText);
			//var activityItems = new[] { item, message };
			//var activityController = new UIActivityViewController(activityItems, null);
			//var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			//while (topController.PresentedViewController != null)
			//{
			//    topController = topController.PresentedViewController;
			//}

			//topController.PresentViewController(activityController, true, () => { });

			string name = Path.GetFileName(fileName);
			Device.BeginInvokeOnMainThread(() => {
				QLPreviewItemFileSystem prevItem = new QLPreviewItemFileSystem(name, fileName);
				QLPreviewController previewController = new QLPreviewController();
				previewController.DataSource = new PreviewControllerDS(prevItem);
				var visibleViewController = GetVisibleViewController();
				visibleViewController.PresentViewController(previewController, true, null);
				previewController.DidDismiss += (sender, e) => {
					if (action != null) {
						action.Invoke();
					}
				};
			});
		}

		UIViewController GetVisibleViewController(UIViewController controller = null)
		{
			controller = controller ?? UIApplication.SharedApplication.KeyWindow.RootViewController;

			if (controller.PresentedViewController == null)
				return controller;

			if (controller.PresentedViewController is UINavigationController) {
				return ((UINavigationController)controller.PresentedViewController).VisibleViewController;
			}

			if (controller.PresentedViewController is UITabBarController) {
				return ((UITabBarController)controller.PresentedViewController).SelectedViewController;
			}

			return GetVisibleViewController(controller.PresentedViewController);
		}

		public class PreviewControllerDS : QLPreviewControllerDataSource
		{
			private QLPreviewItem _item;

			public PreviewControllerDS(QLPreviewItem item)
			{
				_item = item;
			}

			public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, System.nint index)
			{
				return _item;
			}

			public override System.nint PreviewItemCount(QLPreviewController controller)
			{
				return 1;
			}
		}

		public class QLPreviewItemFileSystem : QLPreviewItem
		{
			string _fileName, _filePath;

			public QLPreviewItemFileSystem(string fileName, string filePath)
			{
				_fileName = fileName;
				_filePath = filePath;
			}

			public override string ItemTitle {
				get {
					return _fileName;
				}
			}

			public override Foundation.NSUrl ItemUrl {
				get {
					return Foundation.NSUrl.FromFilename(_filePath);
				}
			}
		}
	}
}