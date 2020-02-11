using System.IO;

namespace NSWallet.iOS.Helpers.Files
{
	public static class FileController
	{
		public const string Png = "png";
		public const string Jpeg = "jpeg";
		public const string Jpg = "jpg";
		public const string Zip = "zip";

		public static byte[] GetBytesFromFile(string url)
		{
			return File.ReadAllBytes(url);
		}
	}
}