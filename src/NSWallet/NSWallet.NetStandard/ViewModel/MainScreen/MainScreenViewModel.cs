using System;
using System.Collections.Generic;
using System.ComponentModel;
using NSWallet.Shared;
using Xamarin.Forms;
using NSWallet.Helpers;
using System.IO;
using NSWallet.Model;
using System.Linq;
using static NSWallet.NSWFormsItemModel;
using NSWallet.Enums;
using NSWallet.NetStandard.Helpers.UI.Popups.Pages.ExportImport;

namespace NSWallet
{
	public static class StateHandler
	{
		public static bool CopyFieldLocallyActivated;
		public static bool DeleteFieldActivated;
	}

	public class MainScreenViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;


		public Action<bool> SearchEntryShowHideCommandCallback { get; set; }
		public Action LaunchEditTitlePopupCommandCallback { get; set; }
		public Action HideEditTitlePopupCommandCallback { get; set; }
		public Action LaunchFieldEditTitlePopupCommandCallback { get; set; }
		public Action HideFieldEditTitlePopupCommandCallback { get; set; }
		public Action LaunchSettingsPopupCommandCallback { get; set; }
		public Action HideSettingsCommandCallback { get; set; }
		public Action LaunchPopupCommandCallback { get; set; }
		public Action LaunchMenuPopupCommandCallback { get; set; }
		public Action HidePopupCommandCallback { get; set; }
		public Action HideMenuPopupCommandCallback { get; set; }
		public Action<string, string, string, ImageSource> LaunchFieldMenuCommandCallback { get; set; }
		public Action HideFieldMenuCommandCallback { get; set; }
		public Action<string, string, string> MessageCommand { get; set; }
		public Action<string, string> ConfirmDeleteMessageCommand { get; set; }
		public Action<string, string> MessageBoxNotEmptyFolderCommand { get; set; }
		public Action LaunchNotImplementedCommand { get; set; }
		public Action<bool> LaunchShowHideRestrictionCommand { get; set; }
		public Action ShowEmptyAddButtonCommand { get; set; }
		public Action HideEmptyAddButtonCommand { get; set; }
		public Action<bool> SetSearchIconCommand { get; set; }

		ItemTypes currentItemType;

		INavigation navigation;

		string pasteItemID;
		string pasteFieldID;
		string pasteFromItemID;

		public MainScreenViewModel(INavigation navigation)
		{
			this.navigation = navigation;
			currentItemType = ItemTypes.Folder;
			BL.SetExpirationSettings(Settings.IsExpiringSoon, Settings.ExpiringPeriod);
			BL.SetRecentlyViewSetting(Settings.IsRecentlyViewed);
			BL.SetMostlyViewSetting(Settings.IsMostlyViewed);
			AddButtonVisible = true;
			IsRoot = true;
			IsSearchEnabled = false;
			PasteText = TR.Tr("main_paste_item");

			IsEditItemPanel = false;
			IsEditFolderPanel = false;

			//IsCopyEnabled = false;
			//IsMoveEnabled = false;

			setItemsList(BL.GetListByParentID(GConsts.ROOTID, true));

			MessagingCenter.Subscribe<ReorderFieldViewModel>(this, "/reloaditems", (sender) => {
				SetFieldsList(BL.GetCurrentItem().Fields);
			});

			MessagingCenter.Subscribe<MainScreenChooseIconViewModel>(this, "/reloaditems", (sender) => {
				BL.ResetData(true, false, false);
				setItemsList(BL.GetListByParentID(BL.CurrentItemID, false));
			});

			MessagingCenter.Subscribe<ManageFieldViewModel>(this, "/reloaditems", (sender) => {
				BL.ResetData(false, true, false);
				SetFieldsList(BL.GetCurrentItem().Fields);
			});

			MessagingCenter.Subscribe<MainScreenChooseIconViewModel>(this, "/reloadicon", (sender) => {
				BL.ResetData(true, false, false);
				CurrentItemIcon = getImage(BL.GetCurrentItem().Icon);
				//CurrentItemIcon = ImageSource.FromStream(() => NSWRes.GetImage(BL.GetCurrentItem().Icon));
			});

			IsLocalClipboardActivated = false;


			SearchEntryShowHideCommandCallback = (x) => { };
			LaunchEditTitlePopupCommandCallback = () => { };
			HideEditTitlePopupCommandCallback = () => { };
			LaunchSettingsPopupCommandCallback = () => { };
			HideSettingsCommandCallback = () => { };
			LaunchPopupCommandCallback = () => { };
			LaunchMenuPopupCommandCallback = () => { };
			HidePopupCommandCallback = () => { };
			HideMenuPopupCommandCallback = () => { };
			LaunchFieldMenuCommandCallback = (x, y, z, h) => { };
			HideFieldMenuCommandCallback = () => { };
			MessageCommand = (x, y, z) => { };
			ConfirmDeleteMessageCommand = (arg1, arg2) => { };
			MessageBoxNotEmptyFolderCommand = (arg1, arg2) => { };
			LaunchShowHideRestrictionCommand = (x) => { };
			ShowEmptyAddButtonCommand = () => { };
			HideEmptyAddButtonCommand = () => { };
			SetSearchIconCommand = (x) => { };
		}

		NSWItem GetRecentlyFolder()
		{
			var nswItem = new NSWItem {
				ItemID = "recentlyfolder_id",
				ParentID = GConsts.ROOTID,
				Name = TR.Tr("recentlyviewed_folder"),
				CreateTimestamp = DateTime.Now,
				UpdateTimestamp = DateTime.Now,
				Folder = true,
				Icon = "Icons.items.icon_folderrecent.png",
				Deleted = false
			};

			return nswItem;
		}

		void CreateRecentlyFolder()
		{
			if (Settings.IsRecentlyViewed)
				Items.Insert(0, new NSWFormsItemModel(GetRecentlyFolder()));
		}

		List<NSWFormsItemModel> items;
		public List<NSWFormsItemModel> Items {
			get { return items; }
			set {
				if (items == value)
					return;
				items = value;
				OnPropertyChanged("Items");
			}
		}

		string itemToScrollTo;
		public string ItemToScrollTo {
			get {
				if (itemToScrollTo == null) {
					if (Items != null) {
						if (Items.Count > 0) {
							return Items[0].ItemID;
						}
					}
				}
				return itemToScrollTo;
			}
			set {
				if (itemToScrollTo == value)
					return;
				itemToScrollTo = value;
			}
		}

		string emptyAddButtonText;
		public string EmptyAddButtonText {
			get { return emptyAddButtonText; }
			set {
				if (emptyAddButtonText == value)
					return;
				emptyAddButtonText = value;
				OnPropertyChanged("EmptyAddButtonText");
			}
		}

		bool listIsNotEmpty;
		public bool ListIsNotEmpty {
			get { return listIsNotEmpty; }
			set {
				if (listIsNotEmpty == value)
					return;
				listIsNotEmpty = value;
				OnPropertyChanged("ListIsNotEmpty");
			}
		}

		bool listIsEmpty;
		public bool ListIsEmpty {
			get { return listIsEmpty; }
			set {
				if (listIsEmpty == value)
					return;
				listIsEmpty = value;
				OnPropertyChanged("ListIsEmpty");
			}
		}

		bool isHeaderVisible;
		public bool IsHeaderVisible {
			get { return isHeaderVisible; }
			set {
				isHeaderVisible = value;
				OnPropertyChanged("IsHeaderVisible");
			}
		}

		bool isSearchEnabled;
		public bool IsSearchEnabled {
			get { return isSearchEnabled; }
			set {
				isSearchEnabled = value;
				OnPropertyChanged("IsSearchEnabled");
			}
		}

		bool isIconCircle;
		public bool IsIconCircle {
			get { return isIconCircle; }
			set {
				isIconCircle = value;
				OnPropertyChanged("IsIconCircle");
			}
		}

		bool isIconNotCircle;
		public bool IsIconNotCircle {
			get { return isIconNotCircle; }
			set {
				isIconNotCircle = value;
				OnPropertyChanged("IsIconNotCircle");
			}
		}

		object selectedItem;
		public object SelectedItem {
			get { return selectedItem; }
			set {
				if (selectedItem == value)
					return;
				selectedItem = value;
				OnPropertyChanged("SelectedItem");
				SelectedItemCommand.Execute(selectedItem);
			}
		}

		ImageSource currentItemIcon;
		public ImageSource CurrentItemIcon {
			get { return currentItemIcon; }
			set {
				if (currentItemIcon == value)
					return;
				currentItemIcon = value;
				OnPropertyChanged("CurrentItemIcon");
			}
		}

		string pasteText;
		public string PasteText {
			get { return pasteText; }
			set {
				if (pasteText == value)
					return;
				pasteText = value;
				OnPropertyChanged("PasteText");
			}
		}

		string path;
		public string Path {
			get { return path; }
			set {
				if (path == value)
					return;
				path = value;
				OnPropertyChanged("Path");
			}
		}

		string title;
		public string Title {
			get { return title; }
			set {
				if (title == value)
					return;
				title = value;
				OnPropertyChanged("Title");
			}
		}

		string tempFieldTitle;
		public string TempFieldTitle {
			get { return tempFieldTitle; }
			set {
				if (tempFieldTitle == value)
					return;
				tempFieldTitle = value;
				OnPropertyChanged("TempFieldTitle");
			}
		}

		bool addButtonVisible;
		public bool AddButtonVisible {
			get { return addButtonVisible; }
			set {
				if (addButtonVisible == value)
					return;
				addButtonVisible = value;
				OnPropertyChanged("AddButtonVisible");
			}
		}

		TimeSpan tempFieldTitleTime;
		public TimeSpan TempFieldTitleTime {
			get { return tempFieldTitleTime; }
			set {
				if (tempFieldTitleTime == value)
					return;
				tempFieldTitleTime = value;
				OnPropertyChanged("TempFieldTitleTime");
			}
		}

		DateTime tempFieldTitleDate;
		public DateTime TempFieldTitleDate {
			get { return tempFieldTitleDate; }
			set {
				if (tempFieldTitleDate == value)
					return;
				tempFieldTitleDate = value;
				OnPropertyChanged("TempFieldTitleDate");
			}
		}

		string tempTitle;
		public string TempTitle {
			get { return tempTitle; }
			set {
				if (tempTitle == value)
					return;
				tempTitle = value;
				OnPropertyChanged("TempTitle");
			}
		}

		ImageSource icon;
		public ImageSource Icon {
			get { return icon; }
			set {
				if (icon == value)
					return;
				icon = value;
				OnPropertyChanged("Icon");
			}
		}

		string searchText;
		public string SearchText {
			get { return searchText; }
			set {
				if (searchText == value)
					return;
				searchText = value;
				if (searchText == "") { 					setItemsList(null, false); 				} else { 					SearchCommand.Execute(searchText); 				}
				OnPropertyChanged("SearchText");
			}
		}

		string clipboardName;
		public string ClipboardName {
			get { return clipboardName; }
			set {
				if (clipboardName == value)
					return;
				clipboardName = value;
				OnPropertyChanged("ClipboardName");
			}
		}

		ImageSource clipboardIcon;
		public ImageSource ClipboardIcon {
			get { return clipboardIcon; }
			set {
				if (clipboardIcon == value)
					return;
				clipboardIcon = value;
				OnPropertyChanged("ClipboardIcon");
			}
		}

		bool isClipboardIconCircle;
		public bool IsClipboardIconCircle {
			get { return isClipboardIconCircle; }
			set {
				if (isClipboardIconCircle == value)
					return;
				isClipboardIconCircle = value;
				OnPropertyChanged("IsClipboardIconCircle");
			}
		}

		bool isClipboardIconNotCircle;
		public bool IsClipboardIconNotCircle {
			get { return isClipboardIconNotCircle; }
			set {
				if (isClipboardIconNotCircle == value)
					return;
				isClipboardIconNotCircle = value;
				OnPropertyChanged("IsClipboardIconNotCircle");
			}
		}

		bool isLocalClipboardActivated;
		public bool IsLocalClipboardActivated {
			get { return isLocalClipboardActivated; }
			set {
				if (isLocalClipboardActivated == value)
					return;
				isLocalClipboardActivated = value;
				OnPropertyChanged("IsLocalClipboardActivated");
			}
		}

		string unavailableClipboardText;
		public string UnavailableClipboardText {
			get { return unavailableClipboardText; }
			set {
				if (unavailableClipboardText == value)
					return;
				unavailableClipboardText = value;
				OnPropertyChanged("UnavailableClipboardText");
			}
		}

		bool isUnavailableClipboard;
		public bool IsUnavailableClipboard {
			get { return isUnavailableClipboard; }
			set {
				if (isUnavailableClipboard == value)
					return;
				isUnavailableClipboard = value;
				OnPropertyChanged("IsUnavailableClipboard");
			}
		}

		bool isEditItemPanel;
		public bool IsEditItemPanel {
			get { return isEditItemPanel; }
			set {
				if (isEditItemPanel == value)
					return;
				isEditItemPanel = value;
				OnPropertyChanged("IsEditItemPanel");
			}
		}

		bool isEditFolderPanel;
		public bool IsEditFolderPanel {
			get { return isEditFolderPanel; }
			set {
				if (isEditFolderPanel == value)
					return;
				isEditFolderPanel = value;
				OnPropertyChanged("IsEditFolderPanel");
			}
		}

		void setUnavailableClipboard(bool isUnavailable, string type = null)
		{
			IsUnavailableClipboard = isUnavailable;
			if (type != null) {
				switch (type) {
					case GConsts.ITEM:
						UnavailableClipboardText = TR.Tr("copy_locally_unavailable_item");
						break;
					case GConsts.FOLDER:
						UnavailableClipboardText = TR.Tr("copy_locally_unavailable_folder");
						break;
					case GConsts.FIELD:
						UnavailableClipboardText = TR.Tr("copy_locally_unavailable_field");
						break;
				}
			}
		}

		bool isMoveEnabled;
		public bool IsMoveEnabled {
			get { return isMoveEnabled; }
			set {
				if (isMoveEnabled == value)
					return;
				isMoveEnabled = value;
				OnPropertyChanged("IsMoveEnabled");
			}
		}

		bool isCopyEnabled;
		public bool IsCopyEnabled {
			get { return isCopyEnabled; }
			set {
				if (isCopyEnabled == value)
					return;
				isCopyEnabled = value;
				OnPropertyChanged("IsCopyEnabled");
			}
		}

		Command searchCommand;
		public Command SearchCommand {
			get {
				return searchCommand ?? (searchCommand = new Command(ExecuteSearchCommand));
			}
		}

		protected void ExecuteSearchCommand(object obj)
		{
			if (obj != null) {
				if (!string.IsNullOrEmpty(obj.ToString())) {
					var searchPhrase = obj.ToString();
					var count = BL.CountSearchPhrase(searchPhrase);

					if (count < 3) {
						LaunchShowHideRestrictionCommand(true);
						setItemsList(null, false);
					} else {
						LaunchShowHideRestrictionCommand(false);
						var nswItems = BL.Search(searchText);
						setItemsList(nswItems, false);
					}
					HideEmptyAddButtonCommand.Invoke();
				}
			}
			//LaunchShowHideRestrictionCommand(true);
			//setItemsList(null, false);
			//HideEmptyAddButtonCommand.Invoke();
		}

		Command searchLaunchCommand;
		public Command SearchLaunchCommand {
			get {
				return searchLaunchCommand ?? (searchLaunchCommand = new Command(ExecuteSearchLaunchCommand));
			}
		}

		bool show = false;
		protected void ExecuteSearchLaunchCommand()
		{
			if (!IsLocalClipboardActivated) {

				SearchEntryShowHideCommandCallback.Invoke(false);
				SearchText = null;
				if (show) {
					if (searchID != null) {
						SearchEntryShowHideCommandCallback.Invoke(true);
						BL.SetCurrentItemID(searchID);
						searchID = null;
					}
					setItemsList(BL.GetCurrentItems());
					IsSearchEnabled = false;
					show = false;
					SetHeader();
					AddButtonVisible = true;
					LaunchShowHideRestrictionCommand.Invoke(false);
					ShowEmptyAddButtonCommand.Invoke();
				} else {
					setItemsList(null);
					LaunchShowHideRestrictionCommand.Invoke(true);
					IsSearchEnabled = true;
					show = true;
					HideHeader();
					AddButtonVisible = false;
					HideEmptyAddButtonCommand.Invoke();
				}

			}
		}

		Command launchPopupCommand;
		public Command LaunchPopupCommand {
			get {
				return launchPopupCommand ?? (launchPopupCommand = new Command(ExecuteLaunchPopupCommand));
			}
		}

		protected void ExecuteLaunchPopupCommand()
		{
			if (!IsLocalClipboardActivated && !IsSearchEnabled) {
				if (BL.GetCurrentItem().Folder) {
					LaunchPopupCommandCallback.Invoke();
				} else {
					var nswItem = BL.GetCurrentItem();
					AppPages.CreateField(navigation, false, nswItem);
				}
			}
		}

		Command createItemCommand;
		public Command CreateItemCommand {
			get {
				return createItemCommand ?? (createItemCommand = new Command(ExecuteCreateItemCommand));
			}
		}

		protected void ExecuteCreateItemCommand()
		{
			//HidePopupCommandCallback.Invoke();
			AppPages.CreateItem(navigation);
		}

		Command createFolderCommand;
		public Command CreateFolderCommand {
			get {
				return createFolderCommand ?? (createFolderCommand = new Command(ExecuteCreateFolderCommand));
			}
		}

		protected void ExecuteCreateFolderCommand()
		{
			HidePopupCommandCallback.Invoke();
			AppPages.CreateFolder(navigation);
		}

		Command hideCommand;
		public Command HideCommand {
			get {
				return hideCommand ?? (hideCommand = new Command(ExecuteHideCommand));
			}
		}

		protected void ExecuteHideCommand()
		{
			HidePopupCommandCallback.Invoke();
		}

		Command launchMenuPopupCommand;
		public Command LaunchMenuPopupCommand {
			get {
				return launchMenuPopupCommand ?? (launchMenuPopupCommand = new Command(ExecuteLaunchMenuPopupCommand));
			}
		}

		protected void ExecuteLaunchMenuPopupCommand()
		{
			LaunchMenuPopupCommandCallback.Invoke();
		}

		Command hideMenuPopupCommand;
		public Command HideMenuPopupCommand {
			get {
				return hideMenuPopupCommand ?? (hideMenuPopupCommand = new Command(ExecuteHideMenuPopupCommand));
			}
		}

		protected void ExecuteHideMenuPopupCommand()
		{
			HideMenuPopupCommandCallback.Invoke();
		}

		Command createCommand;
		public Command CreateCommand {
			get {
				return createCommand ?? (createCommand = new Command(ExecuteCreateCommand));
			}
		}

		protected void ExecuteCreateCommand()
		{

		}

		private Command changeTitleCommand;
		public Command ChangeTitleCommand {
			get {
				return changeTitleCommand ?? (changeTitleCommand = new Command(ExecuteChangeTitleCommand));
			}
		}

		void ExecuteChangeTitleCommand()
		{
			TempTitle = Title;
			//HideSettingsCommandCallback.Invoke();
			LaunchEditTitlePopupCommandCallback.Invoke();
		}

		private Command changeIconCommand;
		public Command ChangeIconCommand {
			get {
				return changeIconCommand ?? (changeIconCommand = new Command(ExecuteChangeIconCommand));
			}
		}

		void ExecuteChangeIconCommand()
		{
			//HideSettingsCommandCallback.Invoke();

			var currentItem = BL.GetCurrentItem();

			var itemType = NSWItemType.Item;

			if (currentItem.Folder) {
				itemType = NSWItemType.Folder;
			}

			AppPages.EditItemIcon(navigation, itemType, true, currentItem.Name);
		}

		private Command deleteFieldCommand;
		public Command DeleteFieldCommand {
			get {
				return deleteFieldCommand ?? (deleteFieldCommand = new Command(ExecuteDeleteFieldCommand));
			}
		}

		void ExecuteDeleteFieldCommand()
		{
			HideFieldMenuCommandCallback.Invoke();
			MessageCommand.Invoke("Confirmation", "Are you sure you want to delete this field?", "/deletefield");
		}

		private Command deleteFieldConfirmCommand;
		public Command DeleteFieldConfirmCommand {
			get {
				return deleteFieldConfirmCommand ?? (deleteFieldConfirmCommand = new Command(ExecuteDeleteFieldConfirmCommand));
			}
		}

		void ExecuteDeleteFieldConfirmCommand()
		{
			BL.DeleteField(BL.CurrentFieldID);
			var currentItem = BL.GetItemByID(BL.CurrentItemID);
			currentItem.ClearFields();
			SetFieldsList(currentItem.Fields);
			SetHeader();
		}

		private Command changeFieldCommand;
		public Command ChangeFieldCommand {
			get {
				return changeFieldCommand ?? (changeFieldCommand = new Command(ExecuteChangeFieldCommand));
			}
		}

		void ExecuteChangeFieldCommand(object fieldValue)
		{
			//HideFieldMenuCommandCallback.Invoke();
			var nswField = BL.GetCurrentItem().Fields.Find(x => x.FieldID == BL.CurrentFieldID);
			var nswItem = BL.GetCurrentItem();
			AppPages.UpdateField(navigation, true, nswItem, nswField.FieldID, fieldValue.ToString());
		}

		private Command deleteCommand;
		public Command DeleteCommand {
			get {
				return deleteCommand ?? (deleteCommand = new Command(ExecuteDeleteCommand));
			}
		}

		void ExecuteDeleteCommand()
		{
			//HideSettingsCommandCallback.Invoke();
			var currentItem = BL.GetCurrentItem();
			MessageCommand.Invoke("Confirmation", "Are you sure you want to delete item " + currentItem.Name + "?", "/delete");
		}

		private Command deleteConfirmCommand;
		public Command DeleteConfirmCommand {
			get {
				return deleteConfirmCommand ?? (deleteConfirmCommand = new Command(ExecuteDeleteConfirmCommand));
			}
		}

		void ExecuteDeleteConfirmCommand()
		{
			var currentItem = BL.GetCurrentItem();


			if (currentItem.Folder) {
				currentItem = BL.GetCurrentItem();
				var success = BL.DeleteFolder(currentItem.ItemID);

				if (success) {
					setItemsList(BL.GetListByParentID(currentItem.ParentID, true));
					SetHeader();
				} else {
					MessageBoxNotEmptyFolderCommand.Invoke("Error!", "The folder is not empty. Remove all items from it and then try again.");
				}
			} else {
				BL.DeleteItem(currentItem.ItemID);
				setItemsList(BL.GetListByParentID(currentItem.ParentID, true));
				SetHeader();
			}
		}

		private Command okTitleFieldChangeCommand;
		public Command OKTitleFieldChangeCommand {
			get {
				return okTitleFieldChangeCommand ?? (okTitleFieldChangeCommand = new Command(ExecuteOKTitleFieldChangeCommand));
			}
		}

		void ExecuteOKTitleFieldChangeCommand(object obj)
		{
			/*
            var fieldID = BL.LastFieldID;
            var valueType = obj.ToString();

            switch (valueType)
            {
                case GConsts.VALUETYPE_DATE:
                    BL.UpdateFieldTitle(fieldID, Common.ConvertDateTimeDB(TempFieldTitleDate));
                    break;
                case GConsts.VALUETYPE_TIME:
                    BL.UpdateFieldTitle(fieldID, TempFieldTitleTime.ToString());
                    break;
                default:
                    BL.UpdateFieldTitle(fieldID, TempFieldTitle);
                    break;
            }

            BL.ResetData();
            TempFieldTitle = null;
            TempFieldTitleDate = default(DateTime);
            TempFieldTitleTime = default(TimeSpan);

            Items = new List<NSWFormsItemModel>();

            var item = BL.GetCurrentItem();
            var nswItems = item.Fields;

            foreach (var nswItem in nswItems)
            {
                if (!nswItem.Deleted)
                    Items.Add(new NSWFormsItemModel(nswItem));
            }
            */

			HideFieldEditTitlePopupCommandCallback.Invoke();
		}

		private Command copyClipFieldCommand;
		public Command CopyClipFieldCommand {
			get {
				return copyClipFieldCommand ?? (copyClipFieldCommand = new Command(ExecuteCopyClipFieldCommand));
			}
		}

		void ExecuteCopyClipFieldCommand(object obj)
		{
			if (obj != null) {
				var value = obj.ToString();
				PlatformSpecific.CopyToClipboard(value);
				PlatformSpecific.DisplayShortMessage(TR.Tr("field_clipboard_copied"));
				HideFieldMenuCommandCallback.Invoke();
			}
		}

		private Command cancelTitleFieldChangeCommand;
		public Command CancelTitleFieldChangeCommand {
			get {
				return cancelTitleFieldChangeCommand ?? (cancelTitleFieldChangeCommand = new Command(ExecuteCancelTitleFieldChangeCommand));
			}
		}

		void ExecuteCancelTitleFieldChangeCommand()
		{
			HideFieldEditTitlePopupCommandCallback.Invoke();
		}

		private Command okTitleChangeCommand;
		public Command OKTitleChangeCommand {
			get {
				return okTitleChangeCommand ?? (okTitleChangeCommand = new Command(ExecuteOKTitleChangeCommand));
			}
		}

		void ExecuteOKTitleChangeCommand(object obj)
		{
			var text = obj.ToString();
			var itemID = BL.CurrentItemID;

			BL.UpdateItemTitle(itemID, text);

			BL.ResetData(true, false, false);

			var currentParentItem = BL.GetItemByID(itemID);

			if (Path != null) {
				Path = Path.Remove(Path.Length - Title.Length - 1) + currentParentItem.Name + "/";
			}

			Title = currentParentItem.Name;
		}

		private Command cancelTitleChangeCommand;
		public Command CancelTitleChangeCommand {
			get {
				return cancelTitleChangeCommand ?? (cancelTitleChangeCommand = new Command(ExecuteCancelTitleChangeCommand));
			}
		}

		void ExecuteCancelTitleChangeCommand()
		{
			HideEditTitlePopupCommandCallback.Invoke();
		}

		private Command selectedItemCommand;
		public Command SelectedItemCommand {
			get {
				return selectedItemCommand ?? (selectedItemCommand = new Command(ExecuteSelectedItemCommand));
			}
		}

		void SetEmptyList(bool isEmpty)
		{
			switch (currentItemType) {
				case ItemTypes.Folder:
					EmptyAddButtonText = TR.Tr("add_button_item_text");
					break;
				case ItemTypes.Item:
					EmptyAddButtonText = TR.Tr("add_button_field_text");
					break;
			}

			ListIsEmpty = isEmpty;
			ListIsNotEmpty = !isEmpty;
			if (pasteItemID == null && pasteFieldID == null) {
				AddButtonVisible = !isEmpty;
			}
		}

		void setItemsList(List<NSWItem> itemsList, bool setHeader = true)
		{
			var itemsStorage = new List<NSWFormsItemModel>();
			if (itemsList != null) {
				if (itemsList.Count == 0) {
					SetEmptyList(true);
				} else {
					SetEmptyList(false);
					foreach (var nswItem in itemsList) {
						if (!nswItem.Deleted)
							itemsStorage.Add(new NSWFormsItemModel(nswItem));
					}

					Items = new List<NSWFormsItemModel>(itemsStorage);
				}
			} else {
				SetEmptyList(true);
			}

			if (setHeader) {
				SetHeader();
			}

			if (IsSpecialFolder) {
				SetEmptyList(false);
			}
		}

		void SetFieldsList(List<NSWField> fieldsList)
		{
			Items = new List<NSWFormsItemModel>();
			if (fieldsList == null) {
				SetEmptyList(true);
			} else {
				if (fieldsList.Count == 0) {
					SetEmptyList(true);
				} else {
					SetEmptyList(false);
				}
			}
			foreach (var nswField in fieldsList) {
				if (!nswField.Deleted)
					Items.Add(new NSWFormsItemModel(nswField));
			}
			Items = Items.OrderBy(x => x.FieldData.SortWeight).ToList();
		}

		public bool IsRoot { get; set; }

		void HideHeader()
		{
			IsHeaderVisible = false;
		}

		public void SetHeader()
		{
			if (BL.CurrentItemID == GConsts.ROOTID) {
				IsRoot = true;
				IsHeaderVisible = false;
				return;
			} else {
				IsRoot = false;
			}
			IsHeaderVisible = true;
			Path = BL.GetCurrentPath();
			Title = BL.GetCurrentItem().Name;

			var item = BL.GetCurrentItem();
			if (item != null) {
				CurrentItemIcon = getImage(item.Icon);
				//var image = NSWRes.GetImage(item.Icon);
				//if (image == null) {
				//	// FIXME: the same code as in NSWFormsItemModel.cs: 214, unify!
				//	var customIcons = BL.GetIcons();
				//	var customIcon = customIcons.Find(x => item.Icon.Contains(x.IconID));
				//	if (customIcon != null) {
				//		icon = ImageSource.FromStream(() => new MemoryStream(customIcon.Icon));
				//		setImageCircle(customIcon.IsCircle);
				//	} else {
				//		icon = ImageSource.FromStream(() => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON));
				//		setImageCircle(false);
				//	}
				//	CurrentItemIcon = icon;
				//} else {
				//	CurrentItemIcon = Resources.GetImageSource(item.Icon);
				//	setImageCircle(false);
				//}
			}
		}

		ImageSource getImage(string techName)
		{
			var image = NSWRes.GetImage(techName);
			if (image == null) {
				var customIcons = BL.GetIcons();
				var customIcon = customIcons.Find(x => techName.Contains(x.IconID));
				if (customIcon != null) {
					setImageCircle(customIcon.IsCircle);
					return ImageSource.FromStream(() => new MemoryStream(customIcon.Icon));
				}
				setImageCircle(false);
				return ImageSource.FromStream(() => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON));
			}
			setImageCircle(false);
			return Resources.GetImageSource(techName);
		}

		void setImageCircle(bool isCircle)
		{
			IsIconCircle = isCircle;
			IsIconNotCircle = !isCircle;
		}

		bool isInsideFolder = false;
		string insideFolderID;

		string searchID;
		string savedSearchText;

		void ExecuteSelectedItemCommand(object obj)
		{
			IsEditItemPanel = false;
			IsEditFolderPanel = false;
			if (obj == null) return;

			if (IsSearchEnabled) {
				//SearchLaunchCommand.Execute(null);
				SearchEntryShowHideCommandCallback.Invoke(true);
				LaunchShowHideRestrictionCommand(false);
				searchID = BL.CurrentItemID;
				savedSearchText = SearchText;
				IsSearchEnabled = false;
				ShowEmptyAddButtonCommand.Invoke();
				SetSearchIconCommand.Invoke(true);
			}

			var item = (NSWFormsItemModel)obj;

			if (item.ItemID == insideFolderID) {
				isInsideFolder = true;
			}

			switch (item.ItemType) {
				case ItemTypes.Folder:
					if (pasteItemID != null) {
						if (isInsideFolder) {
							IsCopyEnabled = true;
							IsMoveEnabled = false;
							setUnavailableClipboard(false);
						} else {
							IsCopyEnabled = true;
							IsMoveEnabled = true;
							setUnavailableClipboard(false);
						}
					}

					if (pasteFieldID != null) {
						IsCopyEnabled = false;
						IsMoveEnabled = false;
						setUnavailableClipboard(true, GConsts.FOLDER);
					}

					currentItemType = ItemTypes.Folder;
					setItemsList(BL.GetListByParentID(item.ItemID, true));
					SetHeader();
					break;
				case ItemTypes.Item:
					if (pasteItemID != null) {
						IsCopyEnabled = false;
						IsMoveEnabled = false;
						var pasteItem = BL.GetItemByID(pasteItemID);
						switch (pasteItem.Folder) {
							case true:
								setUnavailableClipboard(true, GConsts.FOLDER);
								break;
							case false:
								setUnavailableClipboard(true, GConsts.ITEM);
								break;
						}
					}
					currentItemType = ItemTypes.Item;
					SetFieldsList(item.Fields);
					BL.SetCurrentItemID(item.ItemID);
					if (pasteFieldID != null) {
						IsCopyEnabled = true;
						setUnavailableClipboard(false);
						var it = BL.GetCurrentItem();
						var fields = it.Fields;
						if (fields != null) {
							var field = fields.SingleOrDefault(x => x.FieldID == pasteFieldID);
							if (field != null) {
								Device.BeginInvokeOnMainThread(() => {
									IsMoveEnabled = false;
								});
							} else {
								Device.BeginInvokeOnMainThread(() => {
									IsMoveEnabled = true;
								});
							}
						}
					}
					SetHeader();
					ItemsStatsManager.LogView(item.ItemID);
					break;
				case ItemTypes.Field:
					if (item.FieldID != null)
						BL.SetCurrentFieldID(item.FieldID);
					/*
                    string val = null;
                    if (item.FieldData.ValueType.Equals(GConsts.VALUETYPE_DATE))
                        val = Common.ConvertStringToStringDate(item.FieldData.FieldValue);
                    else if (item.FieldData.ValueType.Equals(GConsts.VALUETYPE_TIME))
                        val = Common.ConvertStringToStringTime(item.FieldData.FieldValue);
                    else
                        val = item.FieldData.FieldValue;
                        */
					if (!IsLocalClipboardActivated) {
						//LaunchFieldMenuCommandCallback.Invoke(item.LowAdditionalRow,
						//item.FieldData.HumanReadableValue,
						//item.FieldData.ValueType,
						//ImageSource.FromStream(() => NSWRes.GetImage(item.FieldData.Icon)));
						StateHandler.CopyFieldLocallyActivated = false;
						ChangeFieldCommand.Execute(item.FieldData.HumanReadableValue);
					}
					break;
			}
			ItemToScrollTo = null;
		}

		private Command backCommand;
		public Command BackCommand {
			get {
				return backCommand ?? (backCommand = new Command(ExecuteBackCommand));
			}
		}

		void ExecuteBackCommand()
		{
			IsEditItemPanel = false;
			IsEditFolderPanel = false;
			ItemToScrollTo = BL.CurrentItemID;

			if (pasteItemID != null) {
				var currentItemID = BL.CurrentItemID;

				if (currentItemID == insideFolderID) {
					isInsideFolder = false;
				}
				if (isInsideFolder) {
					IsCopyEnabled = true;
					IsMoveEnabled = false;
					setUnavailableClipboard(false);
				} else {
					if (currentItemID == pasteItemID) {
						IsCopyEnabled = true;
						IsMoveEnabled = false;
						setUnavailableClipboard(false);
					} else {
						IsCopyEnabled = true;
						IsMoveEnabled = true;
						setUnavailableClipboard(false);
					}
				}
			}

			if (pasteFieldID != null) {
				IsCopyEnabled = false;
				IsMoveEnabled = false;
				setUnavailableClipboard(true, GConsts.FIELD);
			} else {
				setUnavailableClipboard(false);
			}

			if (searchID != null) {
				IsSearchEnabled = true;
				BL.SetCurrentItemID(searchID);
				setItemsList(BL.GetCurrentItems());
				SearchEntryShowHideCommandCallback.Invoke(false);
				LaunchShowHideRestrictionCommand(false);
				SearchText = savedSearchText;
				SearchCommand.Execute(savedSearchText);
				searchID = null;
				HideHeader();
			} else {
				setItemsList(BL.GoUpAndGetCurrentItems());
				SetHeader();
			}

			currentItemType = ItemTypes.Folder;
		}

		private Command shareItemCommand;
		public Command ShareItemCommand {
			get {
				return shareItemCommand ?? (shareItemCommand = new Command(ExecuteShareItemCommand));
			}
		}

		void ExecuteShareItemCommand()
		{
			ExportImportPopupUIController.LaunchAlertPopup((x) => {
				Device.BeginInvokeOnMainThread(() => {
					if (x.Result) {
						var itemID = BL.CurrentItemID;
						var item = BL.GetItemByID(itemID);
						if (item != null) {
							string message = String.Format("{0}\n", item.Name);
							if (item.Fields != null) {
								foreach (var field in item.Fields) {
									message += String.Format("{0}: {1}\n", BL.GetFieldTypeNameByShortName(field.FieldType), field.FieldValue);
								}
							}
							PlatformSpecific.Share(message);
						}
					}
				});
			});
		}

		void setClipboardIconCircle(bool isCircle)
		{
			IsClipboardIconCircle = isCircle;
			IsClipboardIconNotCircle = !isCircle;
		}

		Command menuTappedCommand;
		public Command MenuTappedCommand {
			get {
				return menuTappedCommand ?? (menuTappedCommand = new Command(ExecuteMenuTappedCommand));
			}
		}

		public void CopyLocally()
		{
			IsLocalClipboardActivated = true;
			pasteFieldID = BL.CurrentFieldID;
			var itemField = BL.GetCurrentItem();
			var field = new NSWField();
			if (itemField.Fields != null) {
				field = itemField.Fields.SingleOrDefault(x => x.FieldID == pasteFieldID);
			}
			ClipboardIcon = ImageSource.FromStream(() => NSWRes.GetImage(field.Icon));
			setClipboardIconCircle(false);
			ClipboardName = field.FieldValue;
			pasteFromItemID = BL.CurrentItemID;
			HideFieldMenuCommandCallback.Invoke();
			Device.BeginInvokeOnMainThread(() => {
				Application.Current.MainPage.DisplayAlert(TR.Tr("attention"), TR.Tr("main_paste_field_description"), TR.OK);
				AddButtonVisible = false;
				IsCopyEnabled = true;
				IsMoveEnabled = false;
				setUnavailableClipboard(false);
			});
		}

		void ExecuteMenuTappedCommand(object obj)
		{
			if (obj != null) {
				var popupItem = (PopupItem)obj;
				switch (popupItem.Action) {
					case "DeleteFieldCommand":
						DeleteFieldCommand.Execute(popupItem.Parameter);
						break;
					case "CopyLocallyFieldCommand":
						// TODO:
						IsLocalClipboardActivated = true;
						pasteFieldID = BL.CurrentFieldID;
						var itemField = BL.GetCurrentItem();
						var field = new NSWField();
						if (itemField.Fields != null) {
							field = itemField.Fields.SingleOrDefault(x => x.FieldID == pasteFieldID);
						}
						ClipboardIcon = ImageSource.FromStream(() => NSWRes.GetImage(field.Icon));
						setClipboardIconCircle(false);
						ClipboardName = field.FieldValue;
						pasteFromItemID = BL.CurrentItemID;
						HideFieldMenuCommandCallback.Invoke();
						Device.BeginInvokeOnMainThread(() => {
							Application.Current.MainPage.DisplayAlert(TR.Tr("attention"), TR.Tr("main_paste_field_description"), TR.OK);
							AddButtonVisible = false;
							IsCopyEnabled = true;
							IsMoveEnabled = false;
							setUnavailableClipboard(false);
						});
						break;
					case "CopyClipFieldCommand":
						CopyClipFieldCommand.Execute(popupItem.Parameter);
						break;
					case "ChangeFieldCommand":
						ChangeFieldCommand.Execute(popupItem.Parameter);
						break;
					case "DeleteCommand":
						DeleteCommand.Execute(popupItem.Parameter);
						break;
					case "ChangeIconCommand":
						ChangeIconCommand.Execute(popupItem.Parameter);
						break;
					case "ChangeTitleCommand":
						ChangeTitleCommand.Execute(popupItem.Parameter);
						break;
					case "CopyLocallyCommand":
						IsLocalClipboardActivated = true;
						AddButtonVisible = false;
						var item = BL.GetCurrentItem();
						pasteItemID = item.ItemID;
						ClipboardIcon = getImage(item.Icon);
						setClipboardIconCircle(IsIconCircle);
						ClipboardName = item.Name;
						//HideSettingsCommandCallback.Invoke();
						Device.BeginInvokeOnMainThread(() => {
							if (currentItemType == ItemTypes.Item) {
								Application.Current.MainPage.DisplayAlert(TR.Tr("attention"), TR.Tr("main_paste_item_description"), TR.OK);
								IsCopyEnabled = false;
								IsMoveEnabled = false;
								setUnavailableClipboard(true, GConsts.ITEM);
							} else {
								Application.Current.MainPage.DisplayAlert(TR.Tr("attention"), TR.Tr("main_paste_folder_description"), TR.OK);
								IsCopyEnabled = true;
								IsMoveEnabled = false;
								isInsideFolder = true;
								insideFolderID = BL.CurrentItemID;
							}
						});
						break;
					case "CutCommand":
						var isItem = !BL.GetCurrentItem().Folder;
						if (isItem) {
							AppPages.ReorderField(navigation, Items);
						}
						break;
					case "ShareCommand":
						ShareItemCommand.Execute(popupItem.Parameter);
						break;
					case "CreateFolderCommand":
						CreateFolderCommand.Execute(popupItem.Parameter);
						break;
					case "CreateItemCommand":
						CreateItemCommand.Execute(popupItem.Parameter);
						break;
				}
				IsEditItemPanel = false;
				IsEditFolderPanel = false;
			}
		}

		public bool IsFolder {
			get {
				var item = BL.GetCurrentItem();
				return item.Folder;
			}
		}

		public bool IsSpecialFolder {
			get {
				var currentItemID = BL.CurrentItemID;
				switch (currentItemID) {
					case GConsts.EXPIRING_SOON_ID:
						return true;
					case GConsts.MOSTLY_VIEWED_ID:
						return true;
					case GConsts.RECENTLY_VIEWED_FOLDER_ID:
						return true;
					default:
						return false;
				}
			}
		}

		Command copyCommand;
		public Command CopyCommand {
			get {
				return copyCommand ?? (copyCommand = new Command(ExecuteCopyCommand));
			}
		}

		void ExecuteCopyCommand()
		{
			if (currentItemType != ItemTypes.Item) {
				if (!string.IsNullOrEmpty(pasteItemID)) {
					BL.CopyItem(pasteItemID);
				}
				setItemsList(BL.GetCurrentItems());
			} else {
				BL.CopyField(pasteFromItemID);
				var item = BL.GetCurrentItem();
				if (item.Fields != null) {
					SetFieldsList(item.Fields);
				}
			}
			IsLocalClipboardActivated = false;
			AddButtonVisible = true;
			clearPaste();
		}

		Command moveCommand;
		public Command MoveCommand {
			get {
				return moveCommand ?? (moveCommand = new Command(ExecuteMoveCommand));
			}
		}

		void ExecuteMoveCommand()
		{
			if (currentItemType != ItemTypes.Item) {
				if (!string.IsNullOrEmpty(pasteItemID)) {
					BL.MoveItem(pasteItemID);
				}
				setItemsList(BL.GetCurrentItems());
			} else {
				BL.MoveField(pasteFromItemID);
				var item = BL.GetCurrentItem();
				if (item.Fields != null) {
					SetFieldsList(item.Fields);
				}
			}
			IsLocalClipboardActivated = false;
			AddButtonVisible = true;
			clearPaste();
		}

		void clearPaste()
		{
			pasteItemID = null;
			pasteFieldID = null;
			pasteFromItemID = null;
		}

		Command closeClipboardCommand;
		public Command CloseClipboardCommand {
			get {
				return closeClipboardCommand ?? (closeClipboardCommand = new Command(ExecuteCloseClipboardCommand));
			}
		}

		void ExecuteCloseClipboardCommand()
		{
			IsLocalClipboardActivated = false;
			AddButtonVisible = true;
			clearPaste();
		}

		Command settingsCommand;
		public Command SettingsCommand {
			get {
				return settingsCommand ?? (settingsCommand = new Command(ExecuteSettingsCommand));
			}
		}

		void ExecuteSettingsCommand()
		{
			if (currentItemType == ItemTypes.Item) {
				IsEditItemPanel = !IsEditItemPanel;
			}

			if (currentItemType == ItemTypes.Folder) {
				IsEditFolderPanel = !IsEditFolderPanel;
			}
		}

		protected void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}
}