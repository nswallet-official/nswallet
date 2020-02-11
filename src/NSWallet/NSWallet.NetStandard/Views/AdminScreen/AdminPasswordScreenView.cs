using System;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class AdminPasswordScreenView : ContentPage
    {
        public AdminPasswordScreenView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
			UINavigationHeader.SetCommonTitleView(this, TR.Tr("admin_panel"));

            BackgroundColor = Theme.Current.ListBackgroundColor;

            var pageVM = new AdminPasswordScreenViewModel(Navigation);
            BindingContext = pageVM;

            var mainStackLayout = new StackLayout();
            mainStackLayout.Padding = Theme.Current.BodyPadding;

            var passwordField = new RectangularEntry();
			passwordField.FontFamily = NSWFontsController.CurrentTypeface;
			passwordField.HeightRequest = Theme.Current.PasswordHeight;
            passwordField.Margin = Theme.Current.PasswordMargin;
            passwordField.Placeholder = TR.Tr("admin_password_placeholder");
			passwordField.FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Entry));
			passwordField.SetBinding(Entry.TextProperty, "Password");
            passwordField.IsPassword = true;
			passwordField.FontFamily = NSWFontsController.CurrentTypeface;
            mainStackLayout.Children.Add(passwordField);

            var enterButton = new Button();
            enterButton.BackgroundColor = Theme.Current.CommonButtonBackground;
            enterButton.Margin = Theme.Current.PasswordMargin;
            enterButton.TextColor = Theme.Current.CommonButtonTextColor;
            enterButton.FontAttributes = Theme.Current.LoginButtonFontAttributes;
            enterButton.FontFamily = NSWFontsController.CurrentTypeface;
			enterButton.FontSize = FontSizeController.GetSize(NamedSize.Default, typeof(Button));
			enterButton.FontFamily = NSWFontsController.CurrentBoldTypeface;
			enterButton.Text = TR.Tr("admin_password_enter");
            enterButton.SetBinding(Button.CommandProperty, "EnterCommand");
            mainStackLayout.Children.Add(enterButton);

            Content = mainStackLayout;
        }
    }
}