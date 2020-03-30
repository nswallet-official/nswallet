using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSWallet.Controls.EntryPopup;
using NSWallet.Model;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers.UI.Popups
{
	public class PopupUIController
	{
		static Page page {
			get {
				return Application.Current.MainPage;
			}
		}

		public static void LaunchSheet(string name, string cancel, string destruction, string[] buttons, Action<Task<string>> action = null)
		{
			page.DisplayActionSheet(name, cancel, destruction, buttons).ContinueWith(action);
		}

		public static void LaunchMessageBox(string name, string message, string cancel, Action<Task> action = null)
		{
			if (action != null) {
				page.DisplayAlert(name, message, cancel).ContinueWith(action);
			} else {
				page.DisplayAlert(name, message, cancel);
			}
		}

		public static void LaunchMessageBox(string name, string message, string accept, string cancel, Action<Task<bool>> action = null)
		{
			if (action != null) {
				page.DisplayAlert(name, message, accept, cancel).ContinueWith(action);
			} else {
				page.DisplayAlert(name, message, accept, cancel);
			}
		}

		public static void LaunchEntryPopup(string description, EventHandler<EntryPopupClosedArgs> action)
		{
			var popup = new EntryPopup(description);
			popup.PopupClosed += action;
			popup.Show();
		}
	}
}