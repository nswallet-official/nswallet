using System;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.Events
{
    public static partial class AppEvents
    {
        /*
        public static void SetChristmas(bool isMainView, Layout<View> layout)
        {
            if (checkChristmas())
            {
                switch (isMainView)
                {
                    case true:
                        setChristmasMainView(layout);
                        break;
                    case false:
                        setChristmasSubscriptionView(layout);
                        break;
                }
            }
        }
        */

        static bool checkChristmas()
        {
            var dateCurrent = DateTime.Now;
            var dateStart = new DateTime(1, 12, 15);
            var dateEnd = new DateTime(1, 1, 7);

            if (dateCurrent.Day >= dateStart.Day && dateCurrent.Month == dateStart.Month)
            {
                return true;
            }

            if (dateCurrent.Day <= dateEnd.Day && dateCurrent.Month <= dateEnd.Month)
            {
                return true;
            }

            return false;
        }

        /*
        static void setChristmasMainView(Layout<View> layout)
        {
            var image = new CachedImage();
            image.HorizontalOptions = LayoutOptions.CenterAndExpand;
            image.HeightRequest = Theme.Current.ChristmasImageHeight;

            if (Premium.PremiumManagement.IsPremium)
            {
                image.Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ChristmasCongratsImage));
               
                var congratsLabel = new Label();
                congratsLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                congratsLabel.Text = TR.Tr("events_xmas_congrats");

                layout.Children.Add(image);
                layout.Children.Add(congratsLabel);
            }
            else
            {
                image.Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ChristmasDiscountImage));

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (sender, e) => Pages.Premium(layout.Navigation);
                image.GestureRecognizers.Add(tapGesture);
                layout.Children.Add(image);
            }
        }
        */

        /*
        static void setChristmasSubscriptionView(Layout<View> layout)
        {
            var label = new Label();
            label.HorizontalOptions = LayoutOptions.CenterAndExpand;
            label.Text = TR.Tr("events_xmas_discount_50");
            layout.Children.Add(label);
        }
        */
    }
}