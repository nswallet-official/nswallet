using System.Linq;

namespace NSWallet.Shared
{
    public static partial class BL
    {
		public static void DeleteField(string id, string itemID = null)
		{
			if (itemID == null) {
				itemID = CurrentItemID;
			}
			DataAccessLayer.GetInstance().DeleteField(id, itemID);
		}

		public static string AddField(string type, string fieldValue, string itemID = null, int weight = 0, string fieldID = null)
		{
			var dal = DataAccessLayer.GetInstance();
			if (fieldID == null) {
				fieldID = Common.GenerateUniqueString(GConsts.FIELDID_LENGTH);
			}
			if (itemID == null) {
				itemID = CurrentItemID;
				var nswItem = dal.GetItemByID(itemID);
				weight = 100;
				if (nswItem.Fields != null) {
					if (nswItem.Fields.Count > 0) {
						var maxWeight = nswItem.Fields.Max((x) => x.SortWeight);
						weight = maxWeight + 100;
					}
				}
			}

			dal.CreateField(itemID, fieldID, type, fieldValue, weight);
			return fieldID;
		}

        public static void UpdateField(string fieldID, string fieldValue, int sortWeight)
        {
            string oldPassword = null;
            var dal = DataAccessLayer.GetInstance();


            NSWField field = dal.GetFieldByItemFieldIDs(CurrentItemID, fieldID);
            NSWField oldPassField = dal.GetFieldByFieldType(CurrentItemID, GConsts.FLDTYPE_OLDP);

            if (field != null && oldPassField != null && field.FieldType == GConsts.FLDTYPE_PASS)
            {
                oldPassword = field.FieldValue;
            }

            dal.UpdateField(CurrentItemID, fieldID, fieldValue, sortWeight);

            if (!string.IsNullOrEmpty(oldPassword))
            {
                if (oldPassField != null)
                {
                    dal.UpdateField(CurrentItemID, oldPassField.FieldID, oldPassword, DataAccessLayer.DO_NOT_CHANGE_SORT);
                }
            }
        }
    }
}