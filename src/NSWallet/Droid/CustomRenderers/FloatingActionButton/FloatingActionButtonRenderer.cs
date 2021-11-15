using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using NSWallet;
using NSWallet.Droid;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FAB = Google.Android.Material.FloatingActionButton.FloatingActionButton;

[assembly: ExportRenderer(typeof(FloatingActionButton), typeof(FloatingActionButtonRenderer))]
namespace NSWallet.Droid
{
    
    public class FloatingActionButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<FloatingActionButton, FAB>
    {
        public FloatingActionButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                ViewGroup.SetClipChildren(false);
                ViewGroup.SetClipToPadding(false);
            }

            if (e.NewElement == null)
                return;

            var fab = new FAB(Context);

            // set the bg

            try // Some droids do not support setting background, so do it carefully
            {
                fab.BackgroundTintList = ColorStateList.ValueOf(Element.ButtonColor.ToAndroid());
            }
            catch(Exception ex)
            {
				AppLogs.Log(ex.Message, nameof(OnElementChanged), nameof(FloatingActionButtonRenderer));
                var fabControl = e.NewElement as FloatingActionButton;
                if (fabControl != null)
                    fabControl.ErrorStatus = true;
            }


            // set the icon
            var elementImage = Element.Image;
            var imageFile = elementImage?.File;

            if ((Context.Resources != null) && (imageFile != null))
            {
                fab.SetImageDrawable(Context.GetDrawable(imageFile));
            }

            fab.Click += Fab_Click;
            SetNativeControl(fab);
        }
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            Control.BringToFront();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                var fab = (FAB)Control;
                if (e.PropertyName == nameof(Element.ButtonColor))
                {
                    fab.BackgroundTintList = ColorStateList.ValueOf(Element.ButtonColor.ToAndroid());
                }
                if (e.PropertyName == nameof(Element.Image))
                {
                    var elementImage = Element.Image;
                    var imageFile = elementImage?.File;

                    if (imageFile != null)
                    {
                        fab.SetImageDrawable(Context.GetDrawable(imageFile));
                    }
                }
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(OnElementPropertyChanged), nameof(FloatingActionButtonRenderer));
			}
            base.OnElementPropertyChanged(sender, e);

        }

        private void Fab_Click(object sender, EventArgs e)
        {
            // proxy the click to the element
            ((IButtonController)Element).SendClicked();
        }
    }


}