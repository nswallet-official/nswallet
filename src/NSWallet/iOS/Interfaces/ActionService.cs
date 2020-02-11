using System;
using Foundation;
using NSWallet.iOS;
using NSWallet.NetStandard;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ActionService))]
namespace NSWallet.iOS
{
	public class ActionService : IAction
	{
		public void OpenPhoneDialer(string phone)
		{
			var url = new NSUrl(String.Format("tel:{0}", phone));
			UIApplication.SharedApplication.OpenUrl(url);
		}
	}
}