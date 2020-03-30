using NSWallet.NetStandard.Helpers.Fonts;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers.UI.NavigationHeader
{
	public static class UINavigationHeader
	{
		public static void SetCommonTitleView(BindableObject bindableObject, string titleName, string titleNameProperty = null)
		{
			NavigationPage.SetTitleView(
				bindableObject,
				getCommonTitleView(titleName, titleNameProperty)
			);
		}

		static View getCommonTitleView(string titleName, string titleNameProperty = null)
		{
			var titleLabel = new Label {
				TextColor = Theme.Current.AppHeaderTextColor,
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				Text = titleName
			};

			if (titleNameProperty != null) {
				titleLabel.SetBinding(Label.TextProperty, titleNameProperty);
			}

			var contentView = new ContentView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Content = titleLabel
			};

			if (Device.RuntimePlatform == Device.Android) {
				contentView.HorizontalOptions = LayoutOptions.StartAndExpand;
			}

			return contentView;
		}
	}
}