using SQLite;

namespace NSWallet.Shared
{

	public class nswallet_properties
	{
		[PrimaryKey, NotNull]
		public string database_id { get; set; }  // 32
		public string lang { get; set; }         // 2
		public string version { get; set; }      //10
		public string email { get; set; }        // 200
		public string sync_timestamp { get; set;}
		public string update_timestamp { get; set; }
	}

	public class nswallet_items
	{
		[PrimaryKey, NotNull]
		public string item_id { get; set; } // 8
		public string parent_id { get; set; } // 8
		public byte[] name { get; set; }
		public string icon { get; set; } // 48
		public string field_id { get; set; } // 4
		public bool folder { get; set; } 
		public string create_timestamp { get; set; }
		public string change_timestamp { get; set; }
		public bool deleted { get; set; }
	}

	public class nswallet_labels
	{
		[PrimaryKey, NotNull]
		public string field_type { get; set; }
		public string label_name { get; set; }
		public string value_type { get; set; }
		public string icon { get; set; }
		public bool system { get; set; }
		public string change_timestamp { get; set; }
		public bool deleted { get; set; }
	}

	public class nswallet_fields
	{
		[PrimaryKey, NotNull]
		public string item_id { get; set; } 
		[PrimaryKey, NotNull]
		public string field_id { get; set; }
		public string type { get; set; }
		public byte[] value { get; set; }
		public string change_timestamp { get; set; }
		public bool deleted { get; set; }
		public int sort_weight { get; set; }
	}

	public class nswallet_icons
	{
		[PrimaryKey, NotNull]
		public string icon_id { get; set; }
		public string name { get; set; }
		public byte[] icon_blob { get; set; }
		public int group_id { get; set; }
		public bool is_circle { get; set; } = true;
		public bool deleted { get; set; }
	}

    public class nswallet_groups
    {
        [PrimaryKey, NotNull]
        public int group_id { get; set; }
        public string name { get; set; }
		public bool deleted { get; set; }
    }

	public class nswallet_labels_view
	{
		public string field_type { get; set; }
		public string label_name { get; set; }
		public string value_type { get; set; }
		public string icon { get; set; }
		public bool system { get; set; }
		public string change_timestamp { get; set; }
		public bool deleted { get; set; }
		public int usage { get; set; }
	}
}
