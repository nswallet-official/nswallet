using System.ComponentModel;
using Android.Graphics.Drawables;
using Android.Views;
using NSWallet.Droid.CustomRenderers.ViewCells;
using NSWallet.NetStandard;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]
namespace NSWallet.Droid.CustomRenderers.ViewCells
{
	public class ExtendedViewCellRenderer : ViewCellRenderer
	{
		Android.Views.View cellCore;
		Drawable unselectedBackground;
		bool selected;

		protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Android.Content.Context context)
		{
			cellCore = base.GetCellCore(item, convertView, parent, context);
			selected = false;
			unselectedBackground = cellCore.Background;
			return cellCore;
		}

		protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnCellPropertyChanged(sender, e);

			if (e.PropertyName == "IsSelected") {
				selected = !selected;
				if (selected) {
					var customTextCell = sender as ExtendedViewCell;
					cellCore.SetBackgroundColor(customTextCell.SelectedBackgroundColor.ToAndroid());
				} else {
					cellCore.SetBackground(unselectedBackground);
				}
			}
		}
	}
}