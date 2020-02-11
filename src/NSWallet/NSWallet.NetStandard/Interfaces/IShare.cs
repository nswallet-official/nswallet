using System;
namespace NSWallet.Interfaces
{
    public interface IShare
    {
        void Share(string message);
		void ShareFile(string fileName, string extraText, string mimeType, string popupText, Action action);
    }
}
