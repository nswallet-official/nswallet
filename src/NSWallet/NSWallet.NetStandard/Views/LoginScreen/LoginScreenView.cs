using System.Collections.Generic;
using Xamarin.Forms;
using NSWallet.Shared;
using NSWallet.Helpers;
using NSWallet.Premium;
using System;
using NSWallet.Consts;
using System.Threading.Tasks;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.NetStandard.Helpers.Fonts;
using FFImageLoading.Forms;

namespace NSWallet
{
	public class LoginScreenView : ContentPage
    {
        StackLayout mainStackLayout;
        Button loginButton;
        public List<string> Features { get; set; }
		public static bool ManualExit;

		LoginScreenViewModel pageVM;

		public LoginScreenView(bool manualExit = false)
		{
			FingerprintHelper.CheckFingerprintSettings();
			pageVM = new LoginScreenViewModel(Navigation);
			BindingContext = pageVM;
			BackgroundColor = Theme.Current.ListBackgroundColor;
			Icon = Theme.Current.AppHeaderIcon;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("app_name"));

			var importToolbarItem = new ToolbarItem();
			importToolbarItem.Text = TR.Tr("menu_import");
			importToolbarItem.Icon = Theme.Current.ToolBarImportIcon;
			importToolbarItem.Clicked += (sender, e) => { Pages.ImportBackupHelp(Navigation); };
			ToolbarItems.Add(importToolbarItem);

			var aboutToolbarItem = new ToolbarItem();
			aboutToolbarItem.Text = TR.Tr("menu_about");
			aboutToolbarItem.Icon = Theme.Current.ToolbarAboutIcon;
			aboutToolbarItem.Clicked += (sender, e) => { Pages.AboutModal(Navigation); };
			ToolbarItems.Add(aboutToolbarItem);

			mainStackLayout = new StackLayout();
			mainStackLayout.Padding = new Thickness(10, 10, 10, 0);

			var passLayout = new StackLayout();
			passLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
			passLayout.Orientation = StackOrientation.Horizontal;

			var password = new RectangularEntry();
			password.WidthRequest = 1;
			password.FontFamily = NSWFontsController.CurrentTypeface;
			password.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(RectangularEntry));
			password.HorizontalOptions = LayoutOptions.FillAndExpand;
			password.Placeholder = TR.Tr("password_placeholder");
			password.AutomationId = AutomationIdConsts.PASSWORD_PLACEHOLDER_ID;
			password.IsPassword = true;
			password.HeightRequest = Theme.Current.PasswordHeight;
			password.SetBinding(Entry.TextProperty, "Password");

			var failTrigger = new DataTrigger(typeof(Entry));
			var binding = new Binding();
			binding.Source = pageVM;
			binding.Path = "AnimationStatus";
			failTrigger.Binding = binding;
			failTrigger.Value = 1;
			failTrigger.EnterActions.Add(new LoginScreenFailTrigger(pageVM));
			password.Triggers.Add(failTrigger);

			passLayout.Children.Add(password);

			mainStackLayout.Children.Add(passLayout);

			var checkPassword = new RectangularEntry();
			checkPassword.FontFamily = NSWFontsController.CurrentTypeface;
			checkPassword.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(RectangularEntry));
			checkPassword.Placeholder = TR.Tr("password_check_placeholder");
			checkPassword.AutomationId = AutomationIdConsts.PASSWORD_CHECK_PLACEHOLDER_ID;
			checkPassword.IsPassword = true;
			checkPassword.HeightRequest = Theme.Current.PasswordHeight;
			checkPassword.SetBinding(Entry.TextProperty, "CheckPassword");
			mainStackLayout.Children.Add(checkPassword);

			loginButton = new Button();
			loginButton.FontFamily = NSWFontsController.CurrentBoldTypeface;
			loginButton.CornerRadius = Theme.Current.ButtonRadius;
			loginButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			loginButton.TextColor = Theme.Current.CommonButtonTextColor;
			loginButton.FontAttributes = Theme.Current.LoginButtonFontAttributes;
			loginButton.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
			loginButton.Text = TR.Tr("login_button");
			loginButton.AutomationId = AutomationIdConsts.LOGIN_BUTTON_ID;
			loginButton.SetBinding(Button.CommandProperty, "LoginCommand");
			mainStackLayout.Children.Add(loginButton);

			var releaseNotesLabel = new Label();
			releaseNotesLabel.FontFamily = NSWFontsController.CurrentTypeface;
			releaseNotesLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			releaseNotesLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
			releaseNotesLabel.HorizontalTextAlignment = TextAlignment.Center;
			releaseNotesLabel.TextColor = Theme.Current.LinkColor;
			releaseNotesLabel.SetBinding(Label.TextProperty, "ReleaseNotes");
			releaseNotesLabel.SetBinding(IsVisibleProperty, "IsNewBuild");

			var releaseTapGesture = new TapGestureRecognizer();
			releaseTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "ReleaseCommand");
			releaseNotesLabel.GestureRecognizers.Add(releaseTapGesture);

			mainStackLayout.Children.Add(releaseNotesLabel);

			var saveLabel = new Label();
			saveLabel.FontFamily = NSWFontsController.CurrentTypeface;
			saveLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			saveLabel.TextColor = Theme.Current.ListTextColor;
			saveLabel.Text = TR.Tr("password_remember");
			saveLabel.Margin = new Thickness(20);
			saveLabel.HorizontalTextAlignment = TextAlignment.Center;
			mainStackLayout.Children.Add(saveLabel);

			var bodyLayout = new StackLayout();
			bodyLayout.Padding = Theme.Current.BodyPadding;

			var socialButtons = SocialView.GetButtons();
			socialButtons.HorizontalOptions = LayoutOptions.CenterAndExpand;
			bodyLayout.Children.Add(socialButtons);

			mainStackLayout.Children.Add(bodyLayout);

			if (Settings.IsFingerprintActive) {
				var fingerPrintImage = new CachedImage {
				//Aspect = Aspect.AspectFit,
					HeightRequest = Theme.Current.FingerPrintImageHeight,
					Margin = Theme.Current.SocialIconMargin,
					Style = ImageProperties.DefaultCachedImageStyle
				};

				if (FingerprintHelper.IsFaceID) {
					fingerPrintImage.Source = ImageSource.FromStream(() => NSWRes.GetImage("LoginScreen.img_face_id.png"));
				} else {
					fingerPrintImage.Source = ImageSource.FromStream(() => NSWRes.GetImage("LoginScreen.img_touch_id.png"));
				}

				var touchTapGesture = new TapGestureRecognizer();
				touchTapGesture.Tapped += async (sender, e) => await authFinger();
				fingerPrintImage.GestureRecognizers.Add(touchTapGesture);

				mainStackLayout.Children.Add(fingerPrintImage);
			}

			if (PremiumManagement.IsFree) { 
				//mainStackLayout.Children.Add(getIndicators(5, 0));

				if (!BL.IsNew()) // FIXME: business layer should not be in views!!!!!
				{
					var premiumButtonLayout = new Grid();
					premiumButtonLayout.VerticalOptions = LayoutOptions.EndAndExpand;

					var premiumButton = new Button();
					premiumButton.Margin = new Thickness(0, 0, 0, 20);
					premiumButton.FontFamily = NSWFontsController.CurrentTypeface;
					premiumButton.Text = TR.Tr("buypremium_button");
					premiumButton.HeightRequest = 50;
					premiumButton.VerticalOptions = LayoutOptions.Center;
					premiumButton.HorizontalOptions = LayoutOptions.Fill;
					premiumButton.TextColor = Theme.Current.PremiumButtonTextColor;
					premiumButton.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
					premiumButton.FontAttributes = Theme.Current.PremiumButtonFontAttributes;
					premiumButton.BackgroundColor = Theme.Current.PremiumButtonBackground;
					premiumButton.SetBinding(Button.CommandProperty, "BuyPremiumCommand");
					premiumButtonLayout.Children.Add(premiumButton);

					var premiumStar = new CachedImage {
						HeightRequest = 70,
						Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.PremiumIcon)),
						HorizontalOptions = LayoutOptions.End,
						VerticalOptions = LayoutOptions.Center,
						Style = ImageProperties.DefaultCachedImageStyle,
						Margin = new Thickness(0, 0, 0, 20)
					};
					premiumButtonLayout.Children.Add(premiumStar);

					mainStackLayout.Children.Add(premiumButtonLayout);
				}
			}

			if (BL.IsNew()) // FIXME: business layer should not be in views!!!!!
			{
				saveLabel.IsVisible = true;
				bodyLayout.IsVisible = false;
				checkPassword.IsVisible = true;
				password.Completed += (s, e) => { checkPassword.Focus(); };
				checkPassword.Completed += (s, e) => { loginButton.Command.Execute(null); };
			} else {
				saveLabel.IsVisible = false;
				bodyLayout.IsVisible = true;
				checkPassword.IsVisible = false;
				password.Completed += (s, e) => { loginButton.Command.Execute(null); };
			}

			pageVM.TipAlertCommandCallback = DisplayTip;

			//mainStackLayout.SizeChanged += (sender, e) => launchTimer();

			//if (!manualExit) {
			//	if (Settings.IsFingerprintActive) {
			//		Task.Run(async () => await authFinger());
			//	}
			//}

			Settings.LastLoginDate = DateTime.Now;

			Content = new ScrollView { Content = mainStackLayout };
		}

		protected override void OnAppearing()
		{
			if (Settings.IsFingerprintActive) {
				if (Settings.IsAutoFinger) {
					if (!ManualExit) {
						ManualExit = false;
						Device.BeginInvokeOnMainThread(async () => await authFinger());
					}
				}
			}
			base.OnAppearing();
		}

		public async Task authFinger()
		{
			var authenticated = await FingerprintHelper.Authenticate(TR.Tr("settings_fingerprint_message"));
			if (authenticated) {
				Settings.FingerprintCount += 1;
				pageVM.Password = Settings.RememberedPassword;
				loginButton.Command.Execute(true);
			}
		}

        /*
        void launchTimer()
        {
            if (PremiumManagement.IsFree)
                Device.StartTimer(TimeSpan.FromSeconds(3), OnTimerTick);
        }

        bool OnTimerTick()
        {
            removeIndicators();
            var position = featuresView.Position;
            if (position.Equals(4))
            {
                addIndicators(0);
                featuresView.Position = 0;
            }
            else
            {
                addIndicators(position++);
                featuresView.Position = position;
            }
            return true;
        }
        */

        void removeIndicators()
        {
            /*if (PremiumManagement.IsFree)
            {
                mainStackLayout.Children.RemoveAt(6);
            }*/
        }

        void addIndicators(int index)
        {
            /*if (PremiumManagement.IsFree)
            {
                mainStackLayout.Children.Insert(6, getIndicators(5, index));
            }*/
        }

        StackLayout getIndicators(int numberOfIndicators, int activeIndex)
        {
            var stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;

            if (PremiumManagement.IsFree)
            {
                for (int i = 0; i < numberOfIndicators; i++)
                {
					var indicator = new CachedImage {
						HeightRequest = 30,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Style = ImageProperties.DefaultCachedImageStyle
					};
					if (activeIndex.Equals(i))
                        indicator.Source = "indicator_active.png";
                    else
                        indicator.Source = "indicator_inactive.png";

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += indicatorClicked;
                    tapGestureRecognizer.CommandParameter = i;
                    indicator.GestureRecognizers.Add(tapGestureRecognizer);

                    stackLayout.Children.Add(indicator);
                }
            }

            return stackLayout;
        }

        void indicatorClicked(object sender, EventArgs e)
        {
            var eventArgs = (TappedEventArgs)e;
            var index = (int)eventArgs.Parameter;
            removeIndicators();
            addIndicators(index);
        }

        void DisplayTip()
        {
            DisplayAlert(TR.Tr("app_name"), TR.Tr("password_hint") + ": " + Settings.PasswordTip, TR.Tr("ok"));
        }

        protected override bool OnBackButtonPressed()
        {

            return base.OnBackButtonPressed();
        }
    }
}
