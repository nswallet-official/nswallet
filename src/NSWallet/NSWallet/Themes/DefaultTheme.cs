using Xamarin.Forms;

namespace NSWallet
{
    public class DefaultTheme : ITheme
    {
        #region Common consts

        // Colors

        virtual public Color AppStatusBarBackground { get { return Color.FromRgb(0, 118, 186); } }
        virtual public Color AppBackground { get { return Color.FromRgb(50, 145, 240); } }
        virtual public Color AppHeaderBackground { get { return Color.FromRgb(0, 144, 227); } }
        virtual public Color AppHeaderTextColor { get { return Color.White; } }
        virtual public Color DefaultLinkColor { get { return Color.Navy; } }
        virtual public Color MenuTextColor { get { return Color.FromRgb(120, 120, 120); } }
        virtual public Color MenuBackgroundColor { get { return Color.FromRgb(250, 250, 250); } }
        virtual public Color GroupBackground { get { return AppHeaderBackground; } }
        virtual public Color GroupDangerBackground { get { return Color.Red; } }
        virtual public Color GroupTextColor { get { return Color.White; } }
        virtual public Color MenuTopBackgroundColor { get { return Color.FromRgb(0, 157, 248); } }
        virtual public Color MenuTopTextColor { get { return Color.White; } }
        virtual public Color MenuTopPremiumColor { get { return Color.Yellow; } }
        virtual public Color MenuTopFreeColor { get { return Color.White; } }
        virtual public Color MenuPremiumBackgroundColor { get { return Color.LimeGreen; ; } }
        virtual public Color MenuPremiumTextColor { get { return Color.Yellow; } }
        virtual public Color PremiumButtonPremScrTextColor { get { return Color.White; } }
        virtual public Color PremiumButtonPremScrBackgroundColor { get { return Color.Green; } }
        virtual public Color MainTitleTextColor { get { return Color.White; } }
        virtual public Color MainSearchCancelButtonColor { get { return Color.White; } }
        virtual public Color MainPathTextColor { get { return Color.FromRgb(228, 228, 228); } }
        virtual public Color LinkColor { get { return Color.FromHex("3366BB"); } }
        virtual public Color NormalTextColor { get { return Color.Gray; } }
		virtual public Color CommonRejectButtonBackgroundColor { get { return Color.Red; } }
		virtual public Color CommonRejectButtonTextColor { get { return Color.White; } }
		virtual public Color SelectedViewCellBackgroundColor { get { return Color.FromHex("#E0E0E0"); } }
		virtual public Color DeleteAllButtonTextColor { get { return Color.Red; } }
		// List views colors

		virtual public Color ListBackgroundColor { get { return Color.WhiteSmoke; } }
        virtual public Color ListTextColor { get { return Color.Black; } }
        virtual public Color ListSecondaryTextColor { get { return Color.Gray; } }
        virtual public Color ListSeparatorColor { get { return Color.LightGray; } }

		virtual public Color CommonIconBorderColor { get { return Color.FromHex("#546e7a"); } }
		virtual public Color IconPageIconName { get { return Color.White; } }
		virtual public Color CommonGroupHeaderBackground { get { return Color.FromRgb(1, 87, 155); } }

		// Images and icons

		virtual public string AppHeaderIcon { get { return "old_app_icon_150.png"; } }
        virtual public string AppIcon72 { get { return "Icons.app_icon_72.png"; } }
        virtual public string AppIcon1024 { get { return "Icons.app_icon_1024.png"; } }

        const string appSearchIcon = "ic_search_white.png";
        virtual public string AppSearchIcon { get { return appSearchIcon; } }

        const string appSearchIconCancel = "ic_search_cancel_white.png";
        virtual public string AppSearchIconCancel { get { return appSearchIconCancel; } }

        const string appIconNoBack = "Icons.app_icon_noback.png";
        virtual public string AppIconNoBack { get { return appIconNoBack; } }

        const string appContextMenuIcon = "dots.png";
        virtual public string AppContextMenuIcon { get { return appContextMenuIcon; } }

        const string closeIcon = "close.png";
        virtual public string CloseIcon { get { return closeIcon; } }

        const string developerIcon = "Icons.LogoBykovSoft144.png";
        virtual public string DeveloperIcon { get { return developerIcon; } }

        const string menuIcon = "menu.png";
        virtual public string MenuIcon { get { return menuIcon; } }

        const string menuIconHome = "Menu.menu_home.png";
        virtual public string MenuIconHome { get { return menuIconHome; } }

        const string menuIconAbout = "Menu.menu_about.png";
        virtual public string MenuIconAbout { get { return menuIconAbout; } }

		virtual public string MenuIconDeveloper { get { return "Menu.menu_hammer.png"; }}
		virtual public string MenuIconImportExport { get { return "Menu.menu_export_import.png"; }}

        const string menuIconSettings = "Menu.menu_settings.png";
        virtual public string MenuIconSettings { get { return menuIconSettings; } }

        const string menuIconLogout = "Menu.menu_logout.png";
        virtual public string MenuIconLogout { get { return menuIconLogout; } }

        const string menuIconLabels = "Menu.menu_labels.png";
        virtual public string MenuIconLabels { get { return menuIconLabels; } }

		const string menuIconIcons = "Menu.menu_icons.png";
		virtual public string MenuIconIcons { get { return menuIconIcons; } }

		const string menuIconBackup = "Menu.menu_backup.png";
        virtual public string MenuIconBackup { get { return menuIconBackup; } }

		const string menuIconNewFeature = "Menu.ic_menu_new_feature.png";
		virtual public string MenuIconNewFeature { get { return menuIconNewFeature; } }

		const string menuIconFeedback = "Menu.ic_menu_feedback.png";
		virtual public string MenuIconFeedback { get { return menuIconFeedback; } }

		const string mainArrowBackIcon = "Icons.arrow_left.png";
        virtual public string MainArrowBackIcon { get { return mainArrowBackIcon; } }

        const string mainPageAdd = "ic_add_white.png";
        virtual public string MainPageAdd { get { return mainPageAdd; } }

        const string labelPageAddImage = "ic_add_white.png";
        virtual public string LabelPageAddImage { get { return labelPageAddImage; } }

        const string reorderUpIcon = "ic_reorder_up_grey.png";
        virtual public string ReorderUpIcon { get { return reorderUpIcon; } }

        const string reorderDownIcon = "ic_reorder_down_grey.png";
        virtual public string ReorderDownIcon { get { return reorderDownIcon; } }

		virtual public string ToolbarAboutIcon { get { return "ic_about.png"; } }
		virtual public string ToolBarImportIcon { get { return "ic_import.png"; } }

        const string mainPageAddRoundIcon = "MainScreen.Add.ic_add_round_blue.png";
        virtual public string MainPageAddRoundIcon { get { return mainPageAddRoundIcon; } }

        const string managePlus = "MainScreen.plus.png";
        virtual public string ManagePlus { get { return managePlus; } }

        const string manageMinus = "MainScreen.minus.png";
        virtual public string ManageMinus { get { return manageMinus; } }

		const string closeClipboardIcon = "MainScreen.ic_clipboard_close_white.png";
		virtual public string CloseClipboardIcon { get { return closeClipboardIcon; } }

		const string lockIcon = "LabelsScreen.lock_icon.png";
		virtual public string LockIcon { get { return lockIcon; } }

		const string galleryPickerIcon = "ic_gallery_picker.png";
		virtual public string GalleryPickerIcon { get { return galleryPickerIcon; } }

		const string settingsNightModeIcon = "SettingsScreen.settings_night_mode.png";
		virtual public string SettingsNightModeIcon { get { return settingsNightModeIcon; } }

		// Paddings
		//

		static Thickness innerMenuPadding = new Thickness(15, 6, 15, 6);
        virtual public Thickness InnerMenuPadding { get { return innerMenuPadding; } }

        static Thickness entryMargin = new Thickness(10, 0, 10, 0);
        virtual public Thickness EntryMargin { get { return entryMargin; } }

        static Thickness mainPageHeaderPadding = new Thickness(10, 20);
        virtual public Thickness MainPageHeaderPadding { get { return mainPageHeaderPadding; } }

        static Thickness mainPageHeaderIconPadding = new Thickness(10, 0);
        virtual public Thickness MainPageHeaderIconPadding { get { return mainPageHeaderIconPadding; } }

        static Thickness mainPageAddButtonMargin = new Thickness(20, 20);
        virtual public Thickness MainPageAddButtonMargin { get { return mainPageAddButtonMargin; } }

		static Thickness fontPageLabelMargin = new Thickness(20);
		virtual public Thickness FontPageLabelMargin { get { return fontPageLabelMargin; } }

		// Sizes
		//

		static Thickness commonListHeaderPadding = new Thickness(10);
		virtual public Thickness CommonListHeaderPadding { get { return commonListHeaderPadding; } }

		static Thickness commonIconMargin = new Thickness(10);
		virtual public Thickness CommonIconMargin { get { return commonIconMargin; } }

		static Thickness iconPageItemPadding = new Thickness(5, 3);
		virtual public Thickness IconPageItemPadding { get { return iconPageItemPadding; } }

		// Sizes
		//

		const double menuIconHeight = 30;
        virtual public double MenuIconHeight { get { return menuIconHeight; } }

        const double menuIconWidth = 30;
        virtual public double MenuIconWidth { get { return menuIconWidth; } }

        const double menuBox_16 = 16;
        virtual public double MenuBox_16 { get { return menuBox_16; } }

        const double menuBox_48 = 48;
        virtual public double MenuBox_48 { get { return menuBox_48; } }

        static double menuLabelFontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        virtual public double MenuLabelFontSize { get { return menuLabelFontSize; } }

        static double menuLabelDescrFontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        virtual public double MenuLabelDescrFontSize { get { return menuLabelDescrFontSize; } }

        static double entryHeight = 45;
        virtual public double EntryHeight { get { return entryHeight; } }

        static double editorHeight = 150;
        virtual public double EditorHeight { get { return editorHeight; } }

        static double mainBackArrowHeight = 15;
        virtual public double MainBackArrowHeight { get { return mainBackArrowHeight; } }

        static double mainTitleHeight = 25;
        virtual public double MainTitleHeight { get { return mainTitleHeight; } }

        static double mainPathHeight = 15;
        virtual public double MainPathHeight { get { return mainPathHeight; } }

        static double mainSettingsImageHeight = 50;
        virtual public double MainSettingsImageHeight { get { return mainSettingsImageHeight; } }

        static double mainPageBodySpacing = 0;
        virtual public double MainPageBodySpacing { get { return mainPageBodySpacing; } }

        static double mainPageHeaderTitleSpacing = 10;
        virtual public double MainPageHeaderTitleSpacing { get { return mainPageHeaderTitleSpacing; } }

        static double mainPageAddButtonHeight = 60;
        virtual public double MainPageAddButtonHeight { get { return mainPageAddButtonHeight; } }

        static double mainPageListIconHeight = 50;
        virtual public double MainPageListIconHeight { get { return mainPageListIconHeight; } }

        static double mainPageHeaderIconHeight = 70;
        virtual public double MainPageHeaderIconHeight { get { return mainPageHeaderIconHeight; } }

        static int noteMaxLinesPreview = 3;
        virtual public int NoteMaxLinesPreview { get { return noteMaxLinesPreview; } }

		virtual public double ListViewTitleFontSize {
			get {
				switch(Device.RuntimePlatform) {
					case Device.iOS:
						return Device.GetNamedSize(NamedSize.Medium, typeof(Label));
					case Device.Android:
						return Device.GetNamedSize(NamedSize.Medium, typeof(Label)); 
					default:
						return Device.GetNamedSize(NamedSize.Medium, typeof(Label));
				}
			}
		}

		static readonly float commonIconBorderWidth = 2;
		virtual public float CommonIconBorderWidth { get { return commonIconBorderWidth; } }

		// Fonts
		//

		static FontAttributes menuLabelFontAttributes = FontAttributes.Bold;
        virtual public FontAttributes MenuLabelFontAttributes { get { return menuLabelFontAttributes; } }

        static FontAttributes mainTitleFontAttributes = FontAttributes.Bold;
        virtual public FontAttributes MainTitleFontAttributes { get { return mainTitleFontAttributes; } }

        static FontAttributes mainPathFontAttributes = FontAttributes.Italic;
        virtual public FontAttributes MainPathFontAttributes { get { return mainPathFontAttributes; } }

        #endregion

        #region LoginScreen consts

        // Colors
        //

        static Color featureTextColor = Color.Gray;
        virtual public Color FeatureTextColor { get { return featureTextColor; } }

        static Color labelTextColor = Color.Black;
        virtual public Color LabelTextColor { get { return labelTextColor; } }

		static Color commonButtonBackground = Color.FromRgb(1, 87, 155);
        virtual public Color CommonButtonBackground { get { return commonButtonBackground; } }

		static Color commonButtonTextColor = Color.White;
        virtual public Color CommonButtonTextColor { get { return commonButtonTextColor; } }

        static Color premiumButtonBackground = Color.FromRgb(62, 172, 248);
        virtual public Color PremiumButtonBackground { get { return premiumButtonBackground; } }

        static Color premiumButtonTextColor = Color.White;
        virtual public Color PremiumButtonTextColor { get { return premiumButtonTextColor; } }

        // Fonts
        //

        const FontAttributes loginButtonFontAttributes = FontAttributes.Bold;
        virtual public FontAttributes LoginButtonFontAttributes { get { return loginButtonFontAttributes; } }

        const FontAttributes premiumButtonFontAttributes = FontAttributes.Italic;
        virtual public FontAttributes PremiumButtonFontAttributes { get { return premiumButtonFontAttributes; } }

        // Sizes
        //

        const double featureCellTickHeight = 20;
        virtual public double FeatureCellTickHeight { get { return featureCellTickHeight; } }

        const double featuresRowHeight = 30;
        virtual public double FeaturesRowHeight { get { return featuresRowHeight; } }

        const double labelFontSize = 18;
        virtual public double LabelFontSize { get { return labelFontSize; } }

        virtual public double DescriptionLabelFontSize { get { return 14; } }

        const double loginButtonFontSize = 15;
        virtual public double LoginButtonFontSize { get { return loginButtonFontSize; } }

        const double passwordHeight = 45;
        virtual public double PasswordHeight { get { return passwordHeight; } }

        const double premiumButtonFontSize = 15;
        virtual public double PremiumButtonFontSize { get { return premiumButtonFontSize; } }

        const double premiumLayoutHeight = 45;
        virtual public double PremiumLayoutHeight { get { return premiumLayoutHeight; } }

        const int socialIconHeight = 50;
        virtual public int SocialIconHeight { get { return socialIconHeight; } }

		public int FingerPrintImageHeight { get { return socialIconHeight * 3; } }

		const int commonIconHeight = 60;
		virtual public double CommonIconHeight { get { return commonIconHeight; } }

		const int commonIconWidth = 60;
		virtual public double CommonIconWidth { get { return commonIconWidth; } }

		// Images and icons
		//

		const string attentionIcon = "LoginScreen.attention_image.png";
        virtual public string AttentionIcon { get { return attentionIcon; } }

        const string cellTickIcon = "LoginScreen.tick_image.png";
        virtual public string CellTickIcon { get { return cellTickIcon; } }

        const string facebookIcon = "Social.facebook_image.png";
        virtual public string FacebookIcon { get { return facebookIcon; } }

        const string shareIcon = "Social.share_image.png";
        virtual public string ShareIcon { get { return shareIcon; } }

        const string thumbsUpIcon = "Social.thumbsup_image.png";
        virtual public string ThumbsUpIcon { get { return thumbsUpIcon; } }

        const string twitterIcon = "Social.twitter_image.png";
        virtual public string TwitterIcon { get { return twitterIcon; } }

        const string premiumIcon = "LoginScreen.premium_image.png";
        virtual public string PremiumIcon { get { return premiumIcon; } }

        const string iconPremiumFeedback = "LoginScreen.premium_feedback.png";
        virtual public string ICON_PREMIUM_FEEDBACK { get { return iconPremiumFeedback; } }

        const string iconPremiumOther = "LoginScreen.premium_other.png";
        virtual public string ICON_PREMIUM_OTHER { get { return iconPremiumOther; } }

        const string iconPremiumSearch = "LoginScreen.premium_search.png";
        virtual public string ICON_PREMIUM_SEARCH { get { return iconPremiumSearch; } }

        const string iconPremiumSpecial = "LoginScreen.premium_special.png";
        virtual public string ICON_PREMIUM_SPECIAL { get { return iconPremiumSpecial; } }

        const string iconPremiumThemes = "LoginScreen.premium_themes.png";
        virtual public string ICON_PREMIUM_THEMES { get { return iconPremiumThemes; } }

		const string filterIcon = "ic_filter_white.png";
		virtual public string FilterIcon { get { return filterIcon; } }

		// Indents
		//

		static Thickness bodyPadding = new Thickness(10, 10, 10, 10);
        virtual public Thickness BodyPadding { get { return bodyPadding; } }

        static Thickness featureCellPadding = new Thickness(25, 5, 0, 5);
        virtual public Thickness FeatureCellPadding { get { return featureCellPadding; } }

        static Thickness featureLabelMargin = new Thickness(10, 0, 0, 0);
        virtual public Thickness FeatureLabelMargin { get { return featureLabelMargin; } }

        static Thickness featureMargin = new Thickness(0, 0, 0, 15);
        virtual public Thickness FeatureMargin { get { return featureMargin; } }

        static Thickness featuresViewContentPadding = new Thickness(0, 10, 0, 0);
        virtual public Thickness FeaturesViewContentPadding { get { return featuresViewContentPadding; } }

        static Thickness featuresViewMargin = new Thickness(0, 10, 0, 0);
        virtual public Thickness FeaturesViewMargin { get { return featuresViewMargin; } }

        static Thickness labelMargin = new Thickness(9, 0, 0, 0);
        virtual public Thickness LabelMargin { get { return labelMargin; } }

        static Thickness passwordMargin = new Thickness(10, 0, 10, 0);
        virtual public Thickness PasswordMargin { get { return passwordMargin; } }

        static Thickness socialIconMargin = new Thickness(0, 0, 10, 0);
        virtual public Thickness SocialIconMargin { get { return socialIconMargin; } }

        // Other stuff
        //


        virtual public bool FeaturesEnabled { get { return false; } }

        virtual public SeparatorVisibility FeaturesSeparatorVisibility { get { return SeparatorVisibility.None; } }

        #endregion

        #region AboutScreen consts

        // Sizes
        //

        virtual public double DeveloperIconHeight { get { return 50; } }

        // Indents
        //

        virtual public Thickness AppIconPadding { get { return new Thickness(7, 10, 5, 5); } }

        #endregion

        #region SettingsScreen consts

        // Images and icons
        //

        static string settingsPremium = "SettingsScreen.settings_premium.png";
        virtual public string SettingsPremium { get { return settingsPremium; } }

        static string settingsLanguage = "SettingsScreen.settings_language.png";
        virtual public string SettingsLanguage { get { return settingsLanguage; } }

        static string settingsTheme = "SettingsScreen.settings_theme.png";
        virtual public string SettingsTheme { get { return settingsTheme; } }

        static string settingsFonts = "SettingsScreen.settings_fonts.png";
        virtual public string SettingsFonts { get { return settingsFonts; } }

        static string settingsSocial = "SettingsScreen.settings_social.png";
        virtual public string SettingsSocial { get { return settingsSocial; } }

        static string settingsChangePass = "SettingsScreen.settings_chpass.png";
        virtual public string SettingsChangePass { get { return settingsChangePass; } }

        virtual public string SettingsBackupLevel { get { return "SettingsScreen.settings_backup_level.png"; } }
        virtual public string SettingsExpiring { get { return "SettingsScreen.settings_folderexpiring.png"; } }
        virtual public string SettingsAutoLogin { get { return "SettingsScreen.settings_autologin.png"; } }
        virtual public string SettingsBackupDelete { get { return "SettingsScreen.settings_backup_delete.png"; } }
        virtual public string SettingsClipboard { get { return "SettingsScreen.settings_clipboard.png"; } }
        virtual public string SettingsExpiringPeriod { get { return "SettingsScreen.settings_expiring_period.png"; } }
        virtual public string SettingsMost { get { return "SettingsScreen.settings_foldermost.png"; } }
        virtual public string SettingsRecent { get { return "SettingsScreen.settings_folderrecent.png"; } }
        virtual public string SettingsHidePass { get { return "SettingsScreen.settings_hidepass.png"; } }
        virtual public string SettingsLogoutInterval { get { return "SettingsScreen.settings_logout_interval.png"; } }
        virtual public string SettingsPasswordTip { get { return "SettingsScreen.settings_password_tip.png"; } }
		virtual public string SettingsFingerprint { get { return "SettingsScreen.settings_fingerprint.png"; } }
		virtual public string SettingsDeleteAll{ get { return "SettingsScreen.settings_restore_default.png"; } }
		virtual public string SettingsFontSize { get { return "SettingsScreen.settings_font_size.png"; } }

		static string checkBox_Yes = "SettingsScreen.checkbox_yes.png";
        virtual public string CheckBox_Yes { get { return checkBox_Yes; } }

        static string checkBox_No = "SettingsScreen.checkbox_no.png";
        virtual public string CheckBox_No { get { return checkBox_No; } }

        #endregion

        #region BackupScreen consts

        static Color backupTextColor = Color.Black;
        virtual public Color BackupTextColor { get { return backupTextColor; } }

        #endregion

        // Events

        static string christmasDiscountImage = "Events.Christmas.xmas_discount.png";
        virtual public string ChristmasDiscountImage { get { return christmasDiscountImage; } }

        static string christmasCongratsImage = "Events.Christmas.xmas_congrat.png";
        virtual public string ChristmasCongratsImage { get { return christmasCongratsImage; } }

        static double christmasImageHeight = 200;
        virtual public double ChristmasImageHeight { get { return christmasImageHeight; } }

        virtual public int ButtonRadius { get { return 5; } }

		virtual public string IEMenuExport2PDF { get { return "Menu.menu_pdf.png"; }}

		virtual public string IEMenuExport2TXT { get { return "Menu.menu_text.png"; } }

		virtual public string IEMenuImportFromBackup { get { return "Menu.menu_import.png"; } }

	


		//public string ToolBarImportIcon => throw new System.NotImplementedException();
	}
}