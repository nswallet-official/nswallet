using NSWallet.Controls.EntryPopup;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.Helpers
{
    public static class NativePopup
    {
        public static string PopupData { get; set; }

        public static void EntryPopup(string title, string text, Command command = null, bool secured = false)
        {
            var popup = new EntryPopup(title, text, secured);

            popup.PopupClosed += (o, closedArgs) =>
            {
                if (closedArgs.OkClicked)
                {
                    if (closedArgs.Text == string.Empty)
                    {
                        PopupData = null;
                        if (command != null)
                            command.Execute(PopupData);
                        return;
                    }

                    PopupData = closedArgs.Text;
                    if (command != null)
                        command.Execute(PopupData);
                }
            };

            popup.Show();
        }

        public static void ActionSheet(string title, string[] buttons, Command command = null)
        {
            Application.Current.MainPage.DisplayActionSheet(title, TR.Cancel, null, buttons).ContinueWith(t =>
            {
                if (t.Result != null && t.Result != TR.Cancel)
                {
                    var valueType = LM.ValueTypeByTranslation(t.Result);
                    PopupData = valueType;
                    if (command != null)
                        command.Execute(PopupData);
                }
                else
                {
                    PopupData = null;
                    if (command != null)
                        command.Execute(PopupData);
                }
            });
        }
    }
}