namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsLangPT(string trTag)
        {
            switch (trTag)
            {
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
                case "giftcard":
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
                case "linux2":
                case "key2":
                case "mail3":
                case "psp":
                case "server":
                case "taxi":
                case "terminal":
                case "wall":
                case "wiki":
                case "sync":
                case "menu_backup":
                case "manual":
                case "popupmenu_item":
                case "document":
                case "wireless":
                case "ipad3":
                case "lock":
                case "tune":
                case "pencil":
                case "money2":
                case "premiumlegacy_status":
				case "settings_group_extra":
				case "category_internet":
				case "category_social":
				case "font_sizes_standard":
				case "menu_feedback_request":
				case "nswallet_feedback":
				case "continue":
					return true;

            }
            return false;
        }
    }
}
