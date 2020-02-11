namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsLangBG(string trTag)
        {
            switch (trTag)
            {
                case "airplane":
                case "ok":
                case "flag":
                case "giftcard":
                case "giftcard2":
                case "heart":
                case "helicopter":
                case "home":
                case "info":
                case "ipad":
                case "ipad3":
                case "iphone":
                case "iphone2":
                case "key":
                case "key2":
                case "linux1":
                case "linux2":
                case "lock":
                case "mac":
                case "mail3":
                case "mailbox":
                case "money2":
                case "psp":
                case "securitye":
                case "securityq":
                case "tune":
                case "vacation":
                case "wall":
                case "wiki":
                case "windows":
                case "xcode":
                case "MAIL":
                case "init_items_facebook":
                case "init_items_twitter":
                case "link":
                case "mail":
                case "premium_upgrade":
                    return true;

            }
            return false;
        }
    }
}
