using NSWallet.NetStandard.Helpers.Fonts;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.Icons.ViewCells
{
	public class IPHeaderViewCell : ExtendedViewCell
	{
		public IPHeaderViewCell()
		{
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var label = new Label {
				TextColor = Theme.Current.CommonButtonTextColor,
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label))
			};

			label.SetBinding(Label.TextProperty, "Title");

			var groupLayout = new StackLayout {
				BackgroundColor = Theme.Current.CommonGroupHeaderBackground,
				Padding = Theme.Current.CommonListHeaderPadding,
				Children = {
					label
				}
			};

			View = groupLayout;
		}
	}
}