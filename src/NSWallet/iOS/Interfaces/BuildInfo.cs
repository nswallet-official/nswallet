using System;
using Foundation;
using NSWallet.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(BuildInfo))]
namespace NSWallet.iOS
{
    public class BuildInfo : IBuild
    {
        public string GetBuildNumber()
        {
            return NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString(); 
        }

        public string GetPlatform()
        {
            return "iOS";
        }

        public string GetVersion() {
            return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
        }
    }
}