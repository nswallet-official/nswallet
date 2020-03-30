using System;
using NSWallet.NetStandard.Helpers.UI.Popups.Pages.ExportImport;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class ExportImportScreenViewModel : NSWallet.ViewModel
	{
		public Action ShowImportHelpCallback { get; set; }
		// Generate PDF
		Command generatePDFCommand;
		public Command GeneratePDFCommand {
			get {
				return generatePDFCommand ?? (generatePDFCommand = new Command(ExecuteGeneratePDFCommand));
			}
		}

		protected void ExecuteGeneratePDFCommand()
		{
			ExportImportPopupUIController.LaunchAlertPopup((x) => {
				if (x.Result) {
					Device.BeginInvokeOnMainThread(DependencyService.Get<IExport>().GeneratePDF);
				}
			});
		}

		// Generate TEXT
		Command generateTXTCommand;
		public Command GenerateTXTCommand {
			get {
				return generateTXTCommand ?? (generateTXTCommand = new Command(ExecuteGenerateTXTCommand));
			}
		}

		protected void ExecuteGenerateTXTCommand()
		{
			ExportImportPopupUIController.LaunchAlertPopup((x) => {
				if (x.Result) {
					Device.BeginInvokeOnMainThread(DependencyService.Get<IExport>().GenerateTXT);
				}
			});
		}

		// Show screen
		Command importBackupCommand;
		public Command ImportBackupCommand {
			get {
				return importBackupCommand ?? (importBackupCommand = new Command(ExecuteImportBackupCommand));
			}
		}

		protected void ExecuteImportBackupCommand()
		{

			ShowImportHelpCallback?.Invoke();

			//Pages.ImportBackupHelp(Navigation);
		}
	}
}