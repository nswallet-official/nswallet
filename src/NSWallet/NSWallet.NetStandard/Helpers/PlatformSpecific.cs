using NSWallet.Interfaces;
using System.Collections.Generic;
using Xamarin.Forms;
using System;
using System.IO;
using Xamarin.Essentials;
//using Xamarin.Essentials;


namespace NSWallet
{
    public static class PlatformSpecific
    {
        public static void SetStatusBarColor(Color color) {
            DependencyService.Get<IThemeNative>().SetColors(color);
        }

        public static void DisplayShortMessage(string message)
        {
            DependencyService.Get<IMessage>().ShowShortAlert(message);
        }

        public static void DisplayLongMessage(string message)
        {
            DependencyService.Get<IMessage>().ShowLongAlert(message);
        }

        public static string GetBuildNumber()
        {
            return DependencyService.Get<IBuild>().GetBuildNumber();
        }

        public static string GetVersion()
        {
            return DependencyService.Get<IBuild>().GetVersion();
        }

		public static string GetPlatform()
		{
			return DependencyService.Get<IBuild>().GetPlatform();
		}

		public static string GetDBFile()
		{
			return DependencyService.Get<IFile>().GetInternalFilePath();
		}

        public static string GetStatsFile() {
            return Path.GetDirectoryName(GetDBFile()) + "stats.json";
        }

		public static string GetDBDirectory()
		{
			return DependencyService.Get<IFile>().GetInternalDirPath();
		}

		public static string CheckOldFile()
		{
			return DependencyService.Get<IFile>().CheckOldFile();
		}

        public static string GetOldDBDirectory()
        {
            return DependencyService.Get<IFile>().GetOldDBDirectory();
        }

        public static string GetBackupPath()
        {
			return DependencyService.Get<IFile>().GetBackupPath();
        }

        public static IEnumerable<string> GetFilePaths(string path)
        {
            return DependencyService.Get<IFile>().GetFilePaths(path);
        }

        public static IEnumerable<string> GetFileNames(string path)
        {
            return DependencyService.Get<IFile>().GetFileNames(path);
        }

        public static long GetFileSize(string path)
        {
            return DependencyService.Get<IFile>().GetFileSize(path);
        }

		public static bool FileExists(string path)
		{
			return DependencyService.Get<IFile>().FileExists(path);
		}

		public static bool DirectoryExists(string path)
		{
			return DependencyService.Get<IFile>().DirectoryExists(path);
		}

		public static void RemoveFile(string path)
		{
			DependencyService.Get<IFile>().RemoveFile(path);
		}

        public static void MoveFile(string pathFrom, string pathTo)
        {
            DependencyService.Get<IFile>().MoveFile(pathFrom, pathTo);
        }

		public static void CreateFile(string path)
		{
            DependencyService.Get<IFile>().CreateFile(path);
		}

        public static void WriteInFile(string path, string content)
		{
            DependencyService.Get<IFile>().WriteInFile(path, content);
		}

        public static string ReadFromFile(string path)
		{
            try
            {
                return DependencyService.Get<IFile>().ReadFromFile(path);
			} catch(Exception ex) {
				log(ex.Message, nameof(ReadFromFile));
                return "";
            }
		}

        public static List<string> ReadZip(string path)
        {
            try
            {
                return DependencyService.Get<IFile>().ReadZip(path);
            }
			catch(Exception ex)
            {
				log(ex.Message, nameof(ReadZip));
				return null;
            }
        }

		public static void CopyFile(string srcFilename, string destFilename)
		{
			try {
				DependencyService.Get<IFile>().CopyFile(srcFilename, destFilename);
			} catch (Exception ex) {
				log(ex.Message, nameof(CopyFile));
			}
		}

		public static bool Unzip(string pathFrom, string pathTo)
		{
			try
			{
				DependencyService.Get<IFile>().Unzip(pathFrom, pathTo);
				return true;
			}
			catch(Exception ex)
			{
				log(ex.Message, nameof(Unzip));
				return false;
			}
		}

		public static bool CreateZip(string pathSourceFolder, string pathDestinationFolder, string fileName)
		{
			try
			{
				DependencyService.Get<IFile>().CreateZip(pathSourceFolder, pathDestinationFolder, fileName);
				return true;
			}
			catch(Exception ex)
			{
				log(ex.Message, nameof(CreateZip));
				return false;
			}
		}

		public static void ShareFile(string filePath, string extraText, string mimeType, string popupText, Action action)
        {
			DependencyService.Get<IShare>().ShareFile(filePath, extraText, mimeType, popupText, action);
        }

        public static void Share(string message)
        {
            DependencyService.Get<IShare>().Share(message);
        }


		public static void CopyToClipboard(string text)
		{
				DependencyService.Get<IClipboardService>().CopyToClipboard(text);
		}


		public static void CleanClipboard()
		{
			DependencyService.Get<IClipboardService>().CleanClipboard();
		}

        public static byte[] GetBytesFromFile(string path)
        {
            return DependencyService.Get<IFile>().GetBytesFromFile(path);
        }

        public static bool RemoveDirectoryWithContents(string path)
        {
            return DependencyService.Get<IFile>().RemoveDirectoryWithContents(path);
        }


		public static void OpenPhoneDialer(string phone)
		{

			if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android) {
				Launcher.OpenAsync(new Uri(String.Format("tel:{0}", phone)));
			}
		}

        public static string GetTempFolder()
        {
            return DependencyService.Get<IFile>().GetTempFolder();
        }

        public static void CreateFolder(string path)
        {
            DependencyService.Get<IFile>().CreateFolder(path);
        }

		static void log(string message, string method = null)
		{
			//AppLogs.Log(message, method, nameof(PlatformSpecific));
		}
    }
}
