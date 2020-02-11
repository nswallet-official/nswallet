using System.Linq;
/*  Changes related only to upgrade from DB version 02 to 03
 *  adding 2 new columns to the table icons
 *  adding 2 new column to the table groups
 *  checks, if the fields where added properly
 */


namespace NSWallet.Shared
{
	public partial class NSWalletDB
	{
		const string ALTER_TABLE_ICONS_IS_CIRCLE_UPGRADE_03 = "ALTER TABLE nswallet_icons ADD COLUMN [is_circle] BOOL DEFAULT 0";
		const string ALTER_TABLE_ICONS_DELETED_UPGRADE_03 = "ALTER TABLE nswallet_icons ADD COLUMN [deleted] BOOL DEFAULT 0";
		const string ALTER_TABLE_GROUPS_DELETED_UPGRADE_03 = "ALTER TABLE nswallet_groups ADD COLUMN [deleted] BOOL DEFAULT 0";

		public bool AddColumnsToTablesUpgrade03()
		{
			try {
				CheckConnection();
				BeginTransaction();

				bool isAlterExecuted = false;

				if (!CheckForColumnInIcons("is_circle")) {
					conn.Execute(ALTER_TABLE_ICONS_IS_CIRCLE_UPGRADE_03);
					isAlterExecuted = true;
				}

				if (!CheckForColumnInIcons("deleted")) {
					conn.Execute(ALTER_TABLE_ICONS_DELETED_UPGRADE_03);
					isAlterExecuted = true;
				}

				if (!CheckForColumnInGroups("deleted")) {
					conn.Execute(ALTER_TABLE_GROUPS_DELETED_UPGRADE_03);
					isAlterExecuted = true;
				}

				if (isAlterExecuted) {
					CommitTransaction();
				} else {
					RollbackTransaction();
				}
				return true;
			} catch {
				RollbackTransaction();
				return false;
			}
		}

		bool CheckForColumnInIcons(string column)
		{
			var table = conn.Table<nswallet_icons>();
			if (table != null) {
				if (table.Table != null) {
					var columns = table.Table.Columns;
					if (columns.Any(x => x.Name == column)) {
						return true;
					}
				}
			}
			return false;
		}

		bool CheckForColumnInGroups(string column)
		{
			var table = conn.Table<nswallet_groups>();
			if (table != null) {
				if (table.Table != null) {
					var columns = table.Table.Columns;
					if (columns.Any(x => x.Name == column)) {
						return true;
					}
				}
			}
			return false;
		}


	}

}
