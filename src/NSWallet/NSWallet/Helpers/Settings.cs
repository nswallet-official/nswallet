// Helpers/Settings.cs
using System;
using NSWallet.Shared;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace NSWallet.Helpers
{

    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        static ISettings AppSettings {
			get { return CrossSettings.Current; }
		}

        // Constants

        const string FirstLaunchKey = "firstlaunch_key";
        static readonly bool FirstLaunchDefault = true;

        const string BuildKey = "build_key";
        static readonly string BuildDefault = "0";


        const string AutoBackupDateKey = "autobackuptime_key";
        static readonly string AutoBackupDateDefault = default(DateTime).ToString();

        const string BackupDeletionKey = "settingsdeletion_key";
        static readonly int BackupDeletionDefault = 30;

        const string AutoBackupKey = "autobackup_key";
        static readonly int AutoBackupDefault = 1;

        const string LangKey = "lang_key";
        static readonly string LangDefault = GConsts.DEFAULT_LANG;

        const string ThemeKey = "apptheme_key";
        static readonly string ThemeDefault = "Default theme";

        const string HidePassKey = "hidepass_key";
        static readonly bool HidePassDefault = true;

        const string SocKey = "soc_key";
        static readonly bool SocDefault = true;

        const string AutoLogoutKey = "autologout_key";
        static readonly int AutoLogoutDefault = 5;

        const string AndroidLogoutBackKey = "androidbacklogout_key";
        static readonly bool AndroidLogoutBackDefault = true;

		const string FontFamilyKey = "font_family_key";
		static readonly string FontFamilyDefault = null;

        const string ExpiringPeriodKey = "expiringperiod_key";
        static readonly int ExpiringPeriodDefault = 10;

        const string IsExpiringSoonKey = "isexpiringsoon_key";
        static readonly bool IsExpiringSoonDefault = false;

        const string IsRecentlyViewedKey = "isrecentlyviewed_key";
        static readonly bool IsRecentlyViewedDefault = false;

        const string IsMostlyViewedKey = "ismostlyviewed_key";
        static readonly bool IsMostlyViewedDefault = false;

        const string PasswordTipKey = "passtip_key";
        static readonly string PasswordTipDefault = null;

        const string AutoLoginKey = "autologin_key";
        static readonly bool AutoLoginDefault = false;

        const string ClipboardCleanKey = "clipboardclean_key";
        static readonly bool ClipboardCleanDefault = true;

        const string ManageLowerCaseKey = "managelowercase_key";
        static readonly bool ManageLowerCaseDefault = true;

        const string ManageUpperCaseKey = "manageuppercase_key";
        static readonly bool ManageUpperCaseDefault = true;

        const string ManageDigitsKey = "managedigits_key";
        static readonly bool ManageDigitsDefault = true;

        const string ManageSpecialSymbolsKey = "managespecialsymbols_key";
        static readonly bool ManageSpecialSymbolsDefault = false;

        const string PassGenLengthKey = "manage_passgen_key";
        static readonly int PassGenLengthDefault = 10;

		const string RememberedPasswordKey = "rempass_key";
		static readonly string RememberedPasswordDefault = "";

		const string IsFingerprintActiveKey = "isfingerprintactive_key";
		static readonly bool IsFingerprintActiveDefault = false;

		const string FingerprintCountKey = "fingerprintcount_key";
		static readonly int FingerprintCountDefault = 0;

		const string LastLoginDateKey = "lastlogin_key";
		static readonly DateTime LastLoginDateDefault;

		const string UsedFingerprintBeforeKey = "usedfingerprintbefore_key";
		static readonly bool UsedFingerprintBeforeDefault = false;

		const string DevOpsOnKey = "dev_ops_key";
		static readonly bool DevOpsOnDefault = false;

		const string LaunchCountKey = "launch_count_key";
		static readonly int LaunchCountDefault = 0;

		const string LaunchCountRememberedKey = "launch_count_remembered_key";
		static readonly bool LaunchCountRememberedDefault = false;

		const string IsFeedbackKey = "is_feedback_key";
		static readonly bool IsFeedbackDefault = false;

		const string FontSizeKey = "font_size_key";
		static readonly string FontSizeDefault = "font_sizes_standard";

		const string IconsFilterKey = "icons_filter_key";
		static readonly bool IconsFilterDefault = false;

		const string GalleryPermissionKey = "gallery_permission_key";
		static readonly bool GalleryPermissionDefault = false;

		const string IsAutoNightModeKey = "is_auto_night_mode_key";
		static readonly bool IsAutoNightModeDefault = false;

		const string AreLogsActiveKey = "are_logs_active_key";
		static readonly bool AreLogsActiveDefault = false;

		const string PrivacyPolicyAcceptedKey = "privacy_policy_accepted_key";
		static readonly bool PrivacyPolicyAcceptedDefault = false;

		const string TermsOfUseAcceptedKey = "terms_of_use_accepted_key";
		static readonly bool TermsOfUseAcceptedDefault = false;

		const string ChangePasswordUnicodeIOSBugKey = "change_password_unicode_ios_key";
		static readonly bool ChangePasswordUnicodeIOSBugDefault = false;

		const string IsAutoFingerKey = "is_auto_finger_key";
		static readonly bool IsAutoFingerDefault = false;



		public static void ResetSettings()
		{
			AppSettings.Clear();
		}

        public static bool FirstLaunch
        {
            get
            {
                return AppSettings.GetValueOrDefault(FirstLaunchKey, FirstLaunchDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FirstLaunchKey, value);
            }
        }

        public static string Build
        {
            get
            {
                return AppSettings.GetValueOrDefault(BuildKey, BuildDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BuildKey, value);
            }
        }

        public static string AutoBackupTime
        {
            get
            {
                return AppSettings.GetValueOrDefault(AutoBackupDateKey, AutoBackupDateDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AutoBackupDateKey, value);
            }
        }

        public static int BackupDeletion
        {
            get
            {
                return AppSettings.GetValueOrDefault(BackupDeletionKey, BackupDeletionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BackupDeletionKey, value);
            }
        }

        public static int AutoBackup
        {
            get
            {
                return AppSettings.GetValueOrDefault(AutoBackupKey, AutoBackupDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AutoBackupKey, value);
            }
        }

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(LangKey, LangDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LangKey, value);
            }
        }

        public static string Theme
        {
            get
            {
                return AppSettings.GetValueOrDefault(ThemeKey, ThemeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ThemeKey, value);
            }
        }

        public static string FontFamily
        {
            get
            {
                return AppSettings.GetValueOrDefault(FontFamilyKey, FontFamilyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FontFamilyKey, value);
            }
        }

        public static bool IsHidePasswordEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(HidePassKey, HidePassDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(HidePassKey, value);
            }
        }


        public static int AutoLogout
        {
            get
            {
                return AppSettings.GetValueOrDefault(AutoLogoutKey, AutoLogoutDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AutoLogoutKey, value);
            }
        }

        public static bool AndroidBackLogout
        {
            get
            {
                return AppSettings.GetValueOrDefault(AndroidLogoutBackKey, AndroidLogoutBackDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AndroidLogoutBackKey, value);
            }
        }

        public static int ExpiringPeriod
        {
            get
            {
                return AppSettings.GetValueOrDefault(ExpiringPeriodKey, ExpiringPeriodDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ExpiringPeriodKey, value);
            }
        }

        public static bool IsExpiringSoon
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsExpiringSoonKey, IsExpiringSoonDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsExpiringSoonKey, value);
            }
        }

        public static bool IsRecentlyViewed
        {
            get { return AppSettings.GetValueOrDefault(IsRecentlyViewedKey, IsRecentlyViewedDefault); }
            set { AppSettings.AddOrUpdateValue(IsRecentlyViewedKey, value); }
        }

        public static bool IsMostlyViewed
        {
            get { return AppSettings.GetValueOrDefault(IsMostlyViewedKey, IsMostlyViewedDefault); }
            set { AppSettings.AddOrUpdateValue(IsMostlyViewedKey, value); }
        }

        public static string PasswordTip
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordTipKey, PasswordTipDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordTipKey, value);
            }
        }

        public static bool IsAutoLoginEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(AutoLoginKey, AutoLoginDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AutoLoginKey, value);
            }
        }

        public static bool IsClipboardClean
        {
            get
            {
                return AppSettings.GetValueOrDefault(ClipboardCleanKey, ClipboardCleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ClipboardCleanKey, value);
            }
        }

        public static bool ManageUpperCase
        {
            get
            {
                return AppSettings.GetValueOrDefault(ManageUpperCaseKey, ManageUpperCaseDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ManageUpperCaseKey, value);
            }
        }

        public static bool ManageLowerCase
        {
            get
            {
                return AppSettings.GetValueOrDefault(ManageLowerCaseKey, ManageLowerCaseDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ManageLowerCaseKey, value);
            }
        }

        public static bool ManageDigits
        {
            get
            {
                return AppSettings.GetValueOrDefault(ManageDigitsKey, ManageDigitsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ManageDigitsKey, value);
            }
        }

        public static bool ManageSpecialSymbols
        {
            get
            {
                return AppSettings.GetValueOrDefault(ManageSpecialSymbolsKey, ManageSpecialSymbolsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ManageSpecialSymbolsKey, value);
            }
        }

        public static int PassGenLength
        {
            get
            {
                return AppSettings.GetValueOrDefault(PassGenLengthKey, PassGenLengthDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PassGenLengthKey, value);
            }
        }

		public static string RememberedPassword {
			get {
				return AppSettings.GetValueOrDefault(RememberedPasswordKey, RememberedPasswordDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(RememberedPasswordKey, value);
			}
		}

		public static bool IsFingerprintActive {
			get {
				return AppSettings.GetValueOrDefault(IsFingerprintActiveKey, IsFingerprintActiveDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(IsFingerprintActiveKey, value);
			}
		}

		public static int FingerprintCount {
			get {
				return AppSettings.GetValueOrDefault(FingerprintCountKey, FingerprintCountDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(FingerprintCountKey, value);
			}
		}

		public static DateTime LastLoginDate {
			get {
				return AppSettings.GetValueOrDefault(LastLoginDateKey, LastLoginDateDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(LastLoginDateKey, value);
			}
		}

		public static bool UsedFingerprintBefore {
			get {
				return AppSettings.GetValueOrDefault(UsedFingerprintBeforeKey, UsedFingerprintBeforeDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(UsedFingerprintBeforeKey, value);
			}
		}

		public static bool DevOpsOn {
			get {
				return AppSettings.GetValueOrDefault(DevOpsOnKey, DevOpsOnDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(DevOpsOnKey, value);
			}
		}

		public static int LaunchCount {
			get {
				return AppSettings.GetValueOrDefault(LaunchCountKey, LaunchCountDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(LaunchCountKey, value);
			}
		}

		public static bool LaunchRememberedCount {
			get {
				return AppSettings.GetValueOrDefault(LaunchCountRememberedKey, LaunchCountRememberedDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(LaunchCountRememberedKey, value);
			}
		}

		public static bool IsFeedback {
			get {
				return AppSettings.GetValueOrDefault(IsFeedbackKey, IsFeedbackDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(IsFeedbackKey, value);
			}
		}

		public static string FontSize {
			get {
				return AppSettings.GetValueOrDefault(FontSizeKey, FontSizeDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(FontSizeKey, value);
			}
		}

		public static bool IconsFilter {
			get {
				return AppSettings.GetValueOrDefault(IconsFilterKey, IconsFilterDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(IconsFilterKey, value);
			}
		}

		public static bool GalleryPermission {
			get {
				return AppSettings.GetValueOrDefault(GalleryPermissionKey, GalleryPermissionDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(GalleryPermissionKey, value);
			}
		}

		public static bool IsAutoNightMode {
			get {
				return AppSettings.GetValueOrDefault(IsAutoNightModeKey, IsAutoNightModeDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(IsAutoNightModeKey, value);
            }
        }
        
		public static bool AreLogsActive {
			get {
				return AppSettings.GetValueOrDefault(AreLogsActiveKey, AreLogsActiveDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(AreLogsActiveKey, value);
			}
		}

		public static bool PrivacyPolicyAccepted {
			get {
				return AppSettings.GetValueOrDefault(PrivacyPolicyAcceptedKey, PrivacyPolicyAcceptedDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(PrivacyPolicyAcceptedKey, value);
			}
		}

		public static bool TermsOfUseAccepted {
			get {
				return AppSettings.GetValueOrDefault(TermsOfUseAcceptedKey, TermsOfUseAcceptedDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(TermsOfUseAcceptedKey, value);
			}
		}

		public static bool ChangePasswordUnicodeIOSBug {
			get {
				return AppSettings.GetValueOrDefault(ChangePasswordUnicodeIOSBugKey, ChangePasswordUnicodeIOSBugDefault);
			} set {
				AppSettings.AddOrUpdateValue(ChangePasswordUnicodeIOSBugKey, value);
			}
		}

		public static bool IsAutoFinger {
			get {
				return AppSettings.GetValueOrDefault(IsAutoFingerKey, IsAutoFingerDefault);
			}
			set {
				AppSettings.AddOrUpdateValue(IsAutoFingerKey, value);
			}
		}
	}
}