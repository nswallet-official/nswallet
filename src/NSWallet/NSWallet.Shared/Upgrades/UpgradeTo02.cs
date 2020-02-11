using System;
using System.Collections.Generic;

namespace NSWallet.Shared
{
    public static partial class UpgradeManager
    {
        static bool UpgradeTo02(string dbDirectory, bool isIconsetExists, List<ImageModel> images)
		{
            if (RetrieveCurrentDBVersion() != 1) 
            {
                return false;
            }

            addCustomIconsAndGroupsToDB(dbDirectory, isIconsetExists, images);
            BL.AddSystemLabels(true);
			BL.SetDBVersion("2");

			return true;
		}

        static void addCustomIconsAndGroupsToDB(string dbDirectory, bool isIconSetExists, List<ImageModel> images)
		{
            if (dbDirectory != null)
            {
                var iconSet = getIconSet(dbDirectory, isIconSetExists);

                if (iconSet != null)
                {
                    saveGroups(iconSet);
                }

                if (iconSet != null)
                {
                    saveIcons(images, iconSet);
                }
            }
		}

        static IconSet getIconSet(string dbDirectory, bool isIconsetExists)
        {
            var iconsDirectoryPath = dbDirectory + "/icons";
            var iconSetPath = iconsDirectoryPath + "/iconset.xml";
            if (isIconsetExists)
            {
                return XMLManager<IconSet>.GetIconSet(iconSetPath);
            }
            return null;
        }

        static void saveGroups(IconSet iconSet)
        {
            var groups = iconSet.Icongroup.FindAll(x => Convert.ToInt32(x.Id) > 8);
            if (groups != null)
            {
                if (groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        var groupID = Convert.ToInt32(group.Id);
                        string name = null;
                        if (group.Name.En != null)
                            name = group.Name.En;
                        else if (group.Name.Ru != null)
                            name = group.Name.Ru;
                        BL.InsertGroup(groupID, name);
                    }
                }
            }
        }

        static void saveIcons(List<ImageModel> images, IconSet iconSet)
        {
            if (images != null)
            {
                if (images.Count > 0)
                {
                    foreach (var image in images)
                    {
                        var icon = iconSet.Icon.Find(x => x.Type == "custom" && image.Path.Contains(x.Id));
                        if (icon != null)
                        {
                            string name = null;
                            if (icon.Name.En != null)
                                name = icon.Name.En;
                            else if (icon.Name.Ru != null)
                                name = icon.Name.Ru;
                            var group = Convert.ToInt32(icon.Group);
                            var iconBLOB = image.Image;
                            BL.InsertIcon(iconBLOB, group, icon.Id, name);
                        }
                        icon = null;
                    }
                }
            }
        }
    }
}