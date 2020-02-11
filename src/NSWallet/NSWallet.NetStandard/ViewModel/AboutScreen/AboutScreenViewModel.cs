using System;
using NSWallet.Helpers;
using NSWallet.NetStandard.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class AboutScreenViewModel : ViewModel
	{
		public Action AdminPanelCallback { get; set; }

		static bool isFromLogin;
		INavigation navigation;

		public AboutScreenViewModel(INavigation navigation, bool fromLogin = false)
		{
			isFromLogin = fromLogin;
			this.navigation = navigation;
			AdminPanelCallback = () => { Pages.AdminPanelPassword(navigation); };
		}

		/// <summary>
		/// Application website link
		/// </summary>
		Command appWebsiteCommand;
		public Command AppWebsiteCommand {
			get {
				return appWebsiteCommand ?? (appWebsiteCommand = new Command(ExecuteAppWebsiteCommand));
			}
		}

		protected static void ExecuteAppWebsiteCommand()
		{
			WebPage.NavigateTo(GConsts.APP_WEBSITE_URI);
		}

		/// <summary>
		/// Warranty disclaimer webpage
		/// </summary>
		Command appDisclaimerCommand;
		public Command AppDisclaimerCommand {
			get {
				return appDisclaimerCommand ?? (appDisclaimerCommand = new Command(ExecuteAppDisclaimerCommand));
			}
		}

		protected static void ExecuteAppDisclaimerCommand()
		{
			WebPage.NavigateTo(GConsts.APP_DISCLAIMER_URI);
		}

		Command openDevCommand;
		public Command OpenDevCommand {
			get {
				return openDevCommand ?? (openDevCommand = new Command(ExecuteOpenDevCommand));
			}
		}

		protected void ExecuteOpenDevCommand()
		{
			Pages.Diagnostics(navigation, isFromLogin);
		}

		/// <summary>
		/// Company website
		/// </summary>
		Command appDevIconCommand;
		public Command AppDevIconCommand {
			get {
				return appDevIconCommand ?? (appDevIconCommand = new Command(ExecuteAppDevIconCommand));
			}
		}

		protected static void ExecuteAppDevIconCommand()
		{
			WebPage.NavigateTo(GConsts.APP_DEV_WEBSITE_URI);
		}

		Command appDevWebsiteCommand;
		public Command AppDevWebsiteCommand {
			get {
				return appDevWebsiteCommand ?? (appDevWebsiteCommand = new Command(ExecuteAppDevWebsiteCommand));
			}
		}

		protected static void ExecuteAppDevWebsiteCommand()
		{
			WebPage.NavigateTo(GConsts.APP_DEV_WEBSITE_URI);
		}

		/// <summary>
		/// Release notes webpage
		/// </summary>
		Command releaseCommand;
		public Command ReleaseCommand {
			get {
				return releaseCommand ?? (releaseCommand = new Command(ExecuteReleaseCommand));
			}
		}

		void ExecuteReleaseCommand()
		{
			WebPage.NavigateTo(GConsts.APP_DEV_RELEASE_NOTES_URI);
		}

		/// <summary>
		/// FAQ webpage (help and support)
		/// </summary>
		Command faqCommand;
		public Command FAQCommand {
			get {
				return faqCommand ?? (faqCommand = new Command(ExecuteFAQCommand));
			}
		}

		void ExecuteFAQCommand()
		{
			WebPage.NavigateTo(GConsts.APP_DEV_FAQ_URI);
		}

		/// <summary>
		/// Privacy Policy internal webpage
		/// </summary>
		Command privacyPolicyCommand;
		public Command PrivacyPolicyCommand {
			get {
				return privacyPolicyCommand ?? (privacyPolicyCommand = new Command(ExecutePrivacyPolicyCommand));
			}
		}

		void ExecutePrivacyPolicyCommand()
		{
			Device.BeginInvokeOnMainThread(() => LicenseController.OpenPrivacyPolicyPage(!Settings.PrivacyPolicyAccepted));
		}

		/// <summary>
		/// Terms of Use internal webpage
		/// </summary>
		Command termsOfUseCommand;
		public Command TermsOfUseCommand {
			get {
				return termsOfUseCommand ?? (termsOfUseCommand = new Command(ExecuteTermsOfUseCommand));
			}
		}

		void ExecuteTermsOfUseCommand()
		{
			Device.BeginInvokeOnMainThread(() => LicenseController.OpenTermsOfUsePage(!Settings.TermsOfUseAccepted));
		}
	}
}