using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using NSWallet.Shared;
using System.Linq;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using NSWallet.Mac;
using Foundation;
using NSWallet.Shared.Helpers.Logs.AppLog;

[assembly: Dependency(typeof(FileService))]
namespace NSWallet.Mac
{
	public class FileService : IFile
	{
		const string DATABASE_IOS_FOLDER = @"/.nsw/nswallet";

		public string CheckOldFile()
		{
			return ""; // FIXME
		}

		public string GetOldDBDirectory()
		{
			return GetInternalDirPath(); // FIXME
		}

		public string GetInternalFilePath()
		{
			string dbFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + DATABASE_IOS_FOLDER;

			if (!Directory.Exists(dbFolderPath)) {
				Directory.CreateDirectory(dbFolderPath);
			}

			return dbFolderPath + "/" + GConsts.DATABASE_FILENAME;
		}

		public string GetInternalDirPath()
		{
			string dbFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + DATABASE_IOS_FOLDER;

			if (!Directory.Exists(dbFolderPath)) {
				Directory.CreateDirectory(dbFolderPath);
			}

			return dbFolderPath;
		}

		public string GetBackupPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		public IEnumerable<string> GetFilePaths(string path)
		{
			try {
				return Directory.EnumerateFiles(path);
			} catch (DirectoryNotFoundException ex) {
				Directory.CreateDirectory(path);
				log(ex.Message, nameof(GetFilePaths));
				return Directory.EnumerateFiles(path);
			}
		}

		public IEnumerable<string> GetFileNames(string path)
		{
			return Directory.EnumerateFiles(path, "nswb-*.zip").Select(f => Path.GetFileName(f));
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
			return File.Exists(path);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public void MoveFile(string pathFrom, string pathTo)
		{
			try {
				File.Move(pathFrom, pathTo);
			} catch (Exception ex) {
				log(ex.Message, nameof(MoveFile));
			}
		}

		public void RemoveFile(string path)
		{
			try {
				File.Delete(path);
			} catch (Exception ex) {
				log(ex.Message, nameof(RemoveFile));
			}
		}

		public void CreateFile(string path)
		{
			try {
				File.Create(path);
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
			try {
				ZipFile.ExtractToDirectory(pathFrom, pathTo);
				return true;
			} catch (Exception ex) {
				log(ex.Message, nameof(Unzip));
				return false;
			}
		}

		public bool CreateZip(string pathSourceFolder, string pathDestinationFolder, string fileName)
		{
			try {
				ZipFile.CreateFromDirectory(pathSourceFolder, pathDestinationFolder + "/" + fileName);
				return true;
			} catch (Exception ex) {
				log(ex.Message, nameof(CreateZip));
				return false;
			}
		}

		public byte[] GetBytesFromFile(string path)
		{
			try {
				var bytes = File.ReadAllBytes(path);
				return bytes;
			} catch (Exception ex) {
				log(ex.Message, nameof(GetBytesFromFile));
				return null;
			}
		}

		public bool RemoveDirectoryWithContents(string path)
		{
			try {
				Directory.Delete(path, true);
				return true;
			} catch (Exception ex) {
				log(ex.Message, nameof(RemoveDirectoryWithContents));
				return false;
			}
		}

		public string GetTempFolder()
		{
			try {
				var temp = NSFileManager.DefaultManager.GetTemporaryDirectory().Path;
				return temp;
			} catch (Exception ex) {
				log(ex.Message, nameof(GetTempFolder));
				return null;
			}
		}

		public void CreateFolder(string path)
		{
			try {
				Directory.CreateDirectory(path);
			} catch (Exception ex) {
				log(ex.Message, nameof(CreateFolder));
			}
		}

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(FileService));
		}

		public void CopyFile(string srcFilename, string destFilename)
		{
			throw new NotImplementedException();
		}
	}
}