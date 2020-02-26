using NSWallet.Helpers;
using NSWallet.NetStandard.Themes.NightMode;
using NSWallet.Shared;

namespace NSWallet
{
    public static class AppTheme
    {
        static ITheme curTheme = new DefaultTheme();

        public static void SetCurrentTheme()
        {
            SetTheme(Settings.Theme);
        }

        public const string ThemeDark = "theme_dark";
        public const string ThemeRed = "theme_red";
        public const string ThemeGreen = "theme_green";
        public const string ThemeGray = "theme_gray";
        public const string ThemeYellow = "theme_yellow";
        public const string ThemeBlue = "theme_default";
        public const string ThemeDefault = ThemeBlue;

		public static void SetTheme(string theme)
		{
			switch (theme) {
				case ThemeDark: curTheme = new DarkTheme(); break;
				case ThemeRed: curTheme = new RedTheme(); break;
				case ThemeGreen: curTheme = new GreenTheme(); break;
				case ThemeGray: curTheme = new GrayTheme(); break;
				case ThemeYellow: curTheme = new YellowTheme(); break;
				default: curTheme = new DefaultTheme(); break;
			}

			Theme.Set(curTheme);
			Settings.Theme = theme;
		}
    }
}