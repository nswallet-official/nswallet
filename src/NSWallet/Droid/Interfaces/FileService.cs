using NSWallet.Droid;
using Xamarin.Forms;
using NSWallet.Shared;
using System.IO;
using System.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using NSWallet.Droid.Helpers;
using NSWallet.Shared.Helpers.Logs.AppLog;

[assembly: Dependency(typeof(FileService))]
namespace NSWallet.Droid
{
    public class FileService : IFile
    {
        const string OLD_DROID_FILEPATH = "/app_nswallet/nswallet.dat";
        const string OLD_DROID_DIRPATH = "/app_nswallet";

        public string CheckOldFile()
        {
			try {
				var dbDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var oldFile = dbDir.Replace("/files", OLD_DROID_FILEPATH);
				var oldFileInfo = new FileInfo(oldFile);
				if (oldFileInfo.Exists == true) {
					return oldFile;
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(CheckOldFile));
			}
            return string.Empty;
        }

        public string GetOldDBDirectory()
        {
			try {
				var dbDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var oldDir = dbDir.Replace("/files", OLD_DROID_DIRPATH);
				var oldDirInfo = new DirectoryInfo(oldDir);
				if (oldDirInfo.Exists) {
					return oldDir;
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(GetOldDBDirectory));
			}
            return GetInternalDirPath();
        }

        public string GetInternalFilePath()
        {
			try {
				var dbDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

				dbDir += "/" + GConsts.DATABASE_FOLDER;
				var dInfo = new DirectoryInfo(dbDir);
				if (dInfo.Exists == false) {
					dInfo.Create();
				}

				return dbDir + "/" + GConsts.DATABASE_FILENAME;
			} catch (Exception ex) {
				log(ex.Message, nameof(GetInternalFilePath));
				return null;
			}
        }

		public string GetInternalDirPath()
		{
			var dbDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			dbDir += "/" + GConsts.DATABASE_FOLDER;
			var dInfo = new DirectoryInfo(dbDir);
			if (dInfo.Exists == false)
			{
				dInfo.Create();
			}

			return dbDir;
		}

        public string GetBackupPath()
        {
			try {
				Java.IO.File file = MainActivity.Instance.Application.BaseContext.GetExternalFilesDir(null);
				string extPath = file.AbsolutePath;
				extPath = extPath + "/" +GConsts.BACKUP_FOLDER;
				return extPath;
				//return Legacy.GetDefPrefs().GetString(GConsts.PREFS_BACKUPPATH, Legacy.GetDefaultBackupsFolderPath());
			} catch(Exception ex) {
				log(ex.Message, nameof(GetBackupPath));
				return null;
			}
        }

		public IEnumerable<string> GetFilePaths(string path)
		{
			try
			{
				return Directory.EnumerateFiles(path);
			}
			catch (DirectoryNotFoundException ex)
			{
				Directory.CreateDirectory(path);
				log(ex.Message, nameof(GetFilePaths));
				return Directory.EnumerateFiles(path);
			}
		}

        public IEnumerable<string> GetFileNames(string path)
        {
            if (RequestPermissionsManager.ReadWriteStoragePermission() == true)
            {
                try
                {
                    return Directory.EnumerateFiles(path).Select(f => Path.GetFileName(f));
                }
                catch (DirectoryNotFoundException ex)
                {
                    Directory.CreateDirectory(path);
					log(ex.Message, nameof(GetFileNames));
                    return Directory.EnumerateFiles(path).Select(f => Path.GetFileName(f));
                }
                catch (Exception ex)
                {
					log(ex.Message, nameof(GetFileNames));
                    return null;
                }
            }
            return null;
        }	

        public long GetFileSize(string path)
        {
			try {
				var fi = new FileInfo(path);
				return fi.Length;
			} catch (Exception ex) {
				log(ex.Message, nameof(GetFileSize));
				return -1;
			}
        }

		public bool FileExists(string path)
		{
			return System.IO.File.Exists(path);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public void MoveFile(string pathFrom, string pathTo)
        {
			try {
				System.IO.File.Move(pathFrom, pathTo);
			} catch (Exception ex) {
				log(ex.Message, nameof(MoveFile));
			}
        }

		public void RemoveFile(string path)
		{
			try {
				System.IO.File.Delete(path);
			} catch (Exception ex) {
				log(ex.Message, nameof(RemoveFile));
			}
		}

		public void CreateFile(string path)
		{
			try {
				System.IO.File.Create(path);
			} catch (Exception ex) {
				log(ex.Message, nameof(CreateFile));
			}
		}

		public void WriteInFile(string path, string content)
		{
			try {
				using (var streamWriter = new StreamWriter(path, false)) {
					streamWriter.WriteLine(content);
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(WriteInFile));
			}
 		}

		public string ReadFromFile(string path)
		{
			try {
				using (var streamReader = new StreamReader(path)) {
					string content = streamReader.ReadToEnd();
					return content;
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(ReadFromFile));
				return null;
			}
		}

        public List<string> ReadZip(string path)
        {
			try {
				var zip = ZipFile.OpenRead(path);
				var entries = new List<string>();
				if (zip.Entries != null) {
					if (zip.Entries.Count != 0) {
						foreach (var entry in zip.Entries) {
							entries.Add(entry.FullName);
						}
					}
				}
				return entries;
			} catch (Exception ex) {
				log(ex.Message, nameof(ReadZip));
				return null;
			}
        }

		public bool Unzip(string pathFrom, string pathTo)
		{
			if (RequestPermissionsManager.ReadWriteStoragePermission() == true) {
				try {
					ZipFile.ExtractToDirectory(pathFrom, pathTo, true);
					return true;
				} catch (Exception ex) {
					log(ex.Message, nameof(Unzip));
					return false;
				}
			}
			return false;
		}

        public bool CreateZip(string pathSourceFolder, string pathDestinationFolder, string fileName)
		{
            if (RequestPermissionsManager.ReadWriteStoragePermission() == true)
            {
                try
                {
                    ZipFile.CreateFromDirectory(pathSourceFolder, pathDestinationFolder + "/" + fileName);
					return true;
                }
                catch (Exception ex)
                {
					log(ex.Message, nameof(CreateZip));
					return false;
                }
            }
            return false;
		}

		public byte[] GetBytesFromFile(string path)
		{
			try
			{
				var bytes = System.IO.File.ReadAllBytes(path);
				return bytes;
			}
			catch (Exception ex)
			{
				log(ex.Message, nameof(GetBytesFromFile));
				return null;
			}
		}

		public void CopyFile(string srcFilename, string destFilename)
		{
			try {
				System.IO.File.Copy(srcFilename, destFilename);
			} catch (Exception ex) {
				log(ex.Message, nameof(CopyFile));
			}
		}

		public bool RemoveDirectoryWithContents(string path)
        {
            try
            {
                Directory.Delete(path, true);
                return true;
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(RemoveDirectoryWithContents));
				return false;
            }
        }

        public string GetTempFolder()
        {
            try
            {
                var temp = Android.App.Application.Context.CacheDir.AbsolutePath;
                return temp;
            }
            catch(Exception ex)
            {
				log(ex.Message, nameof(GetTempFolder));
				return null;
            }
        }

        public void CreateFolder(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
				log(ex.Message, nameof(CreateFolder));
			}
        }

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(FileService));
		}
    }
}