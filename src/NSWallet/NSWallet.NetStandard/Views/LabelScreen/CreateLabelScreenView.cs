using System;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class CreateLabelScreenView : ContentPage
    {
        public CreateLabelScreenView(string labelName, string labelType, string fieldType = null, string actionType = null, Command command = null)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var pageVM = new CreateLabelScreenViewModel(Navigation, labelName, labelType, fieldType, actionType, command);
            BindingContext = pageVM;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_labels"));

            BackgroundColor = Theme.Current.ListBackgroundColor;

            var listView = new FlowListView();
            listView.BackgroundColor = Theme.Current.ListBackgroundColor;
            listView.Margin = new Thickness(0, 10, 0, 0);
            listView.RowHeight = 100;
            listView.FlowColumnCount = 4;
            listView.SetBinding(FlowListView.FlowItemTappedCommandProperty, "CreateCommand");
            listView.SetBinding(FlowListView.FlowItemsSourceProperty, "IconsList");
            listView.FlowColumnTemplate = new DataTemplate(() =>
            {
                var viewLayout = new StackLayout();

				var icon = new CachedImage {
					Margin = new Thickness(10, 0),
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				};
				icon.SetBinding(CachedImage.SourceProperty, "Image");
                viewLayout.Children.Add(icon);

                var emptyLayout = new StackLayout();
                emptyLayout.HeightRequest = 10;
                viewLayout.Children.Add(emptyLayout);

                return new FlowViewCell { Content = viewLayout };
            });

            Content = new ScrollView
            {
                Content = listView
            };
        }
    }
}