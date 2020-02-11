using System;
using System.Collections.Generic;

namespace NSWallet.Shared
{
    public static partial class UpgradeManager
    {
        // Upgrade to 02 variables
        public static string DBDirectory { get; set; }
        public static bool IsIconsetExists { get; set; }
        public static List<ImageModel> Images { get; set; }

        public static void Upgrade()
        {
            var currentDBVersion = RetrieveCurrentDBVersion();
            var dbVersion = Convert.ToInt32(GConsts.DB_VERSION);

            if (currentDBVersion > dbVersion)
            {
                // FIXME: handle error in a correct way, you are trying to open 
                // newer DB with old app
                throw new Exception("You are trying to open new DB with old app");
            }

            if (currentDBVersion == dbVersion)
                return;

            var version = currentDBVersion + 1;
            for (int iterator = version; iterator <= dbVersion; iterator++)
            {
                if (!PerformUpgradeTo(iterator))
                    break;  // FIXME: Exit and report the error, upgrade failed
            }
        }

        static int RetrieveCurrentDBVersion()
        {
            return Convert.ToInt32(BL.StorageProperties.Version);
        }

        static bool PerformUpgradeTo(int version)
        {
            switch (version)
            {
                case 2:
                    return UpgradeTo02(DBDirectory, IsIconsetExists, Images);
                case 3:
                    return UpgradeTo03();
				case 4:
					return UpgradeTo04();
                default:
                    return true;
            }
        }
    }
}