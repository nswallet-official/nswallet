using System.IO;
using System.Threading.Tasks;
using NSWallet.NetStandard.DependencyServices.GalleryPicker.Interfaces;
using Xamarin.Forms;

namespace NSWallet.NetStandard.DependencyServices.GalleryPicker
{
	public static class GalleryPicker
	{
		public static Task<Stream> PickPhoto()
		{
			try {
				var dependency = DependencyService.Get<IGalleryPicker>();
				return dependency.GetImageStreamAsync();
			} catch {
				return null;
			}
		}
	}
}