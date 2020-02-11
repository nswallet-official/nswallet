using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NSWallet;
using NSWallet.Droid;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]
namespace NSWallet.Droid
{
    public class CustomTimePickerRenderer : TimePickerRenderer
    {
        public CustomTimePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
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