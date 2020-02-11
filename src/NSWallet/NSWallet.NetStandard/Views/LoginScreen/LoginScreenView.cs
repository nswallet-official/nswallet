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
		readonly StackLayout mainStackLayout;
		readonly Button loginButton;
        public List<string> Features { get; set; }
		public static bool ManualExit;
		readonly LoginScreenViewModel pageVM;

		public LoginScreenView()
		{
			FingerprintHelper.CheckFingerprintSettings();
			pageVM = new LoginScreenViewModel(Navigation);
			BindingContext = pageVM;
			BackgroundColor = Theme.Current.ListBackgroundColor;
			IconImageSource = Theme.Current.AppHeaderIcon;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("app_name"));

			var importToolbarItem = new ToolbarItem {
				Text = TR.Tr("menu_import"),
				IconImageSource = Theme.Current.ToolBarImportIcon
			};
			importToolbarItem.Clicked += (sender, e) => { Pages.ImportBackupHelp(Navigation); };
			ToolbarItems.Add(importToolbarItem);

			var aboutToolbarItem = new ToolbarItem {
				Text = TR.Tr("menu_about"),
				IconImageSource = Theme.Current.ToolbarAboutIcon
			};
			aboutToolbarItem.Clicked += (sender, e) => { Pages.AboutModal(Navigation); };
			ToolbarItems.Add(aboutToolbarItem);

			mainStackLayout = new StackLayout {
				Padding = new Thickness(10, 10, 10, 0)
			};

			var passLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal
			};

			var password = new RectangularEntry {
				WidthRequest = 1,
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(RectangularEntry)),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = TR.Tr("password_placeholder"),
				AutomationId = AutomationIdConsts.PASSWORD_PLACEHOLDER_ID,
				IsPassword = true,
				HeightRequest = Theme.Current.PasswordHeight
			};
			password.SetBinding(Entry.TextProperty, "Password");

			var binding = new Binding {
				Source = pageVM,
				Path = "AnimationStatus"
			};

			var failTrigger = new DataTrigger(typeof(Entry)) {
				Binding = binding,
				Value = 1
			};
			failTrigger.EnterActions.Add(new LoginScreenFailTrigger(pageVM));
			password.Triggers.Add(failTrigger);

			passLayout.Children.Add(password);

			mainStackLayout.Children.Add(passLayout);

			var checkPassword = new RectangularEntry {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(RectangularEntry)),
				Placeholder = TR.Tr("password_check_placeholder"),
				AutomationId = AutomationIdConsts.PASSWORD_CHECK_PLACEHOLDER_ID,
				IsPassword = true,
				HeightRequest = Theme.Current.PasswordHeight
			};
			checkPassword.SetBinding(Entry.TextProperty, "CheckPassword");
			mainStackLayout.Children.Add(checkPassword);

			loginButton = new Button {
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				CornerRadius = Theme.Current.ButtonRadius,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				TextColor = Theme.Current.CommonButtonTextColor,
				FontAttributes = Theme.Current.LoginButtonFontAttributes,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				Text = TR.Tr("login_button"),
				AutomationId = AutomationIdConsts.LOGIN_BUTTON_ID
			};
			loginButton.SetBinding(Button.CommandProperty, "LoginCommand");
			mainStackLayout.Children.Add(loginButton);

			var releaseNotesLabel = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Theme.Current.LinkColor
			};
			releaseNotesLabel.SetBinding(Label.TextProperty, "ReleaseNotes");
			releaseNotesLabel.SetBinding(IsVisibleProperty, "IsNewBuild");

			var releaseTapGesture = new TapGestureRecognizer();
			releaseTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "ReleaseCommand");
			releaseNotesLabel.GestureRecognizers.Add(releaseTapGesture);

			mainStackLayout.Children.Add(releaseNotesLabel);

			var saveLabel = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				TextColor = Theme.Current.ListTextColor,
				Text = TR.Tr("password_remember"),
				Margin = new Thickness(20),
				HorizontalTextAlignment = TextAlignment.Center
			};
			mainStackLayout.Children.Add(saveLabel);

			var bodyLayout = new StackLayout {
				Padding = Theme.Current.BodyPadding
			};

			//var socialButtons = SocialView.GetButtons();
			//socialButtons.HorizontalOptions = LayoutOptions.CenterAndExpand;
			//bodyLayout.Children.Add(socialButtons);

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
				touchTapGesture.Tapped += async (sender, e) => await AuthFinger();
				fingerPrintImage.GestureRecognizers.Add(touchTapGesture);

				mainStackLayout.Children.Add(fingerPrintImage);
			}

			if (PremiumManagement.IsFree) { 
				//mainStackLayout.Children.Add(getIndicators(5, 0));

				if (!BL.IsNew()) // FIXME: business layer should not be in views!!!!!
				{
					var premiumButtonLayout = new Grid {
						VerticalOptions = LayoutOptions.EndAndExpand
					};

					var premiumButton = new Button {
						Margin = new Thickness(0, 0, 0, 20),
						FontFamily = NSWFontsController.CurrentTypeface,
						Text = TR.Tr("buypremium_button"),
						HeightRequest = 50,
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Fill,
						TextColor = Theme.Current.PremiumButtonTextColor,
						FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
						FontAttributes = Theme.Current.PremiumButtonFontAttributes,
						BackgroundColor = Theme.Current.PremiumButtonBackground
					};
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


			Settings.LastLoginDate = DateTime.Now;

			Content = new ScrollView { Content = mainStackLayout };
		}

		protected override void OnAppearing()
		{
			if (Settings.IsFingerprintActive) {
				if (Settings.IsAutoFinger) {
					if (!ManualExit) {
						ManualExit = false;
						Device.BeginInvokeOnMainThread(async () => await AuthFinger());
					}
				}
			}
			base.OnAppearing();
		}

		public async Task AuthFinger()
		{
			var authenticated = await FingerprintHelper.Authenticate(TR.Tr("settings_fingerprint_message"));
			if (authenticated) {
				Settings.FingerprintCount += 1;
				pageVM.Password = Settings.RememberedPassword;
				loginButton.Command.Execute(true);
			}
		}


        //void removeIndicators()
        //{
        //    /*if (PremiumManagement.IsFree)
        //    {
        //        mainStackLayout.Children.RemoveAt(6);
        //    }*/
        //}

        //void addIndicators(int index)
        //{
        //    /*if (PremiumManagement.IsFree)
        //    {
        //        mainStackLayout.Children.Insert(6, getIndicators(5, index));
        //    }*/
        //}

     //   StackLayout getIndicators(int numberOfIndicators, int activeIndex)
     //   {
     //       var stackLayout = new StackLayout();
     //       stackLayout.Orientation = StackOrientation.Horizontal;
     //       stackLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;

     //       if (PremiumManagement.IsFree)
     //       {
     //           for (int i = 0; i < numberOfIndicators; i++)
     //           {
					//var indicator = new CachedImage {
					//	HeightRequest = 30,
					//	HorizontalOptions = LayoutOptions.CenterAndExpand,
					//	Style = ImageProperties.DefaultCachedImageStyle
					//};
					//if (activeIndex.Equals(i))
     //                   indicator.Source = "indicator_active.png";
     //               else
     //                   indicator.Source = "indicator_inactive.png";

     //               var tapGestureRecognizer = new TapGestureRecognizer();
     //               tapGestureRecognizer.Tapped += indicatorClicked;
     //               tapGestureRecognizer.CommandParameter = i;
     //               indicator.GestureRecognizers.Add(tapGestureRecognizer);

     //               stackLayout.Children.Add(indicator);
     //           }
     //       }

     //       return stackLayout;
     //   }

        //void indicatorClicked(object sender, EventArgs e)
        //{
        //    var eventArgs = (TappedEventArgs)e;
        //    var index = (int)eventArgs.Parameter;
        //    removeIndicators();
        //    addIndicators(index);
        //}

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
