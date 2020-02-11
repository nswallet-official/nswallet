using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers.UI.ListView
{
	public static class ListViewUIController
	{
		public static void DisableSelection(object sender, SelectedItemChangedEventArgs e)
		{
			((Xamarin.Forms.ListView)sender).SelectedItem = null;
		}
	}
}