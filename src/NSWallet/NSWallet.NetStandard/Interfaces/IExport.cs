using System;

namespace NSWallet
{
	public interface IExport
	{
		void GeneratePDF();
		void GenerateTXT();
	}
}