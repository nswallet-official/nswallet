using System;
using System.Diagnostics;
using System.IO;
using NSWallet.Shared.Helpers.Logs.DeviceInfo;

namespace NSWallet.Shared.Helpers.Logs
{
	public class Logger
	{
		readonly string filePath;
		readonly double maxFileSizeMB;
		readonly LogsDeviceInfo logsDeviceInfo;

		const int separatorLength = 30;
		const string separatorString = "=";

		public Logger(string filePath, double maxFileSizeMB, LogsDeviceInfo logsDeviceInfo)
		{
			try {
				if (!string.IsNullOrEmpty(filePath)) {
					this.filePath = filePath;
					this.maxFileSizeMB = checkMaxFileSize(maxFileSizeMB);
					this.logsDeviceInfo = logsDeviceInfo;
					CheckLogExists();
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}
		}

		public void DropLogs()
		{
			try {
				if (File.Exists(filePath)) {
					File.Delete(filePath);
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}
		}

		public bool Log(string message, string method = null, string controlClass = null)
		{
			try {
				if (filePath == null) {
					return false;
				}
				checkFileLength();
				var msg = generateMessage(message, method, controlClass);
				appendToFile(msg);
				return true;
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				return false;
			}
		}

		public string GetLogsFilePath()
		{
			return filePath;
		}

		public void CheckLogExists()
		{
			try {
				if (!File.Exists(filePath)) {
					Directory.CreateDirectory(Path.GetDirectoryName(filePath));
					addDeviceInfo();
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}
		}

		void addDeviceInfo()
		{
			if (logsDeviceInfo != null) {
				appendToFile(getLogSeparator());
				appendToFile(getLogSeparator());
				appendToFile("Application version: " + logsDeviceInfo.AppVersion);
				appendToFile("Application build: " + logsDeviceInfo.AppBuild);
				appendToFile("Device name: " + logsDeviceInfo.DeviceName);
				appendToFile("Device type: " + logsDeviceInfo.DeviceType);
				appendToFile("Is device: " + logsDeviceInfo.IsDevice);
				appendToFile("Manufacturer: " + logsDeviceInfo.Manufacturer);
				appendToFile("Model: " + logsDeviceInfo.Model);
				appendToFile("OS version: " + logsDeviceInfo.OSVersion);
				appendToFile("Platform: " + logsDeviceInfo.Platform);
				appendToFile(getLogSeparator());
				appendToFile(getLogSeparator());
				appendToFile("");
			}
		}

		string getLogSeparator()
		{
			string separator = "";
			for (int i = 0; i < separatorLength; i++) {
				separator += separatorString;
			}
			return separator;
		}

		void appendToFile(string message)
		{
			try {
				using (var sw = File.AppendText(filePath)) {
					sw.WriteLine(message);
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}
		}

		double checkMaxFileSize(double maxFileSize)
		{
			return maxFileSize <= 0 ? GConsts.LOGS_DEFAULT_MAX_FILE_SIZE_MB : maxFileSize;
		}

		void checkFileLength()
		{
			try {
				if (File.Exists(filePath)) {
					var bytes = new FileInfo(filePath).Length;
					var megabytes = convertBytesToMegabytes(bytes);
					if (megabytes >= maxFileSizeMB) {
						File.Delete(filePath);
					}
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}
		}

		string generateMessage(string message, string method, string controlClass)
		{
			string msg = null;
			string classMethod = null;

			msg = insertStringToSquareBrackets(
				DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
			);

			if (controlClass != null) {
				classMethod = insertColon(controlClass);
			}

			if (method != null) {
				if (classMethod != null) {
					classMethod += " " + method;
				} else {
					classMethod = method;
				}
			}

			if (classMethod != null) {
				msg += " " + insertStringToRoundBrackets(classMethod);
			}

			if (!string.IsNullOrEmpty(message)) {
				msg += " " + message;
				return msg;
			}
			return null;
		}

		string insertStringToSquareBrackets(string str)
		{
			return !string.IsNullOrEmpty(str) ? "[" + str + "]" : null;
		}

		string insertStringToRoundBrackets(string str)
		{
			return !string.IsNullOrEmpty(str) ? "(" + str + ")" : null;
		}

		string insertColon(string str)
		{
			return !string.IsNullOrEmpty(str) ? str + ":" : null;
		}

		static double convertBytesToMegabytes(long bytes)
		{
			try {
				return (bytes / 1024f) / 1024f;
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				return 0;
			}
		}
	}
}