using Android.App;
using Android.Widget;
using NSWallet.Controls.EntryPopup;
using NSWallet.Shared;
using Xamarin.Forms;
using System;

[assembly: Dependency(typeof(NSWallet.Droid.EntryPopupDroid))]
namespace NSWallet.Droid
{
    public class EntryPopupDroid : IEntryPopup
    {
        public void ShowPopup(EntryPopup popup)
        {
            var alert = new AlertDialog.Builder(Forms.Context);

            var edit = new EditText(Forms.Context) { Text = popup.Text };
            if (popup.Secured)
            {
                edit.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationPassword;
            }
            alert.SetView(edit);

            alert.SetTitle(popup.Title);

            alert.SetPositiveButton(TR.Tr("ok"), (senderAlert, args) =>
            {
                popup.OnPopupClosed(new EntryPopupClosedArgs
                {
                    OkClicked = true,
                    Text = edit.Text
                });
            });

            alert.SetNegativeButton(TR.Tr("cancel"), (senderAlert, args) =>
            {
                popup.OnPopupClosed(new EntryPopupClosedArgs
                {
                    OkClicked = false,
                    Text = edit.Text
                });
            });
            alert.Show();
        }
    }
}