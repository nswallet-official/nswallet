using System;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.License
{
	public class LicensePageView : ContentPage
	{
		public LicensePageView(string title, string htmlSource, Action<bool> action, bool buttons = true)
		{
			var viewModel = new LicensePageViewModel(Navigation, htmlSource, action);
			BindingContext = viewModel;

			Title = title;

			if (!buttons) {
				var closeToolbarItem = new ToolbarItem();
				closeToolbarItem.Text = TR.Close;
				closeToolbarItem.Icon = Theme.Current.CloseIcon;
				closeToolbarItem.Clicked += (sender, e) => Navigation.PopModalAsync();
				ToolbarItems.Add(closeToolbarItem);
			}

			var htmlWebViewSource = new HtmlWebViewSource();
			htmlWebViewSource.SetBinding(HtmlWebViewSource.HtmlProperty, "HTMLSource");

			var webView = new WebView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Source = htmlWebViewSource
			};

			var rejectButton = new Button {
				CornerRadius = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonRejectButtonBackgroundColor,
				TextColor = Theme.Current.CommonRejectButtonTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				Text = TR.Tr("reject")
			};

			rejectButton.SetBinding(Button.CommandProperty, "RejectCommand");

			var acceptButton = new Button {
				CornerRadius = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Theme.Current.CommonButtonBackground,
				TextColor = Theme.Current.CommonButtonTextColor,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button)),
				FontFamily = NSWFontsController.CurrentBoldTypeface,
				Text = TR.Tr("accept")
			};

			acceptButton.SetBinding(Button.CommandProperty, "AcceptCommand");

			var buttonsLayout = new StackLayout {
				IsVisible = buttons,
				Spacing = 0,
				VerticalOptions = LayoutOptions.End,
				Orientation = StackOrientation.Horizontal,
				Children = {
					rejectButton,
					acceptButton
				}
			};

			var mainLayout = new StackLayout {
				Children = {
					webView,
					buttonsLayout
				}
			};

			Content = mainLayout;
		}
	}
}