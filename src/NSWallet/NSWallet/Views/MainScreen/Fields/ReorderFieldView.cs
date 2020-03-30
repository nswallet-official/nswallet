using System;
using System.Collections.Generic;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class ReorderFieldView : ContentPage
    {
        public ReorderFieldView(List<NSWFormsItemModel> fields)
        {
            var pageVM = new ReorderFieldViewModel(fields);
            BindingContext = pageVM;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("app_name"));

			Icon = Theme.Current.MenuIcon;

            var closePageToolbar = new ToolbarItem();
            closePageToolbar.Icon = Theme.Current.CloseIcon;
            closePageToolbar.Clicked += (sender, e) => Navigation.PopModalAsync(true);
            ToolbarItems.Add(closePageToolbar);

            var fieldsListView = new ListView();
            fieldsListView.HasUnevenRows = true;
            fieldsListView.BackgroundColor = Theme.Current.ListBackgroundColor;
            fieldsListView.ItemTemplate = new DataTemplate(typeof(ReorderFieldViewCell));
            fieldsListView.SeparatorVisibility = SeparatorVisibility.None;
            fieldsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Fields");
            fieldsListView.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };

            Content = fieldsListView;
        }
    }
}