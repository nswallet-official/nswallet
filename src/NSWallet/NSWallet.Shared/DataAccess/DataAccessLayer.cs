using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared
{
	public partial class DataAccessLayer
	{
		// Static members, everything needed for singleton initialization
		static string DBFile;
		static DataAccessLayer DAL;
		public const int DO_NOT_CHANGE_SORT = 0;

		// Instance of DB
		readonly NSWalletDB nswdb;

		// Instance members
		string currentPassword;
		NSWProperties savedProps;
		Dictionary<string, NSWLabel> labels;
		List<NSWField> fields;
		List<NSWItem> items;
		List<NSWIcon> icons;
		List<NSWGroup> groups;
		DateTime expiryDate = DateTime.MinValue;

		//public static string Password { get; set; }

		public static void Init(string dbFile)
		{
			DBFile = dbFile;
		}

		public static void Close()
		{
			if (DAL != null) {
				DAL.nswdb.Close();
				DAL.currentPassword = "";
			}
			DAL = null;
			GC.Collect();
		}

		public static void RestoreDemoDatabase()
		{
			Close();
			Stream demoFileStream = NSWRes.GetDemoFileTEMPORARY();
			var fileStream = new FileStream(DBFile, FileMode.Create, FileAccess.Write);
			demoFileStream.CopyTo(fileStream);
			fileStream.Dispose();
			demoFileStream.Dispose();
			Init(DBFile);
		}

        public static DataAccessLayer GetInstance()
        {
            if (DAL != null)
            {
                return DAL;
            }
            if (string.IsNullOrEmpty(DBFile))
            {
                throw new InitException("File is not set, missing initialization (DataAccessLayer.Init())?");
            }
            DAL = new DataAccessLayer(DBFile);
            return DAL;
        }

        public DataAccessLayer(string databaseFile)
        {
            nswdb = new NSWalletDB(databaseFile);
        }

		public bool AddColumnsToTablesUpgrade03()
		{
			return nswdb.AddColumnsToTablesUpgrade03();
		}


		public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set
            {
                expiryDate = value;
            }
        }


        public NSWProperties StorageProperties
        {
            get
            {
                if (savedProps == null)
                {
                    var nswProps = nswdb.GetStorageProperties();
                    savedProps = new NSWProperties()
                    {
                        DatabaseID = nswProps.database_id,
                        Lang = nswProps.lang,
                        EncryptionCount = Convert.ToInt32(nswProps.email),
                        Version = nswProps.version,
                        SyncTimestamp = Common.ConvertDBDateTime(nswProps.sync_timestamp),
                        UpdateTimestamp = Common.ConvertDBDateTime(nswProps.update_timestamp)
                    };
                }
                return savedProps;
            }
        }

        public void SetDBVersion(string version)
        {
            nswdb.SetDBVersion(version);
        }

        public bool SearchForRoot()
        {
            return nswdb.SearchForRoot();
        }

        public void CreateOnlyRootItem(string newPassword)
        {
            currentPassword = newPassword;
            string uniqueStr = null;
            uniqueStr = Common.GenerateUniqueString(GConsts.ROOTITEM_LENGTH);
            var encrData = Security.EncryptStringAES(uniqueStr, newPassword, 0, newPassword, out bool ok);
            if (ok)
            {
                nswdb.CreateItem(GConsts.ROOTID, GConsts.ROOT_PARENT_ID, encrData, "", true,
                                 Common.ConvertDateTimeDB(DateTime.Now),
                                 Common.ConvertDateTimeDB(DateTime.Now), false);
            }
            else
            {
                throw new EncryptException("Encrypt failure during root item creation");
            }
        }

		public bool InsertIcon(string iconID, string name, byte[] iconBLOB, int groupID)
        {
			return nswdb.InsertIcon(iconID, name, iconBLOB, groupID);
        }

		public bool DeleteIcon(string iconID)
		{
			return nswdb.DeleteIcon(iconID);
		}

		public bool InsertGroup(int groupID, string name)
        {
            return nswdb.InsertGroup(groupID, name);
        }

		public bool UpdateGroup(int groupID, string name)
		{
			return nswdb.UpdateGroup(groupID, name);
		}

		public bool UpdateIcon(string iconID, string name, byte[] blob = null, int groupID = -1, int isCircle = -1)
		{
			return nswdb.UpdateIcon(iconID, name, blob, groupID, isCircle);
		}

		public void CreateItem(string itemID, string parentID, string name, string icon, bool folder)
        {
            var nameEncrypted = Security.EncryptStringAES(name, currentPassword, 0, currentPassword, out bool ok);
            if (ok)
            {
                nswdb.CreateItem(itemID, parentID, nameEncrypted, icon, folder,
                                 Common.ConvertDateTimeDB(DateTime.Now),
                                 Common.ConvertDateTimeDB(DateTime.Now), false);
            }
            else
            {
                throw new EncryptException("Encrypt failure during item creation");
            }
            items = null;
        }

        public string GetParentIDByItemID(string itemID)
        {
            if (itemID == GConsts.ROOTID) return GConsts.ROOTID;
            return Items.Find(item => item.ItemID == itemID).ParentID;
        }

        public void UpdateItemTitle(string itemID, string title)
        {
            var titleEncrypted = Security.EncryptStringAES(title, currentPassword, 0, currentPassword, out bool ok);

            if (ok)
            {
                nswdb.UpdateItemTitle(itemID, titleEncrypted, Common.ConvertDateTimeDB(DateTime.Now));
            }
            else
            {
                throw new EncryptException("Encrypt failure during field creation");
            }

            items = null;
        }

        public void UpdateItemIcon(string itemID, string icon)
        {
            nswdb.UpdateItemIcon(itemID, icon, Common.ConvertDateTimeDB(DateTime.Now));
            items = null;
        }

		public void UpdateItemParentID(string itemID, string parentID)
		{
			nswdb.UpdateItemParentID(itemID, parentID, Common.ConvertDateTimeDB(DateTime.Now));
			items = null;
		}

		public void DeleteItem(string itemID)
        {
            nswdb.DeleteItem(itemID, Common.ConvertDateTimeDB(DateTime.Now));
            items = null;
        }

        public void DeleteFolder(string folderID)
        {
            nswdb.DeleteFolder(folderID, Common.ConvertDateTimeDB(DateTime.Now));
            items = null;
        }





        public bool CreateLabel(string fieldType, string name, string icon, string valueType, bool system)
        {
            var result = nswdb.CreateLabel(fieldType, name, valueType, icon, system, Common.ConvertDateTimeDB(DateTime.Now), false);
            labels = null;
			return result;
        }

        public bool RemoveLabelForReal(string fieldType)
        {
            var result = nswdb.RemoveLabelForReal(fieldType);
            labels = null;
            return result;
        }

        public int RemoveLabel(string fieldType)
        {
            var result = nswdb.RemoveLabel(fieldType, Common.ConvertDateTimeDB(DateTime.Now));
            labels = null;
            return result;
        }

        public void UpdateLabelIcon(string fieldType, string icon)
        {
            nswdb.UpdateLabelIcon(fieldType, icon, Common.ConvertDateTimeDB(DateTime.Now));
            labels = null;
        }

        public void UpdateLabelTitle(string fieldType, string name)
        {
            nswdb.UpdateLabelTitle(fieldType, name, Common.ConvertDateTimeDB(DateTime.Now));
            labels = null;
        }

        public bool ArePropsSet
        {
            get
            {
				try {
					if (nswdb.PropertiesCount() == 1)
						return true;
				} catch(Exception ex) {
					log(ex.Message, nameof(ArePropsSet));
				}
                return false;
            }
        }

		public void ResetMemoryData(bool resetItems, bool resetFields, bool resetLabel, bool resetIcons = false, bool resetGroups = false)
        {
            if (resetItems) items = null;
            if (resetLabel) labels = null;
            if (resetFields) fields = null;
            if (resetIcons) icons = null;
			if (resetGroups) groups = null;
        }

        public void SetNewProperties(string lang)
        {
            var dbID = Common.GenerateUniqueString(64);
            int defEncrCount = 0;
            nswdb.SetProperties(dbID, lang, GConsts.DB_VERSION, defEncrCount.ToString());
        }

        public bool CheckPassword(string password)
        {
            var encryptedRootData = nswdb.GetRootData();
			Security.DecryptStringAES(encryptedRootData, password,
														StorageProperties.EncryptionCount,
														password,
														out bool ok);
			if (ok == true)
            {
                currentPassword = password;
            }
            return ok;
        }

        public List<NSWItem> GetItemsByIDs(List<string> ids) {
            List<NSWItem> list = new List<NSWItem>();
            foreach(var id in ids) {
                var foundItem = GetItemByID(id);
                if (foundItem != null) {
                    list.Add(foundItem);
                }
            }
            return list;
        }

        public NSWItem GetItemByID(string itemID)
        {
            var itemsExist = Items.Exists(item => item.ItemID == itemID);
            if (itemsExist) 
            {
                var item = items.Find(x => x.ItemID == itemID);
                return item;
            }
            return null;
        }

        public Dictionary<string, NSWLabel> Labels
        {
            get
            {
                if (labels != null)
                {
                    return labels;
                }
                labels = new Dictionary<string, NSWLabel>();

                var queryResult = nswdb.RetrieveAllLabelsView();
                foreach (var label in queryResult)
                {
					var newLabel = new NSWLabel {
						FieldType = label.field_type,
						Icon = "Icons.labels.icon_" + label.icon + "_huge.png",
						Name = TR.Tr(label.label_name),
						System = label.system,
						ValueType = label.value_type,
						Deleted = label.deleted,
						Usage = label.usage
					};
					labels[newLabel.FieldType] = newLabel;

                }
				
                return labels;
            }
        }

        NSWLabel getEmptyLabel()
        {
            return new NSWLabel()
            {
                FieldType = "unknown",
                Icon = "unknown",
                Name = "Unknown",
                System = false,
                ValueType = "text",
                UpdateTimestamp = DateTime.MinValue
            };
        }

        public NSWLabel GetLabelByFieldType(string fieldType)
        {
            if (Labels == null)
            {
                return getEmptyLabel();
            }
            if (Labels.ContainsKey(fieldType) == false)
            {
                return getEmptyLabel();
            }
            return Labels[fieldType];

        }

		public List<NSWGroup> Groups
		{
			get {
				if (groups != null) {
					return groups;
				}

				if (currentPassword == string.Empty) {
					throw new PasswordNotSetException("Cannot decrypt, password is not set");
				}

				groups = new List<NSWGroup>();
				var queryResult = nswdb.RetrieveAllGroups();
				foreach (var group in queryResult) {
					var newGroup = new NSWGroup() {
						GroupID = group.group_id,
						Name = group.name,
						Deleted = group.deleted
					};
					groups.Add(newGroup);
				}
				return groups;
			}
		}

        public List<NSWIcon> Icons
        {
            get
            {
                if (icons != null)
                {
                    return icons;
                }

                if (currentPassword == string.Empty)
                {
                    throw new PasswordNotSetException("Cannot decrypt, password is not set");
                }

                icons = new List<NSWIcon>();

                var queryResult = nswdb.RetrieveAllIcons();
                foreach (var icon in queryResult)
                {
                    var newIcon = new NSWIcon()
                    {
                        IconID = icon.icon_id,
                        Name = icon.name,
                        Icon = icon.icon_blob,
                        GroupID = icon.group_id,
						IsCircle = icon.is_circle,
						Deleted = icon.deleted
                    };

                    icons.Add(newIcon);
                }

                return icons;
            }
        }


        public static void SetExpiryFlags(DateTime expDate, string fieldDate, out bool expiring, out bool expired)
        {
            var fieldExp = Common.ConvertDate(fieldDate);
            if (fieldExp < expDate)
            {
                expiring = true;
            }
            else
            {
                expiring = false;
            }
            if (fieldExp < DateTime.Now)
            {
                expired = true;
            }
            else
            {
                expired = false;
            }
        }

        public bool ChangePassword(string newPassword)
        {
            try
            {
                nswdb.BeginTransaction();
                ResetMemoryData(true, true, true);

                foreach (var field in Fields)
                {
                    var NewFieldValue = Security.EncryptStringAES(field.FieldValue, newPassword,
                                            StorageProperties.EncryptionCount,
                                            newPassword, out bool ok);
                    if (ok == false)
                    {
                        throw new Exception("ChangePassword: field encryption failure");
                    }
                    nswdb.UpdateFieldValueCHPASSONLY(field.ItemID, field.FieldID, NewFieldValue);
                }

                foreach (var item in Items)
                {
                    var NewItemName = Security.EncryptStringAES(item.Name, newPassword,
                                            StorageProperties.EncryptionCount,
                                            newPassword, out bool ok);
                    if (ok == false)
                    {
                        throw new Exception("ChangePassword: item encryption failure");
                    }
                    nswdb.UpdateItemValueCHPASSONLY(item.ItemID, NewItemName);
                }

                nswdb.CommitTransaction();
                ResetMemoryData(true, true, true);

                return CheckPassword(newPassword);
            }
            catch (Exception ex)
            {
                nswdb.RollbackTransaction();
				log(ex.Message, nameof(ChangePassword));
                return false;
            }

        }

        public List<NSWItem> Items
        {
            get
            {
                if (items != null)
                {
                    return items;
                }
                if (currentPassword == string.Empty)
                {
                    throw new PasswordNotSetException("Cannot decrypt, password is not set");
                }
                items = new List<NSWItem>();

                var queryResult = nswdb.RetrieveAllItems();

                foreach (var item in queryResult)
                {
                    var newItem = new NSWItem()
                    {
                        ItemID = item.item_id,
                        ParentID = item.parent_id,
                        Deleted = item.deleted,
                        Folder = item.folder
                    };
                    var decryptedName = Security.DecryptStringAES(item.name, currentPassword,
                                            StorageProperties.EncryptionCount,
                                            currentPassword,
                                            out bool ok);
                    if (ok == false) {
						throw new DecryptException("Decrypt failure during items retrieval");
                    }

					newItem.Name = decryptedName;


					if (newItem.Folder == true) {
						newItem.Icon = ImageManager.ConvertIconFolder2IconPath(item.icon);
					} else {
                        newItem.Icon = ImageManager.ConvertIconName2IconPath(item.icon);
                    }

                    newItem.CreateTimestamp = Common.ConvertDBDateTime(item.create_timestamp);
                    newItem.UpdateTimestamp = Common.ConvertDBDateTime(item.change_timestamp);

                    items.Add(newItem);
                }

                return items;
            }
        }

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(DataAccessLayer));
		}

    }

    public class InitException : Exception
    {
        public InitException(string message) : base(message)
        {

        }
    }

    public class PasswordNotSetException : Exception
    {
        public PasswordNotSetException(string message) : base(message)
        {

        }
    }

    public class DecryptException : Exception
    {
        public DecryptException(string message) : base(message)
        {

        }
    }

    public class EncryptException : Exception
    {
        public EncryptException(string message) : base(message)
        {

        }
    }
}
