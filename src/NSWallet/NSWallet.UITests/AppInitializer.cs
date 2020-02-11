using System;
using NSWallet.UITests.Main.Helpers;
using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace NSWallet.UITests
{
	public class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android) {
                AndroidAppConfigurator androidConfigurator = ConfigureApp.Android;

                string apkPath = Environment.GetEnvironmentVariable("ANDROID_APK_PATH");
                if (!string.IsNullOrEmpty(apkPath))
                {
                    androidConfigurator.ApkFile(apkPath);
                }
                else
                {
                    androidConfigurator.ApkFile("../../../Droid/bin/Debug/com.nyxbull.nswallet-bitrise-signed.apk");
                }

                string emulatorSerial = Environment.GetEnvironmentVariable("BITRISE_EMULATOR_SERIAL");
                if (!string.IsNullOrEmpty(emulatorSerial))
                {
                    androidConfigurator.DeviceSerial(emulatorSerial);
                }

                androidConfigurator.WaitTimes(new WaitTimes());

                return androidConfigurator.StartApp();
            }

            iOSAppConfigurator iosConfigurator = ConfigureApp.iOS;

            string appBundlePath = Environment.GetEnvironmentVariable("APP_BUNDLE_PATH");
            if (!string.IsNullOrEmpty(appBundlePath))
            {
                iosConfigurator.AppBundle(appBundlePath);
            }
            else
            {
                iosConfigurator.AppBundle("../../../iOS/bin/iPhoneSimulator/Debug/NSWallet.iOS.app");
            }

            string simulatorUDID = Environment.GetEnvironmentVariable("IOS_SIMULATOR_UDID");
            if (!string.IsNullOrEmpty(simulatorUDID))
            {
                iosConfigurator.DeviceIdentifier(simulatorUDID);
            }

            iosConfigurator.WaitTimes(new WaitTimes());

            return iosConfigurator.StartApp();
		}
	}
}