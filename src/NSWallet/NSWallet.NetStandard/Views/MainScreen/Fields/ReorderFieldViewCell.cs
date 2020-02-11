using FFImageLoading.Forms;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using Xamarin.Forms;

namespace NSWallet
{
	public class ReorderFieldViewCell : ExtendedViewCell
    {
        public ReorderFieldViewCell()
        {
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var mainStackLayout = new StackLayout();
            mainStackLayout.Spacing = 0;
            mainStackLayout.BackgroundColor = Theme.Current.ListBackgroundColor;

            var bodyStackLayout = new StackLayout();
            bodyStackLayout.Orientation = StackOrientation.Horizontal;
            bodyStackLayout.VerticalOptions = LayoutOptions.Center;
            bodyStackLayout.Padding = new Thickness(5);

			var upField = new CachedImage {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = Theme.Current.MainPageListIconHeight,
				Source = Theme.Current.ReorderUpIcon,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var tapUpGesture = new TapGestureRecognizer();
            tapUpGesture.Tapped += (sender, e) =>
            {
                var execButton = new Button();
				execButton.FontFamily = NSWFontsController.CurrentTypeface;
				execButton.IsVisible = false;
				execButton.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
				execButton.SetBinding(Button.CommandProperty, "UpFieldCommand");
                mainStackLayout.Children.Add(execButton);
                execButton.Command.Execute(null);
            };
            upField.GestureRecognizers.Add(tapUpGesture);

            bodyStackLayout.Children.Add(upField);

            var titleStackLayout = new StackLayout();

			var iconImage = new CachedImage {
				HeightRequest = Theme.Current.MainPageListIconHeight,
				WidthRequest = Theme.Current.MainPageListIconHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var titleLabel = new Label();
            titleLabel.LineBreakMode = LineBreakMode.TailTruncation;
            titleLabel.TextColor = Theme.Current.ListTextColor;
			titleLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			titleLabel.FontFamily = NSWFontsController.CurrentTypeface;

			var description = new Label();
            description.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            description.TextColor = Theme.Current.ListSecondaryTextColor;
			description.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
			description.FontFamily = NSWFontsController.CurrentTypeface;

			var separator = new BoxView();
            separator.HeightRequest = 1;
            separator.BackgroundColor = Theme.Current.ListSeparatorColor;
            separator.VerticalOptions = LayoutOptions.EndAndExpand;

			iconImage.SetBinding(CachedImage.SourceProperty, "Icon");
            titleLabel.SetBinding(Label.TextProperty, "Name");
            titleLabel.SetBinding(AutomationIdBinding.AutomationIdProperty, "Name");
            description.SetBinding(Label.TextProperty, "LowAdditionalRow");

            titleStackLayout.Children.Add(titleLabel);
            titleStackLayout.Children.Add(description);

            bodyStackLayout.Children.Add(iconImage);
            bodyStackLayout.Children.Add(titleStackLayout);

			var downField = new CachedImage {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				HeightRequest = Theme.Current.MainPageListIconHeight,
				Source = Theme.Current.ReorderDownIcon
			};

			var tapDownGesture = new TapGestureRecognizer();
            tapDownGesture.Tapped += (sender, e) =>
            {
                var execButton = new Button();
				execButton.FontFamily = NSWFontsController.CurrentTypeface;
				execButton.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
				execButton.IsVisible = false;
                execButton.SetBinding(Button.CommandProperty, "DownFieldCommand");
                mainStackLayout.Children.Add(execButton);
                execButton.Command.Execute(null);
            };
            downField.GestureRecognizers.Add(tapDownGesture);

            bodyStackLayout.Children.Add(downField);

            mainStackLayout.Children.Add(bodyStackLayout);
            mainStackLayout.Children.Add(separator);

            View = mainStackLayout;
        }
    }
}