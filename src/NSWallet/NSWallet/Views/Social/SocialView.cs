using FFImageLoading.Forms;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public static class SocialView
    {
        public static Layout<View> GetButtons()
        {
            var mainLayout = new StackLayout();
            mainLayout.SetBinding(VisualElement.IsVisibleProperty, "IsVisibleProp");
            mainLayout.Orientation = StackOrientation.Horizontal;
            mainLayout.BindingContext = new SocialViewModel();

			var share = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ShareIcon)),
				Margin = Theme.Current.SocialIconMargin,
				HeightRequest = Theme.Current.SocialIconHeight,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var shareTap = new TapGestureRecognizer();
            shareTap.SetBinding(TapGestureRecognizer.CommandProperty, "ShareCommand");
            share.GestureRecognizers.Add(shareTap);

            mainLayout.Children.Add(share);

			var twitter = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.TwitterIcon)),
				Margin = Theme.Current.SocialIconMargin,
				HeightRequest = Theme.Current.SocialIconHeight,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var twitterTap = new TapGestureRecognizer();
            twitterTap.SetBinding(TapGestureRecognizer.CommandProperty, "TwitterCommand");
            twitter.GestureRecognizers.Add(twitterTap);

            mainLayout.Children.Add(twitter);

            return mainLayout;
        }
    }
}