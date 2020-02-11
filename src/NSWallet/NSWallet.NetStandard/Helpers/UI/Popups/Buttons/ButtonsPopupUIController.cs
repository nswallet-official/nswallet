using NSWallet.Shared;

namespace NSWallet.NetStandard.Helpers.UI.Popups.Buttons
{
	public static class ButtonsPopupUIController
	{
		public static string[] GetIconFilterButtons()
		{
			return new string[] {
				TR.Tr("icons_filter_custom"),
				TR.Tr("icons_filter_all")
			};
		}
	}
}