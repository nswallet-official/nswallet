
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Google.Android.Material.Snackbar;
using NSWallet.Shared;
using static Xamarin.Forms.Forms;

namespace NSWallet.Droid.Helpers
{
    public static class RequestPermissionsManager
    {

        readonly static string[] PermissionsReadWrite =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        };

        public const int RequestReadWriteID = 123;
        static Activity activity;

        public static bool ReadWriteStoragePermission()
        {
            if ((int)Build.VERSION.SdkInt < 23) return true;

            if (Context == null) return false;

            if (ContextCompat.CheckSelfPermission(Context, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted &&
                ContextCompat.CheckSelfPermission(Context, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
            {
                return true;
            }

            activity = (Activity)Context;

            if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.WriteExternalStorage))
            {
                //Explain to the user why we need to read the contacts
                var view = activity.FindViewById(Android.Resource.Id.Content);
                Snackbar.Make(view, TR.Tr("why_storage_access"), Snackbar.LengthIndefinite)
                        .SetAction(TR.OK, v => ActivityCompat.RequestPermissions(activity, PermissionsReadWrite, RequestReadWriteID))
                        .Show();
                return false;
            }



            ActivityCompat.RequestPermissions(activity, PermissionsReadWrite, RequestReadWriteID);
            return false;
        }
    }
}
