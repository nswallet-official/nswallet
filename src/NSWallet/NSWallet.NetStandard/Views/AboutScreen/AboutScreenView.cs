using System;
using FFImageLoading.Forms;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class AboutScreenView : ContentPage
	{
		readonly AboutScreenViewModel aboutScreenVM;
		DateTime? LastTap;
		byte NumberOfTaps;
		const int NumberOfTapsRequired = 10;
		const int ToleranceInMs = 1000;
		readonly bool fromLogin;

		public AboutScreenView(bool fromLogin = false)
		{
			aboutScreenVM = new AboutScreenViewModel(Navigation, fromLogin);
			BindingContext = aboutScreenVM;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_about"));

			this.fromLogin = fromLogin;

			if (fromLogin) {
				var closeToolbarItem = new ToolbarItem {
					Text = TR.Close,
					Icon = Theme.Current.CloseIcon
				};
				closeToolbarItem.Clicked += (sender, e) => Navigation.PopModalAsync();
				ToolbarItems.Add(closeToolbarItem);
			}

			var mainStackLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.ListBackgroundColor

			};

			var appIcon = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.AppIconNoBack)),
				Margin = 0, //Theme.Current.AppIconPadding,
				HeightRequest = 130, //AppConsts.ICON_SIZE,
				WidthRequest = 130,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var appVersion = new Label {
				Text = String.Format("{0}: {1}", TR.Tr("version_label"), PlatformSpecific.GetVersion()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				TextColor = Theme.Current.NormalTextColor
			};

			var appVersionTapGesture = new TapGestureRecognizer();
			appVersionTapGesture.Tapped += TapGestureRecognizer_OnTapped;
			appVersion.GestureRecognizers.Add(appVersionTapGesture);

			var buildNumber = new Label {
				Text = String.Format("{0}: {1}", TR.Tr("build_label"), PlatformSpecific.GetBuildNumber()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};



			var platform = new Label {
				Text = string.Format("{0}: {1}", TR.Tr("platform_label"), PlatformSpecific.GetPlatform()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var appDatabaseVersion = new Label {
				Text = TR.Tr("db_version_name") + ": " + BL.StorageProperties.Version,
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var appWebsite = new Label {
				Text = GConsts.APP_WEBSITE_NAME,
				TextColor = Theme.Current.LinkColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				FontAttributes = FontAttributes.Bold
			};

			var appWebsiteGesture = new TapGestureRecognizer();
			appWebsiteGesture.SetBinding(TapGestureRecognizer.CommandProperty, "AppWebsiteCommand");
			appWebsite.GestureRecognizers.Add(appWebsiteGesture);

			var appDisclaimer = new Button {
				Text = TR.Tr("app_disclaimer"),
				FontAttributes = FontAttributes.Bold,
				TextColor = Theme.Current.CommonButtonTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button)),
				BackgroundColor = Theme.Current.CommonButtonBackground,
				CornerRadius = Theme.Current.ButtonRadius,
				BorderWidth = 0,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				Margin = new Thickness(15, 0, 15, 0)
			};

			appDisclaimer.SetBinding(Button.CommandProperty, "AppDisclaimerCommand");

			var releaseNotesButton = new Button { Text = TR.Tr("release_notes") };
			releaseNotesButton.SetBinding(Button.CommandProperty, "ReleaseCommand");
			releaseNotesButton.FontAttributes = FontAttributes.Bold;
			releaseNotesButton.TextColor = Theme.Current.CommonButtonTextColor;
			releaseNotesButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			releaseNotesButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			releaseNotesButton.CornerRadius = Theme.Current.ButtonRadius;
			releaseNotesButton.BorderWidth = 0;
			releaseNotesButton.Margin = new Thickness(15, 0, 15, 0);
			releaseNotesButton.FontFamily = NSWFontsController.CurrentBoldTypeface;

			var faqButton = new Button { Text = TR.Tr("faq") };
			faqButton.SetBinding(Button.CommandProperty, "FAQCommand");
			faqButton.FontAttributes = FontAttributes.Bold;
			faqButton.TextColor = Theme.Current.CommonButtonTextColor;
			faqButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			faqButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			faqButton.CornerRadius = Theme.Current.ButtonRadius;
			faqButton.BorderWidth = 0;
			faqButton.Margin = new Thickness(15, 0, 15, 0);
			faqButton.FontFamily = NSWFontsController.CurrentBoldTypeface;


			var privacyPolicyButton = new Button { Text = TR.Tr("privacy_policy") };
			privacyPolicyButton.SetBinding(Button.CommandProperty, "PrivacyPolicyCommand");
			privacyPolicyButton.FontAttributes = FontAttributes.Bold;
			privacyPolicyButton.TextColor = Theme.Current.CommonButtonTextColor;
			privacyPolicyButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			privacyPolicyButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			privacyPolicyButton.CornerRadius = Theme.Current.ButtonRadius;
			privacyPolicyButton.BorderWidth = 0;
			privacyPolicyButton.Margin = new Thickness(15, 0, 15, 0);
			privacyPolicyButton.FontFamily = NSWFontsController.CurrentBoldTypeface;

			var termsOfUseButton = new Button { Text = TR.Tr("terms_of_use") };
			termsOfUseButton.SetBinding(Button.CommandProperty, "TermsOfUseCommand");
			termsOfUseButton.FontAttributes = FontAttributes.Bold;
			termsOfUseButton.TextColor = Theme.Current.CommonButtonTextColor;
			termsOfUseButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			termsOfUseButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			termsOfUseButton.CornerRadius = Theme.Current.ButtonRadius;
			termsOfUseButton.BorderWidth = 0;
			termsOfUseButton.Margin = new Thickness(15, 0, 15, 0);
			termsOfUseButton.FontFamily = NSWFontsController.CurrentBoldTypeface;

			var appInfoDetailsLayout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Start,
				Margin = new Thickness(5, 20, 5, 5),
				Children = { appVersion, buildNumber, platform, appDatabaseVersion }
			};

			var appInfoLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = { appIcon, appInfoDetailsLayout },

			};
			mainStackLayout.Children.Add(appInfoLayout);
			mainStackLayout.Children.Add(appWebsite);
			mainStackLayout.Children.Add(privacyPolicyButton);
			mainStackLayout.Children.Add(termsOfUseButton);
			mainStackLayout.Children.Add(releaseNotesButton);
			mainStackLayout.Children.Add(appDisclaimer);
			mainStackLayout.Children.Add(faqButton);


			var appDevStack = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				BackgroundColor = Theme.Current.ListBackgroundColor,
				Margin = 0
			};
			mainStackLayout.Children.Add(appDevStack);
				

			var appDevIcon = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.DeveloperIcon)),
				HeightRequest = Theme.Current.DeveloperIconHeight,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var appDevIconGesture = new TapGestureRecognizer();
			appDevIconGesture.SetBinding(TapGestureRecognizer.CommandProperty, "AppDevIconCommand");
			appDevIcon.GestureRecognizers.Add(appDevIconGesture);

			var appCopyright = new Label {
				Text = String.Format("BykovSoft © 2017 - {0}", DateTime.Now.Year.ToString()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center,
				FontFamily = NSWFontsController.CurrentTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var appDevWebsite = new Label {
				Text = GConsts.APP_DEV_WEBSITE_NAME,
				TextColor = Theme.Current.LinkColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				HorizontalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(0, 0, 0, 20)
			};

			var appDevWebsiteGesture = new TapGestureRecognizer();
			appDevWebsiteGesture.SetBinding(TapGestureRecognizer.CommandProperty, "AppDevWebsiteCommand");
			appDevWebsite.GestureRecognizers.Add(appDevWebsiteGesture);

			appDevStack.Children.Add(appDevIcon);
			appDevStack.Children.Add(appCopyright);
			appDevStack.Children.Add(appDevWebsite);


			Content = new ScrollView {
				BackgroundColor = Theme.Current.ListBackgroundColor,
				HorizontalOptions = LayoutOptions.Fill,
				Content = new StackLayout {
					BackgroundColor = Theme.Current.ListBackgroundColor,
					HorizontalOptions = LayoutOptions.Fill,
					Children = { mainStackLayout }
				}
			};
		}

		void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			if (LastTap == null || (DateTime.Now - LastTap.Value).Milliseconds < ToleranceInMs) {
				if (NumberOfTaps == (NumberOfTapsRequired - 1)) {
					aboutScreenVM.AdminPanelCallback.Invoke();
					NumberOfTaps = 0;
					LastTap = null;
					return;
				} else {
					NumberOfTaps++;
					LastTap = DateTime.Now;
				}
			} else {
				NumberOfTaps = 1;
				LastTap = DateTime.Now;
			}
		}

		protected override bool OnBackButtonPressed()
		{
			if (!fromLogin) {
				if (Settings.AndroidBackLogout) {
					AppPages.Main();
				}
				return true;
			}

			return base.OnBackButtonPressed();
		}
	}
}

