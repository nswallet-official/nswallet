
using System;
using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Content.PM;
using NSWallet.Droid.Interfaces;
using NSWallet.NetStandard.Interfaces;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionService))]
namespace NSWallet.Droid.Interfaces
{
	public class PermissionService : IPermission
	{
		public bool ReadWritePermission {
			get {
				try {
					var activity = (Activity)Forms.Context;
					var writePermission = activity.CheckSelfPermission(Android.Manifest.Permission.WriteExternalStorage);
					var readPermission = activity.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage);
					if (writePermission == Permission.Granted && readPermission == Permission.Granted) {
						return true;
					}
				} catch (Exception ex) {
					AppLogs.Log(ex.Message, nameof(ReadWritePermission), nameof(PermissionService));
					return true;
				}
				return false;
			}
		}
	}
}