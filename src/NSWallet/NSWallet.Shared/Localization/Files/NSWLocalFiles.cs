using System;
using System.IO;
using System.Reflection;
using System.Text;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared
{
    public static class NSWLocalFiles
    {
		const string notPremium = ".notpremium.html";
		const string oldPremium = ".old_premium.html";
		const string helpImportBackup = ".help_import_backup.html";
		const string privacyPolicy = ".privacy_policy.html";
		const string termsOfUse = ".terms_of_use.html";
		const string feedbackDescription = ".feedback_description.html";

		static string RunNameSpace;

        public static void Init(string resNamespace)
        {
            RunNameSpace = resNamespace;
        }

        static Stream GetStream(string langCode, string file)
        {
            var assembly = typeof(NSWLocalFiles).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(RunNameSpace + GConsts.EMBEDDED_LOCAL_FILES_PATH + langCode + file);
        }

        static string GetHTML(string langCode, string file)
        {
            if (string.IsNullOrEmpty(RunNameSpace))
            {
                throw new ResourcesException("Namespace is not set");
            }

            var stream = GetStream(langCode, file);
            if (stream == null)
                stream = GetStream("en", file);

			try {
				using (var reader = new StreamReader(stream, Encoding.UTF8)) {
					return reader.ReadToEnd();
				}
			} catch (Exception ex) {
				AppLogs.Log(ex.Message, nameof(GetHTML), nameof(NSWLocalFiles));
				return "HTML Source is not found: " + GConsts.EMBEDDED_LOCAL_FILES_PATH + langCode + file + ", " + ex.Message;
			}
        }

		public static string GetNotPremiumHTML(string langCode)
		{
			return GetHTML(langCode, notPremium);
		}

		public static string GetOldPremiumHTML(string langCode)
		{
			return GetHTML(langCode, oldPremium);
		}

		public static string GetImportBackupHelpHTML(string langCode) {
			return GetHTML(langCode, helpImportBackup);
		}

		public static string GetPrivacyPolicyHTML(string langCode)
		{
			return GetHTML(langCode, privacyPolicy);
		}

		public static string GetTermsOfUseHTML(string langCode)
		{
			return GetHTML(langCode, termsOfUse);
		}

		public static string GetFeedbackDescriptionHTML(string langCode)
		{
			return GetHTML(langCode, feedbackDescription);
		}
	}
}