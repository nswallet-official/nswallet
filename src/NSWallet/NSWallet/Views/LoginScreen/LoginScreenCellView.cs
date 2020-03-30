using System;
using FFImageLoading.Forms;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class LoginScreenCellView : ExtendedViewCell
    {
        public LoginScreenCellView()
        {
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var mainStackLayout = new StackLayout
            {
                Padding = Theme.Current.FeatureCellPadding,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };

            var tick = new CachedImage
            {
                Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.CellTickIcon)),
                HeightRequest = Theme.Current.FeatureCellTickHeight,
				Style = ImageProperties.DefaultCachedImageStyle
			};

            var feature = new Label
            {
                Margin = Theme.Current.FeatureLabelMargin,
                TextColor = Theme.Current.FeatureTextColor,
                LineBreakMode = LineBreakMode.WordWrap,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontFamily = NSWFontsController.CurrentTypeface
			};

            feature.SetBinding(Label.TextProperty, ".");

            mainStackLayout.Children.Add(tick);
            mainStackLayout.Children.Add(feature);

            View = mainStackLayout;
        }
    }
}
