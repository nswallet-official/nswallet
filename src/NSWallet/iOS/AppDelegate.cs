using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using NSWallet.iOS.Helpers;
using NSWallet.iOS.Helpers.Files;
using NSWallet.NetStandard.Helpers;
using NSWallet.Shared;
using StoreKit;
using UIKit;

namespace NSWallet.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
        App mainForms;  

		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			Xamarin.Forms.Forms.Init();
			ImageCircleRenderer.Init();
			FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
			Plugin.InAppBilling.InAppBillingImplementation.OnShouldAddStorePayment = OnShouldAddStorePayment;
			var current = Plugin.InAppBilling.CrossInAppBilling.Current;

			mainForms = new App();

			ExtendedDevice.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
			ExtendedDevice.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

			LoadApplication(mainForms);

            return base.FinishedLaunching(uiApplication, launchOptions);
		}

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
			switch (url.PathExtension) {
				case FileController.Zip:
					mainForms.ImportZip(url.Path);
					break;
				case FileController.Png:
				case FileController.Jpeg:
				case FileController.Jpg:
					var bytes = FileController.GetBytesFromFile(url.Path);
					var mediaService = new MediaService();
					var resizedBytes = mediaService.ResizeImage(bytes, GConsts.RESIZE_ICON_WIDTH, GConsts.RESIZE_ICON_HEIGHT);
					mainForms.ImportImage(resizedBytes);
					break;
			}
            return true;
        }

		bool OnShouldAddStorePayment(SKPaymentQueue queue, SKPayment payment, SKProduct product)
		{
			return true;
		}
	}
}