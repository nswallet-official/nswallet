using System;
using FFImageLoading.Forms;
using ImageCircle.Forms.Plugin.Abstractions;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Views.Icons.ViewCells
{
	public class IPItemViewCell : ExtendedViewCell
	{
		public IPItemViewCell()
		{
			SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor;

			var iconStyle = new Style(typeof(Image)) {
				Setters = {
					new Setter {
						Property = View.VerticalOptionsProperty,
						Value = LayoutOptions.Center
					},
					new Setter {
						Property = VisualElement.HeightRequestProperty,
						Value = Theme.Current.CommonIconHeight
					},
					new Setter {
						Property = VisualElement.WidthRequestProperty,
						Value = Theme.Current.CommonIconWidth
					},
					new Setter {
						Property = View.MarginProperty,
						Value = Theme.Current.CommonIconMargin
					}
				}
			};
			
			var icon = new CachedImage {
				Style = ImageProperties.DefaultCachedIconStyle
			};

			icon.SetBinding(CachedImage.SourceProperty, "Icon");
			icon.SetBinding(VisualElement.IsVisibleProperty, "IsNotCircle");

			var iconCircle = new CircleImage {
				BorderThickness = Theme.Current.CommonIconBorderWidth,
				BorderColor = Theme.Current.CommonIconBorderColor,
				Aspect = Aspect.AspectFill,
				Style = iconStyle
			};

			iconCircle.SetBinding(Image.SourceProperty, "Icon");
			iconCircle.SetBinding(VisualElement.IsVisibleProperty, "IsCircle");

			var name = new Label {
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				TextColor = Theme.Current.ListTextColor,
				VerticalOptions = LayoutOptions.Center
			};

			name.SetBinding(Label.TextProperty, "Name");

			var lockedIcon = new CachedImage {
				Source = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.LockIcon)),
				HeightRequest = Theme.Current.CommonIconHeight,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				Style = ImageProperties.DefaultCachedImageStyle
			};

			lockedIcon.SetBinding(VisualElement.IsVisibleProperty, "IsLocked");

			var mainStackLayout = new StackLayout {
				Padding = Theme.Current.IconPageItemPadding,
				Orientation = StackOrientation.Horizontal,
				Children = {
					icon,
					iconCircle,
					name,
					lockedIcon
				}
			};

			View = new StackLayout {
				Spacing = 0,
				Children = {
					mainStackLayout,
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