using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
	public partial class MainScreenView : ContentPage
	{
		static Grid GetItemMenu()
		{
			var itemEditLayout = new Grid {
				BackgroundColor = Theme.Current.AppHeaderBackground,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(120) },
					new RowDefinition { Height = new GridLength(120) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			// Item menu items
			var deleteIcon = GetSettingsIcon(true, FontAwesome.Regular.Trash_Alt, TR.Tr("popupmenu_delete"), "DeleteCommand");
			var changeIcon = GetSettingsIcon(false, FontAwesome.Regular.File_Image, TR.Tr("popupmenu_changeicon"), "ChangeIconCommand");
			var changeTitleIcon = GetSettingsIcon(true, FontAwesome.Regular.Pencil_Alt, TR.Tr("popupmenu_changetitle"), "ChangeTitleCommand");
			var copyIcon = GetSettingsIcon(false, FontAwesome.Regular.Copy, TR.Tr("popupmenu_copylocally"), "CopyLocallyCommand");
			var reorderIcon = GetSettingsIcon(true, FontAwesome.Regular.Sort_Amount_Down, TR.Tr("popupmenu_reorder"), "CutCommand");
			var shareIcon = GetSettingsIcon(true, FontAwesome.Regular.Share_Alt, TR.Tr("popupmenu_share"), "ShareCommand");

			itemEditLayout.Children.Add(deleteIcon, 0, 0);
			itemEditLayout.Children.Add(changeIcon, 1, 0);
			itemEditLayout.Children.Add(changeTitleIcon, 2, 0);
			itemEditLayout.Children.Add(copyIcon, 0, 1);
			itemEditLayout.Children.Add(reorderIcon, 1, 1);
			itemEditLayout.Children.Add(shareIcon, 2, 1);

			itemEditLayout.SetBinding(IsVisibleProperty, "IsEditItemPanel");

			return itemEditLayout;
		}
	}
}