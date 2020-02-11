using NSWallet.iOS.Interfaces;
using NSWallet.NetStandard.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionService))]
namespace NSWallet.iOS.Interfaces
{
	public class PermissionService : IPermission
	{
		public bool ReadWritePermission { get { return true; } }
	}
}