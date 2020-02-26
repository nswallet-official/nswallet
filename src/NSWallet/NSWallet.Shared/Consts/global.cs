namespace NSWallet.Shared
{
    public static partial class GConsts
    {
		// ---------------------------------------------------
		// Application info

		//public const string APP_VERSION_PUB = "3.0";
		public const string APP_COMPANY_NAME = "Nyxbull Software";
		public const string APP_WEBSITE_NAME = "www.nswallet.com";
		public const string APP_WEBSITE_URI = "https://nswallet.com";
		public const string APP_DISCLAIMER_URI = "https://privacy.nswallet.com/disclaimer.html";
		public const string APP_DEV_WEBSITE_NAME = "bykovsoft.com";
		public const string APP_DEV_WEBSITE_URI = "https://bykovsoft.com";
		public const string APP_DEV_REQUEST_LANGUAGE_URI = "https://faq.nswallet.com/langs.html";
		public const string APP_DEV_RELEASE_NOTES_URI = "https://releasenotes.nswallet.com";
		public const string APP_DEV_FAQ_URI = "https://faq.nswallet.com";

		public const string APPLINK_GOOGLEPLAY = "https://play.google.com/store/apps/details?id=com.nyxbull.nswallet";
		public const string APPLINK_APPSTORE = "https://itunes.apple.com/us/app/ns-wallet-password-manager/id701558917";
		public const string APPLINK_FACEBOOK = "https://www.facebook.com/nyxbullsoft/";
		public const string APPLINK_TWITTER = "https://twitter.com/nyxbullsoft";

		public const string APP_LOGS_EMAIL = @"incoming+bykovme/ns-wallet-xamarin@incoming.gitlab.com";

		// ---------------------------------------------------
		// Languages

		public const string DEFAULT_LANG = "--";

		// Embedded paths
		public const string EMBEDDED_LANG_PATH = ".Localization.Languages.";
        public const string EMBEDDED_LANG_FILES_PATH = ".Localization.Files.";
        public const string EMBEDDED_ASSETS_PATH = ".Resources.Assets.";
        public const string EMBEDDED_LOCAL_FILES_PATH = ".Localization.Files.";
        public const int MASTER_PASSWORD_RESTRICTION = 3;
        public const string DB_VERSION = "4";
        public const string DATABASE_FOLDER = "nswallet";
        public const string DATABASE_FILENAME = "nswallet.dat";
        public const string ROOTID = "__ROOT__";
        public const string ROOT_PARENT_ID = "________";
        public const string EXPIRING_SOON_ID = "expiringfolder_id";
        public const string RECENTLY_VIEWED_FOLDER_ID = "recentlyfolder_id";
        public const string MOSTLY_VIEWED_ID = "mostlyfolder_id";
		public const string DEFAULT_EXTERNAL_STORAGE_PATH = "sdcard";
        public const string STATS_JSON = "stats.json";
		public const string APP_LOGS_FILE_PATH = "/Logs/app.log";
		public const string DIAGNOSTICS_LOGS_FILE_PATH = "/Logs/diagnostics.log";

		public const string FEATURES_REQUEST_LINK = "https://feedback.nswallet.com/login/{0}";

		public const string NSWB = "nswb";
        public const string BACKUP_AUTO = "auto";
        public const string BACKUP_MANUAL = "manual";
        public const string BACKUP_DATEFORMAT = "yyyyMMdd-hhmmss";

        public const string PREFS_BACKUPPATH = "backup_path";
        public const string BACKUP_FOLDER = "nswBackup";
        public const string PREFS_MNT_STORAGES = "mounted_storages";

        public const string ITEM = "Item";
        public const string FOLDER = "Folder";
		public const string FIELD = "Field";

		public const string DEFAULT_ITEM_ICON = "Icons.items.icon_document_huge.png";
        public const string DEFAULT_FOLDER_ICON = "Icons.items.icon_folder.png";
        public const string DEFAULT_LABEL_ICON = "Icons.labels.icon_labelinfo3_huge.png";

        public const string ICONS_ITEMS_PATH = "Icons.items.";
        public const int ITEMID_LENGTH = 8;
		public const int FIELDID_LENGTH = 4;
		public const int LABELID_LENGTH = 4;
		public const int ICONID_LENGTH = 8;
		public const int ROOTITEM_LENGTH = 64;
        public const int DEFAULT_EXPIRY_DAYS = 30;

        public const int SEARCH_MIN_LENGTH = 3;

        public const int MAX_PASS_LEN = 32;
        public const int MIN_PASS_LEN = 3;
        public const int DEF_PASS_LEN = 10;

		public const float RESIZE_ICON_WIDTH = 128;
		public const float RESIZE_ICON_HEIGHT = 128;

		// Labels
		public const string LBL_TEXT = "Text";
        public const string LBL_DATE = "Date";
        public const string LBL_TIME = "Time";
        public const string LBL_PASS = "Password";
        public const string LBL_LINK = "URL";
        public const string LBL_PHON = "Phone";
        public const string LBL_MAIL = "E-mail";

        // Value types 
        public const string VALUETYPE_TEXT = "text";
        public const string VALUETYPE_DATE = "date";
        public const string VALUETYPE_TIME = "time";
        public const string VALUETYPE_PASS = "pass";
        public const string VALUETYPE_LINK = "link";
        public const string VALUETYPE_PHON = "phon";
        public const string VALUETYPE_MAIL = "mail";

        // System fields definitions and icons
        public const string FLDTYPE_MAIL = "MAIL";  // 01 E-mail * done
        public const string FLDTYPE_EXPD = "EXPD";  // 02 Expiration date * done
        public const string FLDTYPE_PASS = "PASS";  // 03 Password * done
        public const string FLDTYPE_NOTE = "NOTE";  // 04 Note
        public const string FLDTYPE_LINK = "LINK";  // 05 Web page * done
        public const string FLDTYPE_ACNT = "ACNT";  // 06 Account
        public const string FLDTYPE_CARD = "CARD";  // 07 Card
        public const string FLDTYPE_NAME = "NAME";  // 08 Name
        public const string FLDTYPE_PHON = "PHON";  // 09 Phone * done
        public const string FLDTYPE_PINC = "PINC";  // 10 PIN code
        public const string FLDTYPE_USER = "USER";  // 11 User
        public const string FLDTYPE_OLDP = "OLDP";  // 12 Previous/old password * done
        public const string FLDTYPE_DATE = "DATE";  // 13 Date * done
        public const string FLDTYPE_TIME = "TIME";  // 14 Time * done
        public const string FLDTYPE_SNUM = "SNUM";  // 15 Serial number
        public const string FLDTYPE_ADDR = "ADDR";  // 16 Address
        public const string FLDTYPE_SQUE = "SQUE";  // 17 Secret question
        public const string FLDTYPE_SANS = "SANS";  // 18 Secret answer
		public const string FLDTYPE_2FAC = "2FAC"; // 19 2FA code

        // Default icons for field types
        public const string FLDTYPE_MAIL_ICON = "labelmail2";       // 01 E-mail * done
        public const string FLDTYPE_EXPD_ICON = "labelcalendar";    // 02 Expiration date * done
        public const string FLDTYPE_PASS_ICON = "labelpass";        // 03 Password * done
        public const string FLDTYPE_NOTE_ICON = "labeldoc";         // 04 Note
        public const string FLDTYPE_LINK_ICON = "labelworld";       // 05 Web page
        public const string FLDTYPE_ACNT_ICON = "labelmanhead";     // 06 Account
        public const string FLDTYPE_CARD_ICON = "labelcreditcard";  // 07 Card
        public const string FLDTYPE_NAME_ICON = "labelpencil";      // 08 Name
        public const string FLDTYPE_PHON_ICON = "labelphone";       // 09 Phone
        public const string FLDTYPE_PINC_ICON = "labelasterisk";    // 10 PIN code
        public const string FLDTYPE_USER_ICON = "labelman";         // 11 User
        public const string FLDTYPE_OLDP_ICON = "labelpass";        // 12 Previous/old password *
        public const string FLDTYPE_DATE_ICON = "labelcalendar2";   // 13 Date *
        public const string FLDTYPE_TIME_ICON = "labelclock";       // 14 Time *
        public const string FLDTYPE_SNUM_ICON = "labelnumber";      // 15 Serial number
        public const string FLDTYPE_ADDR_ICON = "labelhome";        // 16 Address
        public const string FLDTYPE_SQUE_ICON = "labelquestion";    // 17 Secret question
        public const string FLDTYPE_SANS_ICON = "labelinfo3";       // 18 Secret answer

		public const double LOGS_MAX_FILE_SIZE_MB = 5;
		public const double LOGS_DEFAULT_MAX_FILE_SIZE_MB = 3; 
		public const double DIAGNOSTICS_MAX_FILE_SIZE_MB = 10;

		public const string LOGS_EMAIL_SUBJECT = "User's application logs";
		public const string LOGS_EMAIL_BODY = "Attaching logs for the developer";

		public const int LISTLENGTH_OF_CLEVER_LABELS = 5;
		public const string DEMO_PASSWORD = "DEMO01DEMO02DEMO77"; // it is initiation password
		public const string TEST_PASSWORD = "Test001"; // Actual password for demo database
	}
}
