using FFImageLoading.Forms;
using NSWallet.Consts;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public partial class ManageFieldView : ContentPage
    {
		StackLayout choosePopup;

		public ManageFieldView(bool isEdit, NSWItem nswItem, string fieldID = null, bool viewMode = true, string fieldValue = null)
		{
			choosePopup = new StackLayout();

			BackgroundColor = Theme.Current.ListBackgroundColor;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("app_name"));

			var tbItem = new ToolbarItem();
			tbItem.Icon = "close.png";

			tbItem.Clicked += (sender, e) => {
				if (choosePopup.IsVisible) {
					choosePopup.IsVisible = false;
				} else {
					Navigation.PopModalAsync();
				}
			};

			ToolbarItems.Add(tbItem);
			
            ManageFieldViewModel pageVM = null;
            if (isEdit)
                pageVM = new ManageFieldViewModel(Navigation, nswItem, fieldID, viewMode, fieldValue);
            else
                pageVM = new ManageFieldViewModel(Navigation, nswItem);

            BindingContext = pageVM;

            var mainStackLayout = new StackLayout();

            var typeLayout = new StackLayout();
            typeLayout.Padding = new Thickness(20, 10);
            typeLayout.VerticalOptions = LayoutOptions.Start;

            var innerTypeLayout = new StackLayout();
            innerTypeLayout.Spacing = 20;
            innerTypeLayout.Orientation = StackOrientation.Horizontal;
            innerTypeLayout.VerticalOptions = LayoutOptions.CenterAndExpand;

			var typeIcon = new CachedImage {
				HeightRequest = 40,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};
			typeIcon.SetBinding(CachedImage.SourceProperty, "TypeIcon");
            innerTypeLayout.Children.Add(typeIcon);

            var typeName = new Label();
			typeName.FontFamily = NSWFontsController.CurrentBoldTypeface;
			typeName.VerticalOptions = LayoutOptions.CenterAndExpand;
            typeName.SetBinding(Label.TextProperty, "TypeName");
            typeName.FontAttributes = FontAttributes.Bold;
            typeName.TextColor = Theme.Current.NormalTextColor;
			typeName.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
            innerTypeLayout.Children.Add(typeName);

			var selectorIcon = new CachedImage {
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Source = ImageSource.FromStream(() => NSWRes.GetImage("Icons.arrow_down.png")),
				HeightRequest = 30,
				AutomationId = AutomationIdConsts.SELECTOR_ICON_ID,
				Style = ImageProperties.DefaultCachedImageStyle
			};
			selectorIcon.SetBinding(IsVisibleProperty, "IsCreate");
			innerTypeLayout.Children.Add(selectorIcon);

			typeLayout.Children.Add(innerTypeLayout);

            var typeLayoutGesture = new TapGestureRecognizer();
            typeLayoutGesture.SetBinding(TapGestureRecognizer.CommandProperty, "ShowPopupCommand");
            typeLayout.GestureRecognizers.Add(typeLayoutGesture);

            mainStackLayout.Children.Add(typeLayout);

            #region Fields editors

            var defaultEntry = new RectangularEntry();
			defaultEntry.FontFamily = NSWFontsController.CurrentTypeface;
			defaultEntry.BackgroundColor = Color.White;
            defaultEntry.Placeholder = TR.Tr("field_placeholder");
            defaultEntry.Margin = Theme.Current.EntryMargin;
            defaultEntry.HeightRequest = Theme.Current.EntryHeight;
			defaultEntry.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(RectangularEntry));
			defaultEntry.SetBinding(IsEnabledProperty, "NotViewMode");
            defaultEntry.SetBinding(IsVisibleProperty, "IsDefaultLabel");
            defaultEntry.SetBinding(Entry.TextProperty, "DefaultEditorText");
            defaultEntry.AutomationId = AutomationIdConsts.ITEM_FIELD_DEFAULT_ENTRY_ID;
            defaultEntry.HorizontalTextAlignment = TextAlignment.Start;
            mainStackLayout.Children.Add(defaultEntry);

            var noteEditor = new CustomEditor();
			noteEditor.HorizontalOptions = LayoutOptions.FillAndExpand;
			noteEditor.VerticalOptions = LayoutOptions.FillAndExpand;
			noteEditor.MaxLength = int.MaxValue;
			noteEditor.FontFamily = NSWFontsController.CurrentTypeface;
			noteEditor.BackgroundColor = Color.White;
            noteEditor.Margin = Theme.Current.EntryMargin;
            //noteEditor.HeightRequest = Theme.Current.EditorHeight;
			noteEditor.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(CustomEditor));
			noteEditor.SetBinding(IsVisibleProperty, "IsNoteLabel");
            noteEditor.SetBinding(Editor.TextProperty, "NoteEditorText");
			noteEditor.SetBinding(IsEnabledProperty, "NotViewMode");
            noteEditor.AutomationId = AutomationIdConsts.ITEM_FIELD_NOTE_EDITOR_ID;
            //noteEditor.HorizontalOptions = LayoutAlignment.Start;
            mainStackLayout.Children.Add(noteEditor);

            var datePicker = new CustomDatePicker();
			datePicker.FontFamily = NSWFontsController.CurrentTypeface;
			datePicker.BackgroundColor = Color.White;
            datePicker.Margin = Theme.Current.EntryMargin;
            datePicker.HeightRequest = Theme.Current.EntryHeight;
			datePicker.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(CustomDatePicker));
			datePicker.SetBinding(IsVisibleProperty, "IsDateLabel");
            datePicker.SetBinding(DatePicker.DateProperty, "DateEditorText");
			datePicker.SetBinding(IsEnabledProperty, "NotViewMode");
			datePicker.AutomationId = AutomationIdConsts.ITEM_FIELD_DATE_PICKER_ID;
            mainStackLayout.Children.Add(datePicker);

            var timePicker = new CustomTimePicker();
			timePicker.FontFamily = NSWFontsController.CurrentTypeface;
			timePicker.BackgroundColor = Color.White;
            timePicker.Margin = Theme.Current.EntryMargin;
            timePicker.HeightRequest = Theme.Current.EntryHeight;
			timePicker.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(CustomTimePicker));
            timePicker.SetBinding(IsVisibleProperty, "IsTimeLabel");
            timePicker.SetBinding(TimePicker.TimeProperty, "TimeEditorText");
			timePicker.SetBinding(IsEnabledProperty, "NotViewMode");
			timePicker.AutomationId = AutomationIdConsts.ITEM_FIELD_TIME_PICKER_ID;
            mainStackLayout.Children.Add(timePicker);

			var progressBar = new ProgressBar {
				Margin = Theme.Current.EntryMargin
			};

			progressBar.SetBinding(IsVisibleProperty, "Is2FA");
			progressBar.SetBinding(ProgressBar.ProgressProperty, "TwoFATimeLeftProgress");
			mainStackLayout.Children.Add(progressBar);

            #endregion

            var horStackLayout = new StackLayout();
            horStackLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;
            horStackLayout.Orientation = StackOrientation.Horizontal;

            var buttonsLayout = new StackLayout();
            buttonsLayout.Margin = Theme.Current.EntryMargin;
            buttonsLayout.Orientation = StackOrientation.Horizontal;

   //         var cancel = new Button();
			//cancel.FontFamily = NSWFontsController.CurrentBoldTypeface;
			//cancel.CornerRadius = Theme.Current.ButtonRadius;
   //         cancel.HorizontalOptions = LayoutOptions.FillAndExpand;
   //         cancel.BackgroundColor = Theme.Current.CommonButtonBackground;
   //         cancel.TextColor = Theme.Current.CommonButtonTextColor;
   //         cancel.FontAttributes = Theme.Current.LoginButtonFontAttributes;
   //         cancel.Text = TR.Tr("cancel");
			//cancel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
			//cancel.Clicked += (sender, e) => Pages.CloseModalPage(Navigation);
            //buttonsLayout.Children.Add(cancel);

            var next = new Button
            {
				CornerRadius = Theme.Current.ButtonRadius,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Theme.Current.CommonButtonBackground,
                TextColor = Theme.Current.CommonButtonTextColor,
                FontAttributes = Theme.Current.LoginButtonFontAttributes,
                FontFamily = NSWFontsController.CurrentBoldTypeface,
                FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
                Text = TR.Tr("field_save"),
                AutomationId = AutomationIdConsts.SAVE_BUTTON_ID
            };
            next.SetBinding(Button.CommandProperty, "SaveFieldCommand");

			if (isEdit) {
				next.SetBinding(IsVisibleProperty, "NotViewMode");
			}

			buttonsLayout.Children.Add(next);

            mainStackLayout.Children.Add(buttonsLayout);

            var passwordLayout = new StackLayout();
            passwordLayout.SetBinding(IsVisibleProperty, "IsPassword");

            var generateButton = new Button
            {
                Margin = new Thickness(10, 0, 10, 0),
				CornerRadius = Theme.Current.ButtonRadius,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Theme.Current.CommonButtonBackground,
                TextColor = Theme.Current.CommonButtonTextColor,
                FontAttributes = Theme.Current.LoginButtonFontAttributes,
                FontFamily = NSWFontsController.CurrentBoldTypeface,
                FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
                Text = TR.Tr("password_generation")
            };
            generateButton.SetBinding(Button.CommandProperty, "PasswordGenerateCommand");
            passwordLayout.Children.Add(generateButton);

            // Upper case
            var upperCaseLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0),
            };

            var upperCaseSwitch = new Switch();
            upperCaseSwitch.VerticalOptions = LayoutOptions.CenterAndExpand;
            upperCaseSwitch.SetBinding(Switch.IsToggledProperty, "IsUpperCase");

            var upperCaseLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Theme.Current.NormalTextColor,
                Text = TR.Tr("upper_case_letters"),
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};

            upperCaseLayout.Children.Add(upperCaseSwitch);
            upperCaseLayout.Children.Add(upperCaseLabel);

            passwordLayout.Children.Add(upperCaseLayout);

            // Lower case
            var lowerCaseLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0),
            };

            var lowerCaseSwitch = new Switch();
            lowerCaseSwitch.VerticalOptions = LayoutOptions.CenterAndExpand;
            lowerCaseSwitch.SetBinding(Switch.IsToggledProperty, "IsLowerCase");

            var lowerCaseLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Theme.Current.NormalTextColor,
                Text = TR.Tr("lower_case_letters"),
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};

            lowerCaseLayout.Children.Add(lowerCaseSwitch);
            lowerCaseLayout.Children.Add(lowerCaseLabel);

            passwordLayout.Children.Add(lowerCaseLayout);

            // Digits case
            var digitsCaseLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0),
            };

            var digitsCaseSwitch = new Switch();
            digitsCaseSwitch.VerticalOptions = LayoutOptions.CenterAndExpand;
            digitsCaseSwitch.SetBinding(Switch.IsToggledProperty, "IsDigits");

            var digitsCaseLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Theme.Current.NormalTextColor,
                Text = TR.Tr("digits"),
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};

            digitsCaseLayout.Children.Add(digitsCaseSwitch);
            digitsCaseLayout.Children.Add(digitsCaseLabel);

            passwordLayout.Children.Add(digitsCaseLayout);

            // Special symbols
            var specialCaseLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0),
            };

            var specialCaseSwitch = new Switch();
            specialCaseSwitch.VerticalOptions = LayoutOptions.CenterAndExpand;
            specialCaseSwitch.SetBinding(Switch.IsToggledProperty, "IsSpecial");

            var specialCaseLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Theme.Current.NormalTextColor,
                Text = TR.Tr("special_symbols"),
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};

            specialCaseLayout.Children.Add(specialCaseSwitch);
            specialCaseLayout.Children.Add(specialCaseLabel);

            passwordLayout.Children.Add(specialCaseLayout);

            // Password length
            var passLenCaseLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(15, 0),
            };

            var passwordLenthTip = new Label();
            passwordLenthTip.VerticalOptions = LayoutOptions.CenterAndExpand;
            passwordLenthTip.Text = TR.Tr("password_length");
            passwordLenthTip.TextColor = Theme.Current.NormalTextColor;
			passwordLenthTip.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			passwordLenthTip.FontFamily = NSWFontsController.CurrentTypeface;
			passLenCaseLayout.Children.Add(passwordLenthTip);

			var minusButton = new CachedImage {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ManageMinus)),
				HeightRequest = 30,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var minusGesture = new TapGestureRecognizer();
            minusGesture.SetBinding(TapGestureRecognizer.CommandProperty, "MinusCommand");
            minusButton.GestureRecognizers.Add(minusGesture);

            passLenCaseLayout.Children.Add(minusButton);

            var passLenLabel = new Label();
            passLenLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            passLenLabel.TextColor = Theme.Current.NormalTextColor;
			passLenLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			passLenLabel.FontFamily = NSWFontsController.CurrentTypeface;
			passLenLabel.SetBinding(Label.TextProperty, "PasswordLengthText");
            passLenCaseLayout.Children.Add(passLenLabel);

			var plusButton = new CachedImage {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ManagePlus)),
				HeightRequest = 30,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var plusGesture = new TapGestureRecognizer();
            plusGesture.SetBinding(TapGestureRecognizer.CommandProperty, "PlusCommand");
            plusButton.GestureRecognizers.Add(plusGesture);

            passLenCaseLayout.Children.Add(plusButton);

            passwordLayout.Children.Add(passLenCaseLayout);

            var cleverGenerateButton = new Button
            {
                Margin = new Thickness(10, 0),
				CornerRadius = Theme.Current.ButtonRadius,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Theme.Current.CommonButtonBackground,
                TextColor = Theme.Current.CommonButtonTextColor,
                FontAttributes = Theme.Current.LoginButtonFontAttributes,
                FontFamily = NSWFontsController.CurrentBoldTypeface,
                FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
                Text = TR.Tr("clever_generation")
            };
            cleverGenerateButton.SetBinding(IsVisibleProperty, "NotViewMode");
            cleverGenerateButton.SetBinding(Button.CommandProperty, "CleverGenerateCommand");
            passwordLayout.Children.Add(cleverGenerateButton);

            mainStackLayout.Children.Add(passwordLayout);


            var fieldDescription = new Label
            {
                Margin = new Thickness(10, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
                TextColor = Theme.Current.NormalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
				FontFamily = NSWFontsController.CurrentTypeface
			};
            fieldDescription.SetBinding(Label.TextProperty, "FieldDescription");
            fieldDescription.SetBinding(IsVisibleProperty, "NotViewMode");
            mainStackLayout.Children.Add(fieldDescription);

            pageVM.ClosePopupCommandCallback = ClosePopup;
            pageVM.ShowPopupCommandCallback = ShowPopup;

			var mainScrollView = new ScrollView { Content = mainStackLayout };
			var bottomMenuLayout = GetBottomMenu();
			var globalView = new StackLayout {
				Spacing = 0,
				Children = {
					mainScrollView,
					bottomMenuLayout
				}
			};

			choosePopup = popupWithList("SelectedNSWLabel", "LabelsList", "Icon", "Name",
														TR.Tr("create_new_label"), "Icons.icon_labelplus_huge.png", "CreateLabelCommand");

			choosePopup.IsVisible = false;

			var gridView = new Grid {
				Children = {
					globalView,
					choosePopup
				}
			};

			Content = gridView;
        }

		StackLayout popupWithList(string selectedProperty, string itemSourceProperty, string iconProperty, string nameProperty, string headerText = null, string headerIcon = null, string headerCommandProperty = null)
		{
			var listLayout = new StackLayout { Spacing = 0 };

			if (headerText != null) {
				var headerLayout = new StackLayout();
				headerLayout.BackgroundColor = Color.White;
				headerLayout.Padding = new Thickness(20);
				headerLayout.Orientation = StackOrientation.Horizontal;

				if (headerIcon != null) {
					var headerImage = new CachedImage {
						HeightRequest = 40,
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Source = ImageSource.FromStream(() => NSWRes.GetImage(headerIcon)),
						Style = ImageProperties.DefaultCachedImageStyle
					};
					headerLayout.Children.Add(headerImage);
				}

				var headerLabel = new Label();
				headerLabel.FontFamily = NSWFontsController.CurrentTypeface;
				headerLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
				headerLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
				headerLabel.Text = headerText;
				headerLayout.Children.Add(headerLabel);

				if (headerCommandProperty != null) {
					var headerTap = new TapGestureRecognizer();
					headerTap.SetBinding(TapGestureRecognizer.CommandProperty, headerCommandProperty);
					headerLayout.GestureRecognizers.Add(headerTap);
				}

				listLayout.Children.Add(headerLayout);

				var hr = new BoxView();
				hr.Color = Color.Gray;
				hr.HeightRequest = 1;
				hr.HorizontalOptions = LayoutOptions.FillAndExpand;
				listLayout.Children.Add(hr);
			}

			var listView = new ListView();
			listView.BackgroundColor = Color.White;
			listView.HasUnevenRows = true;
			listView.SetBinding(ListView.SelectedItemProperty, selectedProperty);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, itemSourceProperty);

			listView.ItemTemplate = new DataTemplate(() => {
				var stackLayout = new StackLayout();
				stackLayout.BackgroundColor = Color.White;
				stackLayout.Padding = new Thickness(20);
				stackLayout.Spacing = 20;
				stackLayout.Orientation = StackOrientation.Horizontal;
				stackLayout.VerticalOptions = LayoutOptions.CenterAndExpand;

				var icon = new CachedImage {
					HeightRequest = 40,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Style = ImageProperties.DefaultCachedImageStyle
				};
				icon.SetBinding(CachedImage.SourceProperty, iconProperty);
				stackLayout.Children.Add(icon);

				var typeName = new Label();
				typeName.FontFamily = NSWFontsController.CurrentTypeface;
				typeName.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
				typeName.VerticalOptions = LayoutOptions.CenterAndExpand;
				typeName.SetBinding(Label.TextProperty, nameProperty);
				typeName.SetBinding(AutomationIdBinding.AutomationIdProperty, "AutomationId");
				stackLayout.Children.Add(typeName);

				return new ExtendedViewCell { SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor, View = stackLayout };
			});

			var cancel = new Button();
			cancel.CornerRadius = 0;
			cancel.HorizontalOptions = LayoutOptions.FillAndExpand;
			cancel.BackgroundColor = Theme.Current.CommonButtonBackground;
			cancel.TextColor = Theme.Current.CommonButtonTextColor;
			cancel.FontAttributes = Theme.Current.LoginButtonFontAttributes;
			cancel.FontFamily = NSWFontsController.CurrentBoldTypeface;
			cancel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
			cancel.Text = TR.Cancel;
			cancel.SetBinding(Button.CommandProperty, "ClosePopupCommand");

			listLayout.Children.Add(listView);
			listLayout.Children.Add(cancel);

			return listLayout;
		}





        void ShowPopup()
        {
			choosePopup.IsVisible = true;
        }

        void ClosePopup()
        {
			choosePopup.IsVisible = false;
		}
	}
}