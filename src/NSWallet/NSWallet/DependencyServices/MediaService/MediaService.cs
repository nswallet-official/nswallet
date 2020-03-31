using NSWallet.NetStandard.DependencyServices.MediaService.Interfaces;
using Xamarin.Forms;

namespace NSWallet.NetStandard.DependencyServices.MediaService
{
	public static class MediaService
	{
		public static byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			try {
				var dependency = DependencyService.Get<IMediaService>();
				return dependency.ResizeImage(imageData, width, height);
			} catch {
				return null;
			}
		}
	}
}