using System;
using Foundation;
using NSWallet.Mac;
using Xamarin.Forms;

[assembly: Dependency(typeof(BuildInfo))]
namespace NSWallet.Mac
{
	public class BuildInfo : IBuild
	{
		public string GetBuildNumber()
		{
			return NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
		}

		public string GetPlatform()
		{
			return "Mac";
		}

		public string GetVersion()
		{
			return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
		}
	}

}