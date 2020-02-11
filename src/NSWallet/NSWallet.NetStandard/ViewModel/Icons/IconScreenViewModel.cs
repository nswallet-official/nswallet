using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NSWallet.Helpers;
using NSWallet.Model;
using NSWallet.NetStandard.DependencyServices.GalleryPicker;
using NSWallet.NetStandard.DependencyServices.MediaService;
using NSWallet.NetStandard.Helpers;
using NSWallet.NetStandard.Helpers.UI.Popups;
using NSWallet.NetStandard.Helpers.UI.Popups.Pages.Icons;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class IconScreenViewModel : ViewModel
	{
		readonly string premadeIconIdentifier = "premade_id";

		const string deleteCommandString = "DeleteCommand";
		const string changeTitleCommandString = "ChangeTitleCommand";
		const string changeGroupCommandString = "ChangeGroupCommand";
		const string changeShapeCommandString = "ChangeShapeCommand";

		List<ItemList> savedList { get; set; }

		public IconScreenViewModel()
		{
			savedList = new List<ItemList>();
			updateList();
		}

		string searchText;
		public string SearchText {
			get { return searchText; }
			set {
				if (searchText == value)
					return;
				searchText = value;
				if (searchText != null) {
					if (searchText.Length > 0) {
						searchCommand(searchText);
					} else {
						Items = new List<ItemList>(savedList);
					}
				} else {
					Items = new List<ItemList>(savedList);
				}
				OnPropertyChanged("SearchText");
			}
		}

		void searchCommand(string text)
		{
			Items = new List<ItemList>(searchByText(text));
		}

		List<ItemList> searchByText(string text)
		{
			var loweredText = text.ToLower();
			var searchResult = new List<ItemList>();
			foreach (var item in savedList) {
				var itemList = new ItemList();
				if (item != null) {
					foreach (var innerItem in item) {
						var name = innerItem.Name;
						if (name != null) {
							if (name.ToLower().Contains(loweredText)) {
								itemList.Title = item.Title;
								itemList.Add(new ItemModel {
									Icon = innerItem.Icon,
									Name = innerItem.Name,
									TechName = innerItem.TechName,
									IsCircle = innerItem.IsCircle,
									IsNotCircle = innerItem.IsNotCircle
								});
							}
						}
					}
				}
				if (itemList.Count > 0) {
					searchResult.Add(itemList);
				}
			}
			return searchResult;
		}

		List<ItemList> items;
		public List<ItemList> Items {
			get {
				return items;
			}
			set {
				if (items == value)
					return;
				items = value;
				OnPropertyChanged("Items");
			}
		}

		object selectedItem;
		public object SelectedItem {
			get { return items; }
			set {
				if (selectedItem == value)
					return;
				selectedItem = value;
				OpenMenuCommand.Execute(selectedItem);
				OnPropertyChanged("SelectedItem");
			}
		}

		Command filterCommand;
		public Command FilterCommand {
			get {
				return filterCommand ?? (filterCommand = new Command(ExecuteFilterCommand));
			}
		}

		void ExecuteFilterCommand()
		{
			PopupPagesUIController.LaunchIconsFilterPopup((x) => {
				if (x.Result != TR.Cancel && !string.IsNullOrEmpty(x.Result)) {
					if (x.Result == TR.Tr("icons_filter_custom")) {
						Settings.IconsFilter = true;
						Items = loadIcons();
					} else {
						Settings.IconsFilter = false;
						Items = loadIcons(false);
					}
				}
			});
		}

		Command galleryPickCommand;
		public Command GalleryPickCommand {
			get {
				return galleryPickCommand ?? (galleryPickCommand = new Command(ExecuteGalleryPickCommand));
			}
		}

		void ExecuteGalleryPickCommand()
		{

				Device.BeginInvokeOnMainThread(async () => {
					if (!Settings.GalleryPermission) {
						PopupUIController.LaunchMessageBox(
							TR.Tr("icons_gallery_access_request_" + getDevice()),
							TR.Tr("icons_gallery_permission"),
							TR.Tr("allow"),
							TR.Tr("dont_allow"),
							(x) => {
								if (x.Result) {
									Settings.GalleryPermission = true;
									Device.BeginInvokeOnMainThread(async () => await pickPhoto());
								}
							});
					} else {
						await pickPhoto();
					}
				});

		}

		string getDevice()
		{
			switch (Device.RuntimePlatform) {
				case Device.iOS:
					return "ios";
				default:
					return "android";
			}
		}

		async Task pickPhoto()
		{
			var imageStream = await GalleryPicker.PickPhoto();
			var resizedBytes = resizeImage(imageStream);
			if (resizedBytes != null) {
				ImportIconManager.Imported += updateList;
				ImportIconManager.BeginImportingIcon(resizedBytes);
			}
		}

		Command openMenuCommand;
		public Command OpenMenuCommand {
			get {
				return openMenuCommand ?? (openMenuCommand = new Command(ExecuteOpenMenuCommand));
			}
		}

		protected void ExecuteOpenMenuCommand(object item)
		{
			if (item != null) {
				var itemMdl = (ItemModel)item;
				if (itemMdl.IconID != premadeIconIdentifier) {

					Device.BeginInvokeOnMainThread(async () => {

						var result = await Application.Current.MainPage.DisplayActionSheet(
							itemMdl.Name,
							TR.Cancel,
							TR.Tr("popupmenu_delete"),
							TR.Tr("popupmenu_changetitle"),
							TR.Tr("popupmenu_changegroup"),
							TR.Tr("popupmenu_changeshape")
							);

						if (result == TR.Tr("popupmenu_delete")) {
							delete(itemMdl);
						}

						if (result == TR.Tr("popupmenu_changetitle")) {
							changeTitle(itemMdl);
						}

						if (result == TR.Tr("popupmenu_changegroup")) {
							changeGroup(itemMdl);
						}

						if (result == TR.Tr("popupmenu_changeshape")) {
							changeShape(itemMdl);
						}
					});
				}
			}
		}

		Command menuTappedCommand;
		public Command MenuTappedCommand {
			get {
				return menuTappedCommand ?? (menuTappedCommand = new Command(ExecuteMenuTappedCommand));
			}
		}

		void ExecuteMenuTappedCommand(object obj)
		{
			if (obj != null) {
				var item = (PopupItem)obj;
				var itemMdl = (ItemModel)item.Parameter;
				if (itemMdl != null) {
					switch (item.Action) {
						case deleteCommandString:
							delete(itemMdl);
							break;
						case changeTitleCommandString:
							changeTitle(itemMdl);
							break;
						case changeGroupCommandString:
							changeGroup(itemMdl);
							break;
						case changeShapeCommandString:
							changeShape(itemMdl);
							break;
					}
				}
			}
		}

		void updateList()
		{
			Device.BeginInvokeOnMainThread(() =>
										   Items = new List<ItemList>(loadIcons(Settings.IconsFilter)));
		}

		byte[] resizeImage(Stream imageStream)
		{
			if (imageStream != null) {
				using (var ms = new System.IO.MemoryStream()) {
					imageStream.CopyTo(ms);
					var imageBytes = ms.ToArray();
					return MediaService.ResizeImage(
						imageBytes,
						GConsts.RESIZE_ICON_WIDTH,
						GConsts.RESIZE_ICON_HEIGHT
					);
				}
			}
			return null;
		}

		void delete(ItemModel item)
		{
			bool isUsed = false;
			var itemList = BL.GetAllItems();
			var itemFromList = itemList.SingleOrDefault(
				x => x.Icon == ImageManager.ConvertIconName2IconPath(item.TechName)
			);
			if (itemFromList != null) {
				isUsed = true;
			}
			if (isUsed) {
				PopupPagesUIController.LaunchIconIsUsedMessage();
			} else {
				PopupPagesUIController.LaunchDeleteIconAcceptMessage((obj) => {
					if (obj.Result) {
						Device.BeginInvokeOnMainThread(() => {
							var success = BL.DeleteIcon(item.IconID);
							if (success) {
								PopupPagesUIController.LaunchDeleteIconSuccessMessage(
									x => Items = loadIcons(Settings.IconsFilter)
								);
							} else {
								PopupPagesUIController.LaunchDeleteIconFailureMessage();
							}
						});
					}
				});
			}
		}

		void changeTitle(ItemModel item)
		{
			PopupPagesUIController.LaunchChangeIconNameEntryPopup((sender, e) => {
				if (e.OkClicked) {
					if (!string.IsNullOrEmpty(e.Text)) {
						var result = BL.UpdateIcon(item.IconID, e.Text);
						if (result) {
							PopupPagesUIController.LaunchChangeIconNameSuccessMessage(
								x => Items = loadIcons(Settings.IconsFilter)
							);
						} else {
							PopupPagesUIController.LaunchChangeIconNameFailureMessage();
						}
					}
				}
			});
		}

		void changeGroup(ItemModel item)
		{
			PopupPagesUIController.LaunchChangeGroupPopup((x) => {
				if (x.Result != TR.Cancel) {
					Device.BeginInvokeOnMainThread(() => {
						if (!string.IsNullOrEmpty(x.Result) && x.Result != TR.Cancel) {
							if (x.Result == TR.Tr("create_icon_group_popup_item")) {
								PopupPagesUIController.LaunchCreateGroupEntryPopup((sender, e) => {
									if (e.OkClicked) {
										if (!string.IsNullOrEmpty(e.Text)) {
											var groups = BL.GetGroups();
											var lastID = groups.Max(g => g.GroupID);
											var result = BL.InsertGroup(lastID + 1, e.Text);
											if (result) {
												PopupPagesUIController.LaunchCreateGroupSuccessMessage(
													s => Items = loadIcons(Settings.IconsFilter)
												);
												var groupID = lastID + 1;
												var res = BL.UpdateIcon(item.IconID, null, null, groupID);
												PopupPagesUIController.LaunchChangeGroupOnResultMessage(res,
																										(y) => Items = loadIcons(Settings.IconsFilter));
											} else {
												PopupPagesUIController.LaunchCreateGroupFailureMessage();
											}
										}
									}
								});
							} else {
								var groupID = IconGroups.GetIDByCategoryName(x.Result);
								var result = BL.UpdateIcon(item.IconID, null, null, groupID);
								PopupPagesUIController.LaunchChangeGroupOnResultMessage(result,
																						(y) => Items = loadIcons(Settings.IconsFilter));
							}
						}
					});
				}
			});
		}

		void changeShape(ItemModel item)
		{
			var icons = BL.GetIcons();
			var icon = icons.SingleOrDefault(x => x.IconID == item.IconID);
			bool result = false;

			if (icon.IsCircle) {
				result = BL.UpdateIcon(item.IconID, name: null, blob: null, groupID: -1, isCircle: 0);
			} else {
				result = BL.UpdateIcon(item.IconID, name: null, blob: null, groupID: -1, isCircle: 1);
			}

			if (result) {
				PopupPagesUIController.LaunchChangeShapeSuccessMessage(x => Items = loadIcons(Settings.IconsFilter));
			} else {
				PopupPagesUIController.LaunchChangeShapeFailureMessage();
			}
		}

		List<ItemList> loadIcons(bool onlyCustom = true)
		{
			var itemList = new List<ItemList>();
			var groups = BL.GetGroups();
			foreach (var group in groups) {
				var item = new ItemList {
					Title = TR.Tr(group.Name)
				};

				if (!onlyCustom) {
					loadPremadeIcons(group, item);
				}

				loadCustomIcons(group, item);

				item.Sort((x, y) => string.Compare(x.Name, y.Name));

				if (item.Count > 0) {
					itemList.Add(item);
				}
			}

			itemList.Sort((x, y) => string.Compare(x.Title, y.Title));
			savedList = itemList;
			return itemList;
		}

		void loadPremadeIcons(NSWGroup group, ItemList item)
		{
			var iconsPremade = IconGroups.GetIconsWithGroups();
			var iconsPremadeGroupped = iconsPremade.GroupBy(x => x.Value);
			var premadeIconsList = iconsPremadeGroupped.Where(x => x.Key == group.Name);

			foreach (var iconCategory in premadeIconsList) {
				foreach (var icon in iconCategory) {
					var key = icon.Key;
					item.Add(new ItemModel {
						Name = TR.Tr(key),
						TechName = key,
						Icon = ImageSource.FromStream(() => NSWRes.GetImage(IconHandler.GetPathForItemIcon(key))),
						IsNotCircle = true,
						IsCircle = false,
						IconID = premadeIconIdentifier,
						IsLocked = true
					});
				}
			}
		}

		void loadCustomIcons(NSWGroup group, ItemList item)
		{
			var iconsCustom = BL.GetIcons();
			var iconsCustomGrouped = iconsCustom.GroupBy(x => x.GroupID);
			var customIconList = iconsCustomGrouped.Where(x => x.Key == group.GroupID);

			foreach (var iconCategory in customIconList) {
				foreach (var icon in iconCategory) {
					if (!icon.Deleted) {
						item.Add(new ItemModel {
							Name = icon.Name,
							TechName = icon.IconID,
							IconID = icon.IconID,
							GroupID = icon.GroupID,
							IsCircle = icon.IsCircle,
							IsNotCircle = icon.IsNotCircle,
							Icon = ImageSource.FromStream(() => new MemoryStream(icon.Icon)),
							IsLocked = false
						});
					}
				}
			}
		}
	}
}