namespace NSWallet.Shared
{
    public static partial class UpgradeManager
    {
		static bool UpgradeTo03()
		{
			if (RetrieveCurrentDBVersion() != 2)
			{
				return false;
			}
			var success = BL.AddColumnsToTablesUpgrade03();
			if (success) {
				BL.SetDBVersion("3");
			}
			return true;
		}
    }
}