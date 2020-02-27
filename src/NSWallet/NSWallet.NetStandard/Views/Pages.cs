using System;
using System.Collections.Generic;
using NSWallet.Enums;
using NSWallet.NetStandard;
using NSWallet.NetStandard.Interfaces;
using NSWallet.NetStandard.Pages.Lock.Views;
using NSWallet.NetStandard.Views.Diagnostics;
using NSWallet.NetStandard.Views.Feedback;
using NSWallet.NetStandard.Views.IconsScreen;
using NSWallet.NetStandard.Views.License;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public static class AppPages
    {
        public static void Main()
        {
            var navPage = new NavigationPage(new MainScreenView())
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

			var masterPage = new MasterDetailPage {
				Master = new MainMenuView(),
				Detail = navPage
			};

			Application.Current.MainPage = masterPage;

		}


		public static void ImportBackupHelp(INavigation navigation) {
			var navPage = new NavigationPage(new ImportBackupView()) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			};

			navigation.PushModalAsync(navPage);
		}

        public static void AdminPanelPassword(INavigation navigation)
        {
            var navPage = new NavigationPage(new AdminPasswordScreenView());
            navPage.BarBackgroundColor = Theme.Current.AppHeaderBackground;
            navPage.BarTextColor = Theme.Current.AppHeaderTextColor;
            navigation.PushAsync(navPage);
        }

		public static void AdminPanel(INavigation navigation = null)
        {
			if (navigation != null) {
				var page = new AdminScreenView(navigation);
				navigation.PushAsync(page);
			} else {
				var navPage = new NavigationPage(new AdminScreenView()) {
					BarBackgroundColor = Theme.Current.AppHeaderBackground,
					BarTextColor = Theme.Current.AppHeaderTextColor
				};

				Application.Current.MainPage = new MasterDetailPage {
					Master = new MainMenuView(),
					Detail = navPage
				};
			}
        }

        public static void CreateField(INavigation navigation, bool isEdit, NSWItem nswItem)
        {
            var navPage = new NavigationPage(new ManageFieldView(isEdit, nswItem))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            navigation.PushModalAsync(navPage);
        }

        public static void UpdateField(INavigation navigation, bool isEdit, NSWItem nswItem, string fieldID, string fieldValue)
        {
            var navPage = new NavigationPage(new ManageFieldView(isEdit, nswItem, fieldID, true, fieldValue))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            navigation.PushModalAsync(navPage);
        }

        public static void ReorderField(INavigation navigation, List<NSWFormsItemModel> fields)
        {
            var navPage = new NavigationPage(new ReorderFieldView(fields))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

			//navigation.PopPopupAsync(false);
            navigation.PushModalAsync(navPage);
        }

        public static void CreateItemOrFolder(INavigation navigation, NSWItemType itemType, string name)
        {
            var navPage = new NavigationPage(new MainScreenChooseIconView(itemType, false, name))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            navigation.PushModalAsync(navPage);
        }

		public static void EditItemIcon(INavigation navigation, NSWItemType itemType, bool isEdit = false, string name = null)
		{
			var navPage = new NavigationPage(new MainScreenChooseIconView(itemType, isEdit, name)) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			};

			navigation.PushModalAsync(navPage);
		}

		public static void ExportImport()
		{
			var navPage = new NavigationPage(new ExportImportScreenView()) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			};

			Application.Current.MainPage = new MasterDetailPage {
				Master = new MainMenuView(),
				Detail = navPage
			};
		}

		public static void Feedback()
		{
			var navPage = new NavigationPage(new FeedbackPageView()) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			};

			Application.Current.MainPage = new MasterDetailPage {
				Master = new MainMenuView(),
				Detail = navPage
			};
		}

		public static void About()
        {
            var navPage = new NavigationPage(new AboutScreenView())
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            Application.Current.MainPage = new MasterDetailPage
            {
                Master = new MainMenuView(),
                Detail = navPage
            };
        }

        public static void AboutModal(INavigation navigation)
        {
            var navPage = new NavigationPage(new AboutScreenView(true))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            navigation.PushModalAsync(navPage);
        }

        public static void Settings()
        {
            var navPage = new NavigationPage(new SettingsScreenView())
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            Application.Current.MainPage = new MasterDetailPage
            {
                Master = new MainMenuView(),
                Detail = navPage
            };
        }

        public static void LabelsManagement()
        {
            var navPage = new NavigationPage(new LabelScreenView())
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            Application.Current.MainPage = new MasterDetailPage
            {
                Master = new MainMenuView(),
                Detail = navPage
            };
        }

		public static void Diagnostics(INavigation navigation, bool fromLogin)
		{
			var navPage = new NavigationPage(new DiagnosticsPageView(fromLogin)) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			};

			navigation.PushAsync(navPage);
		}

		public static void IconsManagement()
		{
			var navPage = new NavigationPage(new IconPageView()) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor,
				Title = TR.Tr("admin_panel_diagnostics")
			};

			Application.Current.MainPage = new MasterDetailPage {
				Master = new MainMenuView(),
				Detail = navPage
			};
		}

		public static void Backup(INavigation navigation = null)
        {
			if (DependencyService.Get<IPermission>().ReadWritePermission) {

				if (navigation != null) {
					var page = new BackupScreenView(navigation);
					navigation.PushAsync(page);
				} else {
					var navPage = new NavigationPage(new BackupScreenView()) {
						BarBackgroundColor = Theme.Current.AppHeaderBackground,
						BarTextColor = Theme.Current.AppHeaderTextColor
					};

					Application.Current.MainPage = new MasterDetailPage {
						Master = new MainMenuView(),
						Detail = navPage
					};
				}
			} else {
				Application.Current.MainPage.DisplayAlert(TR.Tr("alert"), TR.Tr("permission_read_write_not_granted"), TR.OK);
			}
        }

        public static NavigationPage AddLabelScreen(string labelName, string labelType, string fieldType = null, string actionType = null, Command command = null)
        {
            var navPage = new NavigationPage(new CreateLabelScreenView(labelName, labelType, fieldType, actionType, command))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            return navPage;
        }

        public static void CreateFolder(INavigation navigation)
        {
            var navPage = new NavigationPage(new MainScreenCreateItemView(NSWItemType.Folder))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            navigation.PushModalAsync(navPage);
        }

        public static void CreateItem(INavigation navigation)
        {
            var navPage = new NavigationPage(new MainScreenCreateItemView(NSWItemType.Item))
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };

            navigation.PushModalAsync(navPage);
        }

		public static void Login()
        {
			Application.Current.MainPage = new NavigationPage(new LoginScreenView())
            {
                BarBackgroundColor = Theme.Current.AppHeaderBackground,
                BarTextColor = Theme.Current.AppHeaderTextColor
            };
        }

		public static void FontSelector()
		{
			Application.Current.MainPage = new NavigationPage(new FontSelectorScreenView()) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			};
		}

		public static void PrivacyPolicy(INavigation navigation, string title, string htmlSource, Action<bool> action, bool buttons = true)
		{
			navigation.PushModalAsync(new NavigationPage(new LicensePageView(title, htmlSource, action, buttons)) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			});
		}

		public static void TermsOfUse(INavigation navigation, string title, string htmlSource, Action<bool> action, bool buttons = true)
		{
			navigation.PushModalAsync(new NavigationPage(new LicensePageView(title, htmlSource, action, buttons)) {
				BarBackgroundColor = Theme.Current.AppHeaderBackground,
				BarTextColor = Theme.Current.AppHeaderTextColor
			});
		}

		public static void CloseModalPage(INavigation navigation)
        {
            navigation.PopModalAsync(true);
        }

        public static void ClosePage(INavigation navigation)
        {
            navigation.PopAsync(true);
        }

		public static void Locker()
		{
			// Hide screen content for iOS devices
			// TODO: find better solution to secure screens in iOS
            if (Device.RuntimePlatform == Device.iOS) {
                if (Application.Current.MainPage != null &&
                    Application.Current.MainPage.Navigation != null) {
                    Application.Current.MainPage.Navigation.PushModalAsync(
                        new NavigationPage(new LockerPageView()), false);
                }
            }
		}

		public static void Unlocker(bool isModal = false)
		{
            // Unhide screen content for iOS devices
            // TODO: find better solution to secure screens in iOS
            if (Device.RuntimePlatform == Device.iOS) {
                if (Application.Current.MainPage != null &&
                Application.Current.MainPage.Navigation != null) {
                    var navigationPage = Application.Current.MainPage.Navigation;

                    if (!isModal && navigationPage.NavigationStack.Count > 0) {
                        Application.Current.MainPage.Navigation.PopAsync(false);
                    }

                    if (isModal && navigationPage.ModalStack.Count > 0) {
                        Application.Current.MainPage.Navigation.PopModalAsync(false);
                    }
                }
            }
		}
    }
}