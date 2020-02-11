using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using NSWallet.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

using System.Linq;
using NSWallet.Enums;
using NSWallet.NetStandard.Helpers;

namespace NSWallet
{
	public class MainScreenChooseIconViewModel : ViewModel
	{
		INavigation navigation;
		NSWItemType itemType;
		bool isEdit;
		List<ItemList> savedList { get; set; }

		public MainScreenChooseIconViewModel(INavigation navigation, NSWItemType itemType, bool isEdit, string name)
		{
			this.itemType = itemType;
			this.navigation = navigation;
			this.isEdit = isEdit;
			NameProperty = name;

			savedList = new List<ItemList>();
			Items = new List<ItemList>(getItems());
			Folders = new List<ItemModel>();
		}

		public string Title {
			get {
				switch (itemType) {
					case NSWItemType.Item: return TR.Tr("new_item_icon") + String.Format(" '{0}'", NameProperty);
					case NSWItemType.Folder: return TR.Tr("new_folder_icon") + String.Format(" '{0}'", NameProperty);
					default: return null;
				}
			}
		}

		object selectedItem;
		public object SelectedItem {
			get { return items; }
			set {
				if (selectedItem == value)
					return;

				selectedItem = value;

				CreateCommand.Execute(selectedItem);

				OnPropertyChanged("SelectedItem");
			}
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

		string nameProperty;
		public string NameProperty {
			get { return nameProperty; }
			set {
				if (nameProperty == value)
					return;
				nameProperty = value;
				OnPropertyChanged("NameProperty");
			}
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

		List<ItemList> getItems()
		{
			var itemList = new List<ItemList>();
			var groups = BL.GetGroups();
			var iconsPremade = IconGroups.GetIconsWithGroups();
			var iconsPremadeGroupped = iconsPremade.GroupBy(x => x.Value);
			var iconsCustom = BL.GetIcons();
			var iconsCustomGrouped = iconsCustom.GroupBy(x => x.GroupID);
			foreach (var group in groups) {

				var item = new ItemList {
					Title = TR.Tr(group.Name)
				};

				var premadeIconsList = iconsPremadeGroupped.Where(x => x.Key == group.Name);

				foreach (var iconCategory in premadeIconsList) {
					foreach (var icon in iconCategory) {
						var key = icon.Key;
						item.Add(new ItemModel {
							Name = TR.Tr(key),
							TechName = key,
							Icon = ImageSource.FromStream(() => NSWRes.GetImage(IconHandler.GetPathForItemIcon(key))),
							IsCircle = false,
							IsNotCircle = true
						});
					}
				}

				var customIconList = iconsCustomGrouped.Where(x => x.Key == group.GroupID);

				foreach (var iconCategory in customIconList) {
					foreach(var icon in iconCategory) {
						if (!icon.Deleted) {
							item.Add(new ItemModel {
								Name = icon.Name,
								TechName = icon.IconID,
								Icon = ImageSource.FromStream(() => new System.IO.MemoryStream(icon.Icon)),
								IsCircle = icon.IsCircle,
								IsNotCircle = icon.IsNotCircle
							});
						}
					}
				}

				item.Sort((x, y) => string.Compare(x.Name, y.Name));

				if (item.Count > 0) {
					itemList.Add(item);
				}
			}

			itemList.Sort((x, y) => string.Compare(x.Title, y.Title));
			savedList = itemList;
			return itemList;
			//var itemList = new List<ItemList>();
			//var resources = NSWRes.GetResourceNames();
			//var iconCategories = IconGroups.GetIconsWithGroups();

			//var commonCategory = new ItemList();
			//commonCategory.Title = TR.Tr("category_common");
			//var internetCategory = new ItemList();
			//internetCategory.Title = TR.Tr("category_internet");
			//var travellingCategory = new ItemList();
			//travellingCategory.Title = TR.Tr("category_travelling");
			//var financesCategory = new ItemList();
			//financesCategory.Title = TR.Tr("category_finances");
			//var technologiesCategory = new ItemList();
			//technologiesCategory.Title = TR.Tr("category_technologies");
			//var socialCategory = new ItemList();
			//socialCategory.Title = TR.Tr("category_social");
			//var gamesCategory = new ItemList();
			//gamesCategory.Title = TR.Tr("category_games");
			//var cloudsCategory = new ItemList();
			//cloudsCategory.Title = TR.Tr("category_clouds");
			//var developmentCategory = new ItemList();
			//developmentCategory.Title = TR.Tr("category_development");

			//foreach (var res in resources) {
			//	if (res.StartsWith(NSWRes.GetRunspaceAssetsPath() + GConsts.ICONS_ITEMS_PATH + "icon_",
			//					   System.StringComparison.CurrentCulture)
			//		&& res.EndsWith("_huge.png", System.StringComparison.CurrentCulture)) {
			//		var itemsCategories = new List<ItemModel>();

			//		if (res.Contains("document")) {
			//			commonCategory.Insert(0, new ItemModel {
			//				Name = TR.Tr(res.Substring(55, res.Length - 64)),
			//				TechName = res.Substring(55, res.Length - 64),
			//				Icon = ImageSource.FromStream(() => NSWRes.GetImage(res.Remove(0, 38)))
			//			});
			//		} else {
			//			var itemModel = new ItemModel {
			//				Name = TR.Tr(res.Substring(55, res.Length - 64)),
			//				TechName = res.Substring(55, res.Length - 64),
			//				Icon = ImageSource.FromStream(() => NSWRes.GetImage(res.Remove(0, 38)))
			//			};

			//			var techName = itemModel.TechName;
			//			var iconsWithGroups = IconGroups.GetIconsWithGroups();
			//			var group = iconsWithGroups[techName];
			//			if (group != null) {
			//				switch (group) {
			//					case "category_common":
			//						commonCategory.Add(itemModel);
			//						break;
			//					case "category_internet":
			//						internetCategory.Add(itemModel);
			//						break;
			//					case "category_travelling":
			//						travellingCategory.Add(itemModel);
			//						break;
			//					case "category_finances":
			//						financesCategory.Add(itemModel);
			//						break;
			//					case "category_technologies":
			//						technologiesCategory.Add(itemModel);
			//						break;
			//					case "category_social":
			//						socialCategory.Add(itemModel);
			//						break;
			//					case "category_games":
			//						gamesCategory.Add(itemModel);
			//						break;
			//					case "category_clouds":
			//						cloudsCategory.Add(itemModel);
			//						break;
			//					case "category_development":
			//						developmentCategory.Add(itemModel);
			//						break;
			//				}
			//			}
			//		}
			//	}
			//}

			//var customImages = BL.GetIcons();
			//var groups = BL.GetGroups();
			//foreach (var customImage in customImages) {

			//	var group = groups.Find(x => x.GroupID == customImage.GroupID);

			//	var nswFormsIcon = new NSWFormsIconModel(customImage);

			//	var itemModel = new ItemModel {
			//		Icon = nswFormsIcon.Icon,
			//		Name = nswFormsIcon.Name,
			//		TechName = nswFormsIcon.IconID
			//	};

			//	if (group != null) {
			//		switch (group.Name) {
			//			case "category_common":
			//				commonCategory.Add(itemModel);
			//				break;
			//			case "category_internet":
			//				internetCategory.Add(itemModel);
			//				break;
			//			case "category_travelling":
			//				travellingCategory.Add(itemModel);
			//				break;
			//			case "category_finances":
			//				financesCategory.Add(itemModel);
			//				break;
			//			case "category_technologies":
			//				technologiesCategory.Add(itemModel);
			//				break;
			//			case "category_social":
			//				socialCategory.Add(itemModel);
			//				break;
			//			case "category_games":
			//				gamesCategory.Add(itemModel);
			//				break;
			//			case "category_clouds":
			//				cloudsCategory.Add(itemModel);
			//				break;
			//			case "category_development":
			//				developmentCategory.Add(itemModel);
			//				break;
			//		}
			//	}
			//}

			//commonCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(commonCategory);
			//internetCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(internetCategory);
			//travellingCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(travellingCategory);
			//financesCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(financesCategory);
			//technologiesCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(technologiesCategory);
			//socialCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(socialCategory);
			//gamesCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(gamesCategory);
			//cloudsCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(cloudsCategory);
			//developmentCategory.Sort((x, y) => string.Compare(x.Name, y.Name));
			//itemList.Add(developmentCategory);

			//itemList.Sort((x, y) => string.Compare(x.Title, y.Title));
			//savedList = itemList;
			//return itemList;
		}

		List<ItemModel> folders;
		public List<ItemModel> Folders {
			get {
				var resources = NSWRes.GetResourceNames();

				foreach (var res in resources) {
					if (res.StartsWith(NSWRes.GetRunspaceAssetsPath() + GConsts.ICONS_ITEMS_PATH + "icon_folder",
									   System.StringComparison.CurrentCulture)) {
						folders.Add(new ItemModel {
							Name = TR.Tr(res.Substring(55, res.Length - 59)),
							TechName = res.Substring(55, res.Length - 59),
							Icon = ImageSource.FromStream(() => NSWRes.GetImage(res.Remove(0, 38)))
						});
					}
				}

				return folders;
			}
			set {
				if (folders == value)
					return;

				folders = value;

				OnPropertyChanged("Folders");
			}
		}

		Command createCommand;
		public Command CreateCommand {
			get {
				return createCommand ?? (createCommand = new Command(ExecuteCreateCommand));
			}
		}

		protected void ExecuteCreateCommand(object item)
		{
			var itemObj = (ItemModel)item;
			string id = null;

			if (!isEdit) {
				if (itemType == NSWItemType.Item) {
					id = BL.AddItem(NameProperty, itemObj.TechName, false);
				}

				if (itemType == NSWItemType.Folder) {
					id = BL.AddItem(NameProperty, itemObj.TechName, true);
				}

				if (!string.IsNullOrEmpty(id)) {
					BL.SetCurrentItemID(id);
				}

				MessagingCenter.Send<MainScreenChooseIconViewModel>(this, "/reloaditems");
			} else {
				var currentItemID = BL.CurrentItemID;



				BL.UpdateItemIcon(currentItemID, itemObj.TechName);

				MessagingCenter.Send<MainScreenChooseIconViewModel>(this, "/reloadicon");
			}

			navigation.PopModalAsync(true);
			navigation.PopModalAsync(true);
		}
	}
}