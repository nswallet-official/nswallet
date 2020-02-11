using System;
using System.Threading.Tasks;
using NSWallet.Shared;

namespace NSWallet.NetStandard.Helpers.UI.Popups.Pages.ExportImport
{
	public class ExportImportPopupUIController
	{
		public static void LaunchAlertPopup(Action<Task<bool>> action)
		{
			PopupUIController.LaunchMessageBox(TR.Tr("alert"), TR.Tr("export_import_alert"), TR.Tr("continue"), TR.Cancel, action);
		}
	}
}