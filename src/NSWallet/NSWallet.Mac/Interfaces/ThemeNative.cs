using NSWallet.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThemeNative))]
namespace NSWallet.iOS
{
	public class ThemeNative : IThemeNative
	{
		public void SetColors(Color color)
		{
			// Do nothing
		}
	}
}