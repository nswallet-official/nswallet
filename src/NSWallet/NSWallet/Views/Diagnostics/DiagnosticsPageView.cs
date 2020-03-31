using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.VM.Diagnostics;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.Diagnostics
{
	public class DiagnosticsPageView : ContentPage
	{
		public DiagnosticsPageView(bool fromLogin)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new DiagnosticsPageViewModel(fromLogin);

			Title = TR.Tr("admin_panel_diagnostics");

			var diagnosticsButton = new Button { Text = TR.Tr("run_diagnostics") };
			diagnosticsButton.SetBinding(Button.CommandProperty, "RunDiagnosticsCommand");
			diagnosticsButton.FontAttributes = FontAttributes.Bold;
			diagnosticsButton.TextColor = Theme.Current.CommonButtonTextColor;
			diagnosticsButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			diagnosticsButton.FontFamily = NSWFontsController.CurrentBoldTypeface;
			diagnosticsButton.BackgroundColor = Theme.Current.CommonButtonBackground;
			diagnosticsButton.CornerRadius = Theme.Current.ButtonRadius;
			diagnosticsButton.BorderWidth = 0;
			diagnosticsButton.Margin = new Thickness(20);
			diagnosticsButton.SetBinding(IsEnabledProperty, "IsDiagnosticsRunning");

			Content = new StackLayout { Children = { diagnosticsButton } };
		}
	}
}