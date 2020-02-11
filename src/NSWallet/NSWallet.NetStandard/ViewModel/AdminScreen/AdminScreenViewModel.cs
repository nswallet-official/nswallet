using System;
using System.Threading.Tasks;
using NSWallet.Helpers;
using NSWallet.Premium;
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
            SetPremiumToggle();
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

		bool premiumChecked;
		public bool PremiumChecked {
			get { return premiumChecked; }
			set {
				if (premiumChecked == value)
					return;
				premiumChecked = value;
				ExecutePremiumChecked(premiumChecked);
				OnPropertyChanged("PremiumChecked");
			}
		}

		void ExecutePremiumChecked(bool premiumStatus)
        {
            PremiumManagement.SetCommonStatus(premiumStatus);
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

        /// <summary>
        /// Sets the premium toggle.
        /// </summary>
        void SetPremiumToggle()
        {
            var premium = PremiumManagement.IsAnyPremium;
            if (premium)
                PremiumChecked = true;
            else
                PremiumChecked = false;
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
			Pages.Main();
		}

        Command checkPremiumCommand;
        public Command CheckPremiumCommand
        {
            get
            {
                return checkPremiumCommand ?? (checkPremiumCommand = new Command(ExecuteCheckPremiumCommand));
            }
        }

        protected void ExecuteCheckPremiumCommand()
        {
            Task.Run(async () =>
            {
                await PremiumManagement.GetPurchases();
                var purchaseList = PremiumManagement.PurchasesList;
                string purchases = null;

                foreach(var purchase in purchaseList)
                {
                    purchases += purchase + "\n";
                }

                Device.BeginInvokeOnMainThread(() => PremiumListCallback.Invoke(purchases));
            });
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