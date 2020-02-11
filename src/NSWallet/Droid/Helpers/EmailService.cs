using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Java.IO;
using NSWallet.Droid.Helpers;
using NSWallet.NetStandard.Interfaces;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

[assembly: Dependency(typeof(EmailService))]
namespace NSWallet.Droid.Helpers
{
	public class EmailService : IEmailService
	{
		public bool OpenEmail(string popupName, List<string> to, string subject, string body, string attachmentPath = null)
		{
			try {
				var email = new Intent(Intent.ActionSend);
				email.PutExtra(Intent.ExtraEmail, to.ToArray());
				email.PutExtra(Intent.ExtraSubject, subject);
				email.PutExtra(Intent.ExtraText, body);

				if (attachmentPath != null) {
					var newFilePath = getTempFilePath(attachmentPath);
					if (newFilePath == null) {
						return false;
					}
					email.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(new File(newFilePath)));
					email.SetType("text/plain");
				}

				var activity = Forms.Context as Activity;
				activity.StartActivityForResult(Intent.CreateChooser(email, popupName), 0);
				return true;
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(OpenEmail), nameof(EmailService));
				return false;
			}
		}

		string getTempFilePath(string sourcePath)
		{
			try {
				var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
				var filename = System.IO.Path.GetFileName(sourcePath);
				var newFilePath = externalPath + "/" + filename;

				if (System.IO.File.Exists(newFilePath)) {
					System.IO.File.Delete(newFilePath);
				}

				System.IO.File.Copy(sourcePath, newFilePath);
				return newFilePath;
			} catch(Exception ex) {
				AppLogs.Log(ex.Message, nameof(getTempFilePath), nameof(EmailService));
				return null;
			}
		}
	}
}