namespace NSWallet.NetStandard.Helpers
{
	public static class IconHandler
	{
		const string firstPartItemIconPath = "Icons.items.icon_";
		const string secondPartItemIconPath = "_huge.png";

		public static string GetPathForItemIcon(string key)
		{
			return firstPartItemIconPath + key + secondPartItemIconPath;
		}
	}
}