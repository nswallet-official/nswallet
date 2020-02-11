using NSWallet;
using NSWallet.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RectangularEntry), typeof(RectangularEntryRenderer))]
namespace NSWallet.iOS
{
    public class RectangularEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BackgroundColor = UIColor.White;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}

