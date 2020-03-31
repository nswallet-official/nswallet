using System;
using Xamarin.Forms;
using NSWallet.Shared;
using System.IO;
using FFImageLoading.Forms;

namespace NSWallet
{
	public static class Resources
	{

		public static CachedImage GetImage(string imageName)
		{
			var image = new CachedImage();
			Stream stream = NSWRes.GetImage(imageName);
			image.Source = ImageSource.FromStream(() => stream);
			return image;
		}

        public static ImageSource GetImageSource(string imageName) {
            return ImageSource.FromStream(() => NSWRes.GetImage(imageName));
        }
	}
}
