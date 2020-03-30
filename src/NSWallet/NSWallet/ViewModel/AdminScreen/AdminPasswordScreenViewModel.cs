using System;
using NSWallet.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class AdminPasswordScreenViewModel : ViewModel
    {
		readonly INavigation navigation;

        public AdminPasswordScreenViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }

        string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password == value)
                    return;
                password = value;
                OnPropertyChanged("Password");
            }
        }

        Command enterCommand;
        public Command EnterCommand
        {
            get { return enterCommand ?? (enterCommand = new Command(ExecuteEnterCommand));}
        }

        void ExecuteEnterCommand()
        {
            switch(СheckPassword(Password))
            {
                case AdminPassword.Empty:
                    SendMessage(TR.Tr("admin_password_empty"));
                    break;
                case AdminPassword.Wrong:
                    SendMessage(TR.Tr("admin_password_wrong"));
                    break;
                case AdminPassword.True:
                    SendMessage(TR.Tr("admin_password_success"));
					Settings.DevOpsOn = true;
					AppPages.AdminPanel(navigation);
                    break;
            }
        }

		AdminPassword СheckPassword(string adminPass)
        {
            if (string.IsNullOrEmpty(password))
                return AdminPassword.Empty;
            var isTrue = AppConsts.ADMIN_PASSWORD.Equals(adminPass);
            if (isTrue)
                return AdminPassword.True;
            return AdminPassword.Wrong;
        }

        void SendMessage(string message)
        {
            PlatformSpecific.DisplayShortMessage(message);
        }
    }
}