using NSWallet;
using NSWallet.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace NSWallet.iOS
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            var customLabel = e.NewElement as CustomLabel;

            if (Control != null)
            {
                this.Control.Lines = customLabel.MaxLines;
            }
        }
    }
}