using System;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Support.V4.Content;
using Java.IO;
using NSWallet.Interfaces;
using NSWallet.iOS.Interfaces;
using NSWallet.Shared;
using Xamarin.Forms;

[assembly: Dependency(typeof(SharingDroid))]
namespace NSWallet.iOS.Interfaces
{
	public class SharingDroid : IShare
	{
        public void Share(string message)
        {
            Intent intentShare = new Intent(Intent.ActionSend);
            intentShare.SetType("*/*");
            intentShare.PutExtra(Intent.ExtraSubject, message);
            intentShare.PutExtra(Intent.ExtraText, message);
            intentShare.AddFlags(ActivityFlags.NewTask);
            Forms.Context.StartActivity(Intent.CreateChooser(intentShare, TR.Tr("share_link")));
        }

		public void ShareFile(string fileName, string extraText, string mimeType, string popupText, Action action)
		{
            Intent intentShareFile = new Intent(Intent.ActionSend);
            File fileWithinMyDir = new File(fileName);

			intentShareFile.SetType(mimeType);

            if (fileWithinMyDir.Exists())
			{
                var api = (int)Build.VERSION.SdkInt;
                var context = Android.App.Application.Context;

                if (api > 23)
                {
                    var fileProvider = FileProvider.GetUriForFile(context, "com.nyxbull.nswallet.provider", fileWithinMyDir);
                    intentShareFile.PutExtra(Intent.ExtraStream, fileProvider);
                    intentShareFile.AddFlags(ActivityFlags.GrantReadUriPermission);
                }
                else
                {
                    var uri = Android.Net.Uri.FromFile(fileWithinMyDir);
                    intentShareFile.PutExtra(Intent.ExtraStream, uri);
                    intentShareFile.AddFlags(ActivityFlags.NewTask);
                }

                intentShareFile.PutExtra(Intent.ExtraSubject, extraText);
                intentShareFile.PutExtra(Intent.ExtraText, extraText);
				Forms.Context.StartActivity(Intent.CreateChooser(intentShareFile, popupText));
			}

			if (action != null) {
				action.Invoke();
			}
		}
	}
}