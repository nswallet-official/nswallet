using System;
using NSWallet.Shared.Helpers.Logs.DeviceInfo;

namespace NSWallet.Shared.Helpers.Logs.Diagnostics
{
	public static class Diagnostics
	{
		static Logger logger;

		public static void Init(string filePath, LogsDeviceInfo logsDeviceInfo)
		{
			logger = new Logger(filePath, GConsts.DIAGNOSTICS_MAX_FILE_SIZE_MB, logsDeviceInfo);
		}

		public static void Log(string message, string method = null, string controlClass = null)
		{
			logger.Log(message, method, controlClass);
		}

		public static string GetDiagnosticsFilePath()
		{
			return logger.GetLogsFilePath();
		}

		public static void DropDiagnosticsFile()
		{
			logger.DropLogs();
		}
	}
}