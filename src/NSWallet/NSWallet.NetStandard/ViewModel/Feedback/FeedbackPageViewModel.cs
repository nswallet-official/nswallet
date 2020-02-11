using System;
//using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using NSWallet.Helpers;
using NSWallet.NetStandard.Helpers;
using NSWallet.Premium;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class FeedbackPageViewModel : ViewModel
	{
		INavigation navigation;

		public FeedbackPageViewModel(INavigation navigation)
		{
			this.navigation = navigation;
			HtmlText = NSWLocalFiles.GetFeedbackDescriptionHTML(AppLanguage.GetCurrentLangCode());
		}

		string htmlText;
		public string HtmlText {
			get { return htmlText; }
			set {
				if (htmlText == value)
					return;
				htmlText = value;
				OnPropertyChanged("HtmlText");
			}
		}

		Command termsOfUseCommand;
		public Command TermsOfUseCommand {
			get {
				return termsOfUseCommand ?? (termsOfUseCommand = new Command(ExecuteTermsOfUseCommand));
			}
		}

		void ExecuteTermsOfUseCommand()
		{
			LicenseController.OpenTermsOfUsePage(!Settings.TermsOfUseAccepted);
		}

		Command privacyPolicyCommand;
		public Command PrivacyPolicyCommand {
			get {
				return privacyPolicyCommand ?? (privacyPolicyCommand = new Command(ExecutePrivacyPolicyCommand));
			}
		}

		void ExecutePrivacyPolicyCommand()
		{
			LicenseController.OpenPrivacyPolicyPage(!Settings.PrivacyPolicyAccepted);
		}

		Command feedbackProceedCommand;
		public Command FeedbackProceedCommand {
			get {
				return feedbackProceedCommand ?? (feedbackProceedCommand = new Command(ExecuteFeedbackProceedCommand));
			}
		}



		void ExecuteFeedbackProceedCommand()
		{
			var databaseId = BL.GetDbID();
			var instanceId = Security.CalcMD5(databaseId);
			var platform = Device.RuntimePlatform;
			var appVersion = PlatformSpecific.GetVersion() + "." + PlatformSpecific.GetBuildNumber();
			var premium = PremiumManagement.IsAnyPremium;
			var device = ExtendedDevice.GetDeviceName();

			var featureRequest = new FeatureRequestModel {
				instance_id = instanceId,
				platform = platform,
				app_version = appVersion,
				premium = premium,
				device = device
			};

			var json = JsonConvert.SerializeObject(featureRequest);
			var encryptedText = Security.EncryptRSA(json, GConsts.RSA_PUBLIC_KEY_XML_FEATURE_REQUEST);
			var encodedString = Base64UrlEncode(encryptedText);
			Device.BeginInvokeOnMainThread(() => {
				Device.OpenUri(new Uri(String.Format(GConsts.FEATURES_REQUEST_LINK, encodedString)));
			});
		}

		private static string Base64UrlEncode(string input)
		{
			var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
			// Special "url-safe" base64 encode.
			return Convert.ToBase64String(inputBytes)
			  .Replace('+', '-')
			  .Replace('/', '_')
			  .Replace("=", "");
		}
	}
}