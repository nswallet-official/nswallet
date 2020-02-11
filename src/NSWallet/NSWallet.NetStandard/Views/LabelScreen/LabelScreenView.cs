using System;
using System.Collections.Generic;
using System.Diagnostics;
using NSWallet.Consts;
using NSWallet.Controls.EntryPopup;
using NSWallet.Helpers;
using NSWallet.Model;
using NSWallet.NetStandard.Helpers.UI.NavigationHeader;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class LabelScreenView : ContentPage
    {
        LabelScreenViewModel pageVM;

        public LabelScreenView()
        {
            pageVM = new LabelScreenViewModel(this.Navigation);
            BindingContext = pageVM;

			UINavigationHeader.SetCommonTitleView(this, TR.Tr("menu_labels"));

			var addLabelToolbarItem = new ToolbarItem();
            addLabelToolbarItem.Text = TR.Tr("labelpage_add_label");
            addLabelToolbarItem.Icon = Theme.Current.LabelPageAddImage;
            addLabelToolbarItem.SetBinding(MenuItem.CommandProperty, "AddLabelCommand");
            ToolbarItems.Add(addLabelToolbarItem);

            var mainLayout = new Grid();
            mainLayout.BackgroundColor = Theme.Current.ListBackgroundColor;

            var labelsListView = new ListView { ItemTemplate = new DataTemplate(typeof(LabelScreenCellView)) };
            labelsListView.BackgroundColor = Theme.Current.ListBackgroundColor;
            labelsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LabelItems");
            labelsListView.SeparatorVisibility = SeparatorVisibility.None;
            labelsListView.HasUnevenRows = true;
            labelsListView.ItemSelected += (sender, e) =>
            {
                if (((ListView)sender).SelectedItem != null)
                {
                    var obj = (NSWFormsLabelModel)e.SelectedItem;
                    pageVM.NSWFormsLabel = obj;
                    var isSystem = obj.System;
                    if (isSystem)
                    {
                        ((ListView)sender).SelectedItem = null;
                        return;
                    }

					Device.BeginInvokeOnMainThread(async () => {
						var result = await DisplayActionSheet(
							obj.Name,
							TR.Cancel,
							TR.Tr("backupmenu_delete"),
							TR.Tr("popupmenu_changetitle"),
							TR.Tr("popupmenu_changeicon")
							);

						if (result == TR.Tr("backupmenu_delete")) {
							pageVM.DeleteCommand.Execute(obj);
						}

						if (result == TR.Tr("popupmenu_changetitle")) {
							pageVM.ChangeTitleCommand.Execute(obj);
						}

						if (result == TR.Tr("popupmenu_changeicon")) {
							pageVM.ChangeIconCommand.Execute(obj);
						}
					});

                    ((ListView)sender).SelectedItem = null;
                }
            };

            mainLayout.Children.Add(labelsListView);

            //var labels = new List<string> { "Text", "Date", "Time", "Password", "URL", "Phone", "E-mail" };

            ///popupPage_Create = new PopupPage();
            //popupPage_Create.BindingContext = pageVM;
            //popupView_Create = new PopupView("New label");
            //var popupLayout_Create = popupView_Create.GetCreateMenu("OKPressedCommand", "CancelPressedCommand", "LabelName", "Label name", "Label value type", labels);
            //popupPage_Create.Content = popupLayout_Create;

            pageVM.LaunchCreatePopupCallback = CreateLabel;
            pageVM.HideCreatePopupCallback = HideCreatePopup;
            pageVM.LaunchEditPopupCallback = ChangeLabelTitle;
            pageVM.HideEditPopupCallback = HideEditPopup;
            pageVM.HideItemMenuPopupCallback = HideItemMenuPopup;
            pageVM.MenuCommandCallback = MenuSelector;
            pageVM.SystemMenuCommandCallback = SystemMenuSelector;
            pageVM.ContextMenuCommandCallback = ContextMenuSelector;
            pageVM.MessageCommand = LaunchMessageBox;
            pageVM.ErrorMessageCommand = ErrorMessage;

            Content = mainLayout;
        }

        public void ChangeLabelTitle()
        {
            var currentState = pageVM.NSWFormsLabel;
            var popup = new EntryPopup(TR.Tr("change_label_title"), currentState.Name, false);
            popup.PopupClosed += (o, closedArgs) =>
            {
                if (closedArgs.OkClicked)
                {
                    if (closedArgs.Text == string.Empty)
                    {
                        DisplayAlert(TR.Tr("app_name"), TR.Tr("change_label_title_empty"), TR.OK);
                        return;
                    }
                    else
                    {
                        pageVM.OKEditPressedCommand.Execute(closedArgs.Text);
                    }
                }
            };

            popup.Show();
        }

        public void CreateLabel()
        {
            var popup = new EntryPopup(TR.Tr("create_label"), null, false);
            popup.PopupClosed += (o, closedArgs) =>
            {
                if (closedArgs.OkClicked)
                {
                    if (closedArgs.Text == string.Empty)
                    {
                        DisplayAlert(TR.Tr("app_name"), TR.Tr("create_label_empty"), TR.OK);
                        return;
                    }
                    else
                    {
                        var labels = new string[]
                        {
                            TR.Tr(GConsts.VALUETYPE_TEXT),
                            TR.Tr(GConsts.VALUETYPE_DATE),
                            TR.Tr(GConsts.VALUETYPE_TIME),
                            TR.Tr(GConsts.VALUETYPE_PASS),
                            TR.Tr(GConsts.VALUETYPE_LINK),
                            TR.Tr(GConsts.VALUETYPE_PHON),
                            TR.Tr(GConsts.VALUETYPE_MAIL)
                        };

                        DisplayActionSheet(TR.Tr("create_label_choose_type"), TR.Cancel, null, labels).ContinueWith(t =>
                        {
                            if (t.Result != null && t.Result != TR.Cancel)
                            {
                                var valueType = ValueTypeByTranslation(t.Result);
                                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(Pages.AddLabelScreen(closedArgs.Text, valueType)));
                            }
                        });
                    }
                }
            };

            popup.Show();
        }

        static string ValueTypeByTranslation(string translation)
        {
            if (translation == TR.Tr(GConsts.VALUETYPE_DATE))
                return GConsts.VALUETYPE_DATE;
            if (translation == TR.Tr(GConsts.VALUETYPE_TIME))
                return GConsts.VALUETYPE_TIME;
            if (translation == TR.Tr(GConsts.VALUETYPE_PASS))
                return GConsts.VALUETYPE_PASS;
            if (translation == TR.Tr(GConsts.VALUETYPE_LINK))
                return GConsts.VALUETYPE_LINK;
            if (translation == TR.Tr(GConsts.VALUETYPE_PHON))
                return GConsts.VALUETYPE_PHON;
            if (translation == TR.Tr(GConsts.VALUETYPE_MAIL))
                return GConsts.VALUETYPE_MAIL;

            return GConsts.VALUETYPE_TEXT;
        }

        void ErrorMessage(int count)
        {
            if (count == 1)
                DisplayAlert(TR.Tr("error"), TR.Tr("labelpage_used_label_single") + ". " + TR.Tr("labelpage_used_label_delete_single") + ".", "OK");
            else
                DisplayAlert(TR.Tr("error"), TR.Tr("labelpage_used_label_multiple") + " (" + count + "). " + TR.Tr("labelpage_used_label_delete_multiple") + ".", "OK");
        }

        async void LaunchMessageBox(string title, string question, string type)
        {
            var answer = await DisplayAlert(title, question, TR.Yes, TR.No);

            if (answer)
            {
				//await Navigation.PopPopupAsync(false);
                MessagingCenter.Send(this, type);
            }
        }

        /*
        void LaunchCreatePopup()
        {
            Navigation.PushPopupAsync(popupPage_Create, false);
        }
        */


        void HideCreatePopup()
        {
			//Navigation.PopPopupAsync(false);
        }

        void HideEditPopup()
        {
			//Navigation.PopPopupAsync(false);
        }

        void HideItemMenuPopup()
        {
			//Navigation.PopPopupAsync(false);
        }

        public void ContextMenuSelector()
        {
            DisplayActionSheet("Choose action", "Cancel", null, "Labels' filtration", "Reset labels").ContinueWith(t =>
            {
                if (pageVM.ContextSelectedCommand.CanExecute(t.Result))
                {
                    pageVM.ContextSelectedCommand.Execute(t.Result);
                }
            });
        }

        public void MenuSelector()
        {
            DisplayActionSheet("Choose action", "Cancel", null, "Delete", "Change label name", "Change icon").ContinueWith(t =>
            {
                if (pageVM.MenuSelectedCommand.CanExecute(t.Result))
                {
                    pageVM.MenuSelectedCommand.Execute(t.Result);
                }
            });
        }

        public void SystemMenuSelector()
        {
            DisplayActionSheet("Choose action", "Cancel", null, "Change label name", "Change icon").ContinueWith(t =>
            {
                if (pageVM.SystemMenuSelectedCommand.CanExecute(t.Result))
                {
                    pageVM.SystemMenuSelectedCommand.Execute(t.Result);
                }
            });
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

