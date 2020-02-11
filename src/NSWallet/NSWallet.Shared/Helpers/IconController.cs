namespace NSWallet.Shared
{
	public static class IconController
	{
		public static string SubstringItemName(string icon)
		{
			return !string.IsNullOrEmpty(icon) ? icon.Substring(17, icon.Length - 26) : null;
		}

		public static string SubstringFolderName(string icon)
		{
			return !string.IsNullOrEmpty(icon) ? icon.Substring(17, icon.Length - 21) : null;
		}
	}
}