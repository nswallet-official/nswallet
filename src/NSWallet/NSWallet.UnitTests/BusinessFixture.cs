using System.IO;
using NSWallet.Shared;
using NUnit.Framework;

namespace NSWallet.UnitTests
{
    [TestFixture]
    public class BusinessFixture
    {
        const string defaultIcon = "document";
        const string defaultLang = "en";
        const string defaultIconPath = "Icons.items.icon_document_huge.png";
        const string _namespaceString = "NSWallet.NetStandard";

        string password;
        string dbPath;
		//string copyPrefix;

        [SetUp]
        public void PrepareDB()
        {
            dbPath = Path.GetTempPath() + "nswallet.dat";
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }

            TR.InitTR(_namespaceString);   // Preparing translations
            TR.SetLanguage(defaultLang);
            BL.InitAPI(dbPath, defaultLang);
            BL.InitNewStorage();
            password = CommonUT.RandomString(12);
            BL.CreateOnlyRootItem(password);
            BL.CreateSampleItems();
			//copyPrefix = TR.Tr("item_prefix_copy") + " ";
		}

        [TearDown]
        public void CloseDB()
        {
            BL.Close();
            if (!string.IsNullOrEmpty(dbPath) && File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        [Test]
        public void CheckProperties()
        {
            var props = BL.StorageProperties;
            Assert.NotNull(props);
            Assert.AreEqual(0, props.EncryptionCount);
            Assert.AreEqual(defaultLang, props.Lang);
            Assert.AreEqual(64, props.DatabaseID.Length);
            Assert.AreEqual(GConsts.DB_VERSION, props.Version);
        }

        [Test]
        public void CheckLogin()
        {
            Assert.IsTrue(BL.CheckPassword(password));
        }

        [Test]
        public void ChangePassword()
        {
            var oldPassowrd = password;
            Assert.IsTrue(BL.CheckPassword(password));
            var newPassword = CommonUT.RandomString(12);
            Assert.IsTrue(BL.ChangePassword(newPassword));
            Assert.IsTrue(BL.CheckPassword(newPassword));
            password = newPassword;
        }


        [Test]
        public void CheckDefaultItems()
        {
            var items = BL.GetCurrentItems();
            Assert.IsNotNull(items);
            Assert.AreEqual(3, items.Count);
        }


        const int SystemLabelsQty = 19;
        [Test]
        public void CheckDefaultLabels()
        {
            var labels = BL.GetLabels();
            Assert.IsNotNull(labels);
            Assert.AreEqual(SystemLabelsQty, labels.Count);
        }


        string itemName = "Test Item 123 !@#$%'\"";
        [Test]
        public void CreateItem()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemName, defaultIcon, false);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            Assert.AreEqual(itemId, item.ItemID);
            Assert.AreEqual(itemName, item.Name);
            Assert.AreEqual(defaultIconPath, item.Icon);
            Assert.AreEqual(false, item.Folder);
            Assert.AreEqual(false, item.Deleted);
            Assert.AreEqual(GConsts.ROOTID, item.ParentID);
        }

        string fieldValue = "Test Field 456 !@#$%'\"";
        [Test]
        public void CreateField()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemName, defaultIcon, false);
            BL.SetCurrentItemID(itemId);
            BL.AddField(GConsts.FLDTYPE_MAIL, fieldValue);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            var field = item.Fields.Find(f => f.FieldValue == fieldValue);
            Assert.NotNull(field);
            Assert.AreEqual(GConsts.FLDTYPE_MAIL, field.FieldType);
            Assert.AreEqual(GConsts.VALUETYPE_MAIL, field.ValueType);
            Assert.AreEqual(false, field.Deleted);
        }

        string labelname = "Test Label 789 !@#$%'\"";
        string labelIcon = "labelcalendar";
        [Test]
        public void CreateDeleteLabel()
        {
            BL.AddLabel(labelname, labelIcon, GConsts.VALUETYPE_DATE);
            var allLabels = BL.GetLabels();
            var createdLabel = allLabels.Find(l => l.Name == labelname);
            Assert.NotNull(createdLabel);
            Assert.AreEqual(false, createdLabel.Deleted);
            Assert.AreEqual(GConsts.VALUETYPE_DATE, createdLabel.ValueType);
            Assert.AreEqual(false, createdLabel.System);

            var labelID = createdLabel.FieldType;
            BL.RemoveLabel(labelID);

            allLabels = BL.GetLabels();
            var deletedLabel = allLabels.Find(l => l.Name == labelname);
            Assert.IsNull(deletedLabel);

        }

        string fieldValueDeletion = "dekdkewbdkjbewjdejkw !@#$%'\"";
        [Test]
        public void DeleteField()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemName, defaultIcon, false);
            BL.SetCurrentItemID(itemId);
            BL.AddField(GConsts.FLDTYPE_PASS, fieldValueDeletion);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            var field = item.Fields.Find(f => f.FieldValue == fieldValueDeletion);
            Assert.NotNull(field, "after creation");
            Assert.AreEqual(GConsts.FLDTYPE_PASS, field.FieldType);
            Assert.AreEqual(GConsts.VALUETYPE_PASS, field.ValueType);
            Assert.AreEqual(false, field.Deleted);
            BL.CheckPassword(password);
            BL.SetCurrentItemID(itemId);
            BL.DeleteField(field.FieldID);
            var field2 = item.Fields.Find(f => f.FieldValue == fieldValue);
            Assert.IsNull(field2, "after deletion");
        }

        string itemNameDeletion = "аиыфьиафывр78ыфвафы23 !@#$%'\"";
        [Test]
        public void DeleteItem()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemNameDeletion, defaultIcon, false);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            Assert.AreEqual(itemId, item.ItemID);
            Assert.AreEqual(itemNameDeletion, item.Name);
            Assert.AreEqual(defaultIconPath, item.Icon);
            Assert.AreEqual(false, item.Folder);
            Assert.AreEqual(false, item.Deleted);
            Assert.AreEqual(GConsts.ROOTID, item.ParentID);
            BL.CheckPassword(password);
            BL.DeleteItem(item.ItemID);
            var itemDeleted = BL.GetItemByID(itemId);
            Assert.IsNull(itemDeleted);
        }

        string itemNameBeforeChange = "аиыфьиафывр78ыфвафы23 !@#$%'\"";
        string itemNameAfterChange = "hjbhjerre9her !@#$%'\"";
        [Test]
        public void ChangeItem()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemNameBeforeChange, defaultIcon, false);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            Assert.AreEqual(itemId, item.ItemID);
            Assert.AreEqual(itemNameBeforeChange, item.Name);
            Assert.AreEqual(defaultIconPath, item.Icon);
            Assert.AreEqual(false, item.Folder);
            Assert.AreEqual(false, item.Deleted);
            Assert.AreEqual(GConsts.ROOTID, item.ParentID);
            BL.CheckPassword(password);
            BL.UpdateItemTitle(item.ItemID, itemNameAfterChange);
            var itemChanged = BL.GetItemByID(itemId);
            Assert.AreEqual(itemNameAfterChange, itemChanged.Name);
        }

        const string maestroIcon = "maestro";
        [Test]
        public void ChangeIcon()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(CommonUT.RandomString(50), defaultIcon, false);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            Assert.AreEqual(itemId, item.ItemID);
            Assert.AreEqual(defaultIconPath, item.Icon);
            Assert.AreEqual(false, item.Folder);
            Assert.AreEqual(false, item.Deleted);
            Assert.AreEqual(GConsts.ROOTID, item.ParentID);
            BL.CheckPassword(password);
            BL.UpdateItemIcon(item.ItemID, maestroIcon);
            var itemChanged = BL.GetItemByID(itemId);
            Assert.AreEqual(ImageManager.ConvertIconName2IconPath(maestroIcon), itemChanged.Icon);
        }


        string fieldValueBeforeChange = "sws ws nw sn !@#$%'\"";
        string fieldValueAfterChange = "safasbJHBJHDE773 !@#$%'\"";
        [Test]
        public void ChangeField()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemName, defaultIcon, false);
            BL.SetCurrentItemID(itemId);
            BL.AddField(GConsts.FLDTYPE_LINK, fieldValueBeforeChange);
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            var field = item.Fields.Find(f => f.FieldValue == fieldValueBeforeChange);
            Assert.NotNull(field, "after creation");
            Assert.AreEqual(GConsts.FLDTYPE_LINK, field.FieldType);
            Assert.AreEqual(GConsts.VALUETYPE_LINK, field.ValueType);
            Assert.AreEqual(false, field.Deleted);
            BL.CheckPassword(password);
            BL.SetCurrentItemID(itemId);

            // Updating field
            BL.UpdateField(field.FieldID, fieldValueAfterChange, DataAccessLayer.DO_NOT_CHANGE_SORT);
            var field2 = item.Fields.Find(f => f.FieldID == field.FieldID);
            Assert.AreEqual(fieldValueAfterChange, field2.FieldValue);
        }

        [Test]
        public void CheckPreviousPassword()
        {
            BL.SetCurrentItemID(GConsts.ROOTID);
            var itemId = BL.AddItem(itemName, defaultIcon, false);
            BL.SetCurrentItemID(itemId);
            var firstPassword = CommonUT.RandomString(24);
            var fieldIDpass = BL.AddField(GConsts.FLDTYPE_PASS, firstPassword);
            var fieldIDoldPass = BL.AddField(GConsts.FLDTYPE_OLDP, "");
            BL.CheckPassword(password);
            var item = BL.GetItemByID(itemId);
            var field = item.Fields.Find(f => f.FieldID == fieldIDpass);
            Assert.NotNull(field, "after creation");
            Assert.AreEqual(firstPassword, field.FieldValue);
            Assert.AreEqual(GConsts.FLDTYPE_PASS, field.FieldType);
            Assert.AreEqual(GConsts.VALUETYPE_PASS, field.ValueType);
            Assert.AreEqual(false, field.Deleted);
            BL.CheckPassword(password);

            BL.SetCurrentItemID(itemId);
            var secondPassword = CommonUT.RandomString(24);
            BL.UpdateField(fieldIDpass, secondPassword, DataAccessLayer.DO_NOT_CHANGE_SORT);

            var fieldUpdated = item.Fields.Find(f => f.FieldID == field.FieldID);
            Assert.AreEqual(secondPassword, fieldUpdated.FieldValue);

            var fieldOldPassword = item.Fields.Find(f => f.FieldID == fieldIDoldPass);
            Assert.NotNull(fieldOldPassword, "oldPasswordField should be there!");
            Assert.AreEqual(firstPassword, fieldOldPassword.FieldValue);
        }

		[Test]
		public void CopyItem()
		{
			BL.SetCurrentItemID(GConsts.ROOTID);
			var itemId = BL.AddItem(itemName, defaultIcon, false);
			var copyItemId = BL.CopyItem(itemId);
			var item = BL.GetItemByID(copyItemId);
			Assert.AreEqual(BL.GetCopyPrefix() + itemName, item.Name);
			Assert.AreEqual(GConsts.ROOTID, item.ParentID);
		}

		[Test]
		public void CopyFolder()
		{
			BL.SetCurrentItemID(GConsts.ROOTID);
			var itemId = BL.AddItem(itemName, defaultIcon, true);
			var copyItemId = BL.CopyItem(itemId);
			var folder = BL.GetItemByID(copyItemId);
			Assert.AreEqual(BL.GetCopyPrefix() + itemName, folder.Name);
			Assert.AreEqual(GConsts.ROOTID, folder.ParentID);
		}

		[Test]
		public void CopyField()
		{
			BL.SetCurrentItemID(GConsts.ROOTID);
			var itemId = BL.AddItem(itemName, defaultIcon, true);
			BL.SetCurrentItemID(itemId);
			var fieldId = BL.AddField(GConsts.FLDTYPE_MAIL, fieldValue);
			BL.SetCurrentFieldID(fieldId);
			var newFieldId = BL.CopyField(itemId);
			var item = BL.GetItemByID(itemId);
			Assert.NotNull(item.Fields);
			var field = item.Fields.Find(f => f.FieldID == newFieldId);
			Assert.NotNull(field);
			Assert.AreEqual(GConsts.FLDTYPE_MAIL, field.FieldType);
			Assert.AreEqual(GConsts.VALUETYPE_MAIL, field.ValueType);
			Assert.AreEqual(false, field.Deleted);
		}

		[Test]
		public void MoveField()
		{
			BL.SetCurrentItemID(GConsts.ROOTID);
			var itemId = BL.AddItem(itemName, defaultIcon, true);
			var itemId2 = BL.AddItem(itemName + "2", defaultIcon, true);
			BL.SetCurrentItemID(itemId);
			var fieldId = BL.AddField(GConsts.FLDTYPE_MAIL, fieldValue);
			BL.SetCurrentFieldID(fieldId);
			BL.MoveField(itemId);
			var item = BL.GetItemByID(itemId2);
			Assert.NotNull(item.Fields);
		}

		[Test]
		public void MoveItem()
		{
			BL.SetCurrentItemID(GConsts.ROOTID);
			var itemId = BL.AddItem(itemName, defaultIcon, true);
			var folderId = BL.AddItem(itemName, defaultIcon, true);
			BL.SetCurrentItemID(folderId);
			BL.MoveItem(itemId);
			var items = BL.GetCurrentItems();
			Assert.NotNull(items);
		}
	}
}
