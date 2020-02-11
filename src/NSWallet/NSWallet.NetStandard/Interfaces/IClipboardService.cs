namespace NSWallet.Interfaces
{
	public interface IClipboardService
	{
		void CopyToClipboard(string text);
		void CleanClipboard();
	}
}