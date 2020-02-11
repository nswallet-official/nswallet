using NSWallet.Mac;
using NSWallet.NetStandard.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionService))]
namespace NSWallet.Mac
{
	public class PermissionService : IPermission
	{
		public bool ReadWritePermission { get { return true; } }
	}
}