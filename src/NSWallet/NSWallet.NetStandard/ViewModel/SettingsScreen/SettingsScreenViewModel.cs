using System;
using System.ComponentModel;
using NSWallet.Shared;
using NSWallet.Helpers;
using Xamarin.Forms;
using System.Threading.Tasks;
using NSWallet.Controls.EntryPopup;
using NSWallet.NetStandard.Helpers.Fonts;

namespace NSWallet
{
	public class SettingsScreenViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		//public Action PremiumAlertCallback { get; set; }

		public Action AutoBackupCommandCallback { get; set; }
		public Action AutoLogoutCommandCallback { get; set; }
		public Action BackupDeletionCommandCallback { get; set; }
		public Action PasswordTipCommandCallback { get; set; }
		public Action<bool> ThemeCommandCallback { get; set; }
		public Action LanguageCommandCallback { get; set; }
		public Action FontSizeCommandCallback { get; set; }
		public Action FontCommandCallback { get; set; }
		public Action ChangePasswordCallback { get; set; }
		public Action ExpiringPeriodCommandCallback { get; set; }

		bool settingsFirstLaunch = true;
		bool settingsStartup = true;

		INavigation navigation;

		public SettingsScreenViewModel(INavigation navigation)
		{
			//PremiumAlertCallback = () => { };

			AutoBackupCommandCallback = () => { };
			AutoLogoutCommandCallback = () => { };
			BackupDeletionCommandCallback = () => { };
			PasswordTipCommandCallback = () => { };
			ThemeCommandCallback = (x) => { };
			LanguageCommandCallback = () => { };
			FontSizeCommandCallback = () => { };
			ChangePasswordCallback = () => { };
			FontCommandCallback = () => { };
			ExpiringPeriodCommandCallback = () => { };

			this.navigation = navigation;

			switch (Settings.AutoBackup) {
				case 0: ChosenAutoBackup = TR.Tr("autobackup_no_backup"); break;
				case 1: ChosenAutoBackup = TR.Tr("autobackup_weekly"); break;
				case 2: ChosenAutoBackup = TR.Tr("autobackup_daily"); break;
			}

			switch (Settings.AutoLogout) {
				case 0: ChosenAutoLogout = TR.Tr("settings_autologout_focus"); break;
				case 5: ChosenAutoLogout = TR.Tr("settings_autologout_5"); break;
				case 10: ChosenAutoLogout = TR.Tr("settings_autologout_10"); break;
				case 30: ChosenAutoLogout = TR.Tr("settings_autologout_30"); break;
			}

			switch (Settings.BackupDeletion) {
				case 5: ChosenBackupDeletion = TR.Tr("backups_deletion_5"); break;
				case 10: ChosenBackupDeletion = TR.Tr("backups_deletion_10"); break;
				case 30: ChosenBackupDeletion = TR.Tr("backups_deletion_30"); break;
				case 90: ChosenBackupDeletion = TR.Tr("backups_deletion_90"); break;
				case 180: ChosenBackupDeletion = TR.Tr("backups_deletion_180"); break;
			}

			switch (Settings.ExpiringPeriod) {
				case 0: ChosenExpiringPeriod = TR.Tr("settings_expiring_period_all"); break;
				case 10: ChosenExpiringPeriod = TR.Tr("settings_expiring_period_10"); break;
				case 30: ChosenExpiringPeriod = TR.Tr("settings_expiring_period_30"); break;
			}

			IsAutoFingerprintChecked = Settings.IsAutoFinger;
			IsDroidLogoutChecked = Settings.AndroidBackLogout;
			IsHidePasswordChecked = Settings.IsHidePasswordEnabled;

			IsClipCleanChecked = Settings.IsClipboardClean;
			IsExpiringSoonChecked = Settings.IsExpiringSoon;
			IsRecentlyViewedChecked = Settings.IsRecentlyViewed;
			IsMostlyViewedChecked = Settings.IsMostlyViewed;
			IsFingerprintChecked = Settings.IsFingerprintActive;
			IsAutoNightModeChecked = Settings.IsAutoNightMode;

			if (IsAutoNightModeChecked) {
				ChosenAutoNightMode = TR.Tr("settings_auto_night_mode_on");
			} else {
				ChosenAutoNightMode = TR.Tr("settings_auto_night_mode_off");
			}

			if (IsExpiringSoonChecked)
				ChosenExpiringSoon = TR.Tr("settings_is_expiring_on");
			else
				ChosenExpiringSoon = TR.Tr("settings_is_expiring_off");

			if (IsRecentlyViewedChecked)
				ChosenRecentlyViewed = TR.Tr("settings_is_recently_viewed_on");
			else
				ChosenRecentlyViewed = TR.Tr("settings_is_recently_viewed_off");

			if (IsMostlyViewedChecked)
				ChosenMostlyViewed = TR.Tr("settings_is_mostly_viewed_on");
			else
				ChosenMostlyViewed = TR.Tr("settings_is_mostly_viewed_off");

			if (IsHidePasswordChecked)
				ChosenHidePassword = TR.Tr("settings_hidepass_on");
			else
				ChosenHidePassword = TR.Tr("settings_hidepass_off");


			if (IsFingerprintChecked)
				ChosenFingerprintActive = TR.Tr("settings_fingerprint_on");
			else
				ChosenFingerprintActive = TR.Tr("settings_fingerprint_off");

			ChosenRestorePremium = TR.Tr("settings_restore_premium_description");
			ChosenPassword = TR.Tr("settings_change_password_description");
			ChosenTheme = TR.Tr(Settings.Theme);
			if (Settings.Language == Lang.LANG_CODE_SYSTEM) {
				ChosenLanguage = TR.Tr("default") + " (" + TR.LanguageHumanReadable + ")";
			} else {
				ChosenLanguage = TR.LanguageHumanReadable;
			}

			ChosenFontSize = TR.Tr(Settings.FontSize);

			// Lang.getLangByCode(Settings.Language).LanguageLocal;
			ChosenFont = NSWFontsController.GetNameByTypeface(Settings.FontFamily);

			var passTip = Settings.PasswordTip;
			if (string.IsNullOrEmpty(passTip))
				ChosenPassTip = TR.Tr("settings_pass_tooltip_off");
			else
				ChosenPassTip = passTip;

			IsAutoLoginChecked = Settings.IsAutoLoginEnabled;
			ExecuteAutoLoginCommand();

			if (IsDroidLogoutChecked)
				ChosenDroidLogout = TR.Tr("settings_android_exit_on");
			else
				ChosenDroidLogout = TR.Tr("settings_android_exit_off");

			if (IsAutoFingerprintChecked)
				ChosenAutoFingerprintActive = TR.Tr("settings_is_auto_fingerprint_on");
			else
				ChosenAutoFingerprintActive = TR.Tr("settings_is_auto_fingerprint_off");

			if (IsClipCleanChecked)
				ChosenClipClean = TR.Tr("settings_clipboard_clean_on");
			else
				ChosenClipClean = TR.Tr("settings_clipboard_clean_off");

			settingsFirstLaunch = false;
			settingsStartup = false;
		}

		Command changepassCommand;
		public Command ChangePasswordCommand {
			get {
				return changepassCommand ?? (changepassCommand = new Command(ExecuteChangePassword));
			}
		}

		protected void ExecuteChangePassword()
		{
			ChangePasswordCallback.Invoke();
		}

		#region Auto-backup Command

		Command autoBackupCommand;
		public Command AutoBackupCommand {
			get {
				return autoBackupCommand ?? (autoBackupCommand = new Command(ExecuteAutoBackupCommand));
			}
		}

		protected void ExecuteAutoBackupCommand()
		{
			AutoBackupCommandCallback.Invoke();
		}

		Command<string> autoBackupSelectedCommand;
		public Command<string> AutoBackupSelectedCommand {
			get {
				return autoBackupSelectedCommand ?? (autoBackupSelectedCommand = new Command<string>(ExecuteAutoBackupSelectedCommand));
			}
		}

		protected void ExecuteAutoBackupSelectedCommand(string selectedBackup)
		{
			if (string.Compare(selectedBackup, TR.Tr("autobackup_no_backup"), StringComparison.Ordinal) == 0) {
				Settings.AutoBackup = 0;
			}

			if (string.Compare(selectedBackup, TR.Tr("autobackup_weekly"), StringComparison.Ordinal) == 0) {
				Settings.AutoBackup = 1;
			}

			if (string.Compare(selectedBackup, TR.Tr("autobackup_daily"), StringComparison.Ordinal) == 0) {
				Settings.AutoBackup = 2;
			}

			Device.BeginInvokeOnMainThread(AppPages.Settings);
		}

		Command autoLogoutCommand;
		public Command AutoLogoutCommand {
			get {
				return autoLogoutCommand ?? (autoLogoutCommand = new Command(ExecuteAutoLogoutCommand));
			}
		}

		protected void ExecuteAutoLogoutCommand()
		{
			AutoLogoutCommandCallback.Invoke();
		}

		Command<string> autoLogoutSelectedCommand;
		public Command<string> AutoLogoutSelectedCommand {
			get {
				return autoLogoutSelectedCommand ?? (autoLogoutSelectedCommand = new Command<string>(ExecuteAutoLogoutSelectedCommand));
			}
		}

		protected void ExecuteAutoLogoutSelectedCommand(string selectedLogout)
		{
			if (string.Compare(selectedLogout, TR.Tr("settings_autologout_focus"), StringComparison.Ordinal) == 0) {
				Settings.AutoLogout = 0;
			}

			if (string.Compare(selectedLogout, TR.Tr("settings_autologout_5"), StringComparison.Ordinal) == 0) {
				Settings.AutoLogout = 5;
			}

			if (string.Compare(selectedLogout, TR.Tr("settings_autologout_10"), StringComparison.Ordinal) == 0) {
				Settings.AutoLogout = 10;
			}

			if (string.Compare(selectedLogout, TR.Tr("settings_autologout_30"), StringComparison.Ordinal) == 0) {
				Settings.AutoLogout = 30;
			}

			Device.BeginInvokeOnMainThread(AppPages.Settings);
		}

		#endregion

		Command backupDeletionCommand;
		public Command BackupDeletionCommand {
			get {
				return backupDeletionCommand ?? (backupDeletionCommand = new Command(ExecuteBackupDeletionCommand));
			}
		}

		protected void ExecuteBackupDeletionCommand()
		{
			BackupDeletionCommandCallback.Invoke();
		}

		Command<string> backupDeletionSelectedCommand;
		public Command<string> BackupDeletionSelectedCommand {
			get {
				return backupDeletionSelectedCommand ?? (backupDeletionSelectedCommand = new Command<string>(ExecuteBackupDeletionSelectedCommand));
			}
		}

		protected void ExecuteBackupDeletionSelectedCommand(string selected)
		{
			if (string.Compare(selected, TR.Tr("backups_deletion_5"), StringComparison.Ordinal) == 0) {
				Settings.BackupDeletion = 5;
			}

			if (string.Compare(selected, TR.Tr("backups_deletion_10"), StringComparison.Ordinal) == 0) {
				Settings.BackupDeletion = 10;
			}

			if (string.Compare(selected, TR.Tr("backups_deletion_30"), StringComparison.Ordinal) == 0) {
				Settings.BackupDeletion = 30;
			}

			if (string.Compare(selected, TR.Tr("backups_deletion_90"), StringComparison.Ordinal) == 0) {
				Settings.BackupDeletion = 90;
			}

			if (string.Compare(selected, TR.Tr("backups_deletion_180"), StringComparison.Ordinal) == 0) {
				Settings.BackupDeletion = 180;
			}

			Device.BeginInvokeOnMainThread(AppPages.Settings);
		}

		Command expiringPeriodCommand;
		public Command ExpiringPeriodCommand {
			get {
				return expiringPeriodCommand ?? (expiringPeriodCommand = new Command(ExecuteExpiringPeriodCommand));
			}
		}

		protected void ExecuteExpiringPeriodCommand()
		{

			ExpiringPeriodCommandCallback.Invoke();

		}

		Command<string> expiringPeriodSelectedCommand;
		public Command<string> ExpiringPeriodSelectedCommand {
			get {
				return expiringPeriodSelectedCommand ?? (expiringPeriodSelectedCommand = new Command<string>(ExecuteExpiringPeriodSelectedCommand));
			}
		}

		protected void ExecuteExpiringPeriodSelectedCommand(string selected)
		{
			if (string.Compare(selected, TR.Tr("settings_expiring_period_10"), StringComparison.Ordinal) == 0) {
				Settings.ExpiringPeriod = 10;
			}

			if (string.Compare(selected, TR.Tr("settings_expiring_period_30"), StringComparison.Ordinal) == 0) {
				Settings.ExpiringPeriod = 30;
			}

			if (string.Compare(selected, TR.Tr("settings_expiring_period_all"), StringComparison.Ordinal) == 0) {
				Settings.ExpiringPeriod = 0;
			}

			Device.BeginInvokeOnMainThread(AppPages.Settings);
		}


		#region Theme Command

		Command themeCommand;
		public Command ThemeCommand {
			get {
				return themeCommand ?? (themeCommand = new Command(ExecuteThemeCommand));
			}
		}

		protected void ExecuteThemeCommand()
		{

			ThemeCommandCallback.Invoke(true);

		}

		Command<string> themeSelectedCommand;
		public Command<string> ThemeSelectedCommand {
			get {
				return themeSelectedCommand ?? (themeSelectedCommand = new Command<string>(ExecuteThemeSelectedCommand));
			}
		}

		protected void ExecuteThemeSelectedCommand(string selectedTheme)
		{
			string themeCode = null;

			if (!string.IsNullOrEmpty(selectedTheme) && string.Compare(selectedTheme, TR.Cancel) != 0) {
				if (string.Compare(selectedTheme, TR.Tr(AppTheme.ThemeDark)) == 0) {
					themeCode = AppTheme.ThemeDark;
				} else if (string.Compare(selectedTheme, TR.Tr(AppTheme.ThemeRed)) == 0) {
					themeCode = AppTheme.ThemeRed;
				} else if (string.Compare(selectedTheme, TR.Tr(AppTheme.ThemeGreen)) == 0) {
					themeCode = AppTheme.ThemeGreen;
				} else if (string.Compare(selectedTheme, TR.Tr(AppTheme.ThemeGray)) == 0) {
					themeCode = AppTheme.ThemeGray;
				} else if (string.Compare(selectedTheme, TR.Tr(AppTheme.ThemeYellow)) == 0) {
					themeCode = AppTheme.ThemeYellow;
				} else {
					themeCode = AppTheme.ThemeDefault;
				}

				if (string.Compare(selectedTheme, TR.Tr("more_themes")) == 0) {

				} else {
					ChosenTheme = selectedTheme;
					Device.BeginInvokeOnMainThread(() => { AppTheme.SetTheme(themeCode); });
					Device.BeginInvokeOnMainThread(AppPages.Settings);
				}
			}
		}

		#endregion

		Command fontSizeCommand;
		public Command FontSizeCommand {
			get {
				return fontSizeCommand ?? (fontSizeCommand = new Command(ExecuteFontSizeCommand));
			}
		}

		protected void ExecuteFontSizeCommand()
		{
			FontSizeCommandCallback.Invoke();
		}

		Command<string> fontSizeSelectedCommand;
		public Command<string> FontSizeSelectedCommand {
			get {
				return fontSizeSelectedCommand ?? (fontSizeSelectedCommand = new Command<string>(ExecuteFontSizeSelectedCommand));
			}
		}

		protected void ExecuteFontSizeSelectedCommand(string selectedFontSize)
		{
			if (selectedFontSize == TR.Tr("font_sizes_standard"))
				Settings.FontSize = "font_sizes_standard";
			if (selectedFontSize == TR.Tr("font_sizes_large"))
				Settings.FontSize = "font_sizes_large";
			ChosenFontSize = TR.Tr(Settings.FontSize);
			Device.BeginInvokeOnMainThread(() => AppPages.Settings());
		}

		#region Language Command

		Command languageCommand;
		public Command LanguageCommand {
			get {
				return languageCommand ?? (languageCommand = new Command(ExecuteLanguageCommand));
			}
		}

		protected void ExecuteLanguageCommand()
		{
			LanguageCommandCallback.Invoke();
		}

		//Command<string> languageSelectedCommand;
		public Command<string> LanguageSelectedCommand {
			get {
				return fontSizeSelectedCommand ?? (fontSizeSelectedCommand = new Command<string>(ExecuteLanguageSelectedCommand));
			}
		}

		protected void ExecuteLanguageSelectedCommand(string selectedLanguage)
		{
			if (string.Compare(selectedLanguage, TR.Tr("languages_other")) == 0) {
				Device.BeginInvokeOnMainThread(() => Device.OpenUri(new Uri(GConsts.APP_DEV_REQUEST_LANGUAGE_URI)));
				return;
			}

			SetNewLanguage(selectedLanguage);


		}

		void SetNewLanguage(string newLanguage)
		{
			var langs = Lang.availableLangs();
			foreach (var lang in langs) {
				if (lang.LangCode == newLanguage) {
					Settings.Language = newLanguage;
					newLanguage = AppLanguage.GetCurrentLangCode();

					TR.SetLanguage(newLanguage);
					BL.ResetData(false, false, true);
					Device.BeginInvokeOnMainThread(AppPages.Settings);
					break;
				}
			}
		}

		#endregion

		#region Font Command

		Command fontCommand;
		public Command FontCommand {
			get {
				return fontCommand ?? (fontCommand = new Command(ExecuteFontCommand));
			}
		}

		protected void ExecuteFontCommand()
		{

			AppPages.FontSelector();

		}

		Command<string> fontSelectedCommand;
		public Command<string> FontSelectedCommand {
			get {
				return fontSelectedCommand ?? (fontSelectedCommand = new Command<string>(ExecuteFontSelectedCommand));
			}
		}

		protected void ExecuteFontSelectedCommand(string selectedFont)
		{
			if (string.Compare(selectedFont, null) != 0 || string.Compare(selectedFont, TR.Cancel) != 0) {
				NSWFontsController.SetFont(selectedFont);

				Device.BeginInvokeOnMainThread(AppPages.Settings);
			}
		}

		#endregion

		bool isExpiringSoonChecked;
		public bool IsExpiringSoonChecked {
			get { return isExpiringSoonChecked; }
			set {

				if (isExpiringSoonChecked == value)
					return;
				isExpiringSoonChecked = value;
				ExecuteExoiringSoonCommand();


				OnPropertyChanged("IsExpiringSoonChecked");
			}
		}

		protected void ExecuteExoiringSoonCommand()
		{
			if (IsExpiringSoonChecked) {
				// Checked code below
				Settings.IsExpiringSoon = true;
				ChosenExpiringSoon = TR.Tr("settings_is_expiring_on");
			} else {
				// Unchecked code below
				Settings.IsExpiringSoon = false;
				ChosenExpiringSoon = TR.Tr("settings_is_expiring_off");
			}
		}

		bool isRecentlyViewedChecked;
		public bool IsRecentlyViewedChecked {
			get { return isRecentlyViewedChecked; }
			set {

				if (isRecentlyViewedChecked == value)
					return;
				isRecentlyViewedChecked = value;
				ExecuteRecentlyViewedCommand();

				OnPropertyChanged("IsRecentlyViewedChecked");
			}
		}

		protected void ExecuteRecentlyViewedCommand()
		{
			if (IsRecentlyViewedChecked) {
				// Checked code below
				Settings.IsRecentlyViewed = true;
				ChosenRecentlyViewed = TR.Tr("settings_is_recently_viewed_on");
			} else {
				// Unchecked code below
				Settings.IsRecentlyViewed = false;
				ChosenRecentlyViewed = TR.Tr("settings_is_recently_viewed_off");
			}
		}

		// Is mostly viewed

		bool isMostlyViewedChecked;
		public bool IsMostlyViewedChecked {
			get { return isMostlyViewedChecked; }
			set {

				if (isMostlyViewedChecked == value)
					return;
				isMostlyViewedChecked = value;
				ExecuteMostlyViewedCommand();

				OnPropertyChanged("IsMostlyViewedChecked");
			}
		}

		protected void ExecuteMostlyViewedCommand()
		{
			if (IsMostlyViewedChecked) {
				// Checked code below
				Settings.IsMostlyViewed = true;
				ChosenMostlyViewed = TR.Tr("settings_is_mostly_viewed_on");
			} else {
				// Unchecked code below
				Settings.IsMostlyViewed = false;
				ChosenMostlyViewed = TR.Tr("settings_is_mostly_viewed_off");
			}
		}

		bool isAutoNightModeChecked;
		public bool IsAutoNightModeChecked {
			get { return isAutoNightModeChecked; }
			set {
				if (isAutoNightModeChecked == value)
					return;
				isAutoNightModeChecked = value;
				ExecuteAutoNightModeCommand();
				OnPropertyChanged("IsAutoNightModeChecked");
			}
		}

		protected void ExecuteAutoNightModeCommand()
		{
			if (IsAutoNightModeChecked) {
				// Checked code below
				Settings.IsAutoNightMode = true;
				ChosenAutoNightMode = TR.Tr("settings_auto_night_mode_on");
			} else {
				// Unchecked code below
				Settings.IsAutoNightMode = false;
				ChosenAutoNightMode = TR.Tr("settings_auto_night_mode_off");
			}
		}

		bool isDroidLogoutChecked;
		public bool IsDroidLogoutChecked {
			get { return isDroidLogoutChecked; }
			set {
				if (isDroidLogoutChecked == value)
					return;
				isDroidLogoutChecked = value;
				ExecuteAndroidExitCommand();
				OnPropertyChanged("IsDroidLogoutChecked");
			}
		}

		protected void ExecuteAndroidExitCommand()
		{
			if (IsDroidLogoutChecked) {
				// Checked code below
				Settings.AndroidBackLogout = true;
				ChosenDroidLogout = TR.Tr("settings_android_exit_on");
			} else {
				// Unchecked code below
				Settings.AndroidBackLogout = false;
				ChosenDroidLogout = TR.Tr("settings_android_exit_off");
			}
		}

		bool isAutoFingerprintChecked;
		public bool IsAutoFingerprintChecked {
			get { return isAutoFingerprintChecked; }
			set {
				if (isAutoFingerprintChecked == value)
					return;
				isAutoFingerprintChecked = value;
				ExecuteIsAutoFingerprintCommand();
				OnPropertyChanged("IsAutoFingerprintChecked");
			}
		}

		protected void ExecuteIsAutoFingerprintCommand()
		{
			if (IsAutoFingerprintChecked) {
				// Checked code below
				Settings.IsAutoFinger = true;
				ChosenAutoFingerprintActive = TR.Tr("settings_is_auto_fingerprint_on");
			} else {
				// Unchecked code below
				Settings.IsAutoFinger = false;
				ChosenAutoFingerprintActive = TR.Tr("settings_is_auto_fingerprint_off");
			}
		}


		bool isFingerprintChecked;
		public bool IsFingerprintChecked {
			get { return isFingerprintChecked; }
			set {
				if (isFingerprintChecked == value)
					return;
				isFingerprintChecked = value;
				ExecuteFingerprintCheckedCommand();

				if (!settingsFirstLaunch) {
					if (value) {
						var popup = new EntryPopup(TR.Tr("settings_fingerprint_unsafe"), null, true);
						popup.PopupClosed += (o, closedArgs) => {
							if (closedArgs.OkClicked) {
								if (!string.IsNullOrEmpty(closedArgs.Text)) {
									var password = closedArgs.Text;
									if (BL.CheckPassword(password)) {
										Settings.UsedFingerprintBefore = true;
										Settings.RememberedPassword = password;
										setFingerprintChecked(value);
									} else {
										setFingerprintChecked(false, TR.Tr("settings_fingerprint_password_wrong"));
									}
								} else {
									setFingerprintChecked(false, TR.Tr("settings_fingerprint_empty_password"));
								}
							} else {
								setFingerprintChecked(false);
							}
						};
						popup.Show();
					}
				}

				OnPropertyChanged("IsFingerprintChecked");
			}
		}

		void setFingerprintChecked(bool flag, string message = null)
		{
			if (!string.IsNullOrEmpty(message)) {
				Application.Current.MainPage.DisplayAlert(TR.Tr("alert"), message, TR.OK);
			}

			isFingerprintChecked = flag;
			ExecuteFingerprintCheckedCommand();
		}

		protected void ExecuteFingerprintCheckedCommand()
		{
			if (IsFingerprintChecked) {
				// Checked code below
				Settings.IsFingerprintActive = true;
				ChosenFingerprintActive = TR.Tr("settings_fingerprint_on");
			} else {
				// Unchecked code below
				Settings.UsedFingerprintBefore = false;
				Settings.IsFingerprintActive = false;
				ChosenFingerprintActive = TR.Tr("settings_fingerprint_off");
			}
		}

		bool isClipCleanChecked;
		public bool IsClipCleanChecked {
			get { return isClipCleanChecked; }
			set {
				if (isClipCleanChecked == value)
					return;
				isClipCleanChecked = value;
				ExecuteClipCleanCommand();
				OnPropertyChanged("IsClipCleanChecked");
			}
		}

		protected void ExecuteClipCleanCommand()
		{
			if (IsClipCleanChecked) {
				// Checked code below
				Settings.IsClipboardClean = true;
				ChosenClipClean = TR.Tr("settings_clipboard_clean_on");
			} else {
				// Unchecked code below
				Settings.IsClipboardClean = false;
				ChosenClipClean = TR.Tr("settings_clipboard_clean_off");
			}
		}

		string chosenRestorePremium;
		public string ChosenRestorePremium {
			get { return chosenRestorePremium; }
			set {
				if (chosenRestorePremium == value)
					return;
				chosenRestorePremium = value;
				OnPropertyChanged("ChosenRestorePremium");
			}
		}

		string chosenPassword;
		public string ChosenPassword {
			get { return chosenPassword; }
			set {
				if (chosenPassword == value)
					return;
				chosenPassword = value;
				OnPropertyChanged("ChosenPassword");
			}
		}

		string chosenTheme;
		public string ChosenTheme {
			get { return chosenTheme; }
			set {
				if (chosenTheme == value)
					return;
				chosenTheme = value;
				OnPropertyChanged("ChosenTheme");
			}
		}

		string chosenLanguage;
		public string ChosenLanguage {
			get { return chosenLanguage; }
			set {
				if (chosenLanguage == value)
					return;
				chosenLanguage = value;
				OnPropertyChanged("ChosenLanguage");
			}
		}

		string chosenFontSize;
		public string ChosenFontSize {
			get { return chosenFontSize; }
			set {
				if (chosenFontSize == value)
					return;
				chosenFontSize = value;
				OnPropertyChanged("ChosenFontSize");
			}
		}

		string chosenFont;
		public string ChosenFont {
			get { return chosenFont; }
			set {
				if (chosenFont == value)
					return;
				chosenFont = value;
				OnPropertyChanged("ChosenFont");
			}
		}

		string chosenSocial;
		public string ChosenSocial {
			get { return chosenSocial; }
			set {
				if (chosenSocial == value)
					return;
				chosenSocial = value;
				OnPropertyChanged("ChosenSocial");
			}
		}

		string chosenFingerprintActive;
		public string ChosenFingerprintActive {
			get { return chosenFingerprintActive; }
			set {
				if (chosenFingerprintActive == value)
					return;
				chosenFingerprintActive = value;
				OnPropertyChanged("ChosenFingerprintActive");
			}
		}

		string chosenAutoBackup;
		public string ChosenAutoBackup {
			get { return chosenAutoBackup; }
			set {
				if (chosenAutoBackup == value)
					return;
				chosenAutoBackup = value;
				OnPropertyChanged("ChosenAutoBackup");
			}
		}

		string chosenExpiringPeriod;
		public string ChosenExpiringPeriod {
			get { return chosenExpiringPeriod; }
			set {
				if (chosenExpiringPeriod == value)
					return;
				chosenExpiringPeriod = value;
				OnPropertyChanged("ChosenExpiringPeriod");
			}
		}

		string chosenPassTip;
		public string ChosenPassTip {
			get { return chosenPassTip; }
			set {
				if (chosenPassTip == value)
					return;
				chosenPassTip = value;
				OnPropertyChanged("ChosenPassTip");
			}
		}

		string chosenAutoLogout;
		public string ChosenAutoLogout {
			get { return chosenAutoLogout; }
			set {
				if (chosenAutoLogout == value)
					return;
				chosenAutoLogout = value;
				OnPropertyChanged("ChosenAutoLogout");
			}
		}

		string chosenBackupDeletion;
		public string ChosenBackupDeletion {
			get { return chosenBackupDeletion; }
			set {
				if (chosenBackupDeletion == value)
					return;
				chosenBackupDeletion = value;
				OnPropertyChanged("ChosenBackupDeletion");
			}
		}

		string chosenAutoNightMode;
		public string ChosenAutoNightMode {
			get { return chosenAutoNightMode; }
			set {
				if (chosenAutoNightMode == value)
					return;
				chosenAutoNightMode = value;
				OnPropertyChanged("ChosenAutoNightMode");
			}
		}

		string chosenDroidLogout;
		public string ChosenDroidLogout {
			get { return chosenDroidLogout; }
			set {
				if (chosenDroidLogout == value)
					return;
				chosenDroidLogout = value;
				OnPropertyChanged("ChosenDroidLogout");
			}
		}

		string chosenAutoFingerprintActive;
		public string ChosenAutoFingerprintActive {
			get { return chosenAutoFingerprintActive; }
			set {
				if (chosenAutoFingerprintActive == value)
					return;
				chosenAutoFingerprintActive = value;
				OnPropertyChanged("ChosenAutoFingerprintActive");
			}
		}

		string chosenExpiringSoon;
		public string ChosenExpiringSoon {
			get { return chosenExpiringSoon; }
			set {
				if (chosenExpiringSoon == value)
					return;
				chosenExpiringSoon = value;
				OnPropertyChanged("ChosenExpiringSoon");
			}
		}

		string chosenRecentlyViewed;
		public string ChosenRecentlyViewed {
			get { return chosenRecentlyViewed; }
			set {
				if (chosenRecentlyViewed == value)
					return;
				chosenRecentlyViewed = value;
				OnPropertyChanged("ChosenRecentlyViewed");
			}
		}

		string chosenMostlyViewed;
		public string ChosenMostlyViewed {
			get { return chosenMostlyViewed; }
			set {
				if (chosenMostlyViewed == value)
					return;
				chosenMostlyViewed = value;
				OnPropertyChanged("ChosenMostlyViewed");
			}
		}

		string chosenClipClean;
		public string ChosenClipClean {
			get { return chosenClipClean; }
			set {
				if (chosenClipClean == value)
					return;
				chosenClipClean = value;
				OnPropertyChanged("ChosenClipClean");
			}
		}

		string chosenAutoLogin;
		public string ChosenAutoLogin {
			get { return chosenAutoLogin; }
			set {
				if (chosenAutoLogin == value)
					return;
				chosenAutoLogin = value;
				OnPropertyChanged("ChosenAutoLogin");
			}
		}

		string chosenHidePassword;
		public string ChosenHidePassword {
			get { return chosenHidePassword; }
			set {
				if (chosenHidePassword == value)
					return;
				chosenHidePassword = value;
				OnPropertyChanged("ChosenHidePassword");
			}
		}

		bool isHidePasswordChecked;
		public bool IsHidePasswordChecked {
			get { return isHidePasswordChecked; }
			set {
				if (isHidePasswordChecked == value)
					return;
				isHidePasswordChecked = value;
				ExecuteHidePassCommand();
				OnPropertyChanged("IsHidePasswordChecked");
			}
		}

		bool isAutoLoginChecked;
		public bool IsAutoLoginChecked {
			get { return isAutoLoginChecked; }
			set {

				if (isAutoLoginChecked == value)
					return;
				isAutoLoginChecked = value;
				ExecuteAutoLoginCommand();

				OnPropertyChanged("IsAutoLoginChecked");
			}
		}

		protected void ExecuteAutoLoginCommand()
		{
			if (IsAutoLoginChecked) {
				// Checked code below
				IsAutoLoginChecked = true;
				Settings.IsAutoLoginEnabled = true;
				ChosenAutoLogin = TR.Tr("settings_auto_login_on");
			} else {
				// Unchecked code below
				IsAutoLoginChecked = false;
				Settings.IsAutoLoginEnabled = false;
				ChosenAutoLogin = TR.Tr("settings_auto_login_off");
			}
		}

		protected void ExecuteHidePassCommand()
		{
			if (IsHidePasswordChecked) {
				// Checked code below
				IsHidePasswordChecked = true;
				Settings.IsHidePasswordEnabled = true;
				ChosenHidePassword = TR.Tr("settings_hidepass_on");
			} else {
				// Unchecked code below
				IsHidePasswordChecked = false;
				Settings.IsHidePasswordEnabled = false;
				ChosenHidePassword = TR.Tr("settings_hidepass_off");
			}
		}


		Command autoFingerprintToggleCommand;
		public Command AutoFingerprintToggleCommand {
			get {
				return autoFingerprintToggleCommand ?? (autoFingerprintToggleCommand = new Command(ExecuteAutoFingerprintToggleCommand));
			}
		}

		protected void ExecuteAutoFingerprintToggleCommand()
		{
			IsAutoFingerprintChecked = !IsAutoFingerprintChecked;
		}

		Command fingerprintActiveToggleCommand;
		public Command FingerprintActiveToggleCommand {
			get {
				return fingerprintActiveToggleCommand ?? (fingerprintActiveToggleCommand = new Command(ExecuteFingerprintActiveToggleCommand));
			}
		}

		protected void ExecuteFingerprintActiveToggleCommand()
		{
			IsFingerprintChecked = !IsFingerprintChecked;
		}

		Command isAutoLoginToggleCommand;
		public Command IsAutoLoginToggleCommand {
			get {
				return isAutoLoginToggleCommand ?? (isAutoLoginToggleCommand = new Command(ExecuteIsAutoLoginToggleCommand));
			}
		}

		protected void ExecuteIsAutoLoginToggleCommand()
		{
			IsAutoLoginChecked = !IsAutoLoginChecked;
		}

		Command isClipCleanToggleCommand;
		public Command IsClipCleanToggleCommand {
			get {
				return isClipCleanToggleCommand ?? (isClipCleanToggleCommand = new Command(ExecuteIsClipCleanToggleCommand));
			}
		}

		protected void ExecuteIsClipCleanToggleCommand()
		{
			IsClipCleanChecked = !IsClipCleanChecked;
		}

		Command isDroidLogoutToggleCommand;
		public Command IsDroidLogoutToggleCommand {
			get {
				return isDroidLogoutToggleCommand ?? (isDroidLogoutToggleCommand = new Command(ExecuteIsDroidLogoutToggleCommand));
			}
		}

		protected void ExecuteIsDroidLogoutToggleCommand()
		{
			IsDroidLogoutChecked = !IsDroidLogoutChecked;
		}

		Command isExpiringSoonToggleCommand;
		public Command IsExpiringSoonToggleCommand {
			get {
				return isExpiringSoonToggleCommand ?? (isExpiringSoonToggleCommand = new Command(ExecuteIsExpiringSoonToggleCommand));
			}
		}

		protected void ExecuteIsExpiringSoonToggleCommand()
		{
			IsExpiringSoonChecked = !IsExpiringSoonChecked;
		}

		Command isHidePasswordToggleCommand;
		public Command IsHidePasswordToggleCommand {
			get {
				return isHidePasswordToggleCommand ?? (isHidePasswordToggleCommand = new Command(ExecuteIsHidePasswordToggleCommand));
			}
		}

		protected void ExecuteIsHidePasswordToggleCommand()
		{
			IsHidePasswordChecked = !IsHidePasswordChecked;
		}

		Command isMostlyViewedToggleCommand;
		public Command IsMostlyViewedToggleCommand {
			get {
				return isMostlyViewedToggleCommand ?? (isMostlyViewedToggleCommand = new Command(ExecuteIsMostlyViewedToggleCommand));
			}
		}

		protected void ExecuteIsMostlyViewedToggleCommand()
		{
			IsMostlyViewedChecked = !IsMostlyViewedChecked;
		}

		Command isRecentlyViewedToggleCommand;
		public Command IsRecentlyViewedToggleCommand {
			get {
				return isRecentlyViewedToggleCommand ?? (isRecentlyViewedToggleCommand = new Command(ExecuteIsRecentlyViewedToggleCommand));
			}
		}

		protected void ExecuteIsRecentlyViewedToggleCommand()
		{
			IsRecentlyViewedChecked = !IsRecentlyViewedChecked;
		}

		Command passwordTipCommand;
		public Command PasswordTipCommand {
			get {
				return passwordTipCommand ?? (passwordTipCommand = new Command(ExecutePasswordTipCommand));
			}
		}

		protected void ExecutePasswordTipCommand()
		{

			PasswordTipCommandCallback.Invoke();

		}

		Command passwordTipSuccessCommand;
		public Command PasswordTipSuccessCommand {
			get {
				return passwordTipSuccessCommand ?? (passwordTipSuccessCommand = new Command(ExecutePasswordTipSuccessCommand));
			}
		}

		protected void ExecutePasswordTipSuccessCommand(object obj)
		{
			string tip = null;

			if (obj != null) {
				tip = obj.ToString();
			}

			if (string.IsNullOrEmpty(tip)) {
				ChosenPassTip = TR.Tr("settings_pass_tooltip_off");
				Settings.PasswordTip = null;
			} else {
				ChosenPassTip = tip;
				Settings.PasswordTip = tip;
			}
		}

		Command returnMainCommand;
		public Command ReturnMainCommand {
			get {
				return returnMainCommand ?? (returnMainCommand = new Command(ExecuteReturnMainCommand));
			}
		}

		protected void ExecuteReturnMainCommand()
		{
			// Main command
		}



		bool restored = true;


		Command restoreDefaultCommand;
		public Command RestoreDefaultCommand {
			get {
				return restoreDefaultCommand ?? (restoreDefaultCommand = new Command(ExecuteRestoreDefaultCommand));
			}
		}

		protected void ExecuteRestoreDefaultCommand()
		{
			Device.BeginInvokeOnMainThread(() => {
				if (Device.RuntimePlatform == Device.iOS) {
					Application.Current.MainPage.DisplayActionSheet(TR.Tr("settings_delete_all_alert"), TR.Cancel, TR.Tr("settings_delete_all")).ContinueWith((arg) => {
						if (string.Compare(arg.Result, TR.Cancel) != 0 && !string.IsNullOrEmpty(arg.Result)) {
							resetDefaults();
						}
					});
				} else {
					Application.Current.MainPage.DisplayAlert(TR.Tr("attention"), TR.Tr("settings_delete_all_alert"), TR.Tr("settings_delete_all"), TR.Cancel).ContinueWith((arg) => {
						if (arg.Result) {
							resetDefaults();
						}
					});
				}
			});
		}

		void resetDefaults()
		{
			BackupManager.RemoveBackups();
			Settings.ResetSettings();
			BL.Close();
			PlatformSpecific.RemoveFile(PlatformSpecific.GetDBFile());
			BL.InitAPI(PlatformSpecific.GetDBFile(), AppLanguage.GetCurrentLangCode());
			BL.InitNewStorage();
			AppTheme.SetCurrentTheme();
			TR.SetLanguage(Settings.Language);
			Device.BeginInvokeOnMainThread(() => {
				AppPages.Login();
			});
		}

		/*
        bool purchased = true;
        public void ExecuteBuyPremiumCommand()
        {
            if (purchased)
            {
                Task.Run(async () =>
                {
                    purchased = false;
                    var purchaseResult = await PremiumManagement.Purchase(PremiumStatus.Premium);
                    showMessage(purchaseResult);
                    purchased = true;
                });
            }
        }
        */

		void showMessage(string message)
		{
			Device.BeginInvokeOnMainThread(() => {
				PlatformSpecific.DisplayShortMessage(message);
			});
		}

		protected void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}
}
