using System.Collections.Generic;
using System.Linq;

namespace NSWallet.Shared
{
	public static partial class BL
	{
		//static string copyPrefix = TR.Tr("item_prefix_copy") + " ";

		public static string GetCopyPrefix() {
			return TR.Tr("item_prefix_copy") + " ";;
		}

		public static string CopyItem(string sourceItemID)
		{
			var item = GetItemByID(sourceItemID);
			if (item != null) {
				if (!item.Folder) {
					return copyItem(item);
				}
				return copyFolder(item);
			}
			return null;
		}

		public static string CopyField(string sourceItemID)
		{
			var fieldID = CurrentFieldID;
			var item = GetItemByID(sourceItemID);
			var field = item.Fields.SingleOrDefault(x => x.FieldID == fieldID);
			var weight = getWeight(item);
			if (field != null && item != null) {
				return AddField(field.FieldType, field.FieldValue, null, weight);
			}
			return null;
		}

		public static void MoveItem(string sourceItemID)
		{
			UpdateItemParentID(sourceItemID, CurrentItemID);
		}

		public static void MoveField(string sourceItemID)
		{
			var fieldID = CurrentFieldID;
			var item = GetItemByID(sourceItemID);
			if (item != null) {
				var field = item.Fields.SingleOrDefault(x => x.FieldID == fieldID);
				if (field != null) {
					AddField(field.FieldType, field.FieldValue);
					DeleteField(field.FieldID, item.ItemID);
				}
			}
		}

		static string copyItem(NSWItem item)
		{
			var name = GetCopyPrefix() + item.Name;
			var icon = IconController.SubstringItemName(item.Icon);
			var itemID = AddItem(name, icon, item.Folder);
			copyItemFields(itemID, item.Fields);
			return itemID;
		}

		static void copyItemFields(string itemID, List<NSWField> fields)
		{
			if (fields != null) {
				foreach (var field in fields) {
					AddField(field.FieldType, field.FieldValue, itemID, field.SortWeight);
				}
			}
		}

		static string copyFolder(NSWItem item)
		{
			var itemID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
			var copyItemsData = prepareCopyFolderModel(item, itemID);
			saveLocallyItemsInFolder(copyItemsData, item.ItemID, itemID);
			addFolderToDatabase(copyItemsData);
			return itemID;
		}

		static List<CopyMoveModel> prepareCopyFolderModel(NSWItem item, string newItemID)
		{
			return new List<CopyMoveModel> {
				new CopyMoveModel {
					ItemID = newItemID,
					Item = item,
					ParentID = CurrentItemID
				}
			};
		}

		static void saveLocallyItemsInFolder(List<CopyMoveModel> copyItemsData, string itemID, string parentID)
		{
			var parentList = GetListByParentID(itemID, false);
			foreach (var item in parentList) {
				if (!item.Folder) {
					createNewItemID(copyItemsData, item, parentID);
				} else {
					var newItemID = createNewItemID(copyItemsData, item, parentID);
					saveLocallyItemsInFolder(copyItemsData, item.ItemID, newItemID);
				}
			}
		}

		static string createNewItemID(List<CopyMoveModel> copyItemsData, NSWItem item, string parentID)
		{
			var newItemID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
			copyItemsData.Add(new CopyMoveModel {
				ItemID = newItemID,
				Item = item,
				ParentID = parentID
			});
			return newItemID;
		}

		static void addFolderToDatabase(List<CopyMoveModel> copyItemsData)
		{
			int count = 0;
			foreach (var itemDict in copyItemsData) {
				var item = itemDict.Item;
				string name = item.Name;
				if (count == 0) {
					name = GetCopyPrefix() + name;
				}
				if (item.Folder) {
					AddItem(name, IconController.SubstringFolderName(item.Icon), item.Folder,
							itemDict.ParentID, itemDict.ItemID);
				} else {
					AddItem(name, IconController.SubstringItemName(item.Icon), item.Folder,
							itemDict.ParentID, itemDict.ItemID);
					copyItemFields(itemDict.ItemID, item.Fields);
				}
				count++;
			}
		}

		static int getWeight(NSWItem item)
		{
			int weight = 0;
			if (item.Fields != null) {
				weight = item.Fields.Max(x => x.SortWeight);
			}
			return weight + 100;
		}
	}
}