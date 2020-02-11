namespace NSWallet.Shared
{
    public static class ImageManager
    {
        public static string[] SupportedImageFormats = {
            ".png", ".PNG",
            ".jpg", ".JPG", ".jpeg", ".JPEG"
        };

        public static bool CheckFileForImage(string fileName)
        {
            foreach(var supportedImageFormat in SupportedImageFormats)
            {
                if (fileName.EndsWith(supportedImageFormat, System.StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public static string ConvertIconName2IconPath(string iconName) {
			if (iconName != null) {
				return "Icons.items.icon_" + iconName + "_huge.png";
			}
			return null;
        }

        public static string ConvertIconFolder2IconPath(string iconName)
        {
            return "Icons.items.icon_" + iconName + ".png";
        }
    }
}