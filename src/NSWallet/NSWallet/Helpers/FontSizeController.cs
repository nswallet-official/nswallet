using System;
using NSWallet.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public static class FontSizeController
	{
		public static double GetSize(NamedSize namedSize, Type type)
		{
			if (isLargeSettings()) {
				switch (namedSize) {
					case NamedSize.Micro:
						return Device.GetNamedSize(NamedSize.Small, type);
					case NamedSize.Small:
						return Device.GetNamedSize(NamedSize.Medium, type);
					case NamedSize.Default:
						return Device.GetNamedSize(NamedSize.Medium, type);
					case NamedSize.Medium:
						return Device.GetNamedSize(NamedSize.Large, type);
					case NamedSize.Large:
						return Device.GetNamedSize(NamedSize.Large, type) + 5;
				}
			}
			return Device.GetNamedSize(namedSize, type);
		}

		static bool isLargeSettings()
		{
			var settingsFontSize = Settings.FontSize;
			if (string.Compare(settingsFontSize, "font_sizes_large") == 0)
				return true;
			return false;
		}
	}
}