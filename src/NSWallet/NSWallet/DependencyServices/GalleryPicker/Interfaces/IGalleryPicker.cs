using System.IO;
using System.Threading.Tasks;

namespace NSWallet.NetStandard.DependencyServices.GalleryPicker.Interfaces
{
	public interface IGalleryPicker
	{
		Task<Stream> GetImageStreamAsync();
	}
}