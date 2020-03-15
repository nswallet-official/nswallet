using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NSWallet.Helpers;
using NSWallet.Model;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet
{
	public class BackupScreenViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public Action<string, string, string> MessageCommand { get; set; }
		public Action<bool> MessageCreateManualBackupCommand { get; set; }

		public string FirstLetterToUpper(string str)
		{
			if (str == null)
				return null;

			if (str.Length > 1)
				return char.ToUpper(str[0]) + str.Substring(1);

			return str.ToUpper();
		}

		IEnumerable<string> clearNonZip(IEnumerable<string> paths)
		{
			try {
				return paths?.Where(p => p.Contains(".zip"));
				//if (paths != null) {
				//	foreach (var path in paths) {
				//		if (!path.Contains(".zip")) {
				//			PlatformSpecific.RemoveFile(path);
				//		}
				//	}
				//}
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(clearNonZip), nameof(BackupScreenViewModel));
				return paths;
			}
		}

		void updateList()
		{
			try {
				var backupPath = PlatformSpecific.GetBackupPath();
				var backupFiles = PlatformSpecific.GetFileNames(backupPath);
				backupFiles = clearNonZip(backupFiles); // FIX FOR BACKUPS

				BackupList = new List<BackupModel>();

				if (backupFiles != null) {
					foreach (var backupFile in backupFiles) {
						if (backupFile != null) {
							if (backupFile.Length > 21) {
								var date = Common.GetDateFromBackupFileName(backupFile);

								BackupList.Add(new BackupModel {
									Path = backupPath + "/" + backupFile,
									DateString = date.ToString("F", CultureInfo.CurrentCulture),
									Date = date,
									Type = FirstLetterToUpper(TR.Tr(backupFile.Substring(21, backupFile.Length - 21 - 4))),
									Size = Math.Round((double)(PlatformSpecific.GetFileSize(backupPath + "/" + backupFile)) / 1024, 1) + " Kb"
								});
							}
						}
					}
				}

				if (BackupList != null) {
					BackupList = BackupList.OrderByDescending(x => x.Date).ToList();
				}
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(updateList), nameof(BackupScreenViewModel));
			}
		}

		INavigation navigation;
		bool isAuthorized;

		public BackupScreenViewModel(INavigation navigation, bool isAuthorized)
		{
			try {
				this.navigation = navigation;
				this.isAuthorized = isAuthorized;
				updateList();
				MessageCommand = (x, y, z) => { };
				MessageCreateManualBackupCommand = (x) => { };
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(BackupScreenViewModel), nameof(BackupScreenViewModel));
			}
		}

		List<BackupModel> backupList;
		public List<BackupModel> BackupList {
			get { return backupList; }
			set {
				if (backupList == value)
					return;
				backupList = value;
				OnPropertyChanged("BackupList");
			}
		}

		Command exportBackupCommand;
		public Command ExportBackupCommand {
			get {
				return exportBackupCommand ?? (exportBackupCommand = new Command(ExecuteExportBackupCommand));
			}
		}

		protected void ExecuteExportBackupCommand(object sender)
		{
			try {
				var backup = (BackupModel)((ListView)sender).SelectedItem;
				((ListView)sender).SelectedItem = null;
				PlatformSpecific.ShareFile(backup.Path, TR.Tr("backup_from") + " " + backup.Date, "*/*", TR.Tr("share_backup"), () => {
					//Device.BeginInvokeOnMainThread(() => navigation.PopPopupAsync());
				});

			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(ExecuteExportBackupCommand), nameof(BackupScreenViewModel));
			}
		}

		Command manualBackup;
		public Command ManualBackupCommand {
			get {
				return manualBackup ?? (manualBackup = new Command(ExecuteManualBackupCommand));
			}
		}

		protected void ExecuteManualBackupCommand()
		{
			try {
				var result = BackupManager.CreateManual();

				if (result) {
					updateList();
					MessageCreateManualBackupCommand.Invoke(result);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(ExecuteManualBackupCommand), nameof(BackupScreenViewModel));
			}
		}

		Command deleteBackupCommand;
		public Command DeleteBackupCommand {
			get {
				return deleteBackupCommand ?? (deleteBackupCommand = new Command(ExecuteDeleteBackupCommand));
			}
		}

		protected void ExecuteDeleteBackupCommand(object sender)
		{
			try {
				var backup = (BackupModel)((ListView)sender).SelectedItem;

				var type = "/remove";

				MessageCommand.Invoke("Confirm removing", "Are you sure you want to remove backup by date " + backup.DateString + "?", type);

				MessagingCenter.Subscribe<BackupScreenView>(this, type, (s) => {
					try {
						var path = backup.Path;

						if (PlatformSpecific.FileExists(path)) {
							PlatformSpecific.RemoveFile(path);
							AppPages.Backup();
						}
					} catch (Exception ex) {
						AppLogs.Log(ex.Message, nameof(ExecuteDeleteBackupCommand), nameof(BackupScreenViewModel));
					}
				});
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(ExecuteDeleteBackupCommand), nameof(BackupScreenViewModel));
			}
		}

		Command restoreBackupCommand;
		public Command RestoreBackupCommand {
			get {
				return restoreBackupCommand ?? (restoreBackupCommand = new Command(ExecuteRestoreBackupCommand));
			}
		}

		int reduceRepeat; // FIXME: whole execution should be re-developed, messaging center is called mutliple times
		protected void ExecuteRestoreBackupCommand(object sender)
		{
			try {
				var backup = (BackupModel)((ListView)sender).SelectedItem;

				var type = "/restore";
				reduceRepeat = 0;

				MessageCommand.Invoke(TR.Tr("restore_confirm"), TR.Tr("restore_details").Replace("{RESTORE_DATE}", backup.DateString), type);

				MessagingCenter.Subscribe<BackupScreenView>(this, type, (s) => {
					try {
						if (reduceRepeat == 0) {
							reduceRepeat++;
							var pathFrom = backup.Path;
							var pathTo = PlatformSpecific.GetDBDirectory();

							var isBackup = BackupManager.CheckOnBackup(pathFrom);
							if (!isBackup) {
								Application.Current.MainPage.DisplayAlert(TR.Tr("restore"), TR.Tr("restore_wrong_file"), TR.Close);
							} else {
								if (PlatformSpecific.FileExists(pathFrom) && PlatformSpecific.DirectoryExists(pathTo)) {
									FingerprintHelper.ResetSettings(true, true, true);
									BackupManager.UpdateBackup(pathFrom, pathTo, isAuthorized);
									AppPages.Login();
								}
							}
						}
					} catch (Exception ex) {
						AppLogs.Log(ex.Message, nameof(ExecuteRestoreBackupCommand), nameof(BackupScreenViewModel));
					}
				});
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(ExecuteRestoreBackupCommand), nameof(BackupScreenViewModel));
			}
		}

		Command menuTappedCommand;
		public Command MenuTappedCommand {
			get {
				return menuTappedCommand ?? (menuTappedCommand = new Command(ExecuteMenuTappedCommand));
			}
		}

		void ExecuteMenuTappedCommand(object obj)
		{
			try {
				if (obj != null) {
					var popupItem = (PopupItem)obj;
					switch (popupItem.Action) {
						case "DeleteBackupCommand":
							DeleteBackupCommand.Execute(popupItem.Parameter);
							break;
						case "RestoreBackupCommand":
							RestoreBackupCommand.Execute(popupItem.Parameter);
							break;
						case "ExportBackupCommand":
							ExportBackupCommand.Execute(popupItem.Parameter);
							break;
					}
				}
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(ExecuteMenuTappedCommand), nameof(BackupScreenViewModel));
			}
		}

		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
