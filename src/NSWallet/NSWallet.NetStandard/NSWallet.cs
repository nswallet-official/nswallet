using Xamarin.Forms;
using NSWallet.Shared;
using NSWallet.Helpers;
using System;
using NSWallet.Shared.Helpers.Logs.AppLog;
using NSWallet.NetStandard.Helpers;
using Xamarin.Essentials;

namespace NSWallet
{
    public class App : Application
	{
		public App()
		{
			try {

				var appNamespace = "NSWallet.NetStandard";
				NSWRes.Init(appNamespace); // Initializing access to resources (images, icons)
				NSWLocalFiles.Init(appNamespace);

				TR.InitTR(appNamespace);   // Preparing translations
				TR.SetLanguage(AppLanguage.GetCurrentLangCode());
				ItemsStatsManager.Init();

				// Automatic backup should be done before engine initialization, new version of engine can spoil old DB
				BackupManager.CreateAutoBackup(); // Do not move this line below BL.InitAPI()

				BL.InitAPI(PlatformSpecific.GetDBFile(), AppLanguage.GetCurrentLangCode());

				PCLUpgradeManager.PrepareUpdate();
				BL.InitNewStorage();
				PCLUpgradeManager.RemoveAfterUpdate();

				AppTheme.SetCurrentTheme();

				BackupManager.RemoveOldBackups();

				FingerprintHelper.Initialize();
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(App), nameof(NSWallet));
			}

			AppPages.Login();
		}


		public void ImportImage(byte[] imageBytes)
		{
			
				ImportIconManager.BeginImportingIcon(imageBytes);

		}

        public void ImportZip(string url)
        {
            var isBackup = BackupManager.CheckOnBackup(url);
            if (!isBackup)
            {
                PlatformSpecific.RemoveFile(url);
                Current.MainPage.DisplayAlert(TR.Tr("restore"), TR.Tr("restore_wrong_file"), TR.Close);
                return;
            }

            Current.MainPage.DisplayAlert(TR.Tr("restore"), TR.Tr("restore_details_without_date"), TR.OK, TR.Cancel).ContinueWith(answer =>
            {
                var dbDir = PlatformSpecific.GetDBDirectory();
                var backupDir = PlatformSpecific.GetBackupPath();
                var backupFile = backupDir + "/" + BackupManager.GetBackupName(DateTime.Now, true);

                if (answer.Result)
                {
                    BackupManager.UpdateBackup(url, dbDir);
					FingerprintHelper.ResetSettings(true, true, true);
                    Device.BeginInvokeOnMainThread(() => AppPages.Login());
                }

                PlatformSpecific.MoveFile(url, backupFile);
            });
        }

		void checkFingerprintMessageBox()
		{
			try {
				if (FingerprintHelper.MessageBoxFault) {
					var mainPage = MainPage;
					if (mainPage != null) {
						mainPage.DisplayAlert(TR.Tr("alert"), TR.Tr("fingerprint_expired"), TR.OK);
					}
					FingerprintHelper.MessageBoxFault = false;
				}
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(checkFingerprintMessageBox), nameof(NSWallet));
			}
		}

		protected override void OnStart()
		{
			LicenseController.CheckPrivacyPolicy();
			//PremiumManagement.CheckPreviousPremium(MainPage); Do not ask it on start!
			checkLaunchCount();
			checkFingerprintMessageBox();
		}

		void checkLaunchCount()
		{
			try {
				Settings.LaunchCount++;
				if (!Settings.LaunchRememberedCount) {
					if (Settings.LaunchCount % 33 == 0) {
						Device.BeginInvokeOnMainThread(async () => {
							var result = await MainPage.DisplayAlert(TR.Tr("attention"), TR.Tr("feedback_vote"), TR.Tr("yes"), TR.Tr("later"));
							if (result) {
								Settings.LaunchRememberedCount = true;
								switch (Device.RuntimePlatform) {
									case Device.Android:
										OpenAppStore(GConsts.APPLINK_GOOGLEPLAY);
										break;
									case Device.iOS:
										OpenAppStore(GConsts.APPLINK_APPSTORE);
										break;
								}
							}
						});
					}
				}

			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(checkLaunchCount), nameof(NSWallet));
			}
		}

		void OpenAppStore(string url)
		{
			Launcher.OpenAsync(new Uri(url));
		}

		DateTime dateTimeSleep;

		
		protected override void OnSleep()
		{
			AppPages.Locker();

			if (!FingerprintHelper.IsEnabled) {
				ItemsStatsManager.Save();
			}

			dateTimeSleep = DateTime.Now;
		}

		protected override void OnResume()
		{
			AppPages.Unlocker(isModal: true);

			if (!FingerprintHelper.IsEnabled) {
				ItemsStatsManager.Init();
			}

			var passed = DateTime.Now.Subtract(dateTimeSleep).Minutes;

			if (passed >= Settings.AutoLogout) {
				AppPages.Login();
			}

			FingerprintHelper.IsEnabled = false;
		}
	}
}
