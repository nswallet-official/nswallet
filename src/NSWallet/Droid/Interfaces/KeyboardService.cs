using System;
using Android.App;
using Android.Views.InputMethods;
using NSWallet.Droid;
using NSWallet.Interfaces;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardService))]
namespace NSWallet.Droid
{
    public class KeyboardService : IKeyboard
    {
        public void DismissKeyboard()
        {
            try
            {
                var x = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                var y = x.FindViewById(Android.Resource.Id.Content);
                InputMethodManager imm = (InputMethodManager)(y.Context.GetSystemService(Android.Content.Context.InputMethodService));
                imm.HideSoftInputFromWindow(y.WindowToken, HideSoftInputFlags.None);
            }
            catch(Exception ex)
            {
				AppLogs.Log(ex.Message, nameof(DismissKeyboard), nameof(KeyboardService));
				System.Diagnostics.Debug.WriteLine("Error (Droid / KeyboardService.cs / DismissKeyboard): " + ex.Message);
            }
        }
    }
}