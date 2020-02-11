namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsCommon(string trTag)
        {
            switch (trTag)
            {
                case "admin_password_placeholder":
                case "admin_password_enter":
                case "admin_password_success":
                case "admin_password_empty":
                case "admin_password_wrong":
                case "admin_panel":
                case "admin_premium_list":
                case "admin_check_premium":
                case "admin_panel_premium_toggle":
                case "app_name":
                case "amazon":
                case "americanexpress":
                case "apple":
                case "badoo":
                case "bitcoin":
                case "blogger":
                case "chrome":
                case "corona":
                case "dropbox":
                case "ebay":
                case "evernote":
                case "exchange":
                case "facebook":
                case "firefox":
                case "forpdaru":
                case "gameloft":
                case "gmail":
                case "googlecheckout":
                case "gplay":
                case "gplus":
                case "gtalk":
                case "hostgator":
                case "hrs":
                case "icq":
                case "instg":
                case "lingualeo":
                case "linkedin":
                case "lj":
                case "maestro":
                case "mastercard":
                case "milesmore":
                case "oovoo":
                case "paypal":
                case "picasa":
                case "owncloud":
                case "rss":
                case "skydrive":
                case "skype":
                case "steam":
                case "surfingbird":
                case "teamviewer":
                case "truecrypt":
                case "twitter":
                case "ubuntu":
                case "unfuddle":
                case "utorrent":
                case "vimeo":
                case "visa":
                case "visadebit":
                case "visaelectron":
                case "vkontakte":
                case "win":
                case "wmk":
                case "wmlogo":
                case "wordpress":
                case "xmarks":
                case "yahoo":
                case "yandex":
                case "youtube":
                case "googledrive":
                case "ipad2":
                case "iphone3":
                case "pocket":
                case "live":
                case "kinopoisk":
				case "menu_developer":
				case "admin_panel_detailed_log":
				case "admin_panel_diagnostics":
				case "hide_admin_panel":
                    return true;
            }
            return false;
        }
    }
}
