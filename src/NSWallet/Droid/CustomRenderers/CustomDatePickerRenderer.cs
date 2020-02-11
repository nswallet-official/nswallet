using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NSWallet;
using NSWallet.Droid;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace NSWallet.Droid
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
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