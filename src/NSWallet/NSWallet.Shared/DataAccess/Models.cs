using System;
using System.Collections.Generic;

namespace NSWallet.Shared
{
	public class NSWProperties
	{
		public string DatabaseID { get; set; }  // 32
		public string Lang { get; set; }         // 2
		public string Version { get; set; }      //10
		public int EncryptionCount { get; set; }        // 200
		public DateTime SyncTimestamp { get; set; }
		public DateTime UpdateTimestamp { get; set; }
	}

	public class NSWIcon
	{
		public string IconID { get; set; }
		public string Name { get; set; }
		public byte[] Icon { get; set; }
		public int GroupID { get; set; }
		public bool IsCircle { get; set; }
		public bool IsNotCircle { get { return !IsCircle; } }
		public bool Deleted { get; set; }
	}

	public class NSWGroup
	{
		public int GroupID { get; set; }
		public string Name { get; set; }
		public bool Deleted { get; set; }
	}

	public class NSWItem
	{
		public string ItemID { get; set; }
		public string ParentID { get; set; }
		public string Name { get; set; }
		public string Icon { get; set; }
		public bool Folder { get; set; }
		public DateTime CreateTimestamp { get; set; }
		public DateTime UpdateTimestamp { get; set; }
		public bool Deleted { get; set; }
		public bool Expired { get; set; }
		public int ExpireInDays { get; set; }

		List<NSWField> fields;
		public List<NSWField> Fields {
			get {
				if (fields != null) {
					return fields;
				}
				fields = DataAccessLayer.GetInstance().GetFieldsByItemID(ItemID);
				return fields;
			}
		}

		public void ClearFields()
		{
			fields = null;
		}

		internal object SingleOrDefault()
		{
			throw new NotImplementedException();
		}
	}


	public class NSWField
	{
		public string ItemID { get; set; }
		public string FieldID { get; set; }
		public string FieldType { get; set; }
		public string ValueType { get; set; }
		public string FieldValue { get; set; }
		public string Icon { get; set; }
		public string Label { get; set; }
		public DateTime UpdateTimestamp { get; set; }
		public int SortWeight { get; set; }
		public bool Deleted { get; set; }
		public bool Expired { get; set; }
		public bool Expiring { get; set; }
		public string HumanReadableValue {
			get {
				if (FieldType == GConsts.FLDTYPE_CARD) {
					return Common.ConvertCard2HumanReadable(FieldValue);
				} else if (ValueType == GConsts.VALUETYPE_DATE) {
					return Common.ConvertStringToStringDate(FieldValue);
				} else if (ValueType == GConsts.VALUETYPE_TIME) {
					return Common.ConvertStringToStringTime(FieldValue);
				} else {
					return FieldValue;
				}
			}
		}
	}

	public class NSWLabel
	{
		public string FieldType { get; set; }
		public string Name { get; set; }
		public string ValueType { get; set; }
		public string Icon { get; set; }
		public bool System { get; set; }
		public DateTime UpdateTimestamp { get; set; }
		public bool Deleted { get; set; }
		public int Usage { get; set; }
	}
}