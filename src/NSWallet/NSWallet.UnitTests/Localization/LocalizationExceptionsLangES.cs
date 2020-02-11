namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsLangES(string trTag)
        {
            switch (trTag)
            {
                case "error":
                case "ok":
                case "no":
                case "attention":
                case "nswallet_premium":
                case "password_hint":
                case "login_button":
                case "context_menu":
                case "settings_font":
                case "settings_autologout":
                case "settings_autologout_focus":
                case "settings_pass_tooltip":
                case "settings_pass_tooltip_off":
                case "field_cancel":
                case "coins":
                case "featuresource_search_description":
                case "featuresource_feedback":
                case "settings_pass_tooltip_enter":
                case "settings_pass_tooltip_alert":
                case "settings_auto_login_off":
                case "settings_auto_login_on":
                case "settings_clipboard_clean":
                case "settings_clipboard_clean_on":
                case "settings_clipboard_clean_off":
                case "manual":
                case "auto":
                case "compass":
                case "droid":
                case "earth":
                case "ebook":
                case "euro":
                case "heart":
                case "home":
                case "linux1":
                case "linux2":
                case "lock":
                case "mac":
                case "mailbox":
                case "money":
                case "money2":
                case "psp":
                case "satellitedish":
                case "helicopter":
                case "ipad3":
                case "stick":
                case "taxi":
                case "terminal":
                case "tune":
                case "wall":
                case "wiki":
                case "windows":
                case "wireless":
                case "MAIL":
                case "init_items_internet":
                case "init_items_facebook":
                case "init_items_twitter":
                case "init_items_readme":
                case "text":
                case "link":
                case "mail":
                case "premium_upgrade":
				case "settings_group_extra":
				case "category_internet":
				case "category_social":
					return true;
            }
            return false;
        }
    }
}