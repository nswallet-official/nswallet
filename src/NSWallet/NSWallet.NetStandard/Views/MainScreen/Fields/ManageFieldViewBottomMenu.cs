using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public partial class ManageFieldView : ContentPage
	{
		public StackLayout GetBottomMenu()
		{
			var mainPanel = new Grid {
				BackgroundColor = Theme.Current.AppHeaderBackground,
				HeightRequest = 100,
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(120) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			mainPanel.SetBinding(IsVisibleProperty, "IsEdit");

			var backIcon = CreatePanelButton(FontAwesome.Regular.Caret_Square_Left, TR.Tr("back"));

			var backGesture = new TapGestureRecognizer();
			backGesture.Tapped += (sender, e) => Navigation.PopModalAsync();
			backIcon.GestureRecognizers.Add(backGesture);

			mainPanel.Children.Add(backIcon, 0, 0);

			var deleteIcon = CreatePanelButton(FontAwesome.Regular.Trash_Alt, TR.Tr("popupmenu_delete"));

			var deleteGesture = new TapGestureRecognizer();
			deleteGesture.SetBinding(TapGestureRecognizer.CommandProperty, "DeleteFieldCommand");
			deleteIcon.GestureRecognizers.Add(deleteGesture);

			mainPanel.Children.Add(deleteIcon, 1, 0);

			var copyIcon = CreatePanelButton(FontAwesome.Regular.Copy, TR.Tr("popupmenu_copylocally"));

			var copyLocallyGesture = new TapGestureRecognizer();
			copyLocallyGesture.SetBinding(TapGestureRecognizer.CommandProperty, "CopyLocallyCommand");
			copyIcon.GestureRecognizers.Add(copyLocallyGesture);

			mainPanel.Children.Add(copyIcon, 2, 0);


			var editIcon = CreatePanelButton(FontAwesome.Regular.Edit, TR.Tr("popupmenu_change"));

			var editGesture = new TapGestureRecognizer();
			editGesture.SetBinding(TapGestureRecognizer.CommandProperty, "EditCommand");
			editIcon.GestureRecognizers.Add(editGesture);

			mainPanel.Children.Add(editIcon, 3, 0);

			var dialButton = CustomActionButton(
				FontAwesome.Regular.Phone,
				TR.Tr("field_func_open_phone_dialer"),
				"DialCommand");

			dialButton.VerticalOptions = LayoutOptions.EndAndExpand;
			dialButton.SetBinding(IsVisibleProperty, "IsPhone");

			var mailButton = CustomActionButton(
				FontAwesome.Regular.Envelope,
				TR.Tr("field_func_open_mail"),
				"MailCommand");

			mailButton.VerticalOptions = LayoutOptions.EndAndExpand;
			mailButton.SetBinding(IsVisibleProperty, "IsMail");

			var linkButton = CustomActionButton(
				FontAwesome.Regular.External_Link_Alt,
				TR.Tr("field_func_open_in_browser"),
				"LinkCommand");

			linkButton.VerticalOptions = LayoutOptions.EndAndExpand;
			linkButton.SetBinding(IsVisibleProperty, "IsLink");

			var bottomMenuLayout = new StackLayout {
				Spacing = 0,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Children = {
							dialButton,
							mailButton,
							linkButton,
							mainPanel
						}
			};
			return bottomMenuLayout;
		}

		static Grid CreatePanelButton(string btnIcon, string btnText)
		{
			var icon = new Label {
				FontFamily = NSWFontsController.FontAwesomeRegular,
				TextColor = Theme.Current.MainTitleTextColor,
				FontSize = 40,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = btnIcon
			};

			var text = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				TextColor = Theme.Current.MainTitleTextColor,
				Text = btnText
			};

			var layout = new Grid {
				Margin = new Thickness(0, 0, 0, 0),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(50) },
					new RowDefinition { Height = new GridLength(50) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			layout.Children.Add(icon, 0, 0);
			layout.Children.Add(text, 0, 1);

			return layout;
		}

		static StackLayout CustomActionButton(string icon, string text, string command)
		{
			var iconLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Theme.Current.CommonButtonTextColor,
				FontAttributes = FontAttributes.Bold,
				Text = icon,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button))
			};

			// FIXME: Workaround for MacOS runtime exception for FontAwesome
			if (Device.RuntimePlatform != Device.macOS) {
				iconLabel.FontFamily = NSWFontsController.FontAwesomeSolid;
			}

			var textLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Theme.Current.CommonButtonTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface,
				Text = text
			};

			var layout = new StackLayout {
				Padding = Theme.Current.EntryMargin,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				HeightRequest = 50,
				Children = {
					new StackLayout {
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Orientation = StackOrientation.Horizontal,
						Children = {
							iconLabel, textLabel
						}
					}
				}
			};
			var gesture = new TapGestureRecognizer();
			gesture.SetBinding(TapGestureRecognizer.CommandProperty, command);
			layout.GestureRecognizers.Add(gesture);

			return layout;
		}
	}
}
