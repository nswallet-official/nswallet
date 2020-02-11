using System;
using System.Threading.Tasks;
using NSWallet.Controls.EntryPopup;
using NSWallet.NetStandard.Helpers;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet.NetStandard.VM.Diagnostics
{
	public class DiagnosticsPageViewModel : ViewModel
	{
		bool fromLogin;

		public DiagnosticsPageViewModel(bool fromLogin)
		{
			this.fromLogin = fromLogin;
			SetDiagnosticsEnabled();
		}

		bool isDiagnosticsRunning;
		public bool IsDiagnosticsRunning {
			get { return isDiagnosticsRunning; }
			set {
				if (isDiagnosticsRunning == value)
					return;
				isDiagnosticsRunning = value;
				OnPropertyChanged("IsDiagnosticsRunning");
			}
		}

		Command runDiagnosticsCommand;
		public Command RunDiagnosticsCommand {
			get {
				return runDiagnosticsCommand ?? (runDiagnosticsCommand = new Command(ExecuteRunDiagnosticsCommand));
			}
		}

		void ExecuteRunDiagnosticsCommand()
		{
			Application.Current.MainPage.DisplayAlert(
				TR.Tr("attention"),
				TR.Tr("diagnostics_description"),
				TR.OK,
				TR.Cancel).ContinueWith(x => {
					if (x.Result) {
						var isDiagnosticsReady = BL.PrepareDiagnostics();
						if (!isDiagnosticsReady) {
							LaunchPasswordPopup();
						} else {
							runDiagnostics();
						}
					}
				});
		}

		void runDiagnostics()
		{
			/*
			var dbDirectory = PlatformSpecific.GetDBDirectory();
			var logsDirectory = dbDirectory + GConsts.DIAGNOSTICS_LOGS_FILE_PATH;
			Task.Run(async () => {
				SetDiagnosticsEnabled();
				var result = await BL.RunDiagnostics(logsDirectory, DataService.GetDeviceInfo(), fromLogin);
				if (result) {
					LaunchIssuesPopup();
				} else {
					LaunchNoIssuesPopup();
				}
				SetDiagnosticsEnabled();
			});
			*/
		}

		void LaunchPasswordPopup()
		{
			Device.BeginInvokeOnMainThread(() => {
				var popup = new EntryPopup(TR.Tr("diagnostics_password_enter"), null, true);
				popup.PopupClosed += (o, closedArgs) => {
					if (closedArgs.OkClicked) {
						var checkPassword = BL.CheckPassword(closedArgs.Text);
						if (!checkPassword) {
							LaunchWrongPasswordPopup();
						} else {
							runDiagnostics();
						}
					}
				};
				popup.Show();
			});
		}

		void LaunchWrongPasswordPopup()
		{
			Device.BeginInvokeOnMainThread(
				() => Application.Current.MainPage.DisplayAlert(
					TR.Tr("error"),
					TR.Tr("diagnostics_password_wrong"),
					TR.OK
				)
			);
		}

		void LaunchIssuesPopup()
		{
			Device.BeginInvokeOnMainThread(
				() => Application.Current.MainPage.DisplayAlert(
					TR.Tr("app_name"),
					TR.Tr("diagnostics_issues"),
					TR.OK,
					TR.Cancel
				).ContinueWith(x => {
					if (x.Result) {
						LaunchDiagnosticsEmail();
					}
				})
			);
		}

		void LaunchNoIssuesPopup()
		{
			Device.BeginInvokeOnMainThread(
				() => Application.Current.MainPage.DisplayAlert(
					TR.Tr("app_name"),
					TR.Tr("diagnostics_no_issues"),
					TR.OK
				)
			);
		}

		void SetDiagnosticsEnabled()
		{
			Device.BeginInvokeOnMainThread(() => IsDiagnosticsRunning = BL.IsDiagnosticsRunning());
		}

		void LaunchDiagnosticsEmail()
		{
			var to = GConsts.APP_LOGS_EMAIL;
			var subject = GConsts.LOGS_EMAIL_SUBJECT;
			var body = GConsts.LOGS_EMAIL_BODY;
			var filePath = NSWallet.Shared.Helpers.Logs.Diagnostics.Diagnostics.GetDiagnosticsFilePath();
			switch (Device.RuntimePlatform) {
				case Device.Android:
					LaunchEmailWithAndroid(to, subject, body, filePath);
					break;
				case Device.iOS:
					LaunchEmailWithIOS(to, subject, body, filePath);
					break;
			}
		}

		Command sendLogsCommand;
		public Command SendLogsCommand {
			get {
				return sendLogsCommand ?? (sendLogsCommand = new Command(ExecuteSendLogsCommand));
			}
		}

		void ExecuteSendLogsCommand()
		{
			var to = GConsts.APP_LOGS_EMAIL;
			var subject = GConsts.LOGS_EMAIL_SUBJECT;
			var body = GConsts.LOGS_EMAIL_BODY;
			var filePath = AppLogs.GetLogsFilePath();
			switch (Device.RuntimePlatform) {
				case Device.Android:
					LaunchEmailWithAndroid(to, subject, body, filePath);
					break;
				case Device.iOS:
					LaunchEmailWithIOS(to, subject, body, filePath);
					break;
			}
		}

		const string appStoreMailLink = "https://itunes.apple.com/us/app/mail/id1108187098";

		void LaunchEmailWithIOS(string to, string subject, string body, string filePath)
		{
			/*
			var emailMessenger = CrossMessaging.Current.EmailMessenger;
			if (emailMessenger.CanSendEmail) {
				if (emailMessenger.CanSendEmailAttachments) {
					AppLogs.CheckLogExists();
					var email = new EmailMessageBuilder()
						.To(to)
						.Subject(subject)
						.Body(body)
						.WithAttachment(filePath, "text/plain")
						.Build();
					emailMessenger.SendEmail(email);
					AppLogs.DropLogs();
				} else {
					Application.Current.MainPage.DisplayAlert(
						TR.Tr("alert"),
						TR.Tr("cant_open_mail_with_attachment"),
						TR.OK
					);
				}
			} else {
				Application.Current.MainPage.DisplayAlert(
					TR.Tr("alert"),
					TR.Tr("mail_app_ios_need"),
					TR.Tr("install_mail_app"),
					TR.Cancel
				).ContinueWith(x => {
					if (x.Result) {
						Device.BeginInvokeOnMainThread(
							() => Device.OpenUri(new Uri(appStoreMailLink)));
					}
				});
			}
			*/
		}

		void LaunchEmailWithAndroid(string to, string subject, string body, string filePath)
		{
			var result = DependencyService.Get<NetStandard.Interfaces.IEmailService>().OpenEmail(
				TR.Tr("choose_mailing_app"),
				new System.Collections.Generic.List<string> { to },
				subject,
				body,
				filePath
			);

			if (result) {
				AppLogs.DropLogs();
			}
		}
	}
}