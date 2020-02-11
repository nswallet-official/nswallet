using Xamarin.Forms;

namespace NSWallet.Helpers
{
    public static class MessageBox
    {
        public static void ShowMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PlatformSpecific.DisplayShortMessage(message);
            });
        }
    }
}
