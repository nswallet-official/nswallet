using System;
using Foundation;
using System.Threading;
using NSWallet.Shared.Helpers.Logs.AppLog;

[assembly: Xamarin.Forms.Dependency(typeof(NSWallet.Mac.Localize))]

namespace NSWallet.Mac
{
	public class Localize : ILocalize
	{
		public void SetLocale()
		{
			var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
			var netLocale = iosLocaleAuto.Replace("_", "-");
			System.Globalization.CultureInfo ci;
			try {
				ci = new System.Globalization.CultureInfo(netLocale);
			} catch(Exception ex) {
				ci = GetCurrentCultureInfo();
				log(ex.Message, nameof(SetLocale));
			}
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			Console.WriteLine("SetLocale: " + ci.Name);
		}

		public System.Globalization.CultureInfo GetCurrentCultureInfo()
		{
			var netLanguage = "en";
			var prefLanguageOnly = "en";
			if (NSLocale.PreferredLanguages.Length > 0) {
				var pref = NSLocale.PreferredLanguages[0];
				prefLanguageOnly = pref.Substring(0, 2);
				if (prefLanguageOnly == "pt") {
					if (pref == "pt")
						pref = "pt-BR"; // get the correct Brazilian language strings from the PCL RESX (note the local iOS folder is still "pt")
					else
						pref = "pt-PT"; // Portugal
				}
				netLanguage = pref.Replace("_", "-");
				Console.WriteLine("preferred language:" + netLanguage);
			}
			System.Globalization.CultureInfo ci = null;
			try {
				ci = new System.Globalization.CultureInfo(netLanguage);
			} catch(Exception ex) {
				log(ex.Message, nameof(GetCurrentCultureInfo));
				// iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// fallback to first characters, in this case "en"
				ci = new System.Globalization.CultureInfo(prefLanguageOnly);
			}
			return ci;
		}

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(Localize));
		}
	}
}
