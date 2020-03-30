using NSWallet.NetStandard.Themes.NightMode;
using Xamarin.Forms;

namespace NSWallet
{
	public static class Theme
	{
		static ITheme iThemeInstance;

		static Theme()
		{
			iThemeInstance = new DefaultTheme();
		}

		public static void Set(ITheme iTheme)
		{
			iThemeInstance = iTheme;
            PlatformSpecific.SetStatusBarColor(iThemeInstance.AppStatusBarBackground);
		}

		public static ITheme Current {
			get {
				return ThemeNightMode.IsNightModeNow ? getDarkTheme() : iThemeInstance;
			}
		}

		static ITheme getDarkTheme()
		{
			PlatformSpecific.SetStatusBarColor(Color.FromRgb(0x28, 0x30, 0x3f));
			return new DarkTheme();
		}

		public static readonly Thickness PaddingChooseIconView = GetChooseIconViewPadding();
        static Thickness GetChooseIconViewPadding()
        {
            double topPadding;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    topPadding = 20;
                    break;
                default:
                    topPadding = 0;
                    break;
            }

            return new Thickness(0, topPadding, 0, 0);
        }
    }
}

