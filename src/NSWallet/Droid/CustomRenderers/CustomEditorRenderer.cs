using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NSWallet;
using NSWallet.Droid;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace NSWallet.Droid
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
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