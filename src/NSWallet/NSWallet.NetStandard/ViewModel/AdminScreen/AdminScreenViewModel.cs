using System;
using System.Threading.Tasks;
using NSWallet.Helpers;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet
{
    public class AdminScreenViewModel : ViewModel
    {
		readonly INavigation navigation;
        public Action<string> PremiumListCallback { get; set; }

        public AdminScreenViewModel(INavigation navigation)
        {
            PremiumListCallback = (x) => { };
            this.navigation = navigation;
			LogsChecked = Settings.AreLogsActive;
			FeedbackChecked = Settings.IsFeedback;
        }

        bool logsChecked;
        public bool LogsChecked {
            get { return logsChecked; }
            set
            {
                if (logsChecked == value)
                    return;
				logsChecked = value;
				ExecuteLogsChecked(logsChecked);
                OnPropertyChanged("LogsChecked");
            }
        }

		void ExecuteLogsChecked(bool logsStatus)
		{
			Settings.AreLogsActive = logsStatus;
			AppLogs.SetLogsActivity(logsStatus);
		}



		bool feedbackChecked;
		public bool FeedbackChecked {
			get { return feedbackChecked; }
			set {
				if (feedbackChecked == value)
					return;
				feedbackChecked = value;
				ExecuteFeedbackChecked(feedbackChecked);
				OnPropertyChanged("FeedbackChecked");
			}
		}

		void ExecuteFeedbackChecked(bool feedbackActive)
		{
			Settings.IsFeedback = feedbackActive;
		}

		bool detailedChecked;
		public bool DetailedChecked {
			get { return detailedChecked; }
			set {
				if (detailedChecked == value)
					return;
				detailedChecked = value;
				ExecuteDetailedChecked(detailedChecked);
				OnPropertyChanged("DetailedChecked");
			}
		}

		void ExecuteDetailedChecked(bool detailedLogs)
		{
			
		}



		Command hideAdminPanelCommand;
		public Command HideAdminPanelCommand {
			get {
				return hideAdminPanelCommand ?? (hideAdminPanelCommand = new Command(ExecuteHideAdminPanelCommand));
			}
		}

		protected void ExecuteHideAdminPanelCommand()
		{
			Settings.DevOpsOn = false;
			AppPages.Main();
		}

       

		Command diagnosticsCommand;
		public Command DiagnosticsCommand {
			get {
				return diagnosticsCommand ?? (diagnosticsCommand = new Command(ExecuteDiagnosticsCommand));
			}
		}

		protected void ExecuteDiagnosticsCommand()
		{
			
		}
    }
}