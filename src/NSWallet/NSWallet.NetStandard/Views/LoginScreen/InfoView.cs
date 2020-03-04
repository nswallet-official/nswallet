using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public static class InfoView
	{
		public static StackLayout GetContent()
		{
			var infoLayout = new StackLayout {
				VerticalOptions = LayoutOptions.EndAndExpand,
				Orientation = StackOrientation.Vertical,
				Padding = 20
			};

			
			var infoLabel = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Large, typeof(Label)),
				TextColor = Theme.Current.ListTextColor,
				Text = TR.Tr("info4users"),
				Margin = new Thickness(20),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
		
			};
			//infoLayout.Children.Add(infoLabel);

			var githubButton = new Button {
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				CornerRadius = Theme.Current.ButtonRadius,
				BackgroundColor = Color.Black,
				TextColor = Color.WhiteSmoke,
				FontAttributes = Theme.Current.LoginButtonFontAttributes,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				Text = TR.Tr("github"),
			};
			githubButton.SetBinding(Button.CommandProperty, "GitHubCommand");

			var patreonButton = new Button {
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				CornerRadius = Theme.Current.ButtonRadius,
				BackgroundColor = Color.FromHex("f96854"),
				TextColor = Color.White,
				FontAttributes = Theme.Current.LoginButtonFontAttributes,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				Text = TR.Tr("become_patron"),
			};
			patreonButton.SetBinding(Button.CommandProperty, "PatreonCommand");

			//infoLayout.Children.Add(githubButton);
			infoLayout.Children.Add(patreonButton);

			return infoLayout;
		}
	}
}
