/* using System;
using NSWallet.Shared.Helpers.Logs.AppLog;
using PCLStorage;

namespace NSWallet
{
	public static class FilesManagement
	{
		public static void MoveOldFile(string oldFile, string newFile)
		{
			try {
				IFileSystem fileSystem = FileSystem.Current;
				PCLStorage.IFile of = fileSystem.GetFileFromPathAsync(oldFile).Result;
				if (of != null) {
					of.MoveAsync(newFile, NameCollisionOption.FailIfExists).Wait();
				}
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(MoveOldFile), nameof(FilesManagement));
			}
		}
	}
}
*/