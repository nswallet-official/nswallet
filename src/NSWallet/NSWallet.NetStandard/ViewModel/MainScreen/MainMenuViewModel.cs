using NSWallet.Premium;
using NSWallet.Shared;
using Xamarin.Forms;


namespace NSWallet
{
	public class MainMenuViewModel : ViewModel
	{
		public MainMenuViewModel()
		{
			// Temporary for Mac BETA
			if (Device.RuntimePlatform == Device.macOS) {
				Premium = "Mac BETA";
				PremiumColor = Theme.Current.MenuTopPremiumColor;
				return;
			}

			if (PremiumManagement.IsPremiumSubscription) {
				Premium = TR.Tr("premium_status");
				PremiumColor = Theme.Current.MenuTopPremiumColor;
			} else if (PremiumManagement.IsLegacyPremium) {
				Premium = TR.Tr("premiumlegacy_status");
				PremiumColor = Theme.Current.MenuTopPremiumColor;
			} else {
				Premium = TR.Tr("free_status");
				PremiumColor = Theme.Current.MenuTopFreeColor;
			}
		}

		string premium;
		public string Premium {
			get { return premium; }
			set {
				if (premium == value)
					return;
				premium = value;
				OnPropertyChanged("Premium");
			}
		}

		Color premiumColor;
		public Color PremiumColor {
			get { return premiumColor; }
			set {
				if (premiumColor == value)
					return;
				premiumColor = value;
				OnPropertyChanged("PremiumColor");
			}
		}

		Command homeCommand;
		public Command HomeCommand {
			get {
				return homeCommand ?? (homeCommand = new Command(ExecuteHomeCommand));
			}
		}

		protected static void ExecuteHomeCommand()
		{
			Pages.Main();
		}

		Command labelsCommand;
		public Command LabelsCommand {
			get {
				return labelsCommand ?? (labelsCommand = new Command(ExecuteLabelsCommand));
			}
		}

		protected static void ExecuteLabelsCommand()
		{
			Pages.LabelsManagement();
		}

		Command iconsCommand;
		public Command IconsCommand {
			get {
				return iconsCommand ?? (iconsCommand = new Command(ExecuteIconsCommand));
			}
		}

		protected static void ExecuteIconsCommand()
		{
			Pages.IconsManagement();
		}

		Command logoutCommand;
		public Command LogoutCommand {
			get {
				return logoutCommand ?? (logoutCommand = new Command(ExecuteLogoutCommand));
			}
		}

		protected static void ExecuteLogoutCommand()
		{

			BL.Close();
			LoginScreenView.ManualExit = true;
			Pages.Login();
		}

		Command exportImportCommand;
		public Command ExportImportCommand {
			get {
				return exportImportCommand ?? (exportImportCommand = new Command(ExecuteExportImportCommand));
			}
		}

		protected static void ExecuteExportImportCommand()
		{
			Pages.ExportImport();
		}

		Command aboutCommand;
		public Command AboutCommand {
			get {
				return aboutCommand ?? (aboutCommand = new Command(ExecuteAboutCommand));
			}
		}

		protected static void ExecuteAboutCommand()
		{
			Pages.About();
		}

		Command settingsCommand;
		public Command SettingsCommand {
			get {
				return settingsCommand ?? (settingsCommand = new Command(ExecuteSettingsCommand));
			}
		}

		protected static void ExecuteSettingsCommand()
		{
			Pages.Settings();
		}

		Command backupCommand;
		public Command BackupCommand {
			get {
				return backupCommand ?? (backupCommand = new Command(ExecuteBackupCommand));
			}
		}

		protected static void ExecuteBackupCommand()
		{
			Pages.Backup();
		}

		Command developerCommand;
		public Command DeveloperCommand {
			get {
				return developerCommand ?? (developerCommand = new Command(ExecuteDeveloperCommand));
			}
		}

		protected static void ExecuteDeveloperCommand()
		{
			Pages.AdminPanel();
		}

		Command feedbackCommand;
		public Command FeedbackCommand {
			get {
				return feedbackCommand ?? (feedbackCommand = new Command(ExecuteFeedbackCommand));
			}
		}

		protected static void ExecuteFeedbackCommand()
		{
			Pages.Feedback();
		}
	}
}