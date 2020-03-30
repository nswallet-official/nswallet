using System;
using FFImageLoading.Forms;
using ImageCircle.Forms.Plugin.Abstractions;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using Xamarin.Forms;

namespace NSWallet
{
	public class MainScreenViewCell : ExtendedViewCell
    {
        public MainScreenViewCell()
        {
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var mainStackLayout = new StackLayout();
            mainStackLayout.Spacing = 0;
            //mainStackLayout.BackgroundColor = Theme.Current.ListBackgroundColor;

            var bodyStackLayout = new StackLayout();
            bodyStackLayout.Orientation = StackOrientation.Horizontal;
            bodyStackLayout.VerticalOptions = LayoutOptions.Center;
            bodyStackLayout.Padding = new Thickness(5);

            var titleStackLayout = new StackLayout();

			var iconImage = new CachedImage {
				HeightRequest = Theme.Current.MainPageListIconHeight,
				WidthRequest = Theme.Current.MainPageListIconHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var iconCircleImage = new CircleImage {
				HeightRequest = Theme.Current.MainPageListIconHeight,
				WidthRequest = Theme.Current.MainPageListIconHeight,
				BorderThickness = Theme.Current.CommonIconBorderWidth,
				BorderColor = Theme.Current.CommonIconBorderColor,
				Aspect = Aspect.AspectFill,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var titleLabel = new Label();
			titleLabel.FontFamily = NSWFontsController.CurrentTypeface;
			titleLabel.LineBreakMode = LineBreakMode.TailTruncation;
            titleLabel.TextColor = Theme.Current.ListTextColor;
			titleLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));

            var description = new Label();
			description.FontFamily = NSWFontsController.CurrentTypeface;
			description.FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label));
            description.TextColor = Theme.Current.ListSecondaryTextColor;

            var separator = new BoxView();
            separator.HeightRequest = 1;
            separator.BackgroundColor = Theme.Current.ListSeparatorColor;
            separator.VerticalOptions = LayoutOptions.EndAndExpand;

			iconImage.SetBinding(VisualElement.IsVisibleProperty, "IsNotImageCircle");
			iconImage.SetBinding(CachedImage.SourceProperty, "Icon");
			iconCircleImage.SetBinding(VisualElement.IsVisibleProperty, "IsImageCircle");
			iconCircleImage.SetBinding(Image.SourceProperty, "Icon");
			titleLabel.SetBinding(Label.TextProperty, "Name");
            titleLabel.SetBinding(AutomationIdBinding.AutomationIdProperty, "Name");
            description.SetBinding(Label.TextProperty, "LowAdditionalRow");

            titleStackLayout.Children.Add(titleLabel);
            titleStackLayout.Children.Add(description);

			var copyButton = new CachedImage {
				Margin = new Thickness(0, 0, 5, 0),
				HeightRequest = 30,
				Source = ImageSource.FromFile("popup_copy"),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand
			};

			var copyLayout = new StackLayout {
				WidthRequest = 60,
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Children = {
					copyButton
				}
			};

			copyLayout.SetBinding(VisualElement.IsVisibleProperty, "IsField");

			var copyGesture = new TapGestureRecognizer();
			copyGesture.SetBinding(TapGestureRecognizer.CommandProperty, "CopyValueCommand");
			copyLayout.GestureRecognizers.Add(copyGesture);

            bodyStackLayout.Children.Add(iconImage);
			bodyStackLayout.Children.Add(iconCircleImage);
			bodyStackLayout.Children.Add(titleStackLayout);
			bodyStackLayout.Children.Add(copyLayout);

            mainStackLayout.Children.Add(bodyStackLayout);
            mainStackLayout.Children.Add(separator);

            View = mainStackLayout;
        }
    }
}