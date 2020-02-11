using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Plugin.InAppBilling;
using NSWallet.Shared;
using System;
using System.IO;
using NSWallet.Droid.Helpers.Files;
using NSWallet.Droid.Helpers.Media;
using ImageCircle.Forms.Plugin.Droid;
using System.Threading.Tasks;
using NSWallet.NetStandard.Helpers;

namespace NSWallet.Droid
{
	[Activity(Name= "com.nyxbull.nswallet.startactivity", Label = "NS Wallet", Icon = "@drawable/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeTypes = new[] { @"application/zip", @"image/png", @"image/jpeg" })]
	public class MainActivity : FormsAppCompatActivity
	{
		public static MainActivity Instance { get; private set; }

		App mainForms;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			SetTheme(Resource.Style.MyTheme);
			Instance = this;
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			// Remove the code below to see the actions in pre launch reports
			if (Window != null)
			{
                Window.AddFlags(WindowManagerFlags.Secure);
			}


			Xamarin.Forms.Forms.Init(this, savedInstanceState);
			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
			ImageCircleRenderer.Init();
			FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

			mainForms = new App();

			ExtendedDevice.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
			ExtendedDevice.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

			LoadApplication(mainForms);
			onActionSend();
		}

		void onActionSend()
		{
			try {
				if (Intent.Action == Intent.ActionSend) {
					var uriFromExtras = Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
					var subject = Intent.GetStringExtra(Intent.ExtraSubject);
					var file = Intent.ClipData.GetItemAt(0);

					if (file.Text != null) {
						if (file.Text.Contains(TR.Tr("backup_from"))) {
							return;
						}
					}

					var fileStream = ContentResolver.OpenInputStream(file.Uri);
					var mimeType = FileController.GetMimeType(Application.Context, uriFromExtras);
					if (mimeType == null) {
						if (uriFromExtras.Path != null) {
							var extension = System.IO.Path.GetExtension(uriFromExtras.Path);
							if (extension != null) {
								if (string.Compare(extension, FileController.Zip) == 0) {
									mimeType = FileController.ApplicationZipMimeType;
								}
							}
						}
					}

					if (mimeType != null) {
						switch(mimeType) {
							case FileController.ImageMimePngType:
							case FileController.ImageMimeJpegType:
								var imagePath = FileController.ConfigureTemporaryImage(fileStream, mimeType);
								var imageBytes = FileController.GetBytesFromFile(imagePath);
								var mediaService = new MediaService();
								var imageResizedBytes = mediaService.ResizeImage(imageBytes, GConsts.RESIZE_ICON_WIDTH, GConsts.RESIZE_ICON_HEIGHT);
								mainForms.ImportImage(imageResizedBytes);
								break;
							case FileController.ApplicationZipMimeType:
								var zipPath = FileController.ConfigureTemporaryZipFile(fileStream);
								mainForms.ImportZip(zipPath);
								break;
						}
					}
				}
			} catch(Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return;
			}
		}

		public static readonly int PickImageId = 1000;
		public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);

			if (requestCode == PickImageId) {
				if ((resultCode == Result.Ok) && (data != null)) {
					Android.Net.Uri uri = data.Data;
					Stream stream = ContentResolver.OpenInputStream(uri);
					PickImageTaskCompletionSource.SetResult(stream);
				} else {
					PickImageTaskCompletionSource.SetResult(null);
				}
			}
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,Permission[] grantResults)
        {
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			/*
            switch (requestCode)
            {
                case RequestPermissionsManager.RequestReadWriteID:
                    {
                        if (grantResults != null && grantResults.Length > 0)
                        {
                            if (grantResults[0] == Permission.Granted)
                            {
                                Toast.MakeText(ApplicationContext, TR.Tr("permission_granted"), ToastLength.Long).Show();
                            }
                            else
                            {
                                Toast.MakeText(ApplicationContext, TR.Tr("permission_denied"), ToastLength.Long).Show();
                            }
                        }
                    }
                    break;
            }
            */
		}
	}
}