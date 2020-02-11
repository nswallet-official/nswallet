using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public partial class MainScreenView : ContentPage
	{
		Grid folderEditLayout;

		async void AnimateFolderEditMenu(bool open) 
		{
			if (open) {
				await folderEditLayout.ScaleTo(10);
			}
		}

		Grid GetFolderMenu()
		{
			folderEditLayout = new Grid {
				BackgroundColor = Theme.Current.AppHeaderBackground,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(120) },
					new RowDefinition { Height = new GridLength(120) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			// Folder menu items
			var folderDelete = GetSettingsIcon(true, FontAwesome.Regular.Trash_Alt, TR.Tr("popupmenu_delete"), "DeleteCommand");
			var folderChangeIcon = GetSettingsIcon(false, FontAwesome.Regular.File_Image, TR.Tr("popupmenu_changeicon"), "ChangeIconCommand");
			var folderChangeTitle = GetSettingsIcon(true,FontAwesome.Regular.Pencil_Alt,TR.Tr("popupmenu_changetitle"),"ChangeTitleCommand");
			var folderCopy = GetSettingsIcon(false,FontAwesome.Regular.Copy,TR.Tr("popupmenu_copylocally"),"CopyLocallyCommand");

			folderEditLayout.Children.Add(folderDelete, 0, 0);
			folderEditLayout.Children.Add(folderChangeIcon, 0, 1);
			folderEditLayout.Children.Add(folderChangeTitle, 1, 0);
			folderEditLayout.Children.Add(folderCopy, 1, 1);

			folderEditLayout.SetBinding(IsVisibleProperty, "IsEditFolderPanel");

			return folderEditLayout;
		}
	}
}