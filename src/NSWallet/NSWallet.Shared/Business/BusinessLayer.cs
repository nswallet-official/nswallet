using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSWallet.Shared.Helpers.Logs.AppLog;
using NSWallet.Shared.Helpers.Logs.DeviceInfo;

namespace NSWallet.Shared
{
	public static partial class BL
    {
        static string lang;
		static string lastCurrentID;
        public static string CurrentFieldID { get; private set; }
        public static string CurrentItemID { get; private set; }


        public static void InitAPI(string dbFileName, string language)
        {
            DataAccessLayer.Init(dbFileName);
            lang = language;
            CurrentItemID = GConsts.ROOTID;
        }

		public static bool PrepareDiagnostics()
		{
			var dal = DataAccessLayer.GetInstance();
			return dal.PrepareDiagnostics();
		}

		public static bool IsDiagnosticsRunning()
		{
			var dal = DataAccessLayer.GetInstance();
			return dal.IsDiagnosticsRunning();
		}

		public static async Task<bool> RunDiagnostics(string path, LogsDeviceInfo logsDeviceInfo, bool fromLogin)
		{
			var dal = DataAccessLayer.GetInstance();
			return await dal.RunDiagnostics(path, logsDeviceInfo, fromLogin);
		}

        public static void SetCurrentFieldID(string id)
        {
            CurrentFieldID = id;
        }

        public static void ClearCurrentFieldID()
        {
            CurrentFieldID = null;
        }

        public static void SetExpirationSettings(bool addExpiryFolder, int expPeriod) {
            ExpiringSoon = addExpiryFolder;
            DataAccessLayer.GetInstance().ExpiryDate = DateTime.Now + TimeSpan.FromDays(expPeriod);
        }

        public static void SetRecentlyViewSetting(bool addRecentSetting) {
            RecentlyViewed = addRecentSetting;
        }

        public static void SetMostlyViewSetting(bool addMostSetting)
        {
            MostlyViewed = addMostSetting;
        }

		public static string GetFieldTypeNameByShortName(string fieldTypeShortName)
		{
			switch (fieldTypeShortName) {
				case GConsts.FLDTYPE_ACNT:
					return TR.Tr("ACNT");
				case GConsts.FLDTYPE_ADDR:
					return TR.Tr("ADDR");
				case GConsts.FLDTYPE_CARD:
					return TR.Tr("CARD");
				case GConsts.FLDTYPE_DATE:
					return TR.Tr("DATE");
				case GConsts.FLDTYPE_EXPD:
					return TR.Tr("EXPD");
				case GConsts.FLDTYPE_LINK:
					return TR.Tr("LINK");
				case GConsts.FLDTYPE_MAIL:
					return TR.Tr("MAIL");
				case GConsts.FLDTYPE_NAME:
					return TR.Tr("NAME");
				case GConsts.FLDTYPE_NOTE:
					return TR.Tr("NOTE");
				case GConsts.FLDTYPE_OLDP:
					return TR.Tr("OLDP");
				case GConsts.FLDTYPE_PASS:
					return TR.Tr("PASS");
				case GConsts.FLDTYPE_PHON:
					return TR.Tr("PHON");
				case GConsts.FLDTYPE_PINC:
					return TR.Tr("PINC");
				case GConsts.FLDTYPE_SANS:
					return TR.Tr("SANS");
				case GConsts.FLDTYPE_SNUM:
					return TR.Tr("SNUM");
				case GConsts.FLDTYPE_SQUE:
					return TR.Tr("SQUE");
				case GConsts.FLDTYPE_TIME:
					return TR.Tr("TIME");
				case GConsts.FLDTYPE_USER:
					return TR.Tr("USER");
				default:
					return fieldTypeShortName;
			}
		}

        public static string GetCurrentPath()
        {
            if (CurrentItemID == GConsts.EXPIRING_SOON_ID)
            {
                return "/" + TR.Tr("expiringsoon_folder") + "/";
            }

            if (lastCurrentID == GConsts.EXPIRING_SOON_ID)
            {
                return "/" + TR.Tr("expiringsoon_folder") + "/" + GetCurrentItem().Name + "/";
            }

            if (CurrentItemID == GConsts.RECENTLY_VIEWED_FOLDER_ID)
            {
                return "/" + TR.Tr("recentlyviewed_folder") + "/";
            }

            if (lastCurrentID == GConsts.RECENTLY_VIEWED_FOLDER_ID)
            {
                return "/" + TR.Tr("recentlyviewed_folder") + "/" + GetCurrentItem().Name + "/";
            }

            if (CurrentItemID == GConsts.MOSTLY_VIEWED_ID)
            {
                return "/" + TR.Tr("mostlyviewed_folder") + "/";
            }

            if (lastCurrentID == GConsts.MOSTLY_VIEWED_ID)
            {
                return "/" + TR.Tr("mostlyviewed_folder") + "/" + GetCurrentItem().Name + "/";
            }

            if (bufferedPath.ContainsKey(CurrentItemID))
            {
                return bufferedPath[CurrentItemID];
            }
            bufferedPath.Add(CurrentItemID, GetRecursivePath(CurrentItemID, ""));
            return bufferedPath[CurrentItemID];

        }

        static void ClearPathBuffer()
        {
            bufferedPath?.Clear();
        }

        static Dictionary<string, string> bufferedPath = new Dictionary<string, string>();
        public static string GetRecursivePath(string itemID, string constructedPath)
        {
            if (itemID == GConsts.ROOTID)
            {
                return "/" + constructedPath;
            }
            return GetRecursivePath(DataAccessLayer.GetInstance().GetParentIDByItemID(itemID),
                                    DataAccessLayer.GetInstance().GetItemByID(itemID).Name + "/" + constructedPath);
        }

        public static bool IsNew()
        {
            var dal = DataAccessLayer.GetInstance();
            if (!dal.ArePropsSet || !dal.SearchForRoot())
            {
                return true;
            }
            return false;
        }

		public static bool ExpiringSoon { get; private set; }
        public static bool RecentlyViewed { get; private set; }
        public static bool MostlyViewed { get; private set; }

        public static void Close()
        {
            DataAccessLayer.Close();
        }

        public static void InitNewStorage()
        {
            var dal = DataAccessLayer.GetInstance();

            if (dal.ArePropsSet == false)
            {
                dal.SetNewProperties(lang);

            }
            AddSystemLabels();
			CreateIconGroups();
            UpgradeManager.Upgrade();
        }

		public static bool AddColumnsToTablesUpgrade03()
		{
			var dal = DataAccessLayer.GetInstance();
			return dal.AddColumnsToTablesUpgrade03();
		}

		public static void CreateSampleItems()
        {
            var dal = DataAccessLayer.GetInstance();

            // Banking
            var folderBankingID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(folderBankingID, GConsts.ROOTID, TR.Tr("init_items_banking"), "folderbanking", true);

            var visaID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(visaID, folderBankingID, TR.Tr("init_items_credit_card_sample"), "visa", false);

            var creditNumberID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
            dal.CreateField(visaID, creditNumberID, GConsts.FLDTYPE_CARD, "1234 5678 0987 6543", 100);

            // Internet
            var folderInternetID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(folderInternetID, GConsts.ROOTID, TR.Tr("init_items_internet"), "folderinternet", true);

            // Internet - Support
            var supportID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(supportID, folderInternetID, TR.Tr("init_items_support"), "heart", false);

            var emailSupID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
            dal.CreateField(supportID, emailSupID, GConsts.FLDTYPE_MAIL, "support@nyxbull.com", 100);

            var webSupID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
            dal.CreateField(supportID, webSupID, GConsts.FLDTYPE_LINK, @"www.nyxbull.com/support", 200);

            // Internet - Facebook
            var facebookID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(facebookID, folderInternetID, TR.Tr("init_items_facebook"), "facebook", false);

            var webFacebookID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
            dal.CreateField(facebookID, webFacebookID, GConsts.FLDTYPE_LINK, @"www.facebook.com/nswallet", 100);

            // Internet - Twitter
            var twitterID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(twitterID, folderInternetID, TR.Tr("init_items_twitter"), "twitter", false);

            var webTwitterID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
            dal.CreateField(twitterID, webTwitterID, GConsts.FLDTYPE_LINK, @"www.twitter.com/nyxbullsoft", 100);

            // Read me
            var readmeID = Common.GenerateUniqueString(GConsts.ITEMID_LENGTH);
            dal.CreateItem(readmeID, GConsts.ROOTID, TR.Tr("init_items_readme"), "document", false);

            var readmeNoteID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
            dal.CreateField(readmeID, readmeNoteID, GConsts.FLDTYPE_NOTE, TR.Tr("init_items_readme_text"), 100);
        }

		public static void CreateIconGroups()
		{
			initGroup("category_internet");
			initGroup("category_travelling");
			initGroup("category_finances");
			initGroup("category_technologies");
			initGroup("category_common");
			initGroup("category_social");
			initGroup("category_development");
			initGroup("category_clouds");
			initGroup("category_games");
		}

		static void initGroup(string category)
		{
			var groups = GetGroups();
			var groupExists = groups.Exists(x => string.Compare(x.Name, category) == 0);
			if (!groupExists) {
				int groupID = 0;
				if (groups.Count > 0) {
					groupID = groups.Max(x => x.GroupID) + 1;
				}
				InsertGroup(groupID, category);
			}
		}

		public static NSWProperties StorageProperties
        {
            get
            {
                var dal = DataAccessLayer.GetInstance();
                return dal.StorageProperties;
            }
        }




        public static void SetDBVersion(string version)
        {
            var dal = DataAccessLayer.GetInstance();
            dal.SetDBVersion(version);
        }

        public static int CountSearchPhrase(string searchPhrase)
        {
            return searchPhrase.Length;
        }

        public static List<NSWItem> Search(string searchPhrase)
        {
            if (SM.CheckPhraseLength(searchPhrase, GConsts.SEARCH_MIN_LENGTH))
            {
                var searchResult = new List<NSWItem>();
                searchPhrase = SM.ConvertToLower(searchPhrase);
                var dal = DataAccessLayer.GetInstance();
                var items = dal.Items;

                var searchItems = items.FindAll(x => SM.ConvertToLower(x.Name).Contains(searchPhrase) && !x.Folder);
                if (searchItems != null)
                    if (searchItems.Count > 0)
                        searchResult.AddRange(searchItems);
                var searchFields = items.FindAll(x => x.Fields != null && x.Fields.Exists(y => SM.ConvertToLower(y.FieldValue).Contains(searchPhrase)));
                if (searchFields != null)
                    if (searchFields.Count > 0)
                        searchResult.AddRange(searchFields);
                
                if (searchResult.Count > 0)
                    return searchResult.Distinct().ToList();
                return null;
            }

            return null;
        }

        public static List<NSWIcon> GetIcons()
        {
            return DataAccessLayer.GetInstance().Icons;
        }

		public static List<NSWGroup> GetGroups()
		{
			return DataAccessLayer.GetInstance().Groups;
		}

		public static bool InsertIcon(byte[] iconBLOB, int groupID, string iconID = null, string name = null)
        {
			if (iconID == null) {
				iconID = Common.GenerateUniqueString(GConsts.ICONID_LENGTH);
			}
			if (name == null) {
				name = iconID;
			}
            var dal = DataAccessLayer.GetInstance();
			ResetData(false, false, false, true);
            return dal.InsertIcon(iconID, name, iconBLOB, groupID);
        }

		public static bool DeleteIcon(string iconID)
		{
			if (iconID != null) {
				var dal = DataAccessLayer.GetInstance();
				ResetData(false, false, false, true);
				return dal.DeleteIcon(iconID);
			}
			return false;
		}

		public static bool InsertGroup(int groupID, string name)
		{
			var dal = DataAccessLayer.GetInstance();
            var result = dal.InsertGroup(groupID, name);
			ResetData(false, false, false, false, true);
			return result;
		}

		public static bool UpdateGroup(int groupID, string name)
		{
			var dal = DataAccessLayer.GetInstance();
			var res = dal.UpdateGroup(groupID, name);
			ResetData(false, false, false, false, true);
			return res;
		}

		/// <summary>
		/// Updates the icon.
		/// </summary>
		/// <returns><c>true</c>, if icon was updated, <c>false</c> otherwise.</returns>
		/// <param name="iconID">Icon identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="blob">BLOB.</param>
		/// <param name="groupID">Group identifier.</param>
		/// <param name="isCircle">Is circle: 0 - <see langword="false"/>, 1 - <see langword="true"/>.</param>
		public static bool UpdateIcon(string iconID, string name, byte[] blob = null, int groupID = -1, int isCircle = -1)
		{
			var dal = DataAccessLayer.GetInstance();
			var res = dal.UpdateIcon(iconID, name, blob, groupID, isCircle);
			ResetData(false, false, false, true, false);
			return res;
		}



        public static bool CheckPassword(string password)
        {
            return DataAccessLayer.GetInstance().CheckPassword(password);
        }

        public static string GetDbID()
        {
            return DataAccessLayer.GetInstance().StorageProperties.DatabaseID;
        }

		public static List<NSWItem> GetAllItems()
		{
			var dal = DataAccessLayer.GetInstance();
			return dal.Items;
		}

        public static List<NSWItem> GetCurrentItems()
        {
            return GetListByParentID(CurrentItemID, false);
        }


        public static bool SearchForRoot()
        {
            return DataAccessLayer.GetInstance().SearchForRoot();
        }

        public static void CreateOnlyRootItem(string newPassword)
        {
			if (newPassword == GConsts.DEMO_PASSWORD) {
				// Special logic to be used for testing purposes only 
				DataAccessLayer.RestoreDemoDatabase(); 
				if (CheckPassword(GConsts.TEST_PASSWORD) == false) {
					throw new Exception("Error restoring Demo database");
				}
				return;
			}
            DataAccessLayer.GetInstance().CreateOnlyRootItem(newPassword);
        }

        public static void ResetData(bool resetItems, bool resetFields, bool resetLabels, bool icons = false, bool groups = false)
        {
            bufferedPath.Clear();
            expiringItems = null;
			DataAccessLayer.GetInstance().ResetMemoryData(resetItems, resetFields, resetLabels, icons, groups);
        }

        public static bool ChangePassword(string newPassword)
        {
            return DataAccessLayer.GetInstance().ChangePassword(newPassword);
        }

		public static bool CheckDBVersion(string filename) {
			Int32 currentDBVersion = 0;
			Int32 backupVersion;

			try {
				currentDBVersion = Convert.ToInt32(GConsts.DB_VERSION);
				backupVersion = Convert.ToInt32(NSWalletDBCheck.GetDBVersion(filename));
			} catch(Exception ex) {
				backupVersion = -1;
				log(ex.Message, nameof(CheckDBVersion));
			}
			if (backupVersion <= 0) return false;
			if (backupVersion > currentDBVersion) return false;
			return true;
		}

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(BL));
		}
    }
}
