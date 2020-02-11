using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Fingerprint.Dialog;
using NSWallet.Shared;

namespace NSWallet.Droid
{
	public class MyCustomDialogFragment : FingerprintDialogFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = base.OnCreateView(inflater, container, savedInstanceState);
			view.Background = new ColorDrawable(Color.Rgb(0xe0, 0xe0, 0xe0));  // Color.LightGray);
			Button buttonCancel = (Button)view.FindViewById(Resource.Id.fingerprint_btnCancel);
			buttonCancel.SetTextColor(Color.Gray);

			var buttonFallback = (Button)view.FindViewById(Resource.Id.fingerprint_btnFallback);
			buttonFallback.Visibility = ViewStates.Invisible;


			return view;
		}
	}
}
