using System;
using NSWallet.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(BuildInfo))]
namespace NSWallet.Droid
{
    public class BuildInfo : IBuild
    {
        public string GetBuildNumber()
        {
            return Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionCode.ToString();
        }

        public string GetPlatform()
        {
            return "Android";
        }

        public string GetVersion() {
            return Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionName;
        }
    }
}
