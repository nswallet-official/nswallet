using System;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.SettingsScreen.FontSelectorScreen.ViewCells
{
	public class FontSelectorViewCell : ExtendedViewCell
	{
		public FontSelectorViewCell()
		{
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var fontLabel = new Label {
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				Margin = Theme.Current.FontPageLabelMargin,
				TextColor = Theme.Current.ListTextColor
			};

			fontLabel.SetBinding(Label.FontFamilyProperty, "Typeface");
			fontLabel.SetBinding(Label.TextProperty, "Name");

			View = new StackLayout {
				Spacing = 0,
				Children = {
					fontLabel,
					new BoxView {
						HeightRequest = 1,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Color = Theme.Current.ListSeparatorColor
					}
				}
			};
		}
	}
}