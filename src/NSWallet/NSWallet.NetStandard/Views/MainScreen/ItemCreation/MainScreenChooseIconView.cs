using System;
using System.Diagnostics;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;
using ImageCircle.Forms.Plugin.Abstractions;
using NSWallet.Enums;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class MainScreenChooseIconView : ContentPage
    {
        public MainScreenChooseIconView(NSWItemType itemType, bool isEdit = false, string name = null)
        {
			var createScreenVM = new MainScreenChooseIconViewModel(Navigation, itemType, isEdit, name);
            BindingContext = createScreenVM;

			UINavigationHeader.SetCommonTitleView(this, null, "Title");

			BackgroundColor = Theme.Current.AppBackground;

            var mainLayout = new StackLayout();
            mainLayout.BackgroundColor = Theme.Current.AppBackground;

            if (itemType == NSWItemType.Item)
            {
				var searchBar = new CustomSearchBar();
				searchBar.FontFamily = NSWFontsController.CurrentTypeface;
				searchBar.Placeholder = TR.Tr("search_type_text");
				searchBar.SetBinding(SearchBar.TextProperty, "SearchText");
				searchBar.BackgroundColor = Theme.Current.AppHeaderBackground;
				searchBar.CancelButtonColor = Theme.Current.MainSearchCancelButtonColor;
				switch (Device.RuntimePlatform) {
					case Device.iOS:
						searchBar.TextColor = Color.Black;
						break;
					case Device.Android:
						searchBar.TextColor = Color.White;
						break;
				}
				searchBar.PlaceholderColor = Color.LightGray;

				switch (Device.RuntimePlatform) {
					case Device.Android:
						searchBar.HeightRequest = 40;
						break;
				}

				mainLayout.Children.Add(searchBar);

				var listView = new ListView();
                listView.SeparatorVisibility = SeparatorVisibility.None;
                listView.BackgroundColor = Theme.Current.ListBackgroundColor;
				listView.IsGroupingEnabled = true;
				listView.GroupHeaderTemplate = new DataTemplate(() => {
					var label = new Label {
						TextColor = Theme.Current.CommonButtonTextColor,
						FontFamily = NSWFontsController.CurrentTypeface,
						FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label))
					};
					label.SetBinding(Label.TextProperty, "Title");
					var groupLayout = new StackLayout {
						BackgroundColor = Theme.Current.CommonButtonBackground,
						Padding = new Thickness(10),
						Children = {
							label
						}
					};
					return new ExtendedViewCell { SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor, View = groupLayout };
				});
				listView.HasUnevenRows = true;
				listView.ItemTemplate = new DataTemplate(() => {
					var mainStackLayout = new StackLayout();
					mainStackLayout.Padding = new Thickness(5, 3);
					mainStackLayout.Orientation = StackOrientation.Horizontal;

					var icon = new CachedImage {
						HeightRequest = Theme.Current.CommonIconHeight,
						WidthRequest = Theme.Current.CommonIconWidth,
						VerticalOptions = LayoutOptions.Center,
						AutomationId = "item_icon_id",
						Margin = Theme.Current.CommonIconMargin,
						Style = ImageProperties.DefaultCachedImageStyle
					};

					icon.SetBinding(CachedImage.SourceProperty, "Icon");
					icon.SetBinding(IsVisibleProperty, "IsNotCircle");
					mainStackLayout.Children.Add(icon);

					var iconCircle = new CircleImage {
						HeightRequest = Theme.Current.CommonIconHeight,
						WidthRequest = Theme.Current.CommonIconWidth,
						VerticalOptions = LayoutOptions.Center,
						BorderThickness = Theme.Current.CommonIconBorderWidth,
						BorderColor = Theme.Current.CommonIconBorderColor,
						Aspect = Aspect.AspectFill,
						AutomationId = "item_icon_id",
						Margin = Theme.Current.CommonIconMargin
					};

					iconCircle.SetBinding(Image.SourceProperty, "Icon");
					iconCircle.SetBinding(IsVisibleProperty, "IsCircle");
					mainStackLayout.Children.Add(iconCircle);

					var nameLabel = new Label();
					nameLabel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label));
					nameLabel.TextColor = Theme.Current.ListTextColor;
					nameLabel.VerticalOptions = LayoutOptions.Center;
					nameLabel.FontFamily = NSWFontsController.CurrentTypeface;
					nameLabel.SetBinding(Label.TextProperty, "Name");
					mainStackLayout.Children.Add(nameLabel);

					return new ExtendedViewCell {
						SelectedBackgroundColor = Theme.Current.SelectedViewCellBackgroundColor,
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
						}
					};
				});

                listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Items");
                listView.SetBinding(ListView.SelectedItemProperty, "SelectedItem");

                mainLayout.Children.Add(listView);
            }

            if (itemType == NSWItemType.Folder)
            {
                Padding = Theme.PaddingChooseIconView;

                var flowListView = new FlowListView();
				flowListView.HasUnevenRows = true;
                flowListView.FlowColumnCount = 3;
                flowListView.BackgroundColor = Theme.Current.AppBackground;
                flowListView.FlowColumnTemplate = new DataTemplate(() =>
                {
					var icon = new CachedImage {
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 50,
						Style = ImageProperties.DefaultCachedImageStyle
					};
					icon.SetBinding(CachedImage.SourceProperty, "Icon");
                    icon.AutomationId = "folder_icon_id";
					icon.Margin = new Thickness(10);

                    return new FlowViewCell { Content = icon };
                });

                flowListView.SetBinding(FlowListView.FlowItemsSourceProperty, "Folders");
                flowListView.SetBinding(FlowListView.FlowItemTappedCommandProperty, "CreateCommand");

                mainLayout.Children.Add(flowListView);
            }

            var cancel = new Button();
			cancel.FontFamily = NSWFontsController.CurrentBoldTypeface;
			cancel.VerticalOptions = LayoutOptions.End;
			cancel.CornerRadius = 0;
            cancel.HorizontalOptions = LayoutOptions.FillAndExpand;
            cancel.BackgroundColor = Theme.Current.CommonButtonBackground;
            cancel.TextColor = Theme.Current.CommonButtonTextColor;
            cancel.FontAttributes = Theme.Current.LoginButtonFontAttributes;
            cancel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
            cancel.Text = TR.Tr("cancel");
            cancel.Clicked += (sender, e) => PopModalPages();
            mainLayout.Children.Add(cancel);

            Content = mainLayout;
        }

        void PopModalPages()
        {
            for (int i = 0; i < 2; i++)
                Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            PopModalPages();
            return true;
        }
    }
}


