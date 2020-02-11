using System;
using FFImageLoading.Forms;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class LabelScreenCellView : ExtendedViewCell
    {
        public LabelScreenCellView()
        {
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var mainStackLayout = new StackLayout();
            mainStackLayout.VerticalOptions = LayoutOptions.CenterAndExpand;

            var bodyLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5)
            };

            var icon = new CachedImage
            {
                HeightRequest = 50,
                VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};

            var textLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var title = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Theme.Current.LabelTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};
            textLayout.Children.Add(title);

            var details = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Theme.Current.LabelTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};
            textLayout.Children.Add(details);

            var endLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
			
			var systemIcon = new CachedImage
            {
                HeightRequest = 30,
                Source = ImageSource.FromStream(() => NSWRes.GetImage("LabelsScreen.lock_icon.png")),
                VerticalOptions = LayoutOptions.CenterAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			icon.SetBinding(CachedImage.SourceProperty, "Icon");
            title.SetBinding(Label.TextProperty, "Name");
            details.SetBinding(Label.TextProperty, "ValueTypeHumanReadable");

            systemIcon.SetBinding(VisualElement.IsVisibleProperty, "System");

            bodyLayout.Children.Add(icon);

            bodyLayout.Children.Add(textLayout);
            endLayout.Children.Add(systemIcon);
            bodyLayout.Children.Add(endLayout);
            mainStackLayout.Children.Add(bodyLayout);

            var separator = new BoxView();
            separator.HeightRequest = 1;
            separator.BackgroundColor = Theme.Current.ListSeparatorColor;
            separator.VerticalOptions = LayoutOptions.EndAndExpand;
            mainStackLayout.Children.Add(separator);

            View = mainStackLayout;
        }
    }
}
