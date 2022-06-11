using System;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet
{
	public class PremiumPageView : ContentPage
	{
		public PremiumPageView()
		{
			UINavigationHeader.SetCommonTitleView(this, TR.Tr("nswallet_premium"));

			var pageVM = new PremiumPageViewModel(Navigation);
			BindingContext = pageVM;

			var closeToolbarItem = new ToolbarItem();
			closeToolbarItem.Text = TR.Tr("close");
			closeToolbarItem.IconImageSource = ImageSource.FromFile(Theme.Current.CloseIcon);
			closeToolbarItem.Clicked += (sender, e) => Navigation.PopModalAsync();
			ToolbarItems.Add(closeToolbarItem);

			var mainStackLayout = new StackLayout();

			var buyButton = new Button();
			buyButton.FontFamily = NSWFontsController.CurrentTypeface;
			buyButton.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
			buyButton.SetBinding(Button.CommandProperty, "BuyCommand");
			buyButton.CornerRadius = 0;
			buyButton.BackgroundColor = Theme.Current.PremiumButtonPremScrBackgroundColor;
			buyButton.TextColor = Theme.Current.PremiumButtonPremScrTextColor;
			buyButton.HorizontalOptions = LayoutOptions.FillAndExpand;
			buyButton.SetBinding(Button.TextProperty, "BuySubscriptionText");
			mainStackLayout.Children.Add(buyButton);

			var privacyPolicyLabel = new Label
			{
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

			var privacyPolicyButton = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				Children = {
					privacyPolicyLabel
				},
				GestureRecognizers = {
					privacyPolicyTappedGesture
				}
			};

			var termsOfUseLabel = new Label
			{
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

			var termsOfUseButton = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				Children = {
					termsOfUseLabel
				},
				GestureRecognizers = {
					termsOfUseTappedGesture
				}
			};

			var buttonsLayout = new Grid
			{
				VerticalOptions = LayoutOptions.End,
				//Orientation = StackOrientation.Horizontal,
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
				},
				//Children = {
				//	privacyPolicyButton,
				//	termsOfUseButton
				//}
			};

			buttonsLayout.Children.Add(privacyPolicyButton, 0, 0);
			buttonsLayout.Children.Add(termsOfUseButton, 1, 0);

			mainStackLayout.Children.Add(buttonsLayout);

			var webView = new WebView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var webSource = new HtmlWebViewSource();
			webView.Source = webSource;
			webSource.SetBinding(HtmlWebViewSource.HtmlProperty, "HtmlText");
			mainStackLayout.Children.Add(webView);

			//Content = new ScrollView { Content = mainStackLayout };

			// A crutch to ensure that when any page is updated, the buttons do not slide down or under the WebView
			mainStackLayout.SizeChanged += (sender, e) => {
				try
				{
					if (buttonsLayout != null)
					{
						mainStackLayout.Children.Remove(buttonsLayout);
						mainStackLayout.Children.Add(buttonsLayout);
					}
				}
				catch (Exception ex)
				{
					AppLogs.Log(ex.Message, nameof(PremiumPageView), nameof(PremiumPageView));
				}
			};

			Content = mainStackLayout;
		}
	}
}
