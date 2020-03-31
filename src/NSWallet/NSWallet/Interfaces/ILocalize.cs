using System.Globalization;

namespace NSWallet
{
	public interface ILocalize
	{
		CultureInfo GetCurrentCultureInfo();
	}
}