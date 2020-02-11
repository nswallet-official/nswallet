using System;
using System.IO;
using System.Reflection;

namespace NSWallet.Shared
{
	public static class NSWRes
	{
		public const string APP_ICON = "1024.png";
		static string RunNameSpace;

		public static void Init(string resNamespace)
		{
			RunNameSpace = resNamespace;
		}

        public static string GetRunspaceAssetsPath()
        {
            return RunNameSpace + GConsts.EMBEDDED_ASSETS_PATH;
        }

		public static Stream GetImage(string imageName)
		{
			if (string.IsNullOrEmpty(RunNameSpace))
			{
				throw new ResourcesException("Namespace is not set");
			}

			var assembly = typeof(NSWRes).GetTypeInfo().Assembly;

			return assembly.GetManifestResourceStream(RunNameSpace + GConsts.EMBEDDED_ASSETS_PATH + imageName);
		}

        public static string[] GetResourceNames()
        {
            var assembly = typeof(NSWRes).GetTypeInfo().Assembly;

			return assembly.GetManifestResourceNames();
        }

		// FIXME: temporary solution to have always demo DB available, comment in release version
		// Password: Test001
		// /*
		public static Stream GetDemoFileTEMPORARY()
		{
			if (string.IsNullOrEmpty(RunNameSpace))
			{
				throw new ResourcesException("Namespace is not set");
			}

			var assembly = typeof(NSWRes).GetTypeInfo().Assembly;

			return assembly.GetManifestResourceStream(RunNameSpace + ".Resources.Demo.nswallet.dat");

		}
		// */

	}

	public class ResourcesException : Exception
	{
		public ResourcesException(string message) : base(message)
		{

		}
	}
}
