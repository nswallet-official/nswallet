using System;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class AdminScreenView : ContentPage
    {
		public AdminScreenView(INavigation navigation = null)
        {
			if (navigation != null) {
				NavigationPage.SetHasNavigationBar(this, false);
				UINavigationHeader.SetCommonTitleView(this, TR.Tr("admin_panel"));
			}

			BackgroundColor = Theme.Current.ListBackgroundColor;

            var pageVM = new AdminScreenViewModel(navigation);
            BindingContext = pageVM;

            var mainStackLayout = new StackLayout();

            addCheckbox(mainStackLayout, TR.Tr("admin_panel_premium_toggle"), "PremiumChecked");
            addSeparator(mainStackLayout);

			addCheckbox(mainStackLayout, "Turn logs on", "LogsChecked");
			addSeparator(mainStackLayout);

			var checkPremiumButton = new Button {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				BackgroundColor = Theme.Current.ListBackgroundColor,
				Text = TR.Tr("admin_check_premium")
			};
			checkPremiumButton.SetBinding(Button.CommandProperty, "CheckPremiumCommand");
			mainStackLayout.Children.Add(checkPremiumButton);

			addSeparator(mainStackLayout);

			addCheckbox(mainStackLayout, TR.Tr("feedback_activate"), "FeedbackChecked");
			addSeparator(mainStackLayout);

			//addCheckbox(mainStackLayout, TR.Tr("admin_panel_detailed_log"), "DetailedChecked");
			//addSeparator(mainStackLayout);

			var diagnosticsButton = new Button();
			diagnosticsButton.FontFamily = NSWFontsController.CurrentTypeface;
			diagnosticsButton.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
			diagnosticsButton.BackgroundColor = Theme.Current.ListBackgroundColor;
			diagnosticsButton.Text = TR.Tr("admin_panel_diagnostics");
			diagnosticsButton.SetBinding(Button.CommandProperty, "DiagnosticsCommand");
			mainStackLayout.Children.Add(diagnosticsButton);

			addSeparator(mainStackLayout);

			var hideAdminPanelButton = new Button {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				BackgroundColor = Theme.Current.ListBackgroundColor,
				Text = TR.Tr("hide_admin_panel")
			};
			hideAdminPanelButton.SetBinding(Button.CommandProperty, "HideAdminPanelCommand");
			mainStackLayout.Children.Add(hideAdminPanelButton);


			addSeparator(mainStackLayout);

			var appVersion = new Label {
				Text = String.Format("{0}: {1}", TR.Tr("version_label"), PlatformSpecific.GetVersion()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var buildNumber = new Label {
				Text = String.Format("{0}: {1}", TR.Tr("build_label"), PlatformSpecific.GetBuildNumber()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var platform = new Label {
				Text = String.Format("{0}: {1}", TR.Tr("platform_label"), PlatformSpecific.GetPlatform()),
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var appDatabaseVersion = new Label {
				Text = TR.Tr("db_version_name") + ": " + BL.StorageProperties.Version,
				FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Start,
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.NormalTextColor
			};

			var settings = new Label();
			settings.FontFamily = NSWFontsController.CurrentTypeface;
			settings.Text = "First Launch: " + Settings.FirstLaunch + "\n" +
				"Build: " + Settings.Build + "\n" +
				"Premium Status: " + Settings.PremiumStatus + "\n" +
				"Is Premium Old: " + Settings.IsPremiumOld + "\n" +
				"Premium Subscription Date: " + Settings.PremiumSubscriptionDate + "\n" +
				"Premium Subscription State: " + Settings.PremiumSubscriptionState + "\n" +
				"Auto Backup Date: " + Settings.AutoBackupTime + "\n" +
				"Backup Deletion: " + Settings.BackupDeletion + "\n" +
				"Auto Backup: " + Settings.AutoBackup + "\n" +
				"Language: " + Settings.Language + "\n" +
				"Theme: " + Settings.Theme + "\n" +
				"Hide Pass: " + Settings.IsHidePasswordEnabled + "\n" +
				"Social: " + Settings.IsSocialEnabled + "\n" +
				"Auto Logout: " + Settings.AutoLogout + "\n" +
				"Android Back Logout: " + Settings.AndroidBackLogout + "\n" +
				"Font: " + Settings.FontFamily + "\n" +
				"Expiring Period: " + Settings.ExpiringPeriod + "\n" +
				"Expiring Soon: " + Settings.IsExpiringSoon + "\n" +
				"Recently Viewed: " + Settings.IsRecentlyViewed + "\n" +
				"Mostly Viewed: " + Settings.IsMostlyViewed + "\n" +
				"Password Tip: " + Settings.PasswordTip + "\n" +
				"Auto Login: " + Settings.IsAutoLoginEnabled + "\n" +
				"Is Clipboard Clean: " + Settings.IsClipboardClean + "\n" +
				"Manage Lower Case: " + Settings.ManageLowerCase + "\n" +
				"Manage Upper Case: " + Settings.ManageUpperCase + "\n" +
				"Manage Digits: " + Settings.ManageDigits + "\n" +
				"Manage Special Symbols: " + Settings.ManageSpecialSymbols + "\n" +
				"Pass Generator Length: " + Settings.PassGenLength + "\n" +
				"Is Fingerprint Activated: " + Settings.IsFingerprintActive + "\n" +
				"Fingerprint Count: " + Settings.FingerprintCount + "\n" +
				"Last Login Date: " + Settings.LastLoginDate + "\n" +
				"Used Fingerprint Before: " + Settings.UsedFingerprintBefore + "\n" +
				"Developer Options Active: " + Settings.DevOpsOn;

			var button = new Button();
			button.FontFamily = NSWFontsController.CurrentTypeface;
			button.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			button.Text = "Send";
			button.Clicked += (sender, e) => {
				Device.OpenUri(new Uri("mailto:support@nyxbull.com?subject=NSWallet Settings&body=" + appVersion.Text + "\n" + 
				                       buildNumber.Text + "\n" + platform.Text + "\n" + appDatabaseVersion.Text + "\n" + settings.Text));
			};

			var detailedBlock = new StackLayout {
				Children = {
					appVersion, buildNumber, platform, appDatabaseVersion, settings, button
				}
			};

			mainStackLayout.Children.Add(detailedBlock);

            pageVM.PremiumListCallback = showProducts;

			Content = new ScrollView { Content = mainStackLayout };
        }

        /// <summary>
        /// Adds the checkbox.
        /// </summary>
        /// <param name="mainStackLayout">Main stack layout.</param>
        /// <param name="name">Name.</param>
        /// <param name="checkedProperty">Checked property.</param>
        void addCheckbox(StackLayout mainStackLayout, string name, string checkedProperty)
        {
            var stackLayout = new StackLayout();
            stackLayout.Padding = Theme.Current.InnerMenuPadding;
            stackLayout.Orientation = StackOrientation.Horizontal;

            var nameLabel = new Label();
			nameLabel.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Label));
            nameLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            nameLabel.Text = name;
            nameLabel.TextColor = Theme.Current.ListTextColor;
            nameLabel.Opacity = 0.85;
            nameLabel.FontFamily = NSWFontsController.CurrentTypeface;
            stackLayout.Children.Add(nameLabel);

            var checkbox = new Switch();
            checkbox.HorizontalOptions = LayoutOptions.EndAndExpand;
            checkbox.SetBinding(Switch.IsToggledProperty, checkedProperty);
            stackLayout.Children.Add(checkbox);

            mainStackLayout.Children.Add(stackLayout);
        }

        void showProducts(string purchases)
        {
            var answer = DisplayAlert(TR.Tr("admin_premium_list"), purchases, TR.OK);
        }

        /// <summary>
        /// Adds the separator.
        /// </summary>
        /// <param name="stackLayout">Stack layout.</param>
        void addSeparator(StackLayout stackLayout)
        {
            var separator = new BoxView();
            separator.Color = Theme.Current.ListSeparatorColor;
            separator.HeightRequest = 1;
            separator.Opacity = 0.5;
            stackLayout.Children.Add(separator);
        }
    }
}