using System;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class ImportBackupViewModel: ViewModel
	{
		INavigation navigation;

		public ImportBackupViewModel(INavigation navigation)

		{


			this.navigation = navigation;
			HtmlText = NSWLocalFiles.GetImportBackupHelpHTML(AppLanguage.GetCurrentLangCode());
		}

		string htmlText;
		public string HtmlText {
			get { return htmlText; }
			set {
				if (htmlText == value)
					return;
				htmlText = value;
				OnPropertyChanged("HtmlText");
			}
		}
	}
}
