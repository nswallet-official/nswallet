namespace NSWallet.Shared
{
	public partial class NSWalletDB
	{
		// Workaround to create composite key as sqlite.net library does not support
		// composite keys on its own. If one day sqlite.net will support composite
		// keys then remove this ugly lines of code below
		// Workaround start: part1
		public const string CREATE_FIELDS =
			   "CREATE TABLE IF NOT EXISTS nswallet_fields "
			 + "( item_id          CHAR(8) NOT NULL , "
			 + "  field_id         CHAR(4) NOT NULL , "
			 + "  type             CHAR(4), "
			 + "  value            BLOB, "
			 + "  change_timestamp DATETIME, "
			 + "  deleted          BOOL DEFAULT 0, "
			 + "  sort_weight      INTEGER, "
			 + "PRIMARY KEY (item_id,field_id) ) ";
		// Workaround end: part1

		public const string SELECT_LABELS_WITH_USAGE =
			  "SELECT nswallet_labels.field_type, "
			+ "       nswallet_labels.label_name, "
			+ "       nswallet_labels.value_type, "
			+ "       nswallet_labels.icon, "
			+ "       nswallet_labels.system, "
			+ "       nswallet_labels.change_timestamp, "
			+ "       nswallet_labels.deleted, "
			+ "       count(nswallet_fields.type) as usage "
			+ "FROM nswallet_labels "
			+ "LEFT JOIN nswallet_fields "
			+ "ON nswallet_labels.field_type=nswallet_fields.type "
			+ "WHERE nswallet_labels.deleted=0 "
			+ "GROUP BY nswallet_labels.field_type "
			+ "ORDER BY usage DESC";
	}
}
