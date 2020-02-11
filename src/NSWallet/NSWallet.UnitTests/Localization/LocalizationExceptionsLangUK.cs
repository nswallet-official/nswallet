namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool LocalizationExceptionsLangUK(string trTag)
        {
            switch (trTag)
            {
                case "droid":
                case "mac":
                case "psp":
                case "wiki":
                case "init_items_facebook":
                case "init_items_twitter":
                    return true;
            }
            return false;
        }
    }
}
