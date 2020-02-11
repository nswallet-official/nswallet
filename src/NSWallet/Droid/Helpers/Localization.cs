using System;
using Xamarin.Forms;
using System.Threading;
using NSWallet.Shared.Helpers.Logs.AppLog;

[assembly:Dependency(typeof(NSWallet.Droid.Localize))]

namespace NSWallet.Droid
{
	public class Localize : ILocalize
	{
		public System.Globalization.CultureInfo GetCurrentCultureInfo()
		{
			var androidLocale = Java.Util.Locale.Default;
			var netLanguage = androidLocale.ToString().Replace("_", "-");
			try {
				// Trying to use full locale first, for example "es-AR"
				return new System.Globalization.CultureInfo(netLanguage);
			} catch (Exception ex) {
				log(ex.Message, nameof(GetCurrentCultureInfo));
				try {
					// Trying to use short locale, for example "ru-BY" -> "ru"
					return new System.Globalization.CultureInfo(netLanguage.Substring(0, 2));
				} catch {
					log(ex.Message, nameof(GetCurrentCultureInfo));
					// If nothing works, use english
					return new System.Globalization.CultureInfo("en");
				}
			}
		}

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(Localize));
		}
	}
}
