namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsLangPL(string trTag)
        {
            switch (trTag)
            {
                case "build_label":
                case "ok":
				case "premiumlegacy_status":
				case "free_status":
				case "app_disclaimer":
				case "alert":
				case "nswallet_premium":
				case "premium":
				case "menu_import":
				case "settings_premium":
				case "popupmenu_folder":
				case "comp":
				case "droid":
				case "earth":
				case "euro":
				case "mac":
				case "ipad3":
				case "linux1":
				case "mail3":
				case "mailbox":
				case "psp":
				case "terminal":
				case "tune":
				case "wiki":
				case "wireless":
				case "MAIL":
				case "init_items_internet":
				case "init_items_facebook":
				case "init_items_twitter":
				case "link":
				case "mail":
				case "section_import":
				case "category_internet":
				case "font_sizes_standard":
				case "ebook":
				case "taxi":
				case "featuresource_feedback":
				case "send_logs":
				case "choose_mailing_app":
				case "mail_app_ios_need":
				case "install_mail_appios_need":
				case "cant_open_mail_with_attachment":
				case "run_diagnostics":
				case "diagnostics_description":
				case "diagnostics_password_wrong":
				case "diagnostics_running":
				case "diagnostics_no_issues":
				case "diagnostics_issues":
				case "menu_feedback_request":
				case "change_password_critical_message_ios":
				case "nswallet_feedback":
				case "install_mail_app":
				case "diagnostics_password_enter":


					return true;

            }
            return false;
        }
    }
}
