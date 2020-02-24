using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using NSWallet.Helpers;
using NSWallet.Model;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public class BackupScreenView : ContentPage
	{
		ListView backupList;
		bool fromLogin = false;

		public BackupScreenView(INavigation navigation = null)
		{
			fromLogin |= navigation != null;

			bool isAuthorized = false;
			if (navigation == null) {
				isAuthorized = true;
			} else {
				isAuthorized = false;
			}

			var pageVM = new BackupScreenViewModel(Navigation, isAuthorized);
			BindingContext = pageVM;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_backup"));

			if (navigation != null) {
				var closeToolbarItem = new ToolbarItem();
				closeToolbarItem.Text = TR.Close;
				closeToolbarItem.Icon = Theme.Current.CloseIcon;
				closeToolbarItem.Clicked += (sender, e) => Navigation.PopModalAsync();
				ToolbarItems.Add(closeToolbarItem);
			} else {
				var manualBackupToolbarItem = new ToolbarItem();
				manualBackupToolbarItem.Text = "Manual backup";
				manualBackupToolbarItem.Icon = "manual_backup.png";
				manualBackupToolbarItem.SetBinding(MenuItem.CommandProperty, "ManualBackupCommand");
				ToolbarItems.Add(manualBackupToolbarItem);
			}

			backupList = new ListView {
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate(() => {
					var mainBackupLayout = new StackLayout();
					mainBackupLayout.Orientation = StackOrientation.Horizontal;
					mainBackupLayout.Padding = new Thickness(15);

					var icon = new CachedImage {
						HeightRequest = Theme.Current.MenuIconHeight,
						WidthRequest = Theme.Current.MenuIconWidth,
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Style = ImageProperties.DefaultCachedImageStyle,
						Source = ImageSource.FromStream(() => NSWRes.GetImage("BackupScreen.restore_icon.png"))
					};

					mainBackupLayout.Children.Add(new StackLayout { Children = { icon } });

					var backupDataLayout = new StackLayout();
					backupDataLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

					var date = new Label {
						FontAttributes = Theme.Current.MenuLabelFontAttributes,
						FontFamily = NSWFontsController.CurrentTypeface,
						LineBreakMode = LineBreakMode.TailTruncation,
						FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
						TextColor = Theme.Current.ListTextColor
					};
					date.SetBinding(Label.TextProperty, "DateString");
					backupDataLayout.Children.Add(date);

					var descriptionLayout = new StackLayout();
					descriptionLayout.Orientation = StackOrientation.Horizontal;

					var type = new Label();
					type.FontFamily = NSWFontsController.CurrentTypeface;
					type.FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label));
					type.TextColor = Theme.Current.ListTextColor;
					type.SetBinding(Label.TextProperty, "Type");
					descriptionLayout.Children.Add(type);

					var size = new Label();
					size.FontFamily = NSWFontsController.CurrentTypeface;
					size.FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label));
					size.TextColor = Theme.Current.ListTextColor;
					size.HorizontalOptions = LayoutOptions.EndAndExpand;
					size.SetBinding(Label.TextProperty, "Size");
					descriptionLayout.Children.Add(size);

					backupDataLayout.Children.Add(descriptionLayout);

					mainBackupLayout.Children.Add(backupDataLayout);

					return new ExtendedViewCell {
						SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor,
						View = mainBackupLayout
					};
				})
			};

			backupList.BackgroundColor = Theme.Current.ListBackgroundColor;
			backupList.SeparatorVisibility = SeparatorVisibility.None;
			backupList.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "BackupList");

			backupList.ItemSelected += (sender, e) => {
				if (((ListView)sender).SelectedItem != null) {

					var backupDate = ((BackupModel)((ListView)sender).SelectedItem).DateString;

					Device.BeginInvokeOnMainThread(async () => {
						var result = await DisplayActionSheet(
							backupDate,
							TR.Cancel,
							TR.Tr("backupmenu_delete"),
							TR.Tr("backupmenu_restore"),
							TR.Tr("backupmenu_export")
							);

						if (result == TR.Tr("backupmenu_delete")) {
							pageVM.DeleteBackupCommand.Execute(sender);
						}

						if (result == TR.Tr("backupmenu_restore")) {
							pageVM.RestoreBackupCommand.Execute(sender);
						}

						if (result == TR.Tr("backupmenu_export")) {
							pageVM.ExportBackupCommand.Execute(sender);
						}

						((ListView)sender).SelectedItem = null;
					});
				}
			};

			pageVM.MessageCommand = LaunchMessageBox;
			pageVM.MessageCreateManualBackupCommand = LaunchCreatedMessageBox;

			Content = backupList;
		}

		async void LaunchMessageBox(string title, string question, string type)
		{

			var answer = await DisplayAlert(title, question, "Yes", "No");

			if (answer) {
				//await Navigation.PopPopupAsync(false);
				MessagingCenter.Send(this, type);
			} else {
				//await Navigation.PopPopupAsync(false);
				backupList.SelectedItem = null;
			}

		}

		void LaunchCreatedMessageBox(bool isSuccess)
		{
			if (isSuccess)
				DisplayAlert(TR.Tr("app_name"), TR.Tr("manualbackup_created_success"), TR.Tr("close"));
			else
				DisplayAlert(TR.Tr("app_name"), TR.Tr("manualbackup_created_fail"), TR.Tr("close"));
		}

		protected override bool OnBackButtonPressed()
		{
			if (fromLogin) {
				return base.OnBackButtonPressed();
			}

			if (Settings.AndroidBackLogout) {
				AppPages.Main();
			}

			return true;
		}
	}
}