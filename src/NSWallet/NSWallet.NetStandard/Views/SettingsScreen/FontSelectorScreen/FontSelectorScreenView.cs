using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.NetStandard.Views.SettingsScreen.FontSelectorScreen.ViewCells;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class FontSelectorScreenView : ContentPage
	{
		public FontSelectorScreenView()
		{
			var vm = new FontSelectorScreenViewModel();
			BindingContext = vm;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("settings_select_font"));

			var fontsListView = new ListView {
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate(typeof(FontSelectorViewCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Theme.Current.ListBackgroundColor
			};

			fontsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Fonts");
			fontsListView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");

			var cancelButton = new Button {
				CornerRadius = 0,
				Text = TR.Cancel,
				VerticalOptions = LayoutOptions.End,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				TextColor = Theme.Current.CommonButtonTextColor,
				FontAttributes = Theme.Current.LoginButtonFontAttributes,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button))
			};

			cancelButton.Clicked += (sender, e)
				=> Pages.Settings();

			Content = new StackLayout {
				Spacing = 0,
				Children = {
					fontsListView,
					cancelButton
				}
			};
		}
	}
}