using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using NSWallet.iOS.DependencyServices.GalleryPicker;
using NSWallet.NetStandard.DependencyServices.GalleryPicker.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(GalleryPicker))]
namespace NSWallet.iOS.DependencyServices.GalleryPicker
{
	public class GalleryPicker : IGalleryPicker
	{
		TaskCompletionSource<Stream> taskCompletionSource;
		UIImagePickerController imagePicker;

		public Task<Stream> GetImageStreamAsync()
		{
			imagePicker = new UIImagePickerController {
				SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
				MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
			};

			imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
			imagePicker.Canceled += OnImagePickerCancelled;

			UIWindow window = UIApplication.SharedApplication.KeyWindow;
			var viewController = window.RootViewController;
			viewController.PresentModalViewController(imagePicker, true);

			taskCompletionSource = new TaskCompletionSource<Stream>();
			return taskCompletionSource.Task;
		}

		void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
		{
			if (taskCompletionSource.Task.Status == TaskStatus.WaitingForActivation) {
				UIImage image = args.EditedImage ?? args.OriginalImage;

				if (image != null) {
					NSData data = image.AsJPEG(1);
					Stream stream = data.AsStream();
					taskCompletionSource.SetResult(stream);
				} else {
					taskCompletionSource.SetResult(null);
				}

				imagePicker.DismissModalViewController(true);
			}
		}

		void OnImagePickerCancelled(object sender, EventArgs args)
		{
			taskCompletionSource.SetResult(null);
			imagePicker.DismissModalViewController(true);
		}
	}
}