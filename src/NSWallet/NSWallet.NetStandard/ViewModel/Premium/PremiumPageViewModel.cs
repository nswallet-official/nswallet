using System.Threading.Tasks;
using NSWallet.Helpers;
using NSWallet.NetStandard.Helpers;
using NSWallet.NetStandard.Helpers.UI.Popups.Pages.Premium;
using NSWallet.Premium;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class PremiumPageViewModel : ViewModel
    {
		readonly INavigation navigation;

		public PremiumPageViewModel(INavigation navigation)
		{
			this.navigation = navigation;
			BuySubscriptionText = TR.Tr("buy_subscription") + " (" + TR.Tr("price_loading") + ")";

			Task.Run(async () => {
				var price = await PremiumManagement.GetSubscriptionPrice();
				if (string.Compare(price, TR.Tr("price_loading_failed")) == 0) {
					Device.BeginInvokeOnMainThread(PremiumPagePopupUIController.LaunchGetPriceErrorPopup);
				} else {
					BuySubscriptionText = TR.Tr("buy_subscription") + " (" + price + ")";
				}
			});

            HtmlText = PremiumManagement.IsLegacyPremium
				? NSWLocalFiles.GetOldPremiumHTML(AppLanguage.GetCurrentLangCode())
				: NSWLocalFiles.GetNotPremiumHTML(AppLanguage.GetCurrentLangCode());

			switch (Device.RuntimePlatform) {
				case Device.iOS:
					HtmlText = Common.RemoveTags(HtmlText, "[iosonly]", "[/iosonly]");
					break;
				default:
					HtmlText = Common.RemoveTextBetweenTags(HtmlText, "[iosonly]", "[/iosonly]");
					break;
			}
		}

		bool areButtonsVisible;
		public bool AreButtonsVisible {
			get { return areButtonsVisible; }
			set {
				if (areButtonsVisible == value)
					return;
				areButtonsVisible = value;
				OnPropertyChanged("AreButtonsVisible");
			}
		}

		string buySubscriptionText;
		public string BuySubscriptionText {
			get { return buySubscriptionText; }
			set {
				if (buySubscriptionText == value)
					return;
				buySubscriptionText = value;
				OnPropertyChanged("BuySubscriptionText");
			}
		}

		string htmlText;
        public string HtmlText
        {
            get { return htmlText; }
            set
            {
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

		Command buyCommand;
        public Command BuyCommand
        {
            get
            {
                return buyCommand ?? (buyCommand = new Command(ExecuteBuyCommand));
            }
        }

        bool purchased = true;
        void ExecuteBuyCommand()
        {
            if (purchased)
            {
                Task.Run(async () =>
                {
                    purchased = false;
                    var purchaseResult = await PremiumManagement.Purchase(PremiumStatus.Subscription);
                    showMessage(purchaseResult);
                    if (purchaseResult.Equals(TR.Tr("settings_purchase_premium_success")))
                        Device.BeginInvokeOnMainThread(() => Pages.Login());
                    purchased = true;
                });
            }
        }

        void showMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PlatformSpecific.DisplayShortMessage(message);
            });
        }
    }
}