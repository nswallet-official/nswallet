using Xamarin.Forms;

namespace NSWallet
{
    public class DarkTheme : DefaultTheme
    {
        static Color BackgroundColor = Color.FromRgb(0x39, 0x44, 0x57);
        static Color BackgroundColor2 = Color.FromRgb(0x49, 0x54, 0x67);
        static Color TextColor = Color.White;
        static Color TextColor2 = Color.LightGray;
        static Color HeaderColor = Color.FromRgb(0x32, 0x3f, 0x4f);
        static Color StatusBarColor = Color.FromRgb(0x28, 0x30, 0x3f);
        static Color SeparatorColor = Color.FromRgb(0x43, 0x4f, 0x5b);
        static Color ButtonsColor = Color.FromRgb(0x28, 0x30, 0x3f);
        static Color BackupScreenTextColor = Color.White;

		override public Color AppStatusBarBackground { get { return StatusBarColor; } }
        override public Color AppBackground { get { return BackgroundColor; } }
        override public Color AppHeaderBackground { get { return HeaderColor; } }
        override public Color AppHeaderTextColor { get { return TextColor; } }
        override public Color DefaultLinkColor { get { return Color.Navy; } }
        override public Color MenuTextColor { get { return TextColor; } }
        override public Color MenuBackgroundColor { get { return BackgroundColor2; } }
        override public Color GroupBackground { get { return AppHeaderBackground; } }
        override public Color GroupTextColor { get { return TextColor; } }
        override public Color MenuTopBackgroundColor { get { return BackgroundColor; } }
        override public Color MenuTopTextColor { get { return TextColor; } }
        override public Color MenuTopPremiumColor { get { return Color.Yellow; } }
        override public Color ListBackgroundColor { get { return BackgroundColor; } }
        override public Color ListTextColor { get { return Color.WhiteSmoke; } }
        override public Color ListSecondaryTextColor { get { return TextColor2; } }
        override public Color ListSeparatorColor { get { return SeparatorColor; } }
        override public Color FeatureTextColor { get { return BackgroundColor; } }
        override public Color LabelTextColor { get { return Color.LightGray; } }
        override public Color CommonButtonBackground { get { return ButtonsColor; } }
        override public Color CommonButtonTextColor { get { return TextColor ; } }
        override public Color BackupTextColor { get { return BackupScreenTextColor; }}
        override public Color LinkColor { get { return Color.LightBlue; } }
        override public Color NormalTextColor { get { return Color.LightGray; } }
		override public Color CommonGroupHeaderBackground { get { return ButtonsColor; } }
		override public Color PremiumButtonTextColor { get { return TextColor; } }
		override public Color PremiumButtonBackground { get { return ButtonsColor; } }
		override public Color SelectedViewCellBackgroundColor { get { return HeaderColor; } }


		const string mainPageAddRoundIcon = "MainScreen.Add.ic_add_round_dark.png";

        override public string MainPageAddRoundIcon { get { return mainPageAddRoundIcon; } }
    }
}