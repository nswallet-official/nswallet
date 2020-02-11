using System;
using System.Drawing;
using NSWallet.iOS.Helpers;
using NSWallet.NetStandard.DependencyServices.MediaService.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaService))]
namespace NSWallet.iOS.Helpers
{
	public class MediaService : IMediaService
	{
		public UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null) {
				return null;
			}

			UIImage image;
			try {
				image = new UIImage(Foundation.NSData.FromArray(data));
			} catch (Exception e) {
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}

		public byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);

			var originalHeight = originalImage.Size.Height;
			var originalWidth = originalImage.Size.Width;

			nfloat newHeight = 0;
			nfloat newWidth = 0;

			if (originalHeight > originalWidth) {
				newHeight = height;
				nfloat ratio = originalHeight / height;
				newWidth = originalWidth / ratio;
			} else {
				newWidth = width;
				nfloat ratio = originalWidth / width;
				newHeight = originalHeight / ratio;
			}

			width = (float)newWidth;
			height = (float)newHeight;

			UIGraphics.BeginImageContext(new SizeF(width, height));
			originalImage.Draw(new RectangleF(0, 0, width, height));
			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			var bytesImagen = resizedImage.AsJPEG().ToArray();
			resizedImage.Dispose();
			return bytesImagen;
		}
	}
}