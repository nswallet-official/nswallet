using Xamarin.Forms;

namespace NSWallet
{
    public interface ITheme
    {
        #region Common consts

        // Colors
        //

        Color AppBackground { get; }
        Color AppStatusBarBackground { get; }
        Color AppHeaderBackground { get; }
        Color AppHeaderTextColor { get; }
        Color DefaultLinkColor { get; }
        Color GroupBackground { get; }
        Color GroupTextColor { get; }
        Color ListBackgroundColor { get; }
        Color ListTextColor { get; }
        Color ListSecondaryTextColor { get; }
        Color ListSeparatorColor { get; }
        Color MainTitleTextColor { get; }
        Color MainSearchCancelButtonColor { get; }
        Color MainPathTextColor { get; }
        Color PremiumButtonPremScrTextColor { get; }
        Color PremiumButtonPremScrBackgroundColor { get; }
        Color LinkColor { get; }
        Color NormalTextColor { get; }
		Color CommonIconBorderColor { get; }
		Color IconPageIconName { get; }
		Color CommonGroupHeaderBackground { get; }
		Color CommonRejectButtonBackgroundColor { get; }
		Color CommonRejectButtonTextColor { get; }
		Color SelectedViewCellBackgroundColor { get; }

		Color DeleteAllButtonTextColor { get; }
		// Images and icons
		//

		string AppHeaderIcon { get; }
        string AppIcon72 { get; }
        string AppIcon1024 { get; }
        string AppSearchIcon { get; }
        string AppSearchIconCancel { get; }
        string AppIconNoBack { get; }
        string AppContextMenuIcon { get; }
        string CloseIcon { get; }
        string DeveloperIcon { get; }
        string MenuIcon { get; }
        string MenuIconHome { get; }
        string MenuIconAbout { get; }
        string MenuIconSettings { get; }
        string MenuIconLogout { get; }
        string MenuIconLabels { get; }
		string MenuIconIcons { get; }
		string MenuIconBackup { get; }
		string MenuIconDeveloper { get; }
		string MenuIconImportExport { get; }
		string MenuIconNewFeature { get; }
		string MenuIconFeedback { get; }
		string MainArrowBackIcon { get; }
        string MainPageAdd { get; }
        string LabelPageAddImage { get; }
        string ReorderUpIcon { get; }
        string ReorderDownIcon { get; }
        string ToolbarAboutIcon { get; }
		string ToolBarImportIcon { get; }
        string MainPageAddRoundIcon { get; }
        string ManagePlus { get; }
        string ManageMinus { get; }
		string CloseClipboardIcon { get; }
		string FilterIcon { get; }
		string LockIcon { get; }
		string GalleryPickerIcon { get; }
		string SettingsNightModeIcon { get; }

		// Indents
		//

		Thickness InnerMenuPadding { get; }
        Thickness EntryMargin { get; }
        Thickness MainPageHeaderPadding { get; }
        Thickness MainPageHeaderIconPadding { get; }
        Thickness MainPageAddButtonMargin { get; }
		Thickness FontPageLabelMargin { get; }
		Thickness CommonListHeaderPadding { get; }
		Thickness CommonIconMargin { get; }
		Thickness IconPageItemPadding { get; }

		// Sizes
		//

		double MenuIconHeight { get; }
        double MenuIconWidth { get; }
        double MenuBox_16 { get; }
        double MenuBox_48 { get; }
        double MenuLabelFontSize { get; }
        double MenuLabelDescrFontSize { get; }
        double EntryHeight { get; }
        double EditorHeight { get; }
        double MainBackArrowHeight { get; }
        double MainTitleHeight { get; }
        double MainPathHeight { get; }
        double MainSettingsImageHeight { get; }
        double MainPageBodySpacing { get; }
        double MainPageHeaderTitleSpacing { get; }
        double MainPageAddButtonHeight { get; }
        double MainPageListIconHeight { get; }
        double MainPageHeaderIconHeight { get; }
        int NoteMaxLinesPreview { get; }

		double ListViewTitleFontSize { get; }

		float CommonIconBorderWidth { get; }


        // Fonts
        //

        FontAttributes MenuLabelFontAttributes { get; }
        FontAttributes MainTitleFontAttributes { get; }

        #endregion

        #region LoginScreen consts

        // Colors
        //

        Color FeatureTextColor { get; }
        Color LabelTextColor { get; }
        Color CommonButtonBackground { get; }
        Color CommonButtonTextColor { get; }
        Color PremiumButtonBackground { get; }
        Color PremiumButtonTextColor { get; }
        Color MenuTextColor { get; }
        Color MenuBackgroundColor { get; }
        Color MenuTopBackgroundColor { get; }
        Color MenuTopTextColor { get; }
        Color MenuTopPremiumColor { get; }
        Color MenuTopFreeColor { get; }
        Color MenuPremiumBackgroundColor { get; }
        Color MenuPremiumTextColor { get; }

        // Fonts
        //

        FontAttributes LoginButtonFontAttributes { get; }
        FontAttributes PremiumButtonFontAttributes { get; }
        FontAttributes MainPathFontAttributes { get; }

        // Sizes
        //

        double FeatureCellTickHeight { get; }
        double FeaturesRowHeight { get; }
        double LabelFontSize { get; }
        double LoginButtonFontSize { get; }
        double PasswordHeight { get; }
        double PremiumButtonFontSize { get; }
        double PremiumLayoutHeight { get; }
        int SocialIconHeight { get; }
		int FingerPrintImageHeight { get; }
		double DescriptionLabelFontSize { get; }
		double CommonIconHeight { get; }
		double CommonIconWidth { get; }

		// Images and icons
		//

		string AttentionIcon { get; }
        string CellTickIcon { get; }
        string FacebookIcon { get; }
        string PremiumIcon { get; }
        string ShareIcon { get; }
        string ThumbsUpIcon { get; }
        string TwitterIcon { get; }
        string ICON_PREMIUM_FEEDBACK { get; }
        string ICON_PREMIUM_OTHER { get; }
        string ICON_PREMIUM_SEARCH { get; }
        string ICON_PREMIUM_SPECIAL { get; }
        string ICON_PREMIUM_THEMES { get; }

        // Indents
        //

        Thickness BodyPadding { get; }
        Thickness FeatureCellPadding { get; }
        Thickness FeatureLabelMargin { get; }
        Thickness FeatureMargin { get; }
        Thickness FeaturesViewContentPadding { get; }
        Thickness FeaturesViewMargin { get; }
        Thickness LabelMargin { get; }
        Thickness PasswordMargin { get; }
        Thickness SocialIconMargin { get; }

        // Other stuff
        //

        bool FeaturesEnabled { get; }
        SeparatorVisibility FeaturesSeparatorVisibility { get; }

        #endregion

        #region AboutScreen consts

        // Sizes
        //

        double DeveloperIconHeight { get; }

        // Indents
        //

        Thickness AppIconPadding { get; }

        #endregion

        #region SettingsScreen consts

        // Images and icons
        //

        string SettingsPremium { get; }
        string SettingsLanguage { get; }
		string SettingsFontSize { get; }
        string SettingsTheme { get; }
        string SettingsFonts { get; }
        string SettingsSocial { get; }
        string SettingsChangePass { get; }
        string SettingsExpiring { get; }
        string SettingsBackupLevel { get; }
        string SettingsAutoLogin { get; }
        string SettingsBackupDelete { get; }
        string SettingsClipboard { get; }
        string SettingsExpiringPeriod { get; }
        string SettingsMost { get; }
        string SettingsRecent { get; }
        string SettingsHidePass { get; }
        string SettingsLogoutInterval { get; }
        string SettingsPasswordTip { get; }
		string SettingsFingerprint { get; }
		string SettingsDeleteAll { get; }
        string CheckBox_Yes { get; }
        string CheckBox_No { get; }

        #endregion

        #region BackupScreen consts

        Color BackupTextColor { get; }

        #endregion

        // Events

        string ChristmasDiscountImage { get; }
        string ChristmasCongratsImage { get; }

        double ChristmasImageHeight { get; }

        int ButtonRadius { get; }

		string IEMenuExport2PDF { get; }
		string IEMenuExport2TXT { get; }
		string IEMenuImportFromBackup { get; }
    }
}

