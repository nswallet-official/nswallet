using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

namespace NSWallet
{
    public class LoginScreenFailTrigger : TriggerAction<View>
    {
        const double translationX = 20;
        const int timesNumber = 3;
        const uint duration = 50;

        LoginScreenViewModel loginScreenInstance;

        public LoginScreenFailTrigger(LoginScreenViewModel loginScreenInstance)
        {
            this.loginScreenInstance = loginScreenInstance;
        }

        protected override void Invoke(View sender)
        {
            try
            {
                PlatformSpecific.DisplayLongMessage("Failed to log in!");

                Task.Run(async () =>
                {
                    for (int iterator = 0; iterator < timesNumber; iterator++)
                    {

                        await sender.TranslateTo(-translationX, 0, duration);
                        await sender.TranslateTo(translationX, 0, duration);

                        if (iterator == timesNumber - 1)
                            await sender.TranslateTo(0, 0, duration);
                    }
                });

                loginScreenInstance.AnimationStatus = 0;
            }
            catch (Exception ex) 
            {
				AppLogs.Log(ex.Message, nameof(Invoke), nameof(LoginScreenFailTrigger));
            }
        }
    }
}