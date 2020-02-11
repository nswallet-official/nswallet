using System;
using Android.Content;
using NSWallet;
using NSWallet.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace NSWallet.Droid
{
    public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            var customLabel = e.NewElement as CustomLabel;

            if (Control != null)
            {
                this.Control.SetMaxLines(customLabel.MaxLines);
            }
        }
    }
}