using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NSWallet;
using NSWallet.Droid;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(RectangularEntry), typeof(RectangularEntryRenderer))]
namespace NSWallet.Droid
{
    public class RectangularEntryRenderer : EntryRenderer 
    {
        public RectangularEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.White);
                Control.Gravity = GravityFlags.CenterHorizontal;
            }
        }
    }
}

