using System;
using System.Collections.Generic;
using System.Linq;
using NSWallet.Shared.Helpers;

namespace NSWallet.Shared
{
    public static partial class BL
    {
        public static NSWItem GetItemByID(string itemID)
        {
            return DataAccessLayer.GetInstance().Items.Find(x => x.ItemID == itemID);
        }

        public static List<NSWItem> GetItems()
        {
            return DataAccessLayer.GetInstance().Items;
        }

		public static string AddItem(string name, string icon, bool folder, string parentID = null, string itemID = null)
        {
			string id = null;
			if (itemID == null) {
				id = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
			} else {
				id = itemID;
			}
			var instance = DataAccessLayer.GetInstance();

			if (parentID == null) {
				instance.CreateItem(id, CurrentItemID, name, icon, folder);
			} else {
				instance.CreateItem(id, parentID, name, icon, folder);
			}
            return id;
        }

        public static void UpdateItemTitle(string itemID, string title)
        {
            DataAccessLayer.GetInstance().UpdateItemTitle(itemID, title);
        }

        public static void UpdateItemIcon(string itemID, string icon)
        {
            DataAccessLayer.GetInstance().UpdateItemIcon(itemID, icon);
        }

		public static void UpdateItemParentID(string itemID, string parentID)
		{
			DataAccessLayer.GetInstance().UpdateItemParentID(itemID, parentID);
		}

		public static void DeleteItem(string itemID)
        {
			deleteItemInExpiringItems(itemID);
            DataAccessLayer.GetInstance().DeleteItem(itemID);
        }

		static void deleteItemInExpiringItems(string itemID)
		{
			if (expiringItems != null) {
				var item = expiringItems.SingleOrDefault(x => x.ItemID == itemID);
				if (item != null) {
					expiringItems.Remove(item);
				}
			}
		}

        public static bool DeleteFolder(string folderID)
        {
            var dal = DataAccessLayer.GetInstance();
            var currentItem = GetCurrentItem();

            if (currentItem != null)
            {
                var innerItems = GetListByParentID(currentItem.ItemID, false);

                if (innerItems.Count > 0)
                    return false;
                dal.DeleteFolder(folderID);
                return true;
            }

            return false;
        }

        public static string GetCurrentParentID()
        {
            return DataAccessLayer.GetInstance().GetParentIDByItemID(CurrentItemID);
        }


        public static NSWItem GetCurrentItem()
        {
            if (CurrentItemID == GConsts.EXPIRING_SOON_ID)
            {
                return GetExpiringSoonFolder();
            }
            if (CurrentItemID == GConsts.RECENTLY_VIEWED_FOLDER_ID)
            {
                return GetRecentlyViewedFolder();
            }
            if (CurrentItemID == GConsts.MOSTLY_VIEWED_ID)
            {
                return GetMostlyViewedFolder();
            }
            return DataAccessLayer.GetInstance().GetItemByID(CurrentItemID);
        }

        public static void SetCurrentItemID(string itemID)
        {
            lastCurrentID = CurrentItemID;
			CurrentItemID = itemID;
		}

		public static List<NSWItem> GoUpAndGetCurrentItems()
		{
			var dal = DataAccessLayer.GetInstance();
			string parentID;
			if (CurrentItemID == GConsts.EXPIRING_SOON_ID ||
				CurrentItemID == GConsts.RECENTLY_VIEWED_FOLDER_ID ||
				CurrentItemID == GConsts.MOSTLY_VIEWED_ID) {
				parentID = GConsts.ROOTID;
			} else {
				if (lastCurrentID == GConsts.EXPIRING_SOON_ID) {
					parentID = GConsts.EXPIRING_SOON_ID;
				} else if (lastCurrentID == GConsts.RECENTLY_VIEWED_FOLDER_ID) {
					parentID = GConsts.RECENTLY_VIEWED_FOLDER_ID;
				} else if (lastCurrentID == GConsts.MOSTLY_VIEWED_ID) {
					parentID = GConsts.MOSTLY_VIEWED_ID;
				} else {
					parentID = dal.GetParentIDByItemID(CurrentItemID);
				}
			}

			var itemsList = GetListByParentID(parentID, true);

			return itemsList;

		}

		public static List<NSWItem> GetListByParentID(string parentID, bool setAsCurrent)
		{
			List<NSWItem> filteredList;
			if (parentID == GConsts.EXPIRING_SOON_ID) {
				filteredList = GetExpiringItems();
			} else if (parentID == GConsts.RECENTLY_VIEWED_FOLDER_ID) {
				filteredList = GetRecentItems();
			} else if (parentID == GConsts.MOSTLY_VIEWED_ID) {
				filteredList = GetMostItems();
			} else {
				filteredList = DataAccessLayer.GetInstance().Items.FindAll(act => act.ParentID == parentID).
									 OrderByDescending(x => x.Folder).
									 ThenBy(y => y.Name).ToList();
			}

			if (filteredList == null) {
				filteredList = new List<NSWItem>();
			}


			if (setAsCurrent) {
				lastCurrentID = CurrentItemID;
				CurrentItemID = parentID;
            }

            if (RecentlyViewed && parentID == GConsts.ROOTID)
            {
                filteredList.Insert(0, GetRecentlyViewedFolder());
            }

            if (MostlyViewed && parentID == GConsts.ROOTID)
            {
                filteredList.Insert(0, GetMostlyViewedFolder());
            }

            if (ExpiringSoon && parentID == GConsts.ROOTID)
            {
                filteredList.Insert(0, GetExpiringSoonFolder());
            }

            return filteredList;
        }

        public static List<NSWItem> GetRecentItems()
        {
            return DataAccessLayer.GetInstance().GetItemsByIDs(ItemsStats.GetInstance().SortedByDateTime());
        }

        public static List<NSWItem> GetMostItems()
        {
            return DataAccessLayer.GetInstance().GetItemsByIDs(ItemsStats.GetInstance().SortedByCount());
        }

        static List<NSWItem> expiringItems;

        public static List<NSWItem> GetExpiringItems()
        {
            if (expiringItems != null)
            {
                return expiringItems;
            }
            var filteredFields = DataAccessLayer.GetInstance().Fields.FindAll(f => f.Expired == true || f.Expiring == true).
                                                OrderBy(f => f.FieldValue).ToList();
            expiringItems = new List<NSWItem>();
            foreach (var f in filteredFields)
            {
                var item = GetItemByID(f.ItemID);
                if (f.Expired)
                {
                    item.Expired = true;
                }
                else if (f.Expiring)
                {
                    item.ExpireInDays = Common.ConvertDate(f.FieldValue).Subtract(DateTime.Now).Days + 1;

                }
                expiringItems.Add(item);
            }
            return expiringItems;
        }

        static NSWItem GetExpiringSoonFolder()
        {
            var nswItem = new NSWItem
            {
                ItemID = GConsts.EXPIRING_SOON_ID,
                ParentID = GConsts.ROOTID,
                Name = TR.Tr("expiringsoon_folder"),
                CreateTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now,
                Folder = true,
                Icon = "Icons.items.icon_folderexpiring.png",
                Deleted = false
            };

            return nswItem;
        }

        static NSWItem GetRecentlyViewedFolder()
        {
            var nswItem = new NSWItem
            {
                ItemID = GConsts.RECENTLY_VIEWED_FOLDER_ID,
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

        static NSWItem GetMostlyViewedFolder()
        {
            var nswItem = new NSWItem
            {
                ItemID = GConsts.MOSTLY_VIEWED_ID,
                ParentID = GConsts.ROOTID,
                Name = TR.Tr("mostlyviewed_folder"),
                CreateTimestamp = DateTime.Now,
                UpdateTimestamp = DateTime.Now,
                Folder = true,
                Icon = "Icons.items.icon_foldermost.png",
                Deleted = false
            };

            return nswItem;
        }

    }
}
