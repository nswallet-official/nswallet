using System.IO;
using System.Threading.Tasks;
using Android.Content;
using NSWallet.Droid.DependencyServices.GalleryPicker;
using NSWallet.NetStandard.DependencyServices.GalleryPicker.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(GalleryPicker))]
namespace NSWallet.Droid.DependencyServices.GalleryPicker
{
	public class GalleryPicker : IGalleryPicker
	{
		public Task<Stream> GetImageStreamAsync()
		{
			Intent intent = new Intent();
			intent.SetType("image/*");
			intent.SetAction(Intent.ActionGetContent);

			MainActivity.Instance.StartActivityForResult(
				Intent.CreateChooser(intent, "Select Picture"),
				MainActivity.PickImageId);

			MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();
			return MainActivity.Instance.PickImageTaskCompletionSource.Task;
		}
	}
}