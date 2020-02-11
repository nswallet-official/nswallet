
//using NSWallet.Shared.Helpers.Logs.DeviceInfo;

namespace NSWallet.Shared.Helpers.Logs.AppLog
{

	public static class AppLogs
	{
		static Logger logger;
		static bool AreLogsActive { get; set; } = false;

		public static void SetLogsActivity(bool areEnabled)
		{
			AreLogsActive = areEnabled;
		}

		/*
		public static void Init(string filePath, LogsDeviceInfo logsDeviceInfo, bool areLogsActive)
		{
			SetLogsActivity(areLogsActive);
			logger = new Logger(filePath, GConsts.LOGS_MAX_FILE_SIZE_MB, logsDeviceInfo);
		}
		*/

		public static void CheckLogExists()
		{
			if (logger != null) {
				logger.CheckLogExists();
			}
		}

		public static bool Log(string message, string method = null, string controlClass = null)
		{
			/*
			try {
				if (AreLogsActive) {
					if (logger != null) {
						return logger.Log(message, method, controlClass);
					}
				}
				return false;
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				return false;
			}
			*/
			return true;
		}

		public static string GetLogsFilePath()
		{
			if (logger != null) {
				return logger.GetLogsFilePath();
			}
			return null;
		}

		public static void DropLogs()
		{
			if (logger != null) {
				logger.DropLogs();
			}
		}
	}

}