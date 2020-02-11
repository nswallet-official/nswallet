using NSWallet.Model;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using Xamarin.Forms;

namespace NSWallet
{
	public partial class MainScreenView : ContentPage
	{

		//Grid getSettingsIcon(bool solid, string iconFont, string command, string iconText, object param = null)
		static Grid GetSettingsIcon(bool solid, string iconFont, string iconText, string popupCommandName = "")
		{
			var icon = new Label {
				TextColor = Theme.Current.MainTitleTextColor,
				FontSize = 40,
				FontAttributes = solid ? FontAttributes.Bold : FontAttributes.None,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Text = iconFont
			};

			// FIXME: Workaround for MacOS runtime exception for FontAwesome
			if (Device.RuntimePlatform != Device.macOS) {
				icon.FontFamily = solid ? NSWFontsController.FontAwesomeSolid : NSWFontsController.FontAwesomeRegular;
			}

			if (popupCommandName != "") {
				var popupItem = new PopupItem {
					Action = popupCommandName
				};
				var gesture = new TapGestureRecognizer();
				gesture.SetBinding(TapGestureRecognizer.CommandProperty, "MenuTappedCommand");
				gesture.CommandParameter = popupItem;
				icon.GestureRecognizers.Add(gesture);
			}

			var text = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				TextColor = Theme.Current.MainTitleTextColor,
				Text = iconText
			};

			var layout = new Grid {
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
	}
}
