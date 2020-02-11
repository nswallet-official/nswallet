using NSWallet.iOS.CustomRenderers.ViewCells;
using NSWallet.NetStandard;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]
namespace NSWallet.iOS.CustomRenderers.ViewCells
{
	public class ExtendedViewCellRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			var view = item as ExtendedViewCell;
			cell.SelectedBackgroundView = new UIView {
				BackgroundColor = view.SelectedBackgroundColor.ToUIColor(),
			};
			return cell;
		}
	}
}