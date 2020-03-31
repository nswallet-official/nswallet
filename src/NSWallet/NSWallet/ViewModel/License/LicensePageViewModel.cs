using System;
using NSWallet.NetStandard.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class LicensePageViewModel : ViewModel
	{
		readonly INavigation navigation;
		readonly Action<bool> action;

		public LicensePageViewModel(INavigation navigation, string htmlSource, Action<bool> action)
		{
			this.navigation = navigation;

			if (action != null) {
				this.action = action;
			}

			if (htmlSource != null) {
				HTMLSource = htmlSource;
			}
		}

		string htmlSource;
		public string HTMLSource {
			get { return htmlSource; }
			set {
				if (htmlSource == value)
					return;
				htmlSource = value;
				OnPropertyChanged("HTMLSource");
			}
		}

		Command rejectCommand;
		public Command RejectCommand {
			get {
				return rejectCommand ?? (rejectCommand = new Command(ExecuteRejectCommand));
			}
		}

		void ExecuteRejectCommand()
		{
			action.Invoke(false);
			navigation.PopModalAsync();
		}

		Command acceptCommand;
		public Command AcceptCommand {
			get {
				return acceptCommand ?? (acceptCommand = new Command(ExecuteAcceptCommand));
			}
		}

		void ExecuteAcceptCommand()
		{
			action.Invoke(true);
			navigation.PopModalAsync();
		}
	}
}