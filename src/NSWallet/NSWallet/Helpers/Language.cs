using System;
using System.Globalization;
using Xamarin.Forms;
using NSWallet.Shared;

namespace NSWallet
{
	public static class AppLanguage
	{
		public static string GetCurrentLangCode()
		{
			string curLang = Helpers.Settings.Language;
			if (curLang == GConsts.DEFAULT_LANG)
			{
                return GetSystemLangCode();
			}
			return curLang;
		}

        public static string GetSystemLangCode() {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            var curLang = ci.TwoLetterISOLanguageName;
            if (!Lang.checkIfSupported(curLang))
            {
                return Lang.LANG_CODE_EN;
            }
            return curLang;
        }
	}
}
