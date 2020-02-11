using System.IO;
using Android.Graphics;
using Xamarin.Forms;
using NSWallet.Droid.Helpers.Media;
using NSWallet.NetStandard.DependencyServices.MediaService.Interfaces;

[assembly: Dependency(typeof(MediaService))]
namespace NSWallet.Droid.Helpers.Media
{
	public class MediaService : IMediaService
	{
		public byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			var options = new BitmapFactory.Options {
				InPurgeable = true
			};
			var originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

			float newHeight = 0;
			float newWidth = 0;

			var originalHeight = originalImage.Height;
			var originalWidth = originalImage.Width;

			if (originalHeight > originalWidth) {
				newHeight = height;
				float ratio = originalHeight / height;
				newWidth = originalWidth / ratio;
			} else {
				newWidth = width;
				float ratio = originalWidth / width;
				newHeight = originalHeight / ratio;
			}

			var resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);
			originalImage.Recycle();

			using (MemoryStream ms = new MemoryStream()) {
				resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
				resizedImage.Recycle();
				return ms.ToArray();
			}
		}
	}
}