using System;
using System.Threading.Tasks;
using NSWallet.Shared;

namespace NSWallet.NetStandard.Helpers.UI.Popups.Pages.SignIn
{
	public static class SignInPagePopupUIController
	{
		public static void LaunchPrivacyPolicyPopup(Action<Task<bool>> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("alert"), TR.Tr("privacy_policy_description"), TR.Tr("privacy_policy_read_and_accept"), TR.Cancel, action);
		}

		public static void LaunchTermsOfUsePopup(Action<Task<bool>> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("alert"), TR.Tr("terms_of_use_description"), TR.Tr("terms_of_use_read_and_accept"), TR.Cancel, action);
		}
	}
}