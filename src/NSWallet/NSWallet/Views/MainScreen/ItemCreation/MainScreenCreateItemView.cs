using NSWallet.Consts;
using NSWallet.Enums;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class MainScreenCreateItemView : ContentPage
    {
        public MainScreenCreateItemView(NSWItemType itemType)
        {
			BackgroundColor = Theme.Current.ListBackgroundColor;

            var pageVM = new MainScreenCreateItemViewModel(Navigation, itemType);
            BindingContext = pageVM;

			UINavigationHeader.SetCommonTitleView(this, null, "Title");

			var mainStackLayout = new StackLayout();
			mainStackLayout.Padding = new Thickness(5);
            mainStackLayout.VerticalOptions = LayoutOptions.CenterAndExpand;

            var name = new RectangularEntry();
			name.FontFamily = NSWFontsController.CurrentTypeface;
			name.Margin = Theme.Current.EntryMargin;
            name.HeightRequest = Theme.Current.EntryHeight;
			name.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(RectangularEntry));
			name.SetBinding(Entry.TextProperty, "Name");

            if (itemType.Equals(NSWItemType.Item))
            {
                name.Placeholder = TR.Tr("create_item_placeholder");
                name.AutomationId = AutomationIdConsts.ITEM_NAME_INPUT_ID;
            }
            else
            {
                name.Placeholder = TR.Tr("create_folder_placeholder");
                name.AutomationId = AutomationIdConsts.FOLDER_NAME_INPUT_ID;
            }
                
            mainStackLayout.Children.Add(name);

            var buttonsLayout = new StackLayout();
            buttonsLayout.Margin = Theme.Current.EntryMargin;
            buttonsLayout.Orientation = StackOrientation.Horizontal;

            var cancel = new Button();
			cancel.CornerRadius = 0;
            cancel.HorizontalOptions = LayoutOptions.FillAndExpand;
            cancel.BackgroundColor = Theme.Current.CommonButtonBackground;
            cancel.TextColor = Theme.Current.CommonButtonTextColor;
            cancel.FontAttributes = Theme.Current.LoginButtonFontAttributes;
            cancel.FontFamily = NSWFontsController.CurrentBoldTypeface;
            cancel.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
            cancel.Text = TR.Tr("cancel");
            cancel.SetBinding(Button.CommandProperty, "CancelCommand");
            buttonsLayout.Children.Add(cancel);

            var next = new Button();
			next.CornerRadius = 0;
            next.HorizontalOptions = LayoutOptions.FillAndExpand;
            next.BackgroundColor = Theme.Current.CommonButtonBackground;
            next.TextColor = Theme.Current.CommonButtonTextColor;
            next.FontAttributes = Theme.Current.LoginButtonFontAttributes;
            next.FontFamily = NSWFontsController.CurrentBoldTypeface;
            next.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Button));
            next.Text = TR.Tr("next");
            next.SetBinding(Button.CommandProperty, "NextCommand");
            next.AutomationId = AutomationIdConsts.NEXT_BUTTON_ID;
            buttonsLayout.Children.Add(next);

            mainStackLayout.Children.Add(buttonsLayout);

			name.Focused += (sender, e) => {
				mainStackLayout.VerticalOptions = LayoutOptions.StartAndExpand;
			};

			name.Unfocused += (sender, e) => {
				mainStackLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
			};

            var scroll = new ScrollView();
            scroll.Content = mainStackLayout;

            Content = scroll;
        }
    }
}