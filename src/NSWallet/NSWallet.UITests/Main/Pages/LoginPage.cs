using System;
using Xamarin.UITest;
using NSWallet.Consts;

namespace NSWallet.UITests
{
	public class LoginPage
	{
		public void EnterMasterPassword(IApp app, string passwordValue)
		{
            app.WaitForElement(AutomationIdConsts.PASSWORD_PLACEHOLDER_ID, timeout: Helper.defaultTimeout);
            app.Tap(AutomationIdConsts.PASSWORD_PLACEHOLDER_ID);
			app.EnterText((AutomationIdConsts.PASSWORD_PLACEHOLDER_ID), passwordValue);
		}

		public void ConfirmMasterPassword(IApp app, string passwordValue)
		{
			app.EnterText((AutomationIdConsts.PASSWORD_CHECK_PLACEHOLDER_ID), passwordValue);
		}

		public void TapLoginButton(IApp app)
		{
			app.Tap(AutomationIdConsts.LOGIN_BUTTON_ID);
		}
	}
}