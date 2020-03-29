using System;
using NSWallet.Controls.EntryPopup;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet.Helpers
{
	public static class BackupManager
	{
		public static bool CheckOnBackup(string file)
		{
			var zipEntries = PlatformSpecific.ReadZip(file);
			if (zipEntries != null) {
				var backup = zipEntries.Find(x => x == GConsts.DATABASE_FILENAME);
				return (backup == null) ? false : true;
			}
			return false;
		}

		static string createTempDBFolder()
		{
			var tempFolder = PlatformSpecific.GetTempFolder();
			var tempDBFolder = tempFolder + "/" + GConsts.DATABASE_FOLDER + "." + Common.GenerateUniqueString(8);
			PlatformSpecific.CreateFolder(tempDBFolder);
			return tempDBFolder;
		}

		static string createZipCopy(string zipToCopy, string tempDBFolder)
		{
			var zipCopy = tempDBFolder + "/temp.zip";
			PlatformSpecific.CopyFile(zipToCopy, zipCopy);
			return zipCopy;
		}

		static string getTempFile(string tempDBFolder)
		{
			return tempDBFolder + "/" + GConsts.DATABASE_FILENAME;
		}

		static bool checkDBVersion(string tempFile)
		{
			if (!BL.CheckDBVersion(tempFile)) {
				Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(TR.Tr("error"), TR.Tr("backup_failure_high_version"), TR.OK));
				return false;
			}
			return true;
		}

		static bool initAPI(string fileToInit, string unzipFrom = null, string unzipTo = null)
		{
			if (!string.IsNullOrEmpty(fileToInit)) {
				BL.Close();

				if (!string.IsNullOrEmpty(unzipFrom) && !string.IsNullOrEmpty(unzipTo)) {
					PlatformSpecific.RemoveFile(PlatformSpecific.GetDBFile());
					var result = PlatformSpecific.Unzip(unzipFrom, unzipTo);
					if (!result) {
						return false;
					}
				}

				BL.InitAPI(fileToInit, AppLanguage.GetCurrentLangCode());
				BL.InitNewStorage();
				return true;
			}
			return false;
		}

		static void checkBackupPassword(string password, string unzipFrom, string unzipTo)
		{
			bool result = true;
			if (BL.CheckPassword(password)) {
				Device.BeginInvokeOnMainThread(() => {
					result = initAPI(PlatformSpecific.GetDBFile(), unzipFrom, unzipTo);
					if (!result) {
						showBackupRestoreErrorPopup();
					} else {
						showBackupSuccessPasswordPopup();
					}
				});
			} else {
				result = initAPI(PlatformSpecific.GetDBFile());
				if (!result) {
					showBackupRestoreErrorPopup();
				} else {
					showBackupWrongPasswordPopup();
				}
			}
		}

		static void showBackupRestoreErrorPopup()
		{
			Device.BeginInvokeOnMainThread(() => {
				Application.Current.MainPage.DisplayAlert(TR.Tr("error"), TR.Tr("backup_restore_fail"), TR.OK);
			});
		}

		static void showBackupWrongPasswordPopup()
		{
			Device.BeginInvokeOnMainThread(() => {
				Application.Current.MainPage.DisplayAlert(TR.Tr("error"), TR.Tr("backup_wrong_password"), TR.OK);
			});
		}

		static void showBackupSuccessPasswordPopup()
		{
			Device.BeginInvokeOnMainThread(() => {
				Application.Current.MainPage.DisplayAlert(TR.Tr("alert"), TR.Tr("backup_restored"), TR.OK);
			});
		}

		public static void UpdateBackup(string pathFrom, string pathTo, bool isAuthorized = false)
		{
			try {
				bool errorState = false;
				var tempDBFolder = createTempDBFolder();
				var tempZip = createZipCopy(pathFrom, tempDBFolder);
				errorState = PlatformSpecific.Unzip(pathFrom, tempDBFolder);

				if (errorState) {
					var tempFile = getTempFile(tempDBFolder);

					if (!checkDBVersion(tempFile)) {
						return;
					}

					if (!isAuthorized) {
						errorState = initAPI(tempFile);

						if (errorState) {
							Device.BeginInvokeOnMainThread(() => {
								var checkPasswordPopup = new EntryPopup(TR.Tr("check_password_backup"), null, true);
								checkPasswordPopup.PopupClosed += (sender, e) => {
									if (e.OkClicked) {
										if (e.Text != null) {
											checkBackupPassword(e.Text, tempZip, pathTo);
										}
									}
								};
								checkPasswordPopup.Show();
							});
						} else {
							showBackupRestoreErrorPopup();
						}
					} else {
						var result = initAPI(PlatformSpecific.GetDBFile(), tempZip, pathTo);
						if (!result) {
							showBackupRestoreErrorPopup();
						} else {
							showBackupSuccessPasswordPopup();
						}
					}
				} else {
					showBackupRestoreErrorPopup();
				}
			} catch (Exception ex) {

				Device.BeginInvokeOnMainThread(() => {
					Application.Current.MainPage.DisplayAlert(TR.Tr("error"), TR.Tr("backup_restore_fail"), TR.OK);
				});

				log(ex.Message, nameof(UpdateBackup));
			}
		}

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(BackupManager));
		}

		static void createAutoBackup()
		{
			try {
				var dbDir = PlatformSpecific.GetDBDirectory();
				var backupFolder = PlatformSpecific.GetBackupPath();
				var currentDateTime = DateTime.Now;
				var backupName = GConsts.NSWB + "-" + currentDateTime.ToString(GConsts.BACKUP_DATEFORMAT) + "-" + GConsts.BACKUP_AUTO + ".zip";
				Settings.AutoBackupTime = currentDateTime.ToString();
				PlatformSpecific.CreateZip(dbDir, backupFolder, backupName);
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(createAutoBackup), nameof(BackupManager));
			}
		}

		public static void CreateAutoBackup()
		{
			try {
				var property = BL.StorageProperties;
				var previousDBUpdateTime = property.UpdateTimestamp;
				var previousAutoBackupTimeString = Settings.AutoBackupTime;
				var previousAutoBackupTime = default(DateTime);
				DateTime.TryParse(previousAutoBackupTimeString, out previousAutoBackupTime);
				var nowDateTime = DateTime.Now;
				var passedDays = nowDateTime.Subtract(previousAutoBackupTime).Days;
				var isDateInRange = Common.IsInRange(previousDBUpdateTime, previousAutoBackupTime, nowDateTime);

				if (Settings.AutoBackup == 1) {
					if (passedDays >= 7 && isDateInRange) {
						createAutoBackup();
					}
				} else if (Settings.AutoBackup == 2 && isDateInRange) {
					if (passedDays >= 1) {
						createAutoBackup();
					}
				}
			} catch(Exception ex) {
				log(ex.Message, nameof(CreateAutoBackup));
			}
		}

		public static void ForceCreateAutoBackup()
		{
			createAutoBackup();
		}

		public static string GetBackupName(DateTime date, bool manual)
		{
			if (manual)
				return GConsts.NSWB + "-" + date.ToString(GConsts.BACKUP_DATEFORMAT) + "-" + GConsts.BACKUP_MANUAL + ".zip";
			else
				return GConsts.NSWB + "-" + date.ToString(GConsts.BACKUP_DATEFORMAT) + "-" + GConsts.BACKUP_AUTO + ".zip";

		}

		public static bool CreateManual()
		{
			try {
				var dbDir = PlatformSpecific.GetDBDirectory();
				var backupFolder = PlatformSpecific.GetBackupPath();
				var currentDateTime = DateTime.Now;
				var backupName = GetBackupName(currentDateTime, true);
				return PlatformSpecific.CreateZip(dbDir, backupFolder, backupName);
			} catch(Exception ex) {
				log(ex.Message, nameof(CreateManual));
				return false;
			}
		}

		public static void RemoveOldBackups()
		{
			try {
				var nowDateTime = DateTime.Now;
				var backupPath = PlatformSpecific.GetBackupPath();
				var backupFiles = PlatformSpecific.GetFileNames(backupPath);
				if (backupFiles != null) {
					foreach (var backupFile in backupFiles) {
						if (backupFile.Contains(GConsts.BACKUP_AUTO)) {
							if (countAutoBackups() > 3) {
								var backupDate = Common.ConvertDBStringDateToDT(backupFile);
								if (nowDateTime.Subtract(backupDate).Days > Settings.BackupDeletion)
									PlatformSpecific.RemoveFile(backupPath + "/" + backupFile);
							}
						}
					}
				}
			} catch(Exception ex) {
				log(ex.Message, nameof(RemoveOldBackups));
			}
		}

		static int countAutoBackups()
		{
			int count = 0;
			var backupPath = PlatformSpecific.GetBackupPath();
			var backupFiles = PlatformSpecific.GetFileNames(backupPath);
			foreach (var backupFile in backupFiles) {
				if (backupFile.Contains(GConsts.BACKUP_AUTO)) {
					count++;
				}
			}
			return count;
		}

		public static void RemoveBackups()
		{
			var backupPath = PlatformSpecific.GetBackupPath();
			var backupFiles = PlatformSpecific.GetFileNames(backupPath);
			if (backupFiles != null) {
				foreach (var backupFile in backupFiles) {
					if (backupFile.Contains("nswb")) {
						PlatformSpecific.RemoveFile(backupPath + "/" + backupFile);
					}
				}
			}
		}
	}
}
