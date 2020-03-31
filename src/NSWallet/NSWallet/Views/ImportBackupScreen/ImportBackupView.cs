using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class ImportBackupView : ContentPage
	{
		public ImportBackupView()
		{
			BindingContext = new ImportBackupViewModel(Navigation);

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("nswallet_import_backup"));

			var mainStackLayout = new StackLayout();

			var closeToolbarItem = new ToolbarItem();

			closeToolbarItem.Text = TR.Close;
			closeToolbarItem.Icon = Theme.Current.CloseIcon;
			closeToolbarItem.Clicked += (sender, e) => Navigation.PopModalAsync();
			ToolbarItems.Add(closeToolbarItem);

			var webView = new WebView();
			webView.HeightRequest = 5000;
			webView.VerticalOptions = LayoutOptions.FillAndExpand;
			webView.HorizontalOptions = LayoutOptions.FillAndExpand;
			var webSource = new HtmlWebViewSource();
			webSource.SetBinding(HtmlWebViewSource.HtmlProperty, "HtmlText");
			webView.Source = webSource;
			mainStackLayout.Children.Add(webView);

			var backupScreenButton = new Button();
			backupScreenButton.Text = TR.Tr("menu_backup");
			backupScreenButton.CornerRadius = Theme.Current.ButtonRadius;
			backupScreenButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			backupScreenButton.TextColor = Theme.Current.CommonButtonTextColor;
			backupScreenButton.FontAttributes = Theme.Current.LoginButtonFontAttributes;
			backupScreenButton.FontFamily = NSWFontsController.CurrentBoldTypeface;
			backupScreenButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));

			//backupScreenButton.AutomationId = AutomationIdConsts.LOGIN_BUTTON_ID;
			backupScreenButton.Clicked += BackupScreenButton_Clicked;
			mainStackLayout.Children.Add(backupScreenButton);


			Content = mainStackLayout;
		}

		void BackupScreenButton_Clicked(object sender, System.EventArgs e)
		{
			AppPages.Backup(Navigation);
		}

	}
}
