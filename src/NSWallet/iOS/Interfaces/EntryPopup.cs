using System;
using System.Linq;
using NSWallet.Controls.EntryPopup;
using UIKit;
using Xamarin.Forms;
using NSWallet.Shared;

[assembly: Dependency(typeof(NSWallet.iOS.EntryPopupIOS))]

namespace NSWallet.iOS
{
	public class EntryPopupIOS : IEntryPopup
	{
		public void ShowPopup(EntryPopup popup)
		{
			var alert = new UIAlertView
			{
				Title = popup.Title,
				Message = popup.Text,
                AlertViewStyle = popup.Secured ? UIAlertViewStyle.SecureTextInput : UIAlertViewStyle.PlainTextInput
			};
            alert.AddButton(TR.Tr("ok"));
            alert.AddButton(TR.Tr("cancel"));
			

			alert.Clicked += (s, args) =>
			{
				popup.OnPopupClosed(new EntryPopupClosedArgs
				{
                    OkClicked = !Convert.ToBoolean(args.ButtonIndex),
					Text = alert.GetTextField(0).Text
				});
			};
			alert.Show();
		}
	}
}