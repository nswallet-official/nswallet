namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsLangDE(string trTag)
        {
            switch (trTag)
            {
                case "version_label":
                case "build_label":
                case "platform_label":
                case "premium_status":
                case "premium_upgrade":
                case "ok":
                case "system":
                case "nswallet_premium":
                case "app_disclaimer":
                case "PINC":
                case "NAME":
                case "ACNT":
                case "MAIL":
                case "featuresource_feedback":
                case "menu_home":
                case "premium":
                case "settings_premium":
                case "context_menu":
                case "system_lang":
                case "auto":
                case "init_items_facebook":
                case "init_items_twitter":
                case "init_items_internet":
                case "init_items_banking":
                case "release_notes":
                case "default":
                case "text":
                case "link":
                case "mail":
                case "banking":
                case "cd":
                case "comp":
                case "ebook":
                case "dollar":
                case "euro":
                case "info":
                case "home":
                case "mac":
                case "droid":
                case "linux1":
                case "mail3":
                case "psp":
                case "server":
                case "taxi":
                case "terminal":
                case "wall":
                case "wiki":
                case "sync":
                case "wireless":
                case "premiumlegacy_status":
				case "section_export":
				case "section_import":
				case "category_internet":
				case "category_clouds":
				case "settings_group_extra":
				case "icons_filter":
				case "nswallet_feedback":
					return true;

            }
            return false;
        }
    }
}
