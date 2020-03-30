using FFImageLoading.Forms;
using NSWallet.Consts;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class MainMenuView : ContentPage
    {
        public MainMenuView()
        {
			var mainMenuVM = new MainMenuViewModel();
			BindingContext = mainMenuVM;

            Title = TR.Tr("app_name");
			Icon = Theme.Current.MenuIcon;
            BackgroundColor = Theme.Current.MenuTopBackgroundColor;

            var premiumLabel = new Label
            {
                FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start
            };
            premiumLabel.SetBinding(Label.TextProperty, "Premium");
            premiumLabel.SetBinding(Label.TextColorProperty, "PremiumColor");
            
            var userLayout = new StackLayout
            {
				
                Children =
                {
					new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(0, 10, 0, 0),
                        Children =
                        {
                            new CachedImage
                            {
								Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.AppIconNoBack)),
								WidthRequest = 90,
								HeightRequest = 90,
                                HorizontalOptions = LayoutOptions.Start,
								VerticalOptions = LayoutOptions.CenterAndExpand,
								Style = ImageProperties.DefaultCachedImageStyle
							},

                            new StackLayout 
                            {
                                Orientation = StackOrientation.Vertical,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
								VerticalOptions = LayoutOptions.CenterAndExpand,
                                //Padding = new Thickness(0, 10, 0, 0),


                                Children =
                                {
                                    new Label
                                    {
                                        Text = TR.Tr("app_name"),
										FontSize = FontSizeController.GetSize(NamedSize.Large, typeof(Label)),
										FontAttributes = FontAttributes.Bold,
										HorizontalOptions = LayoutOptions.Start,
										TextColor = Theme.Current.MenuTopTextColor,
										FontFamily = NSWFontsController.CurrentBoldTypeface
									},

                                    new Label
                                    {
                                        Text = TR.Tr("version_label") + " " + PlatformSpecific.GetVersion(),
                                        FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label)),
										FontAttributes = FontAttributes.Bold,
										HorizontalOptions = LayoutOptions.Start,
										TextColor = Theme.Current.MenuTopTextColor,
										FontFamily = NSWFontsController.CurrentBoldTypeface
									},

									premiumLabel
								}
							}
						}
                    }
                },

                HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Start
            };

			switch (Device.RuntimePlatform) {
				case Device.iOS:
					userLayout.Padding = new Thickness(0, 20, 0, 0);
					break;
			}

            AddSeparator(userLayout);


            StackLayout menuLayout = new StackLayout
			{
                BackgroundColor = Theme.Current.MenuBackgroundColor,
				Padding = new Thickness(0, 0, 0, 0),
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				Spacing = 0,

					
			};

			StackLayout restLayout = new StackLayout
			{
				BackgroundColor = Theme.Current.MenuBackgroundColor,
				Padding = new Thickness(0, 0, 0, 0),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = StackOrientation.Vertical,
				Spacing = 0,
				Children = {
					new BoxView{
						Color = Theme.Current.MenuBackgroundColor,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.FillAndExpand
					}
				}
			};

			AddSeparator(menuLayout);
			AddMenu(menuLayout, "HomeCommand", TR.Tr("menu_home"), Theme.Current.MenuIconHome, AutomationIdConsts.MENU_HOME_BUTTON_ID);
			AddSeparator(menuLayout);
			AddMenu(menuLayout, "SettingsCommand", TR.Tr("menu_settings"), Theme.Current.MenuIconSettings, AutomationIdConsts.MENU_SETTINGS_BUTTON_ID);
			AddSeparator(menuLayout);
			AddMenu(menuLayout, "BackupCommand", TR.Tr("menu_backup"), Theme.Current.MenuIconBackup, AutomationIdConsts.MENU_BACKUP_BUTTON_ID);
			AddSeparator(menuLayout);
			AddMenu(menuLayout, "LabelsCommand", TR.Tr("menu_labels"), Theme.Current.MenuIconLabels, AutomationIdConsts.MENU_LABELS_BUTTON_ID);
            AddSeparator(menuLayout);
			AddMenu(menuLayout, "IconsCommand", TR.Tr("menu_icons"), Theme.Current.MenuIconIcons, AutomationIdConsts.MENU_ICONS_BUTTON_ID);
			AddSeparator(menuLayout);
			AddMenu(menuLayout, "ExportImportCommand", TR.Tr("menu_export_import"), Theme.Current.MenuIconImportExport, AutomationIdConsts.MENU_EXPORT_IMPORT_BUTTON_ID); 
			AddSeparator(menuLayout);

			//if (Settings.IsFeedback) {
			//AddMenu(menuLayout, "FeedbackCommand", TR.Tr("menu_feedback_request"), Theme.Current.MenuIconFeedback, AutomationIdConsts.MENU_FEEDBACK_BUTTON_ID);
			//AddSeparator(menuLayout);
			//}

			AddMenu(menuLayout, "AboutCommand", TR.Tr("menu_about"), Theme.Current.MenuIconAbout, AutomationIdConsts.MENU_ABOUT_BUTTON_ID);
			AddSeparator(menuLayout);
			if (Settings.DevOpsOn) {
				AddMenu(menuLayout, "DeveloperCommand", TR.Tr("menu_developer"), Theme.Current.MenuIconDeveloper, AutomationIdConsts.MENU_DEVELOPER_BUTTON_ID);
				AddSeparator(menuLayout);
			}
			AddMenu(menuLayout, "LogoutCommand", TR.Tr("menu_logout"), Theme.Current.MenuIconLogout, AutomationIdConsts.MENU_LOGOUT_BUTTON_ID);
			AddSeparator(menuLayout);

            Thickness paddingStack;
            switch (Device.RuntimePlatform) {
                case Device.Android:
                    paddingStack = new Thickness(0, 20, 0, 0);
                    break;
                case Device.iOS:
                    paddingStack = new Thickness(0, 0, 0, 0);
                    break;
                default:
                    paddingStack = new Thickness(0, 0, 0, 0);
                    break;
            }

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Fill,
                    BackgroundColor = Theme.Current.MenuTopBackgroundColor,
                    Padding = paddingStack,
                    Spacing = 0,
                    Children = { userLayout, menuLayout, restLayout }
                }
            };
        }

        static void AddSeparator(StackLayout menuLayout)
		{
			var separator = new BoxView
			{
                Color = Theme.Current.ListSeparatorColor,
				HeightRequest = 1,
				Opacity = 0.5
			};

			menuLayout.Children.Add(separator);
		}

		void AddMenu(StackLayout stack, string modelCommand, string menuName, string menuIcon, string automationId, bool newFeature = false)
		{
			var itemMenuStack = new StackLayout
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Padding = new Thickness(15, 6, 0, 6),
			};

			if (!string.IsNullOrEmpty(menuIcon))
			{
				var itemImage = new CachedImage
				{
					HeightRequest = 30,
					WidthRequest = 30,
					Source = ImageSource.FromStream(() => NSWRes.GetImage(menuIcon)),
					Style = ImageProperties.DefaultCachedImageStyle
				};

				itemMenuStack.Children.Add(itemImage);
				itemMenuStack.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 16 });
			}
			else 
            {
				itemMenuStack.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 48 });
			}

			var itemLabel = new Label
			{
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontAttributes = FontAttributes.Bold,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				TextColor = Theme.Current.MenuTextColor,
				Opacity = 0.85,
				Text = menuName,
				AutomationId = automationId,
			};

			itemMenuStack.Children.Add(itemLabel);

			if (newFeature) {
				var newFeatureLabel = new Label {
					Text = TR.Tr("new"),
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.EndAndExpand,
					Margin = new Thickness(0, 0, 10, 0),
					TextColor = Color.Red
				};

				itemMenuStack.Children.Add(newFeatureLabel);
			}

			stack.Children.Add(itemMenuStack);

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, modelCommand);
			itemMenuStack.GestureRecognizers.Add(tapGestureRecognizer);
		}
    }
}

