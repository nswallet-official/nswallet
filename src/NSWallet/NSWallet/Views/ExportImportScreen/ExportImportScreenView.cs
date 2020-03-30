using System;
using FFImageLoading.Forms;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class ExportImportScreenView : ContentPage
	{
		ExportImportScreenViewModel pageVM;

		public ExportImportScreenView()
		{
			pageVM = new ExportImportScreenViewModel();
			BindingContext = pageVM;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_export_import"));

			BackgroundColor = Theme.Current.ListBackgroundColor;

			var mainStackLayout = AddMainLayout();

			AddGroup(mainStackLayout, TR.Tr("section_export"));
			AddListButton(mainStackLayout, "GeneratePDFCommand", TR.Tr("export_pdf"), Theme.Current.IEMenuExport2PDF);
			AddSeparator(mainStackLayout);
			AddListButton(mainStackLayout, "GenerateTXTCommand", TR.Tr("export_txt"), Theme.Current.IEMenuExport2TXT);
			AddGroup(mainStackLayout, TR.Tr("section_import"));
			AddListButton(mainStackLayout, "ImportBackupCommand", TR.Tr("import_backup"), Theme.Current.IEMenuImportFromBackup);

			AddSeparator(mainStackLayout);

			pageVM.ShowImportHelpCallback = ShowImportHelp;

			Content = new ScrollView {
				Content = mainStackLayout
			};
		}

		static StackLayout AddMainLayout() {
			return new StackLayout {
				BackgroundColor = Theme.Current.ListBackgroundColor,
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = StackOrientation.Vertical,
				Spacing = 0
			};
		}

		void ShowImportHelp() {
			AppPages.ImportBackupHelp(Navigation);
		}

		static void AddSeparator(StackLayout settingsLayout)
		{
			var separator = new BoxView {
				Color = Theme.Current.ListSeparatorColor,
				HeightRequest = 1,
				Opacity = 0.5
			};

			settingsLayout.Children.Add(separator);
		}

		static void AddGroup(StackLayout layout, string groupName)
		{
			var groupLayout = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Padding = Theme.Current.InnerMenuPadding,
				BackgroundColor = Theme.Current.GroupBackground,
			};

			var group = new Label {
				Text = groupName,
				TextColor = Theme.Current.GroupTextColor,
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label))
			};

			groupLayout.Children.Add(group);
			layout.Children.Add(groupLayout);
		}

		static void AddListButton(StackLayout stack, string modelCommand, string menuName, string menuIcon)
		{
			var itemsImportLayout = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Padding = Theme.Current.InnerMenuPadding
			};

			if (!string.IsNullOrEmpty(menuIcon)) {
				var itemImage = new CachedImage {
					HeightRequest = Theme.Current.MenuIconHeight,
					WidthRequest = Theme.Current.MenuIconWidth,
					Source = ImageSource.FromStream(() => NSWRes.GetImage(menuIcon)),
					Style = ImageProperties.DefaultCachedImageStyle
				};

				itemsImportLayout.Children.Add(itemImage);
				itemsImportLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_16 });
			} else {
				itemsImportLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_48 });
			}

			var itemStackLayout = new StackLayout();

			var itemLabel = new Label {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontAttributes = Theme.Current.MenuLabelFontAttributes,
				TextColor = Theme.Current.ListTextColor,
				Opacity = 0.85,
				Text = menuName,
				FontFamily = NSWFontsController.CurrentBoldTypeface
			};

			itemStackLayout.Children.Add(itemLabel);
			itemsImportLayout.Children.Add(itemStackLayout);
			stack.Children.Add(itemsImportLayout);

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, modelCommand);
			itemsImportLayout.GestureRecognizers.Add(tapGestureRecognizer);
		}
	}
}