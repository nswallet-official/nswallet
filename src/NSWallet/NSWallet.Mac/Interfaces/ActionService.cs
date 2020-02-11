using System;
using Foundation;
using NSWallet.iOS;
using NSWallet.Mac;
using NSWallet.NetStandard;
using Xamarin.Forms;

[assembly: Dependency(typeof(ActionService))]
namespace NSWallet.Mac
{
	public class ActionService : IAction
	{
		public void OpenPhoneDialer(string phone)
		{
			// Do nothing
		}
	}
}