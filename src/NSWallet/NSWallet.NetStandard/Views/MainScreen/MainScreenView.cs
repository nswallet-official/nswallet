using System;
using System.Linq;
using FFImageLoading.Forms;
using ImageCircle.Forms.Plugin.Abstractions;
using NSWallet.Consts;
using NSWallet.Controls.EntryPopup;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public partial class MainScreenView : ContentPage
	{
		ListView itemsListView;
		CustomSearchBar searchBar;
		ToolbarItem searchToolbarItem;
		Label restrictionSearchLabel;
		StackLayout emptyAddStackLayout;

		MainScreenViewModel mainScreenVM;

		FloatingActionButton addButtonFloating; // = new FloatingActionButton(); // Droid ONLY

		Grid mainGrid;

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (StateHandler.CopyFieldLocallyActivated) {
				StateHandler.CopyFieldLocallyActivated = false;
				mainScreenVM.CopyLocally();
			}

			if (StateHandler.DeleteFieldActivated) {
				StateHandler.DeleteFieldActivated = false;
				LaunchMessageBox("Confirmation", "Are you sure you want to delete this field?", "/deletefield");
			}
		}

		public MainScreenView()
		{
			UINavigationHeader.SetCommonTitleView(this, TR.Tr("app_name"));


			// FIXME: remove this part as soon as there are no old bugs detected
			/*
			if (isUnicodePassword) {
				Application.Current.MainPage.DisplayAlert(
					TR.Tr("alert"), TR.Tr("change_password_critical_message_ios"), TR.OK, TR.Cancel).ContinueWith(
						isOK => {
							Settings.ChangePasswordUnicodeIOSBug = true;
							if (isOK.Result) {
								Device.BeginInvokeOnMainThread(Pages.Settings);
							}
						}
					);
			}
			*/

			Icon = Theme.Current.MenuIcon;
			BackgroundColor = Theme.Current.ListBackgroundColor;

			mainScreenVM = new MainScreenViewModel(Navigation);
			BindingContext = mainScreenVM;

			searchToolbarItem = new ToolbarItem {
				Text = TR.Tr("search"),
				Icon = Theme.Current.AppSearchIcon
			};
			searchToolbarItem.SetBinding(MenuItem.CommandProperty, "SearchLaunchCommand");
			ToolbarItems.Add(searchToolbarItem);

			mainGrid = new Grid();

			var bodyStackLayout = new StackLayout {
				Spacing = Theme.Current.MainPageBodySpacing
			};

			// Header Area: start -------------------------------------------
			var headerLayout = new StackLayout {
				BackgroundColor = Theme.Current.AppHeaderBackground
			};
			headerLayout.SetBinding(IsVisibleProperty, "IsHeaderVisible");
			headerLayout.Orientation = StackOrientation.Horizontal;

			var backHeaderGesture = new TapGestureRecognizer();
			backHeaderGesture.SetBinding(TapGestureRecognizer.CommandProperty, "BackCommand");
			headerLayout.GestureRecognizers.Add(backHeaderGesture);

			var headerTextLayout = new StackLayout();
			headerTextLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
			headerTextLayout.Padding = Theme.Current.MainPageHeaderPadding;


			var titleLayout = new StackLayout {
				Spacing = Theme.Current.MainPageHeaderTitleSpacing,
				Orientation = StackOrientation.Horizontal
			};

			// Header Area: Back button
			var backArrowImage = new CachedImage {
				HeightRequest = Theme.Current.MainBackArrowHeight,
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.MainArrowBackIcon)),
				AutomationId = AutomationIdConsts.BACK_BUTTON_ID,
				Style = ImageProperties.DefaultCachedImageStyle
			};
			titleLayout.Children.Add(backArrowImage);

			// Header Area: Item / Folder title
			var titleLabel = new Label {
				LineBreakMode = LineBreakMode.TailTruncation,
				FontSize = Theme.Current.MainTitleHeight,
				TextColor = Theme.Current.MainTitleTextColor,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				FontAttributes = Theme.Current.MainTitleFontAttributes
			};
			titleLabel.SetBinding(Label.TextProperty, "Title");
			titleLayout.Children.Add(titleLabel);

			headerTextLayout.Children.Add(titleLayout);

			var pathLabel = new Label();
			pathLabel.SetBinding(Label.TextProperty, "Path");
			pathLabel.LineBreakMode = LineBreakMode.HeadTruncation;
			pathLabel.FontSize = Theme.Current.MainPathHeight;
			pathLabel.FontFamily = NSWFontsController.CurrentTypeface;
			pathLabel.TextColor = Theme.Current.MainPathTextColor;
			pathLabel.FontAttributes = Theme.Current.MainPathFontAttributes;

			headerTextLayout.Children.Add(pathLabel);

			headerLayout.Children.Add(headerTextLayout);

			var settingsImage = new CachedImage {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				HeightRequest = Theme.Current.MainPageHeaderIconHeight,
				WidthRequest = Theme.Current.MainPageHeaderIconHeight,
				Margin = Theme.Current.MainPageHeaderIconPadding,
				AutomationId = AutomationIdConsts.CURRENT_ITEM_ICON_ID,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			settingsImage.SetBinding(CachedImage.SourceProperty, "CurrentItemIcon");
			settingsImage.SetBinding(IsVisibleProperty, "IsIconNotCircle");

			var settingsImageCircle = new CircleImage {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				HeightRequest = Theme.Current.MainPageHeaderIconHeight,
				WidthRequest = Theme.Current.MainPageHeaderIconHeight,
				BorderThickness = Theme.Current.CommonIconBorderWidth,
				BorderColor = Theme.Current.CommonIconBorderColor,
				Aspect = Aspect.AspectFill,
				Margin = Theme.Current.MainPageHeaderIconPadding,
				AutomationId = AutomationIdConsts.CURRENT_ITEM_ICON_ID
			};

			settingsImageCircle.SetBinding(Image.SourceProperty, "CurrentItemIcon");
			settingsImageCircle.SetBinding(IsVisibleProperty, "IsIconCircle");

			var settingsGesture = new TapGestureRecognizer();
			//settingsGesture.Tapped += OpenSettings;
			settingsGesture.SetBinding(TapGestureRecognizer.CommandProperty, "SettingsCommand");
			settingsImage.GestureRecognizers.Add(settingsGesture);
			settingsImageCircle.GestureRecognizers.Add(settingsGesture);

			headerLayout.Children.Add(settingsImage);
			headerLayout.Children.Add(settingsImageCircle);

			bodyStackLayout.Children.Add(headerLayout);

			searchBar = new CustomSearchBar();
			searchBar.FontFamily = NSWFontsController.CurrentTypeface;
			searchBar.Placeholder = TR.Tr("search_type_text");
			searchBar.SetBinding(SearchBar.TextProperty, "SearchText");
			searchBar.BackgroundColor = Theme.Current.AppHeaderBackground;
			searchBar.CancelButtonColor = Theme.Current.MainSearchCancelButtonColor;
			switch (Device.RuntimePlatform) {
				case Device.iOS:
					searchBar.TextColor = Color.Black;
					break;
				case Device.Android:
					searchBar.TextColor = Color.White;
					break;
			}
			searchBar.PlaceholderColor = Color.LightGray;
			searchBar.IsVisible = false;

			switch (Device.RuntimePlatform) {
				case Device.Android:
					searchBar.HeightRequest = 40;
					break;
			}

			bodyStackLayout.Children.Add(searchBar);

			restrictionSearchLabel = new Label();
			restrictionSearchLabel.IsVisible = false;
			restrictionSearchLabel.Text = TR.Tr("search_symbol_restriction");
			restrictionSearchLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
			restrictionSearchLabel.TextColor = Theme.Current.ListTextColor;
			restrictionSearchLabel.HorizontalTextAlignment = TextAlignment.Center;
			restrictionSearchLabel.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
			restrictionSearchLabel.FontFamily = NSWFontsController.CurrentTypeface;
			bodyStackLayout.Children.Add(restrictionSearchLabel);


			// Adding item and folder menu to main screen (invisible by default)
			bodyStackLayout.Children.Add(GetFolderMenu());
			bodyStackLayout.Children.Add(GetItemMenu());

			itemsListView = new ListView();
			itemsListView.HasUnevenRows = true;
			itemsListView.BackgroundColor = Theme.Current.ListBackgroundColor;
			itemsListView.ItemTemplate = new DataTemplate(typeof(MainScreenViewCell));
			itemsListView.SeparatorVisibility = SeparatorVisibility.None;
			itemsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Items");
			itemsListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			itemsListView.SetBinding(IsVisibleProperty, "ListIsNotEmpty");
			itemsListView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };

			itemsListView.PropertyChanged += (sender, e) => {
				if (e.PropertyName == "ItemsSource") {
					if (mainScreenVM.ItemToScrollTo != null) {
						var item = mainScreenVM.Items.SingleOrDefault(x => x.ItemID == mainScreenVM.ItemToScrollTo && x.FieldID == null);
						itemsListView.ScrollTo(item, ScrollToPosition.Start, true);
					}
				}
			};

			bodyStackLayout.Children.Add(itemsListView);

			var emptyAddButton = new StackLayout();
			emptyAddButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
			emptyAddButton.VerticalOptions = LayoutOptions.CenterAndExpand;

			var emptyAddButtonImage = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage("MainScreen.ic_main_add_button.png")),
				HeightRequest = 100,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};
			emptyAddButtonImage.SetBinding(IsVisibleProperty, "ListIsEmpty");
			emptyAddButton.Children.Add(emptyAddButtonImage);

			var emptyAddButtonText = new Label();
			emptyAddButtonText.FontFamily = NSWFontsController.CurrentTypeface;
			emptyAddButtonText.TextColor = Color.FromHex("cccccc");
			emptyAddButtonText.HorizontalTextAlignment = TextAlignment.Center;
			emptyAddButtonText.HorizontalOptions = LayoutOptions.CenterAndExpand;
			emptyAddButtonText.VerticalOptions = LayoutOptions.CenterAndExpand;
			emptyAddButtonText.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			emptyAddButtonText.SetBinding(IsVisibleProperty, "ListIsEmpty");
			emptyAddButtonText.SetBinding(Label.TextProperty, "EmptyAddButtonText");
			emptyAddButton.Children.Add(emptyAddButtonText);

			emptyAddStackLayout = new StackLayout {
				Children = {
					emptyAddButton
				},
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			emptyAddStackLayout.SetBinding(IsVisibleProperty, "ListIsEmpty");

			var emptyAddButtonGesture = new TapGestureRecognizer();
			emptyAddButtonGesture.SetBinding(TapGestureRecognizer.CommandProperty, "LaunchPopupCommand");
			emptyAddStackLayout.GestureRecognizers.Add(emptyAddButtonGesture);

			bodyStackLayout.Children.Add(emptyAddStackLayout);

			var localClipboardLabel = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				Text = TR.Tr("main_local_clipboard"),
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Theme.Current.AppHeaderTextColor,
				Margin = new Thickness(5)
			};

			var closeClipboardIcon = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.CloseClipboardIcon)),
				HeightRequest = 40,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var tapCloseClipboard = new TapGestureRecognizer();
			tapCloseClipboard.SetBinding(TapGestureRecognizer.CommandProperty, "CloseClipboardCommand");
			closeClipboardIcon.GestureRecognizers.Add(tapCloseClipboard);

			var clipboardHeader = new StackLayout {
				BackgroundColor = Theme.Current.AppHeaderBackground,
				Orientation = StackOrientation.Horizontal,
				Children = {
					localClipboardLabel,
					closeClipboardIcon
				}
			};

			var clipboardIcon = new CachedImage {
				HeightRequest = Theme.Current.MainPageListIconHeight,
				WidthRequest = Theme.Current.MainPageListIconHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			clipboardIcon.SetBinding(CachedImage.SourceProperty, "ClipboardIcon");
			clipboardIcon.SetBinding(IsVisibleProperty, "IsClipboardIconNotCircle");

			var clipboardIconCircle = new CircleImage {
				HeightRequest = Theme.Current.MainPageListIconHeight,
				WidthRequest = Theme.Current.MainPageListIconHeight,
				Aspect = Aspect.Fill,
				BorderThickness = Theme.Current.CommonIconBorderWidth,
				BorderColor = Theme.Current.CommonIconBorderColor,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			clipboardIconCircle.SetBinding(Image.SourceProperty, "ClipboardIcon");
			clipboardIconCircle.SetBinding(IsVisibleProperty, "IsClipboardIconCircle");

			var clipboardName = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				LineBreakMode = LineBreakMode.TailTruncation,
				TextColor = Theme.Current.ListTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			clipboardName.SetBinding(Label.TextProperty, "ClipboardName");

			var clipboardBody = new StackLayout {
				Padding = new Thickness(5),
				Orientation = StackOrientation.Horizontal,
				Children = {
					clipboardIcon,
					clipboardIconCircle,
					clipboardName
				}
			};

			var copyButton = new Button {
				FontFamily = NSWFontsController.CurrentTypeface,
				Text = TR.Tr("copy_here"),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Theme.Current.CommonButtonTextColor,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				CornerRadius = 0,
				BorderWidth = 0,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button))
			};

			copyButton.SetBinding(IsVisibleProperty, "IsCopyEnabled");
			copyButton.SetBinding(Button.CommandProperty, "CopyCommand");

			var moveButton = new Button {
				FontFamily = NSWFontsController.CurrentTypeface,
				Text = TR.Tr("move_here"),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Theme.Current.CommonButtonTextColor,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				CornerRadius = 0,
				BorderWidth = 0,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button))
			};

			moveButton.SetBinding(IsVisibleProperty, "IsMoveEnabled");
			moveButton.SetBinding(Button.CommandProperty, "MoveCommand");

			var clipboardButtons = new StackLayout {
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = {
					copyButton,
					moveButton
				}
			};

			var clipboardUnavailableLabel = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label))
			};
			clipboardUnavailableLabel.SetBinding(Label.TextProperty, "UnavailableClipboardText");

			var clipboardUnavailableLayout = new StackLayout {
				BackgroundColor = Color.Red,
				Padding = new Thickness(10),
				Children = {
					clipboardUnavailableLabel
				}
			};
			clipboardUnavailableLayout.SetBinding(IsVisibleProperty, "IsUnavailableClipboard");

			var clipboardLayout = new StackLayout {
				Children = {
					clipboardHeader,
					clipboardBody,
					clipboardButtons,
					clipboardUnavailableLayout
				},
				VerticalOptions = LayoutOptions.EndAndExpand
			};

			clipboardLayout.SetBinding(IsVisibleProperty, "IsLocalClipboardActivated");

			bodyStackLayout.Children.Add(clipboardLayout);


			mainGrid.Children.Add(bodyStackLayout);

			switch (Device.RuntimePlatform) {
				case Device.Android:
					addButtonFloating = new FloatingActionButton {
						FontFamily = NSWFontsController.CurrentTypeface,
						Margin = Theme.Current.MainPageAddButtonMargin,
						ButtonColor = Theme.Current.AppHeaderBackground,
						TextColor = Color.White,
						FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(FloatingActionButton)),
						HorizontalOptions = LayoutOptions.EndAndExpand,
						VerticalOptions = LayoutOptions.EndAndExpand,
						HeightRequest = Theme.Current.MainPageAddButtonHeight,
						WidthRequest = Theme.Current.MainPageAddButtonHeight,
						Image = Theme.Current.MainPageAdd,
						AutomationId = AutomationIdConsts.LABELS_ADD_BUTTON_ID
					};
					addButtonFloating.SetBinding(Button.CommandProperty, "LaunchPopupCommand");
					//addButtonFloating.SetBinding(IsVisibleProperty, "ListIsNotEmpty");
					addButtonFloating.SetBinding(IsVisibleProperty, "AddButtonVisible");
					mainGrid.Children.Add(addButtonFloating);
					break;
				case Device.macOS:
				case Device.iOS:
					var addButtonToolbar = new ToolbarItem {
						Text = TR.Tr("mainpage_add"),
						Icon = Theme.Current.MainPageAdd,
						AutomationId = AutomationIdConsts.LABELS_ADD_BUTTON_ID
					};
					addButtonToolbar.SetBinding(MenuItem.CommandProperty, "LaunchPopupCommand");
					ToolbarItems.Add(addButtonToolbar);
					break;
			}

			SetCallbacks();

			switch (Device.RuntimePlatform) {
				case Device.Android:
					mainGrid.SizeChanged += MainGrid_SizeChanged;
					break;
			}

			Content = mainGrid;
		}

		void MainGrid_SizeChanged(object sender, EventArgs e)
		{
			if (addButtonFloating != null && mainGrid != null && addButtonFloating.ErrorStatus) {
				mainGrid.Children.Remove(addButtonFloating);

				var addButton = new CachedImage {
					Margin = Theme.Current.MainPageAddButtonMargin,
					HorizontalOptions = LayoutOptions.EndAndExpand,
					VerticalOptions = LayoutOptions.EndAndExpand,
					HeightRequest = Theme.Current.MainPageAddButtonHeight,
					WidthRequest = Theme.Current.MainPageAddButtonHeight,
					Style = ImageProperties.DefaultCachedImageStyle
				};
				addButton.SetBinding(IsVisibleProperty, "AddButtonVisible");
				addButton.Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.MainPageAddRoundIcon));
				addButton.AutomationId = AutomationIdConsts.LABELS_ADD_BUTTON_ID;

				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "LaunchPopupCommand");
				addButton.GestureRecognizers.Add(tapGestureRecognizer);

				mainGrid.Children.Add(addButton);
			}
		}


		public void ChangeItemTitle()
		{
			var currentState = BL.GetCurrentItem();
			string changeTitle = null;
			string emptyItem = null;

			switch (currentState.Folder) {
				case true:
					changeTitle = TR.Tr("change_folder_title");
					emptyItem = TR.Tr("change_folder_title_empty");
					break;
				case false:
					changeTitle = TR.Tr("change_item_title");
					emptyItem = TR.Tr("change_item_title_empty");
					break;
			}

			var popup = new EntryPopup(changeTitle, currentState.Name, false);
			popup.PopupClosed += (o, closedArgs) => {
				if (closedArgs.OkClicked) {
					if (closedArgs.Text == string.Empty) {
						DisplayAlert(TR.Tr("app_name"), emptyItem, TR.OK);
						return;
					} else {
						mainScreenVM.OKTitleChangeCommand.Execute(closedArgs.Text);
					}
				}
			};

			popup.Show();
		}



		void SetCallbacks()
		{
			mainScreenVM.PremiumAlertCallback = PremiumAlert;
			mainScreenVM.SearchEntryShowHideCommandCallback = SearchEntryShowHide;
			mainScreenVM.LaunchEditTitlePopupCommandCallback = ChangeItemTitle;
			mainScreenVM.HidePopupCommandCallback = HideCreatePopup;
			mainScreenVM.LaunchPopupCommandCallback = LaunchCreatePopup;
			mainScreenVM.HideMenuPopupCommandCallback = HideMenuPopup;
			mainScreenVM.LaunchMenuPopupCommandCallback = LaunchMenuPopup;
			mainScreenVM.MessageCommand = LaunchMessageBox;
			mainScreenVM.MessageBoxNotEmptyFolderCommand = LaunchMessageBoxNotEmptyFolder;
			mainScreenVM.LaunchNotImplementedCommand = LaunchNotImplementedMsgBox;
			mainScreenVM.LaunchShowHideRestrictionCommand = ShowHideRestrictionLabel;
			mainScreenVM.ShowEmptyAddButtonCommand = ShowEmptyAddButton;
			mainScreenVM.HideEmptyAddButtonCommand = HideEmptyAddButton;
			mainScreenVM.SetSearchIconCommand = SetSearchIcon;
		}

		void ShowEmptyAddButton()
		{
			if (emptyAddStackLayout != null) {
				emptyAddStackLayout.IsVisible = true;
			}
		}

		void HideEmptyAddButton()
		{
			if (emptyAddStackLayout != null) {
				emptyAddStackLayout.IsVisible = false;
			}
		}

		void LaunchNotImplementedMsgBox()
		{
			var answer = DisplayAlert(TR.Tr("alert"), TR.Tr("not_implemented"), TR.Tr("ok"));
		}

		void PremiumAlert()
		{
			var answer = DisplayAlert(TR.Tr("premium"), TR.Tr("premium_description"), TR.Tr("premium_buy"), TR.Tr("cancel")).ContinueWith((t => {
				if (t.Result) {
					Device.BeginInvokeOnMainThread(() => Pages.Premium(Navigation));
				}
			}));
		}

		void SearchEntryShowHide(bool onlyHide = false)
		{
			if (searchBar.IsVisible) {
				ShowHideRestrictionLabel(false);
				searchBar.IsVisible = false;
				searchBar.Unfocus();
				SetSearchIcon(false);
			} else {
				if (!onlyHide) {
					ShowHideRestrictionLabel(true);
					searchBar.IsVisible = true;
					searchBar.Focus();
					SetSearchIcon(true);
				}
			}
		}

		void SetSearchIcon(bool cancel)
		{
			if (cancel) {
				searchToolbarItem.Icon = Theme.Current.AppSearchIconCancel;
			} else {
				searchToolbarItem.Icon = Theme.Current.AppSearchIcon;
			}
		}

		void ShowHideRestrictionLabel(bool show)
		{
			switch (show) {
				case true:
					restrictionSearchLabel.IsVisible = true;
					break;
				case false:
					restrictionSearchLabel.IsVisible = false;
					break;
			}
		}

		async void LaunchMessageBox(string title, string question, string type)
		{
			var answer = await DisplayAlert(title, question, TR.Tr("yes"), TR.Tr("no"));

			if (answer) {
				switch (type) {
					case "/deletefield":
						mainScreenVM.DeleteFieldConfirmCommand.Execute(true);
						break;
					case "/delete":
						mainScreenVM.DeleteConfirmCommand.Execute(true);
						break;
				}
			}
		}

		void LaunchMessageBoxNotEmptyFolder(string title, string question)
		{
			DisplayAlert(title, question, "OK");
		}

		void LaunchCreatePopup()
		{
			//Navigation.PushPopupAsync(popupPage_Create, false);
			Device.BeginInvokeOnMainThread(async () => {

				var result = await DisplayActionSheet(
					TR.Tr("create_new"),
					TR.Cancel,
					null,
					TR.Tr("popupmenu_folder"),
					TR.Tr("popupmenu_item")
					);

				if (result == TR.Tr("popupmenu_folder")) {
					mainScreenVM.CreateFolderCommand.Execute(null);
				}

				if (result == TR.Tr("popupmenu_item")) {
					mainScreenVM.CreateItemCommand.Execute(null);
				}
			});
		}

		void HideCreatePopup()
		{
			//Navigation.PopPopupAsync(false);
		}

		void LaunchMenuPopup()
		{
			//menuPopupLayout.IsVisible = true;
		}

		void HideMenuPopup()
		{
			//menuPopupLayout.IsVisible = false;
		}

		protected override bool OnBackButtonPressed()
		{
			if (mainScreenVM.IsSearchEnabled) {
				mainScreenVM.SearchLaunchCommand.Execute(null);
			} else {
				if (mainScreenVM.IsRoot) {
					if (Settings.AndroidBackLogout) {
						LogoutConfirm();
					}
				} else {
					mainScreenVM.BackCommand.Execute(null);
				}
			}

			return true;
		}

		async void LogoutConfirm()
		{
			var choice = await DisplayAlert(TR.Tr("app_name"), TR.Tr("logout_confirm"), TR.Yes, TR.No);

			if (choice) {
				LoginScreenView.ManualExit = true;
				Device.BeginInvokeOnMainThread(() => Pages.Login(true));
			}
		}
	}
}