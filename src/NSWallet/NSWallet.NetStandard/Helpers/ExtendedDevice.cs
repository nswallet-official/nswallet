using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers
{
	public static class ExtendedDevice
	{
		public static int ScreenHeight { get; set; }
		public static int ScreenWidth { get; set; }

		public static string GetDeviceName()
		{
			return DependencyService.Get<IExtendedDevice>().GetDeviceName();
		}
	}
}