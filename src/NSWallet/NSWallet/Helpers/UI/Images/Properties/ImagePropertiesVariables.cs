using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace NSWallet
{
	public static partial class ImageProperties
	{
		static Setter defaultCacheDurationSetter = new Setter {
			Property = CachedImage.CacheDurationProperty,
			Value = TimeSpan.FromHours(2)
		};

		static Setter defaultRetryCountSetter = new Setter {
			Property = CachedImage.RetryCountProperty,
			Value = 1
		};

		static Setter defaultRetryDelaySetter = new Setter {
			Property = CachedImage.RetryDelayProperty,
			Value = 250
		};

		static Style defaultCachedImageStyle = new Style(typeof(CachedImage)) {
			Setters = {
				defaultCacheDurationSetter,
				defaultRetryCountSetter,
				defaultRetryDelaySetter
			}
		};

		static Style defaultCachedIconStyle = new Style(typeof(Image)) {
			Setters = {
				defaultCacheDurationSetter,
				defaultRetryCountSetter,
				defaultRetryDelaySetter,
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
  	}
}