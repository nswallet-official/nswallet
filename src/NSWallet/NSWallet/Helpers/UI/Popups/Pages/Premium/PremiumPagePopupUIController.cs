using NSWallet.Shared;

namespace NSWallet.NetStandard.Helpers.UI.Popups.Pages.Premium
{
	public static class PremiumPagePopupUIController
	{
		public static void LaunchGetPriceErrorPopup()
		{
			PopupUIController.LaunchMessageBox(TR.Tr("alert"), TR.Tr("price_loading_failed"), TR.OK, TR.Cancel);
		}
	}
}