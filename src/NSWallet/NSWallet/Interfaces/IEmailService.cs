using System.Collections.Generic;

namespace NSWallet.NetStandard.Interfaces
{
	public interface IEmailService
	{
		bool OpenEmail(string popupName, List<string> to, string subject, string body, string attachmentPath = null);
	}
}