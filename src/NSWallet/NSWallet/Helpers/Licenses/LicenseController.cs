using NSWallet.Helpers;
using NSWallet.NetStandard.Helpers.UI.Popups.Pages.SignIn;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers
{
	public static class LicenseController
	{
		public static bool IsPrivacyPolicy { get { return Settings.PrivacyPolicyAccepted; } }
		public static bool IsTermsOfUse { get { return Settings.TermsOfUseAccepted; } }

		public static bool CheckPrivacyPolicy()
		{
			if (!IsPrivacyPolicy) {
				SignInPagePopupUIController.LaunchPrivacyPolicyPopup(task => {
					Device.BeginInvokeOnMainThread(() => {
						if (task.Result) {
							OpenPrivacyPolicyPage();
						} else {
							CheckTermsOfUse();
						}
					});
				});
				return false;
			}
			return true;
		}

		public static bool CheckTermsOfUse()
		{
			if (!IsTermsOfUse) {
				SignInPagePopupUIController.LaunchTermsOfUsePopup(task => {
					Device.BeginInvokeOnMainThread(() => {
						if (task.Result) {
							OpenTermsOfUsePage();
						}
					});
				});
				return false;
			}
			return true;
		}

		public static void OpenPrivacyPolicyPage(bool buttons = true)
		{
			AppPages.PrivacyPolicy(
				Application.Current.MainPage.Navigation,
				TR.Tr("privacy_policy"),
				getPrivacyPolicyHTML(),
				accepted => {
					if (buttons) {
						Settings.PrivacyPolicyAccepted = accepted;
						CheckTermsOfUse();
					}
				},
				buttons
			);
		}

		public static void OpenTermsOfUsePage(bool buttons = true)
		{
			AppPages.TermsOfUse(
				Application.Current.MainPage.Navigation,
				TR.Tr("terms_of_use"),
				getTermsOfUseHTML(),
				accepted => {
					if (buttons) {
						Settings.TermsOfUseAccepted = accepted;
					}
				},
				buttons
			);
		}

		static string getPrivacyPolicyHTML()
		{
			return NSWLocalFiles.GetPrivacyPolicyHTML(
				AppLanguage.GetCurrentLangCode()
			);
		}

		static string getTermsOfUseHTML()
		{
			return NSWLocalFiles.GetTermsOfUseHTML(
				AppLanguage.GetCurrentLangCode()
			);
		}
	}
}