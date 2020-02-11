using System.Linq;
using System.Threading.Tasks;
using NSWallet.Controls.EntryPopup;
using NSWallet.NetStandard.Helpers.UI.Popups.Pages.Icons;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers
{
	public static class ImportIconManager
	{
		public delegate void AddedSuccessfully();
		public static event AddedSuccessfully Imported;

		public static void BeginImportingIcon(byte[] imageBytes)
		{
			if (imageBytes != null) {
				openEntryPopup(imageBytes);
			}
		}

		static void openEntryPopup(byte[] imageBytes)
		{
			var popup = new EntryPopup(TR.Tr("icon_import_enter_name"), null, false);
			popup.PopupClosed += (o, closedArgs) => {
				if (closedArgs.OkClicked) {
					openActionSheetPopup(imageBytes, closedArgs.Text);
				}
			};
			popup.Show();
		}

		static void openActionSheetPopup(byte[] imageBytes, string name)
		{
			Application.Current.MainPage.DisplayActionSheet(TR.Tr("icon_import_choose_group"), TR.Cancel, null, IconGroups.GetIconGroupsForPopup()).ContinueWith((x) => {
				if (!string.IsNullOrEmpty(x.Result) && x.Result != TR.Cancel) {
					if (x.Result == TR.Tr("create_icon_group_popup_item")) {
						launchCreateGroupPopup(imageBytes, name);
					} else {
						if (string.IsNullOrEmpty(name)) {
							name = null;
						}
						var groupID = IconGroups.GetIDByCategoryName(x.Result);
						var result = BL.InsertIcon(imageBytes, groupID, null, name);
						launchResultPopup(result, name);
					}
				}
			});
		}

		static void launchCreateGroupPopup(byte[] imageBytes, string name)
		{
			Device.BeginInvokeOnMainThread(() => {
				PopupPagesUIController.LaunchCreateGroupEntryPopup((sender, e) => {
					createGroupOKClicked(e, imageBytes, name);
				});
			});
		}

		static void createGroupOKClicked(EntryPopupClosedArgs e, byte[] imageBytes, string name)
		{
			Device.BeginInvokeOnMainThread(() => {
				if (e.OkClicked) {
					if (!string.IsNullOrEmpty(e.Text)) {
						var groups = BL.GetGroups();
						var lastID = groups.Max(g => g.GroupID);
						var res = BL.InsertGroup(lastID + 1, e.Text);
						if (res) {
							launchSuccessCreateGroupPopup(lastID, imageBytes, name);
						} else {
							PopupPagesUIController.LaunchCreateGroupFailureMessage();
						}
					}
				}
			});
		}

		static void launchSuccessCreateGroupPopup(int lastID, byte[] imageBytes, string name)
		{
			Device.BeginInvokeOnMainThread(() => {
				PopupPagesUIController.LaunchCreateGroupSuccessMessage(null);
				var groupID = lastID + 1;
				var r = BL.InsertIcon(imageBytes, groupID, null, name);
				launchResultPopup(r, name);
			});
		}

		static void launchResultPopup(bool result, string name)
		{
			Device.BeginInvokeOnMainThread(() => {
				if (result) {
					var success = string.Format(TR.Tr("icon_import_success"), name);
					Application.Current.MainPage.DisplayAlert(TR.Tr("success"), success, TR.OK).ContinueWith(x => {
						if (Imported != null) {
							Imported.Invoke();
						}
					});
				} else {
					var failure = string.Format(TR.Tr("icon_import_failure"), name);
					Application.Current.MainPage.DisplayAlert(TR.Tr("failure"), failure, TR.OK);
				}
			});
		}
	}
}