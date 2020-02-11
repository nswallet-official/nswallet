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
            mainLayout.SetBinding(StackLayout.IsVisibleProperty, "IsVisibleProp");
            mainLayout.Orientation = StackOrientation.Horizontal;
            mainLayout.BindingContext = new SocialViewModel();

			var facebook = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.FacebookIcon)),
				HeightRequest = Theme.Current.SocialIconHeight,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var facebookTap = new TapGestureRecognizer();
			facebookTap.SetBinding(TapGestureRecognizer.CommandProperty, "FacebookCommand");
			facebook.GestureRecognizers.Add(facebookTap);

			mainLayout.Children.Add(facebook);

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

			var thumbsUp = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ThumbsUpIcon)),
				Margin = Theme.Current.SocialIconMargin,
				HeightRequest = Theme.Current.SocialIconHeight,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			var thumbsUpTap = new TapGestureRecognizer();
            thumbsUpTap.SetBinding(TapGestureRecognizer.CommandProperty, "ThumbsUpCommand");
            thumbsUp.GestureRecognizers.Add(thumbsUpTap);

            mainLayout.Children.Add(thumbsUp);

            return mainLayout;
        }
    }
}