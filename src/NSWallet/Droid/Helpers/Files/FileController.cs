using System;
using System.IO;
using Android.Content;
using Android.Database;
using Android.Provider;

namespace NSWallet.Droid.Helpers.Files
{
	public static class FileController
	{
		public const string TemporaryPngFilename = "image.png";
		public const string TemporaryJpegFilename = "image.jpeg";
		public const string TemporaryZipFilename = "temporary.zip";

		public const string ImageMimeType = "image/*";
		public const string ImageMimePngType = "image/png";
		public const string ImageMimeJpegType = "image/jpeg";
		public const string ApplicationZipMimeType = "application/zip";

		public const string Png = ".png";
		public const string Jpeg = ".jpeg";
		public const string Zip = ".zip";

		public static string GetDocumentsPath {
			get {
				try {
					return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				} catch {
					return null;
				}
			}
		}

		public static string CombinePaths(string path1, string path2)
		{
			try {
				return Path.Combine(path1, path2);
			} catch {
				return null;
			}
		}

		public static void WriteToFile(string path, MemoryStream memoryStream)
		{
			try {
				File.WriteAllBytes(path, memoryStream.ToArray());
			} catch {
				return;
			}
		}

		public static string ConfigureTemporaryZipFile(Stream zipStream)
		{
			try {
				return WriteStreamToDocuments(zipStream, TemporaryZipFilename);
			} catch {
				return null;
			}
		}

		public static string ConfigureTemporaryImage(Stream imageStream, string mimeType)
		{
			try {
				string temporaryFilename = null;
				switch (mimeType) {
					case ImageMimePngType:
						temporaryFilename = TemporaryPngFilename;
						break;
					case ImageMimeJpegType:
						temporaryFilename = TemporaryJpegFilename;
						break;
				}
				if (temporaryFilename != null) {
					return WriteStreamToDocuments(imageStream, temporaryFilename);
				}
				return null;
			} catch {
				return null;
			}
		}

		public static string WriteStreamToDocuments(Stream stream, string filename)
		{
			try {
				var ms = new MemoryStream();
				stream.CopyTo(ms);
				var path = CombinePaths(GetDocumentsPath, filename);
				WriteToFile(path, ms);
				return path;
			} catch {
				return null;
			}
		}

		public static string GetMimeType(Context context, Android.Net.Uri uriImage)
		{
			try {
				string strMimeType = null;
				ICursor cursor = context.ContentResolver.Query(uriImage,
															   new string[] { MediaStore.MediaColumns.MimeType }, null, null, null);
				if (cursor != null && cursor.MoveToNext()) {
					strMimeType = cursor.GetString(0);
				}
				return strMimeType;
			} catch {
				return null;
			}
		}

		public static byte[] GetBytesFromFile(string path)
		{
			try {
				var bytes = File.ReadAllBytes(path);
				return bytes;
			} catch (Exception ex) {
				return null;
			}
		}
	}
}