using System.Collections.Generic;
using NSWallet.iOS.Interfaces;
using NSWallet.NetStandard.Interfaces;

using Xamarin.Forms;

[assembly: Dependency(typeof(EmailService))]
namespace NSWallet.iOS.Interfaces
{
	public class EmailService : IEmailService
	{
		public bool OpenEmail(string popupName, List<string> to, string subject, string body, string attachmentPath = null) { return false; }
	}
}