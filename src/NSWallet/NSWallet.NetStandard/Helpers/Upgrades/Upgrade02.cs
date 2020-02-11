using System;
using System.Collections.Generic;
using System.Diagnostics;
using NSWallet.Shared;

namespace NSWallet.Helpers
{
    public static partial class PCLUpgradeManager
    {
        static void PrepareUpdateTo02()
        {
			var dbDirectoryPath = PlatformSpecific.GetOldDBDirectory();
            var iconsDirectoryPath = dbDirectoryPath + "/icons";
            var iconsetPath = iconsDirectoryPath + "/iconset.xml";

            if (PlatformSpecific.FileExists(iconsetPath))
            {
                UpgradeManager.IsIconsetExists = true;
            }
            else
            {
                UpgradeManager.IsIconsetExists = false;
            }

            if (PlatformSpecific.DirectoryExists(iconsDirectoryPath))
            {
                UpgradeManager.DBDirectory = dbDirectoryPath;

                var filesPaths = PlatformSpecific.GetFilePaths(iconsDirectoryPath);
                var imageList = new List<ImageModel>();

                foreach (var filePath in filesPaths)
                {
                    if (ImageManager.CheckFileForImage(filePath))
                    {
                        imageList.Add(new ImageModel
                        {
                            Image = PlatformSpecific.GetBytesFromFile(filePath),
                            Path = filePath
                        });
                    }
                }

                UpgradeManager.Images = imageList;
            }
        }

        static void RemoveAfterUpdate02()
        {
            var dbDirectoryPath = PlatformSpecific.GetOldDBDirectory();
            var iconsDirectoryPath = dbDirectoryPath + "/icons";
            var deleted = PlatformSpecific.RemoveDirectoryWithContents(iconsDirectoryPath);
            if (!deleted) Debug.WriteLine("Error! Folder with icons wasn't removed!");
        }
    }
}