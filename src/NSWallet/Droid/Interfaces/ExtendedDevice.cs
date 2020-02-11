using Android.OS;
using NSWallet.Droid.Interfaces;
using NSWallet.NetStandard;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidExtendedDevice))]
namespace NSWallet.Droid.Interfaces
{
	public class AndroidExtendedDevice : IExtendedDevice
	{
		public string GetDeviceName()
		{
			return Build.Model;
		}
	}
}