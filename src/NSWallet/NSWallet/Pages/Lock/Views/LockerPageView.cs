using FFImageLoading.Forms;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Pages.Lock.Views
{
	public class LockerPageView : ContentPage
	{
		public LockerPageView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex("#3695FE");

			Content = new CachedImage {
				Margin = new Thickness(40),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile("app_icon_1024")
			};
		}
	}
}