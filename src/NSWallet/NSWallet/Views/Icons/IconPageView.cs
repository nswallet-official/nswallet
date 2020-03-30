using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.ListView;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.NetStandard.Helpers.UI.Toolbar;
using NSWallet.NetStandard.Views.Icons.ViewCells;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.IconsScreen
{
	public class IconPageView : ContentPage
	{
		public IconPageView()
		{
			var viewModel = new IconScreenViewModel();
			BindingContext = viewModel;

			var toolbarUIController = new ToolbarUIController(ToolbarItems);
			toolbarUIController.InsertFilter();
			toolbarUIController.InsertGalleryPicker();

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_icons"));

			var searchBar = new CustomSearchBar {
				FontFamily = NSWFontsController.CurrentTypeface,
				Placeholder = TR.Tr("search_type_text"),
				BackgroundColor = Theme.Current.AppHeaderBackground,
				CancelButtonColor = Theme.Current.MainSearchCancelButtonColor
			};

			switch (Device.RuntimePlatform) {
				case Device.iOS:
					searchBar.TextColor = Color.Black;
					break;
				case Device.Android:
					searchBar.TextColor = Color.White;
					break;
			}
			searchBar.PlaceholderColor = Color.LightGray;

			switch (Device.RuntimePlatform) {
				case Device.Android:
					searchBar.HeightRequest = 40;
					break;
			}

			searchBar.SetBinding(SearchBar.TextProperty, "SearchText");

			var listView = new ListView {
				HasUnevenRows = true,
				IsGroupingEnabled = true,
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Theme.Current.ListBackgroundColor,
				GroupHeaderTemplate = new DataTemplate(typeof(IPHeaderViewCell)),
				ItemTemplate = new DataTemplate(typeof(IPItemViewCell))
			};

			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Items");
			listView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");
			listView.ItemSelected += ListViewUIController.DisableSelection;

			var mainLayout = new StackLayout {
				Spacing = 0,
				BackgroundColor = Theme.Current.AppBackground,
				Children = {
					searchBar,
					listView
				}
			};

			Content = mainLayout;
		}
	}
}