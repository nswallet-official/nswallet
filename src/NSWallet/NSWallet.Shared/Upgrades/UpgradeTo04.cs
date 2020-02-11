namespace NSWallet.Shared
{
	public static partial class UpgradeManager
	{
		static bool UpgradeTo04()
		{
			if (RetrieveCurrentDBVersion() != 3) {
				return false;
			}

			var id = BL.AddLabel(
				TR.TrEn(GConsts.FLDTYPE_2FAC),
				GConsts.FLDTYPE_ACNT_ICON,
				GConsts.VALUETYPE_PASS,
				GConsts.FLDTYPE_2FAC,
				true);

			if (!string.IsNullOrEmpty(id)) {
				BL.SetDBVersion("4");
			}

			return true;
		}
	}
}
