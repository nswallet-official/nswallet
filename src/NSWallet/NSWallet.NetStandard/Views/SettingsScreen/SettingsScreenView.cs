using System.Collections.Generic;
using FFImageLoading.Forms;
using NSWallet.Controls.EntryPopup;
using NSWallet.Helpers;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Helpers.Fonts;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class SettingsScreenView : ContentPage
    {
        SettingsScreenViewModel pageVM;
        ActivityIndicator indicator;
        StackLayout setSL;

        public SettingsScreenView()
        {
			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_settings"));

            pageVM = new SettingsScreenViewModel(Navigation);
            BindingContext = pageVM;

            StackLayout settingsLayout = new StackLayout
            {
                BackgroundColor = Theme.Current.ListBackgroundColor,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            // Security
            AddGroup(settingsLayout, TR.Tr("settings_security"));

            AddSettingButton(settingsLayout, "ChangePasswordCommand", TR.Tr("settings_chpass"), Theme.Current.SettingsChangePass, "ChosenPassword");
            AddSeparator(settingsLayout);

            AddSettingButton(settingsLayout, "AutoLogoutCommand", TR.Tr("settings_autologout"), Theme.Current.SettingsLogoutInterval, "ChosenAutoLogout");
            AddSeparator(settingsLayout);

            AddCheckbox(settingsLayout, TR.Tr("settings_clipboard_clean"), "IsClipCleanChecked", Theme.Current.SettingsClipboard, "IsClipCleanToggleCommand", "ChosenClipClean");
            AddSeparator(settingsLayout);

            AddCheckbox(settingsLayout, TR.Tr("settings_auto_login"), "IsAutoLoginChecked", Theme.Current.SettingsAutoLogin, "IsAutoLoginToggleCommand", "ChosenAutoLogin", true);
            AddSeparator(settingsLayout);

            AddCheckbox(settingsLayout, TR.Tr("settings_hidepass"), "IsHidePasswordChecked", Theme.Current.SettingsHidePass, "IsHidePasswordToggleCommand", "ChosenHidePassword");
            AddSeparator(settingsLayout);

            AddSettingButton(settingsLayout, "PasswordTipCommand", TR.Tr("settings_pass_tooltip"), Theme.Current.SettingsPasswordTip, "ChosenPassTip", true);
			AddSeparator(settingsLayout);

			if (FingerprintHelper.IsAvailable) {
				AddCheckbox(settingsLayout, TR.Tr("settings_fingerprint"), "IsFingerprintChecked", Theme.Current.SettingsFingerprint, "FingerprintActiveToggleCommand", "ChosenFingerprintActive");
				AddSeparator(settingsLayout);

				AddCheckbox(settingsLayout, TR.Tr("settings_is_auto_fingerprint"), "IsAutoFingerprintChecked", Theme.Current.SettingsFingerprint, "AutoFingerprintToggleCommand", "ChosenAutoFingerprintActive");
			}

			// Backup
			AddGroup(settingsLayout, TR.Tr("settings_backup"));

            AddSettingButton(settingsLayout, "AutoBackupCommand", TR.Tr("settings_autobackup"), Theme.Current.SettingsBackupLevel, "ChosenAutoBackup");
			AddSeparator(settingsLayout);

            AddSettingButton(settingsLayout, "BackupDeletionCommand", TR.Tr("settings_backups_deletion"), Theme.Current.SettingsBackupDelete, "ChosenBackupDeletion");

            // Visual design
            AddGroup(settingsLayout, TR.Tr("settings_visual_design"));

            AddSettingButton(settingsLayout, "ThemeCommand", TR.Tr("settings_theme"), Theme.Current.SettingsTheme, "ChosenTheme");
            AddSeparator(settingsLayout);

            AddSettingButton(settingsLayout, "LanguageCommand", TR.Tr("settings_language"), Theme.Current.SettingsLanguage, "ChosenLanguage");
            AddSeparator(settingsLayout);

			AddSettingButton(settingsLayout, "FontSizeCommand", TR.Tr("settings_font_size"), Theme.Current.SettingsFontSize, "ChosenFontSize");
			AddSeparator(settingsLayout);

			AddSettingButton(settingsLayout, "FontCommand", TR.Tr("settings_font"), Theme.Current.SettingsFonts, "ChosenFont", true);
			AddSeparator(settingsLayout);



			//AddCheckbox(settingsLayout, TR.Tr("settings_social"), "IsSocialChecked", Theme.Current.SettingsSocial, "IsSocialToggleCommand", "ChosenSocial", true);
			//AddSeparator(settingsLayout);

			AddCheckbox(settingsLayout, TR.Tr("settings_auto_night_mode"), "IsAutoNightModeChecked", Theme.Current.SettingsNightModeIcon, "IsAutoNightModeCheckedCommand", "ChosenAutoNightMode", true);

			// Special folders
			AddGroup(settingsLayout, TR.Tr("settings_special_folders"));

            AddCheckbox(settingsLayout, TR.Tr("settings_is_expiring"), "IsExpiringSoonChecked", Theme.Current.SettingsExpiring, "IsExpiringSoonToggleCommand", "ChosenExpiringSoon", true);
            AddSeparator(settingsLayout);

            AddSettingButton(settingsLayout, "ExpiringPeriodCommand", TR.Tr("settings_expiring_period"), Theme.Current.SettingsExpiringPeriod, "ChosenExpiringPeriod", true);
            AddSeparator(settingsLayout);

            AddCheckbox(settingsLayout, TR.Tr("settings_is_recently_viewed"), "IsRecentlyViewedChecked", Theme.Current.SettingsRecent, "IsRecentlyViewedToggleCommand", "ChosenRecentlyViewed", true);
            AddSeparator(settingsLayout);

            AddCheckbox(settingsLayout, TR.Tr("settings_is_mostly_viewed"), "IsMostlyViewedChecked", Theme.Current.SettingsMost, "IsMostlyViewedToggleCommand", "ChosenMostlyViewed", true);

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    // App behaviour
                    AddGroup(settingsLayout, TR.Tr("settings_app_behavior"));
                    AddCheckbox(settingsLayout, TR.Tr("settings_android_exit"), "IsDroidLogoutChecked", Theme.Current.MenuIconLogout, "ChosenDroidLogout");
                    AddSeparator(settingsLayout);
                    break;
            }

            AddSeparator(settingsLayout);

			AddGroup(settingsLayout, TR.Tr("settings_group_extra"), true);
			AddSettingButton(settingsLayout, "RestoreDefaultCommand", TR.Tr("settings_delete_all"), Theme.Current.SettingsDeleteAll);
            AddSettingButton(settingsLayout, "OptimizeCommand", TR.Tr("settings_optimize"), Theme.Current.SettingsDeleteAll);
            AddSeparator(settingsLayout);

            pageVM.AutoBackupCommandCallback = AutoBackupSelector;
            pageVM.AutoLogoutCommandCallback = AutoLogoutSelector;
            pageVM.BackupDeletionCommandCallback = BackupsDeletionSelector;
            pageVM.ThemeCommandCallback = ThemeSelector;
            pageVM.LanguageCommandCallback = LanguageSelector;
			pageVM.FontSizeCommandCallback = FontSizeSelector;
			pageVM.FontCommandCallback = FontSelector;
            pageVM.ChangePasswordCallback = EnterPassword;
            pageVM.ExpiringPeriodCommandCallback = ExpiringPeriodSelector;
            pageVM.PasswordTipCommandCallback = EnterPasswordTip;

            indicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Color = Color.Black,
                IsVisible = false
            };

            setSL = new StackLayout
            {
                Children = { new ScrollView { Content = settingsLayout } },
            };

            Content = new AbsoluteLayout
            {
                BackgroundColor = Theme.Current.ListBackgroundColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { setSL, indicator }
            };

            AbsoluteLayout.SetLayoutFlags(setSL, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(setSL, new Rectangle(0, 0, 1, 1));

            AbsoluteLayout.SetLayoutFlags(indicator, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(indicator, new Rectangle(0, 0, 1, 1));

        }

        public async void EnterPasswordTip()
        {
            bool result = true;

            if (string.IsNullOrEmpty(Settings.PasswordTip))
            {
                var answer = await DisplayAlert(TR.Tr("app_name"), TR.Tr("settings_pass_tooltip_alert"), null, TR.OK);
                result = !answer;
            }
            else
            {
                result = true;
            }

            if (result)
            {
                var chosenTip = pageVM.ChosenPassTip;
                var extraButtons = new List<string>() { TR.Tr("settings_pass_tooltip_remove") };
                var popup = new EntryPopup(TR.Tr("settings_pass_tooltip_enter"), null);

                popup.PopupClosed += (o, closedArgs) =>
                {
                    if (closedArgs.OkClicked)
                    {
                        var tip = closedArgs.Text;

                        if (string.IsNullOrEmpty(tip))
                        {
                            pageVM.PasswordTipSuccessCommand.Execute(null);
                        }
                        else
                        {
                            pageVM.PasswordTipSuccessCommand.Execute(tip);
                        }
                    }
                };

                popup.Show();
            }
        }

        public void EnterPassword()
        {
            // FIXME: create correct model view separation, change password should go to model!!!
            var popup = new EntryPopup(TR.Tr("enter_new_password"), string.Empty, true);
            popup.PopupClosed += (o, closedArgs) =>
            {
                if (closedArgs.OkClicked)
                {
                    if (closedArgs.Text == string.Empty)
                    {
                        DisplayAlert(TR.Tr("app_name"), TR.Tr("password_empty"), TR.OK);
                        return;
                    }
                    var popupConfirm = new EntryPopup(TR.Tr("confirm_new_password"), string.Empty, true);
                    popupConfirm.PopupClosed += (o2, closedConfirmedArgs) =>
                    {
                        if (closedConfirmedArgs.OkClicked)
                        {
                            if (closedArgs.Text == closedConfirmedArgs.Text)
                            {

                                if (BL.ChangePassword(closedArgs.Text))
                                {
									if (Settings.UsedFingerprintBefore) {
										Settings.FingerprintCount = 21;
										Settings.RememberedPassword = closedArgs.Text;
									}
                                    DisplayAlert(TR.Tr("password_changed"), TR.Tr("password_remember"), TR.OK);
                                }
                            }
                            else
                            {
                                DisplayAlert(TR.Tr("app_name"), TR.Tr("password_not_confirmed"), TR.OK);
                            }
                        }
                    };
                    popupConfirm.Show();
                }

            };

            popup.Show();
        }

        private void PasswordChanged()
        {

        }

		public void ExpiringPeriodSelector()
		{
            DisplayActionSheet(TR.Tr("settings_expiring_period"), TR.Cancel, null,
							   TR.Tr("settings_expiring_period_10"),
							   TR.Tr("settings_expiring_period_30"),
							   TR.Tr("settings_expiring_period_all")).ContinueWith(t =>
							   {
								   if (pageVM.ExpiringPeriodSelectedCommand.CanExecute(t.Result))
								   {
									   pageVM.ExpiringPeriodSelectedCommand.Execute(t.Result);
								   }
							   });
		}

        public void AutoBackupSelector()
        {
            DisplayActionSheet(TR.Tr("settings_autobackup"), TR.Cancel, null,
                               TR.Tr("autobackup_no_backup"),
                               TR.Tr("autobackup_weekly"),
                               TR.Tr("autobackup_daily")).ContinueWith(t =>
                               {
                                   if (pageVM.AutoBackupSelectedCommand.CanExecute(t.Result))
                                   {
                                       pageVM.AutoBackupSelectedCommand.Execute(t.Result);
                                   }
                               });
        }

        public void AutoLogoutSelector()
        {
            DisplayActionSheet(TR.Tr("settings_autologout"), TR.Cancel, null,
                               TR.Tr("settings_autologout_focus"),
                               TR.Tr("settings_autologout_5"),
                               TR.Tr("settings_autologout_10"),
                               TR.Tr("settings_autologout_30")).ContinueWith(t =>
                               {
                                   if (pageVM.AutoLogoutSelectedCommand.CanExecute(t.Result))
                                   {
                                       pageVM.AutoLogoutSelectedCommand.Execute(t.Result);
                                   }
                               });
        }

		public void BackupsDeletionSelector()
		{
            DisplayActionSheet(TR.Tr("settings_backups_deletion"), TR.Cancel, null,
                               TR.Tr("backups_deletion_5"),
                               TR.Tr("backups_deletion_10"),
                               TR.Tr("backups_deletion_30"),
                               TR.Tr("backups_deletion_90"),
                               TR.Tr("backups_deletion_180")).ContinueWith(t =>
                               {
                                   if (pageVM.BackupDeletionSelectedCommand.CanExecute(t.Result))
                                   {
                                       pageVM.BackupDeletionSelectedCommand.Execute(t.Result);
                                   }
                               });
		}

        public void ThemeSelector(bool premium)
        {
            string[] buttons = null;
            if(premium)
            {
                buttons = new string[]
                {
                    TR.Tr(AppTheme.ThemeDefault),
                    TR.Tr(AppTheme.ThemeDark),
                    TR.Tr(AppTheme.ThemeGreen),
                    TR.Tr(AppTheme.ThemeRed),
                    TR.Tr(AppTheme.ThemeGray),
                    TR.Tr(AppTheme.ThemeYellow)
                };
            }
            else
            {
                buttons = new string[]
                {
                    TR.Tr(AppTheme.ThemeDefault),
                    TR.Tr(AppTheme.ThemeDark),
                    TR.Tr("more_themes")
                };
            }

            DisplayActionSheet(TR.Tr("theme_choose"), TR.Cancel, null, buttons).ContinueWith(t =>
            {
                if (pageVM.ThemeSelectedCommand.CanExecute(t.Result))
                {
                    pageVM.ThemeSelectedCommand.Execute(t.Result);
                }
            });
        }

		public void FontSizeSelector()
		{
			var fontSizes = new List<string> {
				TR.Tr("font_sizes_standard"),
				TR.Tr("font_sizes_large")
			};

			DisplayActionSheet(TR.Tr("settings_font_size_choose"), TR.Cancel, null, fontSizes.ToArray()).ContinueWith(t => {
				if (string.Compare(t.Result, TR.Cancel) != 0 && !string.IsNullOrEmpty(t.Result)) {
					if (pageVM.FontSizeSelectedCommand.CanExecute(t.Result)) {
						pageVM.FontSizeSelectedCommand.Execute(t.Result);
					}
				}
			});
		}

		public void LanguageSelector()
        {
            var langs = Lang.availableLangs();
            var strLangs = new List<string>();
            var systemLang = "";

            foreach (var lang in langs)
            {
                if (lang.LangCode == GConsts.DEFAULT_LANG)
                {
                    systemLang = TR.Tr("default") + " (" + Lang.getLangByCode(AppLanguage.GetSystemLangCode()).LanguageEnglish + ")";
                    strLangs.Add(systemLang);
                }
                else
                {
                    strLangs.Add(lang.LanguageCombined);
                }
            }

            strLangs.Add(TR.Tr("languages_other"));

            DisplayActionSheet("Choose language", TR.Cancel, null, strLangs.ToArray()).ContinueWith(t =>
            {
                if (pageVM.LanguageSelectedCommand.CanExecute(t.Result))
                {
                    if (t.Result == systemLang)
                    {
                        pageVM.LanguageSelectedCommand.Execute(GConsts.DEFAULT_LANG);
                    } else if (t.Result == TR.Tr("languages_other")) {
                        pageVM.LanguageSelectedCommand.Execute(TR.Tr("languages_other"));
                        
                    }
                    else
                    {
                        foreach (var lang in langs)
                        {
                            if (t.Result == lang.LanguageCombined) {
                                pageVM.LanguageSelectedCommand.Execute(lang.LangCode);        
                            }
                        }

                    }
                }
            });
        }

        public void FontSelector()
        {
			var fonts = NSWFontsController.GetFontNames();

            DisplayActionSheet("Choose font", TR.Cancel, null, fonts.ToArray()).ContinueWith(t =>
            {
                if (pageVM.FontSelectedCommand.CanExecute(t.Result))
                {
                    pageVM.FontSelectedCommand.Execute(t.Result);
                }
            });
        }

        static void AddSeparator(StackLayout settingsLayout)
        {
            var separator = new BoxView
            {
                Color = Theme.Current.ListSeparatorColor,
                HeightRequest = 1,
                Opacity = 0.5
            };

            settingsLayout.Children.Add(separator);
        }

        void AddGroup(StackLayout settingsLayout, string groupName, bool dangerGroup = false)
        {
            var groupLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Padding = Theme.Current.InnerMenuPadding,
                BackgroundColor = Theme.Current.GroupBackground,
            };

			if (dangerGroup) {
                groupLayout.BackgroundColor = Theme.Current.GroupDangerBackground;
			}

			var group = new Label {
				Text = groupName,
				TextColor = Theme.Current.GroupTextColor,
				FontFamily = NSWFontsController.CurrentTypeface,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label))
            };

			//FontFamily = Device.OnPlatform("Montserrat-Regular", "Montserrat-Regular.ttf", null, null);
			//group.FontFamily = "Montserrat-Regular.ttf#Montserrat-Regular";
			//group.FontFamily = "Montserrat-Regular";
			groupLayout.Children.Add(group);
            settingsLayout.Children.Add(groupLayout);
        }

        void AddCheckbox(StackLayout settingsLayout, string settingName, string checkedProperty, string menuIcon, string toggleCommand, string chosenSelectionProperty = null, bool isPremium = false)
        {
            var checkboxLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Padding = Theme.Current.InnerMenuPadding
            };

            if (!string.IsNullOrEmpty(menuIcon))
            {
                var itemImage = new CachedImage
                {
                    HeightRequest = Theme.Current.MenuIconHeight,
                    WidthRequest = Theme.Current.MenuIconWidth,
                    Source = ImageSource.FromStream(() => NSWRes.GetImage(menuIcon)),
					Style = ImageProperties.DefaultCachedImageStyle
				};

                checkboxLayout.Children.Add(itemImage);
                checkboxLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_16 });
            }
            else
            {
                checkboxLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_48 });
            }

            var itemStackLayout = new StackLayout();
            itemStackLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            

			var itemLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = Theme.Current.MenuLabelFontAttributes,
                TextColor = Theme.Current.ListTextColor,
                Opacity = 0.85,
                Text = settingName,
				FontFamily = NSWFontsController.CurrentBoldTypeface
			};

            itemStackLayout.Children.Add(itemLabel);

			if (!string.IsNullOrEmpty(chosenSelectionProperty))
			{
                var chosenSelection = new Label
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Theme.Current.ListTextColor,
                    Opacity = 0.85,
					FontFamily = NSWFontsController.CurrentTypeface,
					FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label))
				};
                chosenSelection.SetBinding(Label.TextProperty, chosenSelectionProperty);

                itemStackLayout.Children.Add(chosenSelection);
            }

            checkboxLayout.Children.Add(itemStackLayout);

            checkboxLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_48 });

            var checkBoxSwitch = new Switch();


                checkBoxSwitch = new Switch { HorizontalOptions = LayoutOptions.EndAndExpand };
                checkBoxSwitch.SetBinding(Switch.IsToggledProperty, checkedProperty);
                checkboxLayout.Children.Add(checkBoxSwitch);
            

            settingsLayout.Children.Add(checkboxLayout);

            if (toggleCommand != null)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, toggleCommand);
                checkboxLayout.GestureRecognizers.Add(tapGestureRecognizer);
            }
        }

        void AddSettingButton(StackLayout stack, string modelCommand, string menuName, string menuIcon, string chosenSelectionProperty = null, bool isPremium = false)
        {
            var itemSettingLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Padding = Theme.Current.InnerMenuPadding
            };

            if (!string.IsNullOrEmpty(menuIcon))
            {
				var itemImage = new CachedImage
                {
                    HeightRequest = Theme.Current.MenuIconHeight,
                    WidthRequest = Theme.Current.MenuIconWidth,
                    Source = ImageSource.FromStream(() => NSWRes.GetImage(menuIcon)),
					Style = ImageProperties.DefaultCachedImageStyle
				};

                itemSettingLayout.Children.Add(itemImage);
                itemSettingLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_16 });
            }
            else
            {
                itemSettingLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = Theme.Current.MenuBox_48 });
            }

            var itemStackLayout = new StackLayout();

			var textColor = Theme.Current.ListTextColor;
			if (modelCommand == "RestoreDefaultCommand" || modelCommand == "OptimizeCommand") 
			{
				textColor = Theme.Current.DeleteAllButtonTextColor;
			}

			var itemLabel = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = FontSizeController.GetSize(NamedSize.Medium, typeof(Label)),
				FontAttributes = Theme.Current.MenuLabelFontAttributes,
                TextColor = textColor,
                Opacity = 0.85,
                Text = menuName,
				FontFamily = NSWFontsController.CurrentBoldTypeface
			};

            itemStackLayout.Children.Add(itemLabel);

            if (!string.IsNullOrEmpty(chosenSelectionProperty))
            {
                var chosenSelection = new Label
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = textColor,
                    Opacity = 0.85,
					FontFamily = NSWFontsController.CurrentTypeface,
					FontSize = FontSizeController.GetSize(NamedSize.Small, typeof(Label))
				};
                chosenSelection.SetBinding(Label.TextProperty, chosenSelectionProperty);

                itemStackLayout.Children.Add(chosenSelection);
            }

            itemSettingLayout.Children.Add(itemStackLayout);

            stack.Children.Add(itemSettingLayout);


            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, modelCommand);
            itemSettingLayout.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override bool OnBackButtonPressed()
        {
            if (Settings.AndroidBackLogout)
            {
                Pages.Main();
            }

            return true;
        }
    }
}