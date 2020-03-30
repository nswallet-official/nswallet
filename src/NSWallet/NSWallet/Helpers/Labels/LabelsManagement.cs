using System;
using System.Diagnostics;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet.Helpers
{
    public static class LM
    {
        public static string[] Labels
        {
            get
            {
                return new string[]
                {
                    TR.Tr(GConsts.VALUETYPE_TEXT),
                    TR.Tr(GConsts.VALUETYPE_DATE),
                    TR.Tr(GConsts.VALUETYPE_TIME),
                    TR.Tr(GConsts.VALUETYPE_PASS),
                    TR.Tr(GConsts.VALUETYPE_LINK),
                    TR.Tr(GConsts.VALUETYPE_PHON),
                    TR.Tr(GConsts.VALUETYPE_MAIL)
                };
            }
        }

        public static string ValueTypeByTranslation(string translation)
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

        static INavigation _navigation;
        static Command _command;

        public static void StartCreatingLabel(INavigation navigation, Command lastCommand)
        {
            _navigation = navigation;
            _command = lastCommand;
            NativePopup.EntryPopup(TR.Tr("create_label"), null, CreateNameCommand);
        }

        static string customLabelName { get; set; }
        static string customLabelType { get; set; }

        static Command createNameCommand;
        static Command CreateNameCommand
        {
            get
            {
                return createNameCommand ?? (createNameCommand = new Command(ExecuteCreateNameCommand));
            }
        }

        static void ExecuteCreateNameCommand(object obj)
        {
            if (obj != null)
            {
                NativePopup.ActionSheet(TR.Tr("create_label_choose_type"), Labels, ChooseTypeCommand);
                customLabelName = obj.ToString();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert(TR.Tr("app_name"), TR.Tr("create_label_empty"), TR.OK);
            }
        }

        static Command chooseTypeCommand;
        static Command ChooseTypeCommand
        {
            get
            {
                return chooseTypeCommand ?? (chooseTypeCommand = new Command(ExecuteChooseTypeCommand));
            }
        }

        static void ExecuteChooseTypeCommand(object obj)
        {
            if (obj != null)
            {
                try
                {
                    customLabelType = obj.ToString();
                    Device.BeginInvokeOnMainThread(() => _navigation.PushAsync(AppPages.AddLabelScreen(customLabelName, customLabelType, null, null, _command)));
                }
                catch (Exception ex)
                {
					AppLogs.Log(ex.Message, nameof(ExecuteChooseTypeCommand), nameof(LM));
					Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}