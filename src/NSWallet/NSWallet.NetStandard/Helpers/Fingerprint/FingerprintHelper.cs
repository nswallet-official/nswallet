using NSWallet.Shared;
using System.Threading.Tasks;
using Plugin.Fingerprint;
using Xamarin.Forms;
using System;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Plugin.Fingerprint.Abstractions;

namespace NSWallet.Helpers
{
	public static class FingerprintHelper
	{
		static bool fingerprintAvailable;

		public static bool IsEnabled { get; set; }
		public static bool IsAvailable { get { return fingerprintAvailable; } }
		public static bool IsFaceID { get; set; }

		public static void Initialize()
		{
			Task.Run(async () => {
				var result = await CrossFingerprint.Current.IsAvailableAsync();
				fingerprintAvailable = result;
				if (result) {
					var authType = await CrossFingerprint.Current.GetAuthenticationTypeAsync();
					if (authType == AuthenticationType.Face) {
						IsFaceID = true;
					} else {
						IsFaceID = false;
					}
				}
			}).ContinueWith(t => {
				if (t.Exception.InnerException != null) {
					AppLogs.Log(t.Exception.InnerException.Message, nameof(Initialize), nameof(FingerprintHelper));
				}
			}, TaskContinuationOptions.OnlyOnFaulted);
		}

		public static async Task<bool> Authenticate(string message)
		{
			var result = await CrossFingerprint.Current.AuthenticateAsync(TR.Tr("settings_fingerprint_message"));
			IsEnabled = true;
			return result.Authenticated;
		}

		public static bool MessageBoxFault { get; set; }

		public static void CheckFingerprintSettings()
		{
			var differenceDates = Date.CheckDatesDifference(Settings.LastLoginDate, DateTime.Now);
			if (Settings.IsFingerprintActive) {
				if (differenceDates.Days >= 14 || Settings.FingerprintCount > 20) {
					Settings.IsFingerprintActive = false;
					Settings.FingerprintCount = 0;
					var mainPage = Application.Current.MainPage;
					if (mainPage != null) {
						mainPage.DisplayAlert(TR.Tr("alert"), TR.Tr("fingerprint_expired"), TR.OK);
					} else {
						MessageBoxFault = true;
					}
				}
			}
		}

		public static void ResetSettings(bool count, bool mainSetting, bool password)
		{
			if (count) Settings.FingerprintCount = 0;
			if (mainSetting) Settings.IsFingerprintActive = false;
			if (password) Settings.RememberedPassword = null;
		}
	}
}
