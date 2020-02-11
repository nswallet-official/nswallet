using System;
using System.IO;
using System.Reflection;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.UnitTests
{
	public static class TestResources
	{
		public static Stream GetStreamFromTestResource (string resName) {
			var assembly = typeof(TestResources).GetTypeInfo().Assembly;
			return assembly.GetManifestResourceStream("NSWallet.UnitTests.TestData." + resName);
		}

		public static string WriteResFileToTempFolder(string filename) {
			Stream fileStreamIn = null;
			string tempFile = "";
			try {
				fileStreamIn = GetStreamFromTestResource(filename);
				var tempFolder = Path.GetTempPath();
				tempFile = tempFolder + filename;
				using (var fileStreamOut = File.Create(tempFile)) {
					fileStreamIn.Seek(0, SeekOrigin.Begin);
					fileStreamIn.CopyTo(fileStreamOut);
				}
			} catch(Exception ex) {
				tempFile = "";
				AppLogs.Log(ex.Message, nameof(WriteResFileToTempFolder), nameof(TestResources));
			} finally {
				if (fileStreamIn != null) {
					fileStreamIn.Close();
				}

			}
			return tempFile;
		}


	}
}
