using System;
using System.Threading.Tasks;
using NSWallet.Controls.EntryPopup;
using NSWallet.NetStandard.Helpers.UI.Popups.Buttons;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers.UI.Popups.Pages.Icons
{
	public static class PopupPagesUIController
	{
		public static void LaunchIconsFilterPopup(Action<Task<string>> action)
		{
			PopupUIController.LaunchSheet(TR.Tr("icons_filter"), TR.Cancel, null, ButtonsPopupUIController.GetIconFilterButtons(), action);
		}

		public static void LaunchIconIsUsedMessage()
		{
			PopupUIController.LaunchMessageBox(TR.Tr("alert"), TR.Tr("icon_is_used_alert"), TR.OK);
		}

		public static void LaunchDeleteIconAcceptMessage(Action<Task<bool>> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("alert"), TR.Tr("delete_icon_accept"), TR.Yes, TR.No, action);
		}

		public static void LaunchDeleteIconSuccessMessage(Action<Task> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("success"), TR.Tr("delete_icon_name_success"), TR.OK, action);
		}

		public static void LaunchDeleteIconFailureMessage()
		{
			PopupUIController.LaunchMessageBox(TR.Tr("failure"), TR.Tr("delete_icon_name_failure"), TR.OK);
		}

		public static void LaunchChangeIconNameSuccessMessage(Action<Task> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("success"), TR.Tr("change_icon_name_success"), TR.OK, action);
		}

		public static void LaunchChangeIconNameFailureMessage()
		{
			PopupUIController.LaunchMessageBox(TR.Tr("failure"), TR.Tr("change_icon_name_failure"), TR.OK);
		}

		public static void LaunchChangeIconNameEntryPopup(EventHandler<EntryPopupClosedArgs> eventHandler)
		{
			PopupUIController.LaunchEntryPopup(TR.Tr("change_icon_name_description"), eventHandler);
		}

		public static void LaunchChangeGroupPopup(Action<Task<string>> action)
		{
			try {
				PopupUIController.LaunchSheet(TR.Tr("icon_import_choose_group"), TR.Cancel, null, IconGroups.GetIconGroupsForPopup(), action);
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(LaunchChangeGroupPopup), nameof(PopupPagesUIController));
			}
		}

		public static void LaunchCreateGroupEntryPopup(EventHandler<EntryPopupClosedArgs> eventHandler)
		{
			PopupUIController.LaunchEntryPopup(TR.Tr("create_icon_group_enter_name"), eventHandler);
		}

		public static void LaunchCreateGroupSuccessMessage(Action<Task> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("success"), TR.Tr("create_group_success"), TR.OK, action);
		}

		public static void LaunchCreateGroupFailureMessage()
		{
			PopupUIController.LaunchMessageBox(TR.Tr("failure"), TR.Tr("create_group_failure"), TR.OK);
		}

		public static void LaunchChangeGroupOnResultMessage(bool result, Action<Task> action)
		{
			if (result) {
				PopupUIController.LaunchMessageBox(TR.Tr("success"), TR.Tr("icon_change_group_success"), TR.OK, action);
			} else {
				PopupUIController.LaunchMessageBox(TR.Tr("failure"), TR.Tr("icon_change_group_failure"), TR.OK);
			}
		}

		public static void LaunchChangeShapeSuccessMessage(Action<Task> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("success"), TR.Tr("icon_change_shape_success"), TR.OK, action);
		}

		public static void LaunchChangeShapeFailureMessage()
		{
			PopupUIController.LaunchMessageBox(TR.Tr("failure"), TR.Tr("icon_change_shape_failure"), TR.OK);
		}
	}
}