using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NSWallet.Shared
{
    public partial class DataAccessLayer
    {
        public List<NSWField> Fields
        {
            get
            {
                if (fields != null)
                {
                    return fields;
                }
                if (currentPassword == string.Empty)
                {
                    throw new PasswordNotSetException("Cannot decrypt, password is not set");
                }
                fields = new List<NSWField>();

                var queryResult = nswdb.RetrieveAllFields();
                foreach (var field in queryResult)
                {
                    var newField = new NSWField()
                    {
                        ItemID = field.item_id,
                        FieldID = field.field_id,
                        FieldType = field.type,
                        SortWeight = field.sort_weight,
                        Deleted = field.deleted,
                        FieldValue = Security.DecryptStringAES(field.value, currentPassword,
                                            StorageProperties.EncryptionCount,
                                            currentPassword,
                                            out bool ok)
                    };
                    if (ok == false)
                    {
                        throw new DecryptException("Decrypt failure during fields retrieval");
                    }
                    newField.UpdateTimestamp = Common.ConvertDBDateTime(field.change_timestamp);

                    // Special logic for expiry date
                    if (newField.FieldType == GConsts.FLDTYPE_EXPD)
                    {

                        SetExpiryFlags(ExpiryDate, newField.FieldValue,
                                       out bool expiring, out bool expired);
                        newField.Expired = expired;
                        newField.Expiring = expiring;
                    }

                    var newLabel = GetLabelByFieldType(newField.FieldType);

                    if (newLabel.ValueType == GConsts.VALUETYPE_TIME) {
                        if (newField.FieldValue.Length == 4) {
                            newField.FieldValue = newField.FieldValue.Substring(0, 2) + ":" + newField.FieldValue.Substring(2, 2);
                        }
                    }

                    ////// Add also
                    newField.Icon = newLabel.Icon;
                    newField.ValueType = newLabel.ValueType;
                    newField.Label = newLabel.Name;

                    fields.Add(newField);
                }
                return fields;
            }
        }


        public void DeleteField(string fieldID, string itemID)
        {
            nswdb.DeleteField(itemID, fieldID, Common.ConvertDateTimeDB(DateTime.Now));
			ResetMemoryData(true, true, true);
        }

        public void CreateField(string itemID, string fieldID, string type, string value, int sortWeight)
        {
            var valueEncrypted = Security.EncryptStringAES(value, currentPassword, 0, currentPassword, out bool ok);

            if (ok)
            {
                nswdb.CreateField(itemID, fieldID, type, valueEncrypted, Common.ConvertDateTimeDB(DateTime.Now), false, sortWeight);
            }
            else
            {
                throw new EncryptException("Encrypt failure during field creation");
            }
            GetItemByID(itemID).ClearFields();
            fields = null;
        }

        public void UpdateField(string itemID, string fieldID,  string fieldValue, int sortWeight)
        {
            var valueEncrypted = Security.EncryptStringAES(fieldValue, currentPassword, 0, currentPassword, out bool ok);

            if (ok)
            {
				nswdb.UpdateField(fieldID, valueEncrypted,  sortWeight, Common.ConvertDateTimeDB(DateTime.Now));
            }
            else
            {
                throw new EncryptException("Encrypt failure during field creation");
            }
            GetItemByID(itemID).ClearFields();
            fields = null;
        }

        public List<NSWField> GetFieldsByItemID(string itemID)
        {
            return Fields.FindAll(f => f.ItemID == itemID);
        }

        public NSWField GetFieldByItemFieldIDs(string itemID, string fieldID) {
            var itemFields = GetFieldsByItemID( itemID);
            if (itemFields == null )
                return null;
            return itemFields.Find(field => field.FieldID == fieldID);
        }

        public NSWField GetFieldByFieldType(string itemID, string fieldType) {
            var itemFields = GetFieldsByItemID(itemID);
            if (itemFields == null)
                return null;
            return itemFields.Find(field => field.FieldType == fieldType);
        }
    }
}