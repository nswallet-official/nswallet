using SQLite;
using System;
using System.Linq;
using NSWallet.Shared.Helpers.Logs.AppLog;
using System.Collections.Generic;

namespace NSWallet.Shared
{
    public partial class NSWalletDB
	{ 
		SQLiteConnection conn;
        string DBFile;

        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
            GC.Collect();
            DBFile = "";
        }

        public void BeginTransaction()
        {
            if (conn != null)
                conn.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (conn != null && conn.IsInTransaction)
            {
                conn.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (conn != null && conn.IsInTransaction)
            {
                conn.Rollback();
            }
        }

        public NSWalletDB(string dbFile)
        {
            try
            {
                DBFile = dbFile;
                InitiateConnection();
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(NSWalletDB) + " (ctor)");
                throw new DatabaseException("NSWalletDB: " + ex.Message);
            }
        }

        void CheckConnection()
        {
            if (conn == null)
            {
                InitiateConnection();
            }
            if (conn == null)
            {
                throw new DatabaseException("Connection not create (null)! ");
            }
        }

        void InitiateConnection()
        {
            conn = new SQLiteConnection(DBFile);
            conn.CreateTable<nswallet_properties>();
            conn.CreateTable<nswallet_items>();
            conn.CreateTable<nswallet_labels>();
            conn.CreateTable<nswallet_icons>();
            conn.CreateTable<nswallet_groups>();
			// Workaround start: part2
			//var result = conn.Execute(CREATE_FIELDS);
			conn.Execute(CREATE_FIELDS);
			// conn.CreateTable<nswallet_fields>(); // Uncomment if composite keys are supported
			// Workaround end: part2
		}

		public nswallet_properties GetStorageProperties()
        {
            try
            {
                CheckConnection();
                return conn.Table<nswallet_properties>().FirstOrDefault();
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(GetStorageProperties));
				return null;
                throw new DatabaseException("GetStorageProperties: " + ex.Message);
            }
        }

        void UpdateProperties()
        {
            try
            {
                CheckConnection();
                var properties = GetStorageProperties();
                properties.update_timestamp = Common.ConvertDateTimeDB(DateTime.Now);
                conn.Update(properties);
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(UpdateProperties));
                throw new DatabaseException("UpdateProperties: " + ex.Message);
            }
        }

        public void SetDBVersion(string version)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_properties>().FirstOrDefault();
                query.version = version;
                conn.Update(query);
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction(); // Cancel any successfull operations above
				log(ex.Message, nameof(SetDBVersion));
                throw new DatabaseException("GetStorageProperties: " + ex.Message);
            }
        }

        public bool SearchForRoot()
        {
            try
            {
                CheckConnection();
                return conn.Table<nswallet_items>().Any(item => item.item_id == GConsts.ROOTID);
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(SearchForRoot));
				throw new DatabaseException("SearchForRoot: " + ex.Message);
            }
        }

		public nswallet_items GetRootItem()
		{
			try {
				CheckConnection();
				return conn.Table<nswallet_items>().SingleOrDefault(item => item.item_id == GConsts.ROOTID);
			} catch (Exception ex) {
				log(ex.Message, nameof(GetRootItem));
				throw new DatabaseException("GetRootItem: " + ex.Message);
			}
		}

		public bool RemoveLabelForReal(string fieldType)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                conn.Table<nswallet_labels>().Delete(x => x.field_type == fieldType);
                CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction();// Cancel any successfull operations above
				log(ex.Message, nameof(RemoveLabelForReal));
				throw new DatabaseException("RemoveLabelForReal: " + ex.Message);

            }
        }

        public int RemoveLabel(string fieldType, string changeTS)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var queryItems = conn.Table<nswallet_fields>().Where(x => x.type == fieldType).Where(y => y.deleted != true).ToList();

                if (queryItems.Count == 0)
                {
                    var query = conn.Table<nswallet_labels>().Where(c => c.field_type == fieldType).SingleOrDefault();
                    query.change_timestamp = changeTS;
                    query.deleted = true;
                    conn.Update(query);
                }

                UpdateProperties();

                CommitTransaction();
                return queryItems.Count;
            }
            catch (Exception ex)
            {
                RollbackTransaction(); // Cancel any successfull operations above
				log(ex.Message, nameof(RemoveLabel));
				throw new DatabaseException("RemoveLabel: " + ex.Message);
            }
        }

        public void UpdateLabelIcon(string fieldType, string icon, string change)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_labels>().Where(c => c.field_type == fieldType).SingleOrDefault();
                query.icon = icon;
                query.change_timestamp = change;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction(); // Confirm all the operations above
            }
            catch (Exception ex)
            {
                RollbackTransaction(); // Cancel any successfull operations above
				log(ex.Message, nameof(UpdateLabelIcon));
				throw new DatabaseException("UpdateLabelIcon: " + ex.Message);
            }
        }

        public void UpdateLabelTitle(string fieldType, string name, string change)
        {

            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_labels>().Where(c => c.field_type == fieldType).SingleOrDefault();
                query.label_name = name;
                query.change_timestamp = change;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction(); // Confirm all the operations above
            }
            catch (Exception ex)
            {
                RollbackTransaction(); // Cancel any successfull operations above
				log(ex.Message, nameof(UpdateLabelTitle));
				throw new DatabaseException("UpdateLabelTitle: " + ex.Message);
            }
        }

        public void UpdateItemTitle(string itemID, byte[] title, string change)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_items>().Where(x => x.item_id == itemID).SingleOrDefault();
                query.name = title;
                query.change_timestamp = change;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction(); // Confirm all the operations above
            }
            catch (Exception ex)
            {
                RollbackTransaction(); // Cancel any successfull operations above
				log(ex.Message, nameof(UpdateItemTitle));
				throw new DatabaseException("UpdateItemTitle: " + ex.Message);
            }
        }

        public void UpdateItemIcon(string itemID, string icon, string change)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_items>().Where(x => x.item_id == itemID).SingleOrDefault();
                query.icon = icon;
                query.change_timestamp = change;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(UpdateItemIcon));
				throw new DatabaseException("UpdateItemIcon: " + ex.Message);
            }
        }

		public void UpdateItemParentID(string itemID, string parentID, string change)
		{
			try {
				CheckConnection();
				BeginTransaction();
				var query = conn.Table<nswallet_items>().Where(x => x.item_id == itemID).SingleOrDefault();
				query.parent_id = parentID;
				query.change_timestamp = change;
				conn.Update(query);
				UpdateProperties();
				CommitTransaction();
			} catch (Exception ex) {
				RollbackTransaction();
				log(ex.Message, nameof(UpdateItemParentID));
				throw new DatabaseException("UpdateParentID: " + ex.Message);
			}
		}

		public bool InsertIcon(string iconID, string name, byte[] iconBLOB, int groupID)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                conn.Insert(new nswallet_icons
                {
                    icon_id = iconID,
                    name = name,
                    icon_blob = iconBLOB,
                    group_id = groupID
                });

                UpdateProperties();
                CommitTransaction();
				return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(InsertIcon));

				return false;

				throw new DatabaseException("InsertIcon: " + ex.Message);
            }
        }

		public bool InsertGroup(int groupID, string name)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                conn.Insert(new nswallet_groups
                {
                    group_id = groupID,
                    name = name
                });

                UpdateProperties();
                CommitTransaction();
				return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(InsertGroup));
				return false;
				throw new DatabaseException("InsertGroup: " + ex.Message);
            }
        }

		public bool UpdateGroup(int groupID, string name)
		{
			try {
				CheckConnection();
				BeginTransaction();

				var query = conn.Table<nswallet_groups>().Where(x => x.group_id == groupID).SingleOrDefault();
				query.name = name;
				conn.Update(query);

				UpdateProperties();
				CommitTransaction();
				return true;
			} catch (Exception ex) {
				RollbackTransaction();
				return false;
				throw new DatabaseException("UpdateGroup: " + ex.Message);
			}
		}

		public bool UpdateIcon(string iconID, string name, byte[] blob = null, int groupID = -1, int isCircle = -1)
		{
			try {
				CheckConnection();
				BeginTransaction();

				var query = conn.Table<nswallet_icons>().Where(x => x.icon_id == iconID).SingleOrDefault();
				if (name != null) {
					query.name = name;
				}
				if (groupID != -1) {
					query.group_id = groupID;
				}
				if (blob != null) {
					query.icon_blob = blob;
				}
				if (isCircle != -1) {
					switch (isCircle) {
						case 0:
							query.is_circle = false;
							break;
						case 1:
							query.is_circle = true;
							break;
					}
				}
				conn.Update(query);

				UpdateProperties();
				CommitTransaction();
				return true;
			} catch (Exception ex) {
				RollbackTransaction();
				return false;
				throw new DatabaseException("UpdateIcon: " + ex.Message);
			}
		}

		public bool DeleteIcon(string iconID)
		{
			try {
				CheckConnection();
				BeginTransaction();
				var query = conn.Table<nswallet_icons>().ToList().Find(x => x.icon_id == iconID);
				query.deleted = true;
				conn.Update(query);
				UpdateProperties();
				CommitTransaction();
				return true;
			} catch (Exception ex) {
				RollbackTransaction();
				return false;
				throw new DatabaseException("DeleteIcon: " + ex.Message);
			}
		}

		public TableQuery<nswallet_groups> RetrieveAllGroups()
		{
			try {
				CheckConnection();
				return conn.Table<nswallet_groups>();
			} catch (Exception ex) {
				log(ex.Message, nameof(RetrieveAllGroups));
				throw new DatabaseException("RetrieveAllGroups: " + ex.Message);
			}
		}

        public void CreateItem(string itemID, string parentID, byte[] name, string icon,
                               bool folder, string createTimestamp, string changeTimestamp, bool deleted)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                conn.Insert(new nswallet_items
                {
                    item_id = itemID,
                    parent_id = parentID,
                    name = name,
                    icon = icon,
                    field_id = null,
                    folder = folder,
                    create_timestamp = createTimestamp,
                    change_timestamp = changeTimestamp,
                    deleted = deleted
                });

                UpdateProperties();

                CommitTransaction(); // Confirm all the operations above
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(CreateItem));
				throw new DatabaseException("CreateItem: " + ex.Message);
            }
        }


        public void UpdateFieldValueCHPASSONLY(string itemId, string fieldId, byte[] newValue)
        {
            try
            {
                CheckConnection();
                var queryFieldResult = conn.Table<nswallet_fields>().Where(x => x.item_id == itemId && x.field_id == fieldId).SingleOrDefault();
                queryFieldResult.value = newValue;
                conn.Update(queryFieldResult);
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(UpdateFieldValueCHPASSONLY));
				throw new DatabaseException("UpdateFieldValueCHPASSONLY: " + ex.Message);
            }
        }

        public void UpdateItemValueCHPASSONLY(string itemId, byte[] newValue)
        {
            try
            {
                CheckConnection();
                var queryItemResult = conn.Table<nswallet_items>().Where(x => x.item_id == itemId).SingleOrDefault();
                queryItemResult.name = newValue;
                conn.Update(queryItemResult);
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(UpdateItemValueCHPASSONLY));
				throw new DatabaseException("UpdateItemValueCHPASSONLY: " + ex.Message);
            }
        }

        public void DeleteField(string itemID, string fieldID, string changeTimestamp)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_fields>().ToList().Find(x => x.field_id == fieldID && x.item_id == itemID);
                query.deleted = true;
                query.change_timestamp = changeTimestamp;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(DeleteField));
				throw new DatabaseException("DeleteField: " + ex.Message);
            }
        }

        public void UpdateField(string fieldID, byte[] name, int sortWeight, string changeTimestamp)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_fields>().ToList().Find(x => x.field_id == fieldID);
                query.value = name;
                //query.type = type;
                if (sortWeight > 0)
                {
                    query.sort_weight = sortWeight;
                }
                query.change_timestamp = changeTimestamp;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(UpdateField));
				throw new DatabaseException("UpdateField: " + ex.Message);
            }
        }

        public void DeleteItem(string itemID, string changeTimestamp)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var queryFields = conn.Table<nswallet_fields>().Where(x => x.item_id == itemID).ToList();
                foreach (var queryField in queryFields)
                {
                    var query = queryField;
                    query.deleted = true;
                    query.change_timestamp = changeTimestamp;
                    conn.Update(query);
                }

                var queryItem = conn.Table<nswallet_items>().Where(x => x.item_id == itemID).SingleOrDefault();
				var q = conn.Table<nswallet_items>();
                queryItem.deleted = true;
                queryItem.change_timestamp = changeTimestamp;
                conn.Update(queryItem);
                UpdateProperties();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(DeleteItem));
				throw new DatabaseException("DeleteItem: " + ex.Message);
            }
        }

        public void DeleteFolder(string folderID, string changeTimestamp)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                var query = conn.Table<nswallet_items>().Where(x => x.item_id == folderID).SingleOrDefault();
                query.deleted = true;
                query.change_timestamp = changeTimestamp;
                conn.Update(query);
                UpdateProperties();
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(DeleteFolder));
				throw new DatabaseException("DeleteFolder: " + ex.Message);
            }
        }

        public void CreateField(string itemID, string fieldID, string type, byte[] value, string changeTimestamp, bool deleted, int sortWeight)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                conn.Insert(new nswallet_fields
                {
                    item_id = itemID,
                    field_id = fieldID,
                    type = type,
                    value = value,
                    change_timestamp = changeTimestamp,
                    deleted = deleted,
                    sort_weight = sortWeight
                });

                UpdateProperties();

                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(CreateField));
				throw new DatabaseException("CreateField: " + ex.Message);
            }
        }

        public bool CreateLabel(string fieldType, string name, string valueType, string icon, bool system, string changed, bool deleted)
        {
            try
            {
                CheckConnection();
                BeginTransaction();
                conn.Insert(new nswallet_labels
                {
                    field_type = fieldType,
                    label_name = name,
                    value_type = valueType,
                    icon = icon,
                    system = system,
                    change_timestamp = changed,
                    deleted = deleted
                });

                UpdateProperties();

                CommitTransaction();
				return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
				log(ex.Message, nameof(CreateLabel));
				throw new DatabaseException("CreateLabel: " + ex.Message);
				return false;
            }
        }

        public TableQuery<nswallet_items> RetrieveAllItems()
        {
            try
            {
                CheckConnection();
                return conn.Table<nswallet_items>().Where(v => v.deleted.Equals(0));
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(RetrieveAllItems));
				throw new DatabaseException("RetrieveAllItems: " + ex.Message);
            }
        }

        public TableQuery<nswallet_fields> RetrieveAllFields()
        {
            try
            {
                CheckConnection();
                return conn.Table<nswallet_fields>().Where(v => v.deleted.Equals(0));
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(RetrieveAllFields));
				throw new DatabaseException("RetrieveAllFields: " + ex.Message);
            }
        }

		/*
        public TableQuery<nswallet_labels> RetrieveAllLabels()
        {
            try
            {
                CheckConnection();
                return conn.Table<nswallet_labels>().Where(v => v.deleted.Equals(0));
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(RetrieveAllLabels));
				throw new DatabaseException("RetrieveAllLabels: " + ex.Message);
            }
        }
		*/

		public List<nswallet_labels_view> RetrieveAllLabelsView()
		{
			try {
				CheckConnection();
				List<nswallet_labels_view> labelsList = conn.Query<nswallet_labels_view>(SELECT_LABELS_WITH_USAGE);
				return labelsList;
			} catch (Exception ex) {
				log(ex.Message, nameof(RetrieveAllLabelsView));
				throw new DatabaseException("RetrieveAllLabels: " + ex.Message);
			}
		}

		public TableQuery<nswallet_icons> RetrieveAllIcons()
        {
			try {
				return conn.Table<nswallet_icons>();
			} catch (Exception ex) {
				log(ex.Message, nameof(RetrieveAllIcons));
				return null;
			}
        }

        public byte[] GetRootData()
        {
            try
            {
                CheckConnection();
                var query = conn.Table<nswallet_items>().Where(v => v.item_id.Equals(GConsts.ROOTID));

                if (query.Count() == 1)
                {
                    foreach (var item in query)
                    {
                        return item.name;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(GetRootData));
				throw new DatabaseException("GetRootData: " + ex.Message);
            }
        }

        public void SetProperties(string databaseID, string lang, string version, string email)
        {
            try
            {
                CheckConnection();
                var nswProps = new nswallet_properties()
                {
                    database_id = databaseID,
                    lang = lang,
                    version = version,
                    email = email,
                    update_timestamp = Common.ConvertDateTimeDB(DateTime.Now),
                    sync_timestamp = Common.ConvertDateTimeDB(DateTime.MinValue)
                };
                conn.Insert(nswProps);
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(SetProperties));
				throw new DatabaseException("SetProperties: " + ex.Message);
            }
        }

        public int PropertiesCount()
        {
            try
            {
                CheckConnection();
                return conn.Table<nswallet_properties>().Count();
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(PropertiesCount));
				throw new DatabaseException("PropertiesCount: " + ex.Message);
            }
        }

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(NSWalletDB));
		}
    }

    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {

        }
    }
}
