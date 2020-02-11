using System;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.Feedback
{
	public class FeedbackPageView : ContentPage
	{
		public FeedbackPageView()
		{
			UINavigationHeader.SetCommonTitleView(this, TR.Tr("nswallet_feedback"));

			var pageVM = new FeedbackPageViewModel(Navigation);
			BindingContext = pageVM;

			var mainStackLayout = new StackLayout();

			var proceedToFeedback = new Button();
			proceedToFeedback.FontFamily = NSWFontsController.CurrentTypeface;
			proceedToFeedback.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
			proceedToFeedback.SetBinding(Button.CommandProperty, "FeedbackProceedCommand");
			proceedToFeedback.CornerRadius = 0;
			proceedToFeedback.BackgroundColor = Theme.Current.PremiumButtonPremScrBackgroundColor;
			proceedToFeedback.TextColor = Theme.Current.PremiumButtonPremScrTextColor;
			proceedToFeedback.HorizontalOptions = LayoutOptions.FillAndExpand;
			proceedToFeedback.Text = TR.Tr("proceed_to_feedback");
			mainStackLayout.Children.Add(proceedToFeedback);

			var privacyPolicyLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(15),
				TextColor = Theme.Current.CommonButtonTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Micro, typeof(Button)),
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				Text = TR.Tr("privacy_policy")
			};

			var privacyPolicyTappedGesture = new TapGestureRecognizer();
			privacyPolicyTappedGesture.SetBinding(TapGestureRecognizer.CommandProperty, "PrivacyPolicyCommand");

			var privacyPolicyButton = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				Children = {
					privacyPolicyLabel
				},
				GestureRecognizers = {
					privacyPolicyTappedGesture
				}
			};

			var termsOfUseLabel = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(15),
				TextColor = Theme.Current.CommonButtonTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Micro, typeof(Button)),
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				Text = TR.Tr("terms_of_use")
			};

			var termsOfUseTappedGesture = new TapGestureRecognizer();
			termsOfUseTappedGesture.SetBinding(TapGestureRecognizer.CommandProperty, "TermsOfUseCommand");

			var termsOfUseButton = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				Children = {
					termsOfUseLabel
				},
				GestureRecognizers = {
					termsOfUseTappedGesture
				}
			};

			var buttonsLayout = new Grid {
				VerticalOptions = LayoutOptions.End,
				RowDefinitions = {
					new RowDefinition {
						Height = new GridLength(1, GridUnitType.Star)
					}
				},
				ColumnDefinitions = {
					new ColumnDefinition {
						Width = new GridLength(1, GridUnitType.Star)
					},
					new ColumnDefinition {
						Width = new GridLength(1, GridUnitType.Star)
					}
				}
			};

			buttonsLayout.Children.Add(privacyPolicyButton, 0, 0);
			buttonsLayout.Children.Add(termsOfUseButton, 1, 0);

			mainStackLayout.Children.Add(buttonsLayout);

			var webView = new WebView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var webSource = new HtmlWebViewSource();
			webView.Source = webSource;
			webSource.SetBinding(HtmlWebViewSource.HtmlProperty, "HtmlText");
			mainStackLayout.Children.Add(webView);

			// A crutch to ensure that when any page is updated, the buttons do not slide down or under the WebView
			mainStackLayout.SizeChanged += (sender, e) => {
				try {
					if (buttonsLayout != null) {
						mainStackLayout.Children.Remove(buttonsLayout);
						mainStackLayout.Children.Add(buttonsLayout);
					}
				} catch (Exception ex) {
					AppLogs.Log(ex.Message, nameof(PremiumPageView), nameof(PremiumPageView));
				}
			};

			Content = mainStackLayout;
		}
	}
}