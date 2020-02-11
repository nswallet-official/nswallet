using System;
using Xamarin.UITest;
namespace NSWallet.UITests
{
	public class LoginPageSteps
	{
		private LoginPage loginPage = new LoginPage();

		public void CreateMasterPassword(IApp app, string passwordValue) {
			loginPage.EnterMasterPassword(app, passwordValue);
			loginPage.ConfirmMasterPassword(app, passwordValue);
			loginPage.TapLoginButton(app);
		}

		public void DoLogin(IApp app, string passwordValue)
		{
			loginPage.EnterMasterPassword(app, passwordValue);
			loginPage.TapLoginButton(app);
		}
	}
}