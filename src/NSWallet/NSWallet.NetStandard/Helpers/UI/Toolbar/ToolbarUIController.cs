using System.Collections.Generic;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers.UI.Toolbar
{
	public class ToolbarUIController
	{
		IList<ToolbarItem> toolbarItems;

		public ToolbarUIController(IList<ToolbarItem> toolbarItems)
		{
			this.toolbarItems = toolbarItems;
		}

		void insertToToolbar(ToolbarItem toolbarItem)
		{
			if (toolbarItem != null) {
				Device.BeginInvokeOnMainThread(() =>
											   toolbarItems.Add(toolbarItem));
			}
		}

		ToolbarItem prepareToolbarItem(string name, string icon, string command)
		{
			var toolbarItem = new ToolbarItem {
				Text = name, Icon = icon
			};

			toolbarItem.SetBinding(MenuItem.CommandProperty, command);
			return toolbarItem;
		}

		/// <summary>
		/// Inserts the filter. Use "FilterCommand" for the action.
		/// </summary>
		public void InsertFilter()
		{
			insertToToolbar(
				prepareToolbarItem(
					name: TR.Tr("icons_filter"),
					icon: Theme.Current.FilterIcon,
					command: "FilterCommand"
				)
			);
		}

		/// <summary>
		/// Inserts the gallery picker. Use "GalleryPickCommand" for the action.
		/// </summary>
		public void InsertGalleryPicker()
		{
			insertToToolbar(
				prepareToolbarItem(
					name: TR.Tr("icons_gallery_picker"),
					icon: Theme.Current.GalleryPickerIcon,
					command: "GalleryPickCommand"
				)
			);
		}
	}
}