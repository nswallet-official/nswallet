using System;
using Android.Content;
using NSWallet.Droid;
using NSWallet.NetStandard;
using Xamarin.Forms;

[assembly: Dependency(typeof(ActionService))]
namespace NSWallet.Droid
{
	public class ActionService : IAction
	{
		public void OpenPhoneDialer(string phone)
		{
			var uri = Android.Net.Uri.Parse(String.Format("tel:{0}", phone));
			var intent = new Intent(Intent.ActionDial, uri);
			Forms.Context.StartActivity(intent);
		}
	}
}