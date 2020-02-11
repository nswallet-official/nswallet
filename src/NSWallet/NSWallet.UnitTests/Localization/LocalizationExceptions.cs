namespace NSWallet.UnitTests
{
    public partial class LocalizationFixture
    {
        static bool IsTranslationException(string lang, string trTag)
        {
            // common
            if (LocalizationExceptionsCommon(trTag)) return true;

            // language specific exceptions
            switch (lang)
            {
                case "ca": if (LocalizationExceptionsLangCA(trTag)) return true; break;
                case "uk": if (LocalizationExceptionsLangUK(trTag)) return true; break;
                case "bg": if (LocalizationExceptionsLangBG(trTag)) return true; break;
                case "ru": if (LocalizationExceptionsLangRU(trTag)) return true; break;
                case "de": if (LocalizationExceptionsLangDE(trTag)) return true; break;
                case "pt": if (LocalizationExceptionsLangPT(trTag)) return true; break;
				case "es": if (LocalizationExceptionsLangES(trTag)) return true; break;
				case "pl": if (LocalizationExceptionsLangPL(trTag)) return true; break;
			}
            return false;
        }
    }
}
