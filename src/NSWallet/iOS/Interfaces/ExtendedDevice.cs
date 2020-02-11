using NSWallet.iOS;
using NSWallet.NetStandard;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSExtendedDevice))]
namespace NSWallet.iOS
{
	public class iOSExtendedDevice : IExtendedDevice
	{
		public string GetDeviceName()
		{
			var device = new UIDevice();
			return device.Name;
		}
	}
}