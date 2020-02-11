using System;
using NSWallet.Helpers;

namespace NSWallet.NetStandard.Themes.NightMode
{
	public static class ThemeNightMode
	{
		const int NIGHT_START_HOUR = 23;
		const int NIGHT_END_HOUR = 5;

		public static bool IsNightModeNow {
			get {
				if (Settings.IsAutoNightMode) {
					return getNightMode();
				}
				return false;
			}
		}

		static bool getNightMode()
		{
			var hour = DateTime.Now.Hour;
			return hour >= NIGHT_START_HOUR
				&& hour <= NIGHT_END_HOUR;
		}
	}
}