using Android.Content;
using NSWallet.Droid.Interfaces;
using NSWallet.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClipboardServiceDroid))]
namespace NSWallet.Droid.Interfaces
{
    public class ClipboardServiceDroid : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
			var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
			ClipData clip = ClipData.NewPlainText("clip", text);
			clipboardManager.PrimaryClip = clip;
        }

        public void CleanClipboard()
        {
			var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("clip", "");
			clipboardManager.PrimaryClip = clip;
        }
    }
}